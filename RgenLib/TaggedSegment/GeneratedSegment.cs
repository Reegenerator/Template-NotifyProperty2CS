using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;
using RgenLib.Extensions;
using TextPoint = EnvDTE.TextPoint;

namespace RgenLib.TaggedSegment {
    public partial class Manager<TRenderer, TOptionAttr>
        where TRenderer : TaggedCodeRenderer, new()
        where TOptionAttr : Attribute, new() {
        public class GeneratedSegment : Tag {

            static GeneratedSegment() {
                InitRegex();
            }



            private readonly TextRange _range;
            public TextRange Range {
                get { return _range; }
            }



            public GeneratedSegment(TextRange range) {
                _range = range;
            }

            public bool IsOutdated(OptionTag option) {
                if (RegenMode != option.RegenMode) return true;

                switch (RegenMode) {
                    case RegenModes.Always:
                        return true;
                    default:
                        return Version != option.Version;

                }

            }

            static public bool IsAnyOutdated(Writer info) {
                var segments = Find(info.TargetRange, info.OptionTag.Category);
                return !segments.Any() || segments.Any(x => x.IsOutdated(info.OptionTag));
            }

            #region Regex
            public const string RegionBeginKeyword = "#region";
            public const string RegionEndKeyword = "#endregion";
      

            // ReSharper disable StaticFieldInGenericType
            private static Dictionary<TagFormat, Dictionary<SegmentTypes, Regex>> _regexDict;
  

    
            static private void InitRegex() {
                _regexDict = new Dictionary<TagFormat, Dictionary<SegmentTypes, Regex>>();
                //initialize regex
                const string xmlRegionPatternFormat = @"
                    [^\S\r\n]* #match whitespace (space/tabs due to document formatting)
                    (\{3}\s*(?<textinfo>[^<\r\n]*?)(?<xml><{0}\s*{1}='{2}'.*?/>))\s*
                    (?<content> 
                        (?>
		                (?! \{3} |\{4}) .
	                |
		                \{3} (?<Depth>)
	                |
		                \{4} (?<-Depth>)
	                )*
	                (?(Depth)(?!))
        
                    )
                    \{4}(\r\n)?";

                const string xmlCommentPatternFormat = @"
                    (
                    {3}(?<tag><{0}\s*{1}='{2}'\s*[^<>]*/>)
                    )
                    |           
                    (
                        {3}(?<tag><{0}\s*{1}='{2}'\s*
                            [^<>]*#Match everything but tag symbols
                            (?<!/)>)\s*#Match only > but not />
                        (?<content>.*?)(?<!</{0}>)
                        {3}(?<tagend></{0}>)\s*
                    )";

                //quotes are doubled to escape them inside literal string
                //curly braces are doubled to escape them for string.format
                const string jsonRegionPatternFormat = @"
                    [^\S\r\n]* #match tabs/space, but not newline, before region 
                (\{3}\s*(?<textinfo>[^\r\n]*?)
                            (?<prefix>{0}:)(?<json>\{{{1}:""{2}""[^\r\n]*\}})\s*)
                            (?<content> 
                                (?>
		                        (?! \{3} |\{4}) .
	                        |
		                        \{3} (?<Depth>)
	                        |
		                        \{4} (?<-Depth>)
	                        )*
	                        (?(Depth)(?!))
        
                            )
                            \{4}(\r\n)?";
                const string jsonSingleCommentPatternFormat = @"
                    [^\S\r\n]* #match tabs/space, but not newline, before tag
                    //(?<textinfo>[^\r\n]*?)
                    {0}\s*?:\s*?(?<json>\{{\s*?{1}\s*?:\s*?""{2}""[^\r\n]*\}})
                    ";
                var rendererAttr = TagPrototype.Attribute(XmlRendererAttributeName);
                var tagName = TagPrototype.Name.LocalName;

                var templateName = typeof(TRenderer).Name;
                //xml, statement

                _regexDict.Add(TagFormat.Xml, new Dictionary<SegmentTypes, Regex>());
                var xmlCommentPattern = string.Format(xmlCommentPatternFormat, tagName, rendererAttr.Name, rendererAttr.Value, Constants.CodeCommentPrefix);
                _regexDict[TagFormat.Xml].Add(SegmentTypes.CommentPair,
                    new Regex(xmlCommentPattern, Constants.DefaultRegexOption));
                //xml , region
                var xmlRegPattern = string.Format(xmlRegionPatternFormat, tagName, rendererAttr.Name, rendererAttr.Value, RegionBeginKeyword, RegionEndKeyword);
                _regexDict[TagFormat.Xml].Add(SegmentTypes.Region, new Regex(xmlRegPattern, Constants.DefaultRegexOption));

                //json, region
                _regexDict.Add(TagFormat.Json, new Dictionary<SegmentTypes, Regex>());
                var jsonRegPattern = string.Format(jsonRegionPatternFormat,
                                                    Constants.JsonTagPrefix,
                                                    TemplateNamePropertyName,
                                                    templateName,
                                                    RegionBeginKeyword, RegionEndKeyword);
                _regexDict[TagFormat.Json].Add(SegmentTypes.Region, new Regex(jsonRegPattern, Constants.DefaultRegexOption));

                //json, single comment
   
                var jsonSingleCommentPattern = string.Format(jsonSingleCommentPatternFormat,
                                                    Constants.JsonTagPrefix,
                                                    TemplateNamePropertyName,
                                                    templateName);
                _regexDict[TagFormat.Json].Add(SegmentTypes.SingleComment, new Regex(jsonSingleCommentPattern, Constants.DefaultRegexOption));

              
            }
            #endregion


            #region Parse

            /// <summary>
            /// Extract valid xml inside Region Name and within inline comment
            /// </summary>
            /// <returns></returns>
            /// <remarks>
            /// </remarks>
            public static XElement ExtractXml(Match match, SegmentTypes segType) {

                //var firstline = range.StartPoint.CreateEditPoint().GetLineText();
                //var segmentType = firstline.Trim().StartsWith(RegionBeginKeyword) ? SegmentTypes.Region : SegmentTypes.CommentPair;
                //var text = range.GetText();
                var xmlContent = "";
                switch (segType) {
                    case SegmentTypes.Region:
                        xmlContent = match.Result( "${xml}");
                        break;
                    case SegmentTypes.CommentPair:
                        xmlContent = match.Result( "${tag}${content}${tagend}");
                        break;
                }

                return XDocument.Parse(xmlContent).Root;
            }
            /// <summary>
            /// Extract valid xml inside Region Name and within inline comment
            /// </summary>
            /// <returns></returns>
            /// <remarks>
            /// </remarks>
            public static string ExtractJson(Match match) {

                var json = match.Result( "${json}");

                return json;

            }
            /// <summary>
            /// Parse Attribute Argument into the actual string value
            /// </summary>
            /// <param name="propInfo"></param>
            /// <param name="value"></param>
            /// <remarks>
            /// Attribute argument is presented exactly as it was typed
            /// Ex: SomeArg="Test" would result in the Argument.Value "Test" (with quote)
            /// Ex: SomeArg=("Test") would result in the Argument.Value ("Test") (with parentheses and quote)
            /// </remarks>
            private static object ParseXmlAttributeValue(PropertyInfo propInfo, string value) {

                object parsed;
                var propType = propInfo.PropertyType;
                if (propType.IsEnum) {
                    //if enum, remove the Enum qualifier (e.g TagTypes.InsertPoint becomes InserPoint)
                    parsed = value.StripQualifier();
                }
                else if (propType == typeof(DateTime) || propType == typeof(DateTime?)) {
                    parsed = DateTime.ParseExact(value, Constants.TagDateFormat, Constants.TagDateCulture);
                }
                else if (propType == typeof(string)) {
                    //remove quotes
                    parsed = value.Trim('\"');
                }

                else {
                    parsed = value;
                }
                return parsed;
            }


      

            private static void PopulateSegmentWithXml(GeneratedSegment tag, XElement xTag) {
                try {
                    var xmlProps = XmlAttributeAttribute.GetXmlProperties(typeof(GeneratedSegment));
                    foreach (var attr in xTag.Attributes()) {
                        var name = attr.Name.LocalName;


                        //skip renderer name
                        if (name == XmlRendererAttributeName) {
                            continue;
                        }
                    
                        var prop = xmlProps[name];
                        prop.SetValue(tag, ParseXmlAttributeValue(prop, attr.Value));

                    }
                }
                catch (Exception ex) {

                    Debug.DebugHere(ex);
                    throw;
                }

            }

            #endregion


            #region Find

            /// <summary>
            /// Find textPoint marked with '<code>'<Gen Type="InsertPoint" /></code>
            /// </summary>
            /// <returns></returns>
            /// <remarks>
            /// </remarks>
            static public GeneratedSegment FindInsertionPoint(TaggedRange range)
            {
                //insertion points are always single comment, override the writer info
                var copy = range.Clone();
                copy.SegmentType = SegmentTypes.SingleComment;
                return Find(copy, TagTypes.InsertPoint).FirstOrDefault();
            }
            static public GeneratedSegment[] Find(TaggedRange range, string category) {

                return Find(range, TagTypes.Generated).Where(x => x.Category == category).ToArray();

            }
            static public IEnumerable<GeneratedSegment> Find(TaggedRange range, TagTypes tagType) {
                return FindSegments(range).Where(x => x.TagType == tagType);
            }

            static public TextRange ConvertRegexMatchToTextRange(Match m, TextPoint startPoint)
            {
                var matchStart = startPoint.CreateEditPoint();
                matchStart.CharRightExact(m.Index);
                var matchEnd = matchStart.CreateEditPoint();
                matchEnd.CharRightExact(m.Length);
                return new TextRange(matchStart, matchEnd);
            }

            /// <summary>
            /// Find tagged segment within GenInfo.SearchStart and GenInfo.SearchEnd
            /// </summary>
            /// <returns></returns>
            /// <remarks>
            /// Not using EditPoint.FindPattern because it can only search from startpoint to end of doc, no way to limit to selection
            /// Not using DTE Find because it has to change params of current find dialog, might screw up normal find usage
            ///  </remarks>
            static public GeneratedSegment[] FindSegments(TaggedRange searchRange) {

                var regex = _regexDict[searchRange.TagFormat][searchRange.SegmentType];
                var searchText = searchRange.GetText();
                var matches = regex.Matches(searchText);
                var segments = new List<GeneratedSegment>();
                foreach (var m in matches.Cast<Match>())
                {
                    var matchRange = ConvertRegexMatchToTextRange(m, searchRange.StartPoint);
                    var segment = new GeneratedSegment(matchRange);
                    switch (searchRange.TagFormat) {
                        case TagFormat.Json:
                            var json= ExtractJson(m);
                            try
                            {
                                JsonConvert.PopulateObject(json, segment);

                            }
                            catch (Exception e)
                            {
                                Debug.DebugHere(e);
                                throw;
                            }
                            break;
                        case TagFormat.Xml:
                            var x = ExtractXml(m, searchRange.SegmentType );
                            PopulateSegmentWithXml(segment, x);
                            break;
                    }
                    segments.Add(segment);
                }
                return segments.ToArray();
            }

       




            #endregion

        }
    }
}