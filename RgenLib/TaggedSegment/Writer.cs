
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using EnvDTE80;
using Newtonsoft.Json;
using RgenLib.Extensions;
using TextPoint = EnvDTE.TextPoint;

namespace RgenLib.TaggedSegment {
    public partial class Manager<TRenderer, TOptionAttr>
        where TRenderer : TaggedCodeRenderer, new()
        where TOptionAttr : Attribute, new() {

        /// <summary>
        /// Holds information required to generate code segments
        /// </summary>
        /// <remarks></remarks>
        public class Writer {
            private Manager<TRenderer, TOptionAttr> _manager;

            public Manager<TRenderer, TOptionAttr> Manager { get { return _manager; } }
            public OptionTag OptionTag { get; set; }

            public Writer(Manager<TRenderer, TOptionAttr> manager) {
                _manager = manager;
                OptionTag = new OptionTag();

            }
            public Writer(Manager<TRenderer, TOptionAttr> manager, CodeClass2 cc)
                : this(manager) {
                Class = cc;

            }
            public Writer(Manager<TRenderer, TOptionAttr> manager, CodeClass2 cc, CodeClass2 triggeringBase)
                : this(manager, cc) {
                TriggeringBaseClass = triggeringBase;

            }

            public Writer(Writer sourceWriter) {
                CopyPropertiesFrom(sourceWriter);
            }

            /// <summary>
            /// Clone with new option
            /// </summary>
            /// <param name="optionTag">New Option</param>
            /// <returns></returns>
            public Writer Clone(OptionTag optionTag) {
                var clone = Clone();
                clone.OptionTag = optionTag;
                return clone;
            }
            /// <summary>
            /// Create a new writer with the same Class, TriggeringBaseClass and GeneratorAttribute
            /// </summary>
            /// source of properties to be copied
            /// <remarks></remarks>
            public Writer Clone() {
                var newWriter = new Writer(Manager);
                newWriter.CopyPropertiesFrom(this);
                return newWriter;
            }

            private void CopyPropertiesFrom(Writer source) {
                _manager = source.Manager;
                Class = source.Class;
                TriggeringBaseClass = source.TriggeringBaseClass;
                //Clone instead of reusing parent's attribute, because they may have different property values

                OptionTag = (OptionTag)source.OptionTag.MemberwiseClone();
              
            }

            public TagFormat TagFormat { get { return Manager.TagFormat; } }
            public CodeClass2 TriggeringBaseClass { get; set; }
            public CodeClass2 Class { get; set; }
            //public TRenderer Renderer { get; set; }

            public TaggedRange TargetRange {
                get { return _targetRange; }
                set {
                    _targetRange = value;
                    _targetRange.TagFormat = Manager.TagFormat;
                }
            }


            public TextPoint InsertStart { get; set; }
            public TextPoint InsertedEnd { get; set; }
            public string Content { get; set; }
            public string ProcessedContent { get; set; }
            /// <summary>
            /// Additional note placed before the actual xml
            /// </summary>
            public string TagNote { get; set; }
            private bool _OpenFileOnGenerated = true;
            public bool OpenFileOnGenerated {
                get {
                    return _OpenFileOnGenerated;
                }
                set {
                    _OpenFileOnGenerated = value;
                }
            }
            public bool HasError { get; set; }

            private StringBuilder _Status;
            private TaggedRange _targetRange;

            public StringBuilder Status {
                get { return _Status ?? (_Status = new StringBuilder()); }
            }



            public void OutlineText() {

                var endPointExcludingNewline = InsertedEnd.CreateEditPoint();
                endPointExcludingNewline.CharLeft();
                InsertStart.CreateEditPoint().OutlineSection(endPointExcludingNewline);
            }

            public TextPoint GetContentEndPoint() {
                var endP = InsertStart.CreateEditPoint();
                endP.CharRightExact(Content.Length);
                return endP;
            }

            public string GetSearchText() {
                return TargetRange.GetText();
            }

            public TextPoint Insert_Format_Trim(TextPoint formatEndPoint = null) {
                var text = GenText().DeleteBlanklines();
                InsertedEnd = InsertStart.InsertAndFormat(text, formatEndPoint);
                return InsertedEnd;
            }

            #region Tag Generation

            public string GenTag() {
                return Manager.TagFormat == TagFormat.Json ? GenJsonTag() : GenXmlTag().ToString();
            }
            public string GenJsonTag() {

                var serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore, ContractResolver = Tag.OrderedPropertyResolver };
                var stringWriter = new StringWriter();
                var writer = new JsonTextWriter(stringWriter) { QuoteName = false };
                serializer.Serialize(writer, OptionTag);
                writer.Close();
                var json = stringWriter.ToString();
                return string.Format("{2} {0}:{1}", Constants.JsonTagPrefix, json, TagNote);
            }

            public XElement GenXmlTag() {
            
                var xml = new XElement(Tag.TagPrototype);
                var trigger = OptionTag.Trigger;
                if (trigger != null) {
                    xml.SetAttributeValue("Trigger", trigger.Type.ToString());
                    xml.SetAttributeValue("TriggerInfo", trigger.TriggeringBaseClass);
                }
            

                xml.SetAttributeValue(Tag.GenerateDatePropertyName, DateTime.Now.ToString(Constants.TagDateFormat, Constants.TagDateCulture));
                object debugKey;
                try
                {
                    foreach (var keyValuePair in XmlAttributeAttribute.GetXmlProperties(typeof(Tag)))
                    {
                        debugKey = keyValuePair;
                        var propValue = keyValuePair.Value.GetValue(OptionTag);
                        //only write the xml attribute if it has a value, to keep the tag concise
                        if (propValue != null) {
                            xml.Add(new XAttribute(keyValuePair.Key, propValue));
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return xml;
            }


            public string CreateXmlTaggedCommentText() {
                //?Newline is added surrounding the text because we can't figure out how to add newline in TagXmlWriter
                var xml = GenXmlTag();
                xml.Add(Environment.NewLine + Content + Environment.NewLine);
                return XmlWriter.ToCommentedString(xml);
            }

            public string CreateXmlTaggedRegionName() {
                var xml = GenXmlTag();
                var regionNameXml = XmlWriter.ToRegionNameString(xml);
                return TagNote.Conjoin("\t", regionNameXml);

            }

            private string GenTaggedRegionText(string regionName) {

                var res = string.Format("#region {0}{1}{2}{1}{3}{1}", regionName, Environment.NewLine, Content, "#endregion");
                return res;
            }




            public string GenText() {
                try {
                    OptionTag.GenerateDate = DateTime.Now;
                    switch (Manager.TagFormat) {
                        case TagFormat.Xml:
                            switch (TargetRange.SegmentType) {
                                case SegmentTypes.Region:
                                    return GenTaggedRegionText(CreateXmlTaggedRegionName());
                                case SegmentTypes.CommentPair:
                                    return CreateXmlTaggedCommentText();
                                default:
                                    throw new Exception("Unknown SegmentType");
                            }
                        case TagFormat.Json:
                            switch (TargetRange.SegmentType) {
                                case SegmentTypes.Region:
                                    return GenTaggedRegionText(GenJsonTag());
                                case SegmentTypes.CommentPair:
                                    return Constants.CodeCommentPrefix + GenJsonTag();
                                default:
                                    throw new Exception("Unknown SegmentType");
                            }
                        default:
                            throw new Exception("Unknown TagFormat");
                    }
                }
                catch (Exception e) {
                    Debug.DebugHere(e);
                    throw;
                }

            }



            #endregion


            /// <summary>
            /// Insert or Replace text in taggedRange if outdated (or set to always generate)
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool InsertOrReplace(bool alwaysInsert = false, TextPoint formatEndPoint = null) {
                var generatedSegments = GeneratedSegment.Find(TargetRange, OptionTag.Category);
                var needInsert = false;
                if (generatedSegments.Length == 0) {
                    //if none found, then insert
                    needInsert = true;
                }
                else {
                    //if any is outdated, delete, and reinsert
                    foreach (var t in
                        from t1 in generatedSegments
                        where alwaysInsert || t1.IsOutdated(OptionTag)
                        select t1) {

                        t.Range.DeleteText();
                        needInsert = true;
                    }
                }
                if (!needInsert) {
                    return false;
                }

                Insert_Format_Trim(formatEndPoint);
                //!Open file if requested
                if (OpenFileOnGenerated && Class != null) {
                    if (!Class.ProjectItem.IsOpen) {
                        Class.ProjectItem.Open();
                    }
                }
                return true;
            }


        }

    }
}