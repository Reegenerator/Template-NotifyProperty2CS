using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Attributes;
using EnvDTE;
using EnvDTE80;
using Kodeo.Reegenerator.Generators;
using RgenLib.Extensions;
using RgenLib.TaggedSegment;
using System.IO;
using Constants = EnvDTE.Constants;
using Debug = RgenLib.Extensions.Debug;
using ManagerType = RgenLib.TaggedSegment.Manager<Templates.NotifyProperty, Attributes.NotifyPropertyOptionAttribute>;


namespace Templates {
    /// <summary>
    /// To use this renderer, attach to the target file. And add AutoGenerateAttribute to the class
    /// </summary>
    /// <remarks></remarks>

    public partial class NotifyProperty {

        private readonly ManagerType _manager;
        private static readonly string INotifyPropertyChangedName = typeof(INotifyPropertyChanged).FullName;
        private static readonly Type _optionAttributeType = typeof(NotifyPropertyOptionAttribute);

        /// <summary>
        /// To be overriden by Snippet
        /// </summary>
        protected virtual bool AlwaysInsert { get { return false; } }
        protected virtual bool AutoPropertyExpansionIsTagged { get { return true; } }
        protected virtual TriggerTypes TriggerType { get { return TriggerTypes.Attribute; } }
        public override Type OptionAttributeType {
            get { return _optionAttributeType; }
        }

        private string INotifierFullName {
            get { return ProjectItem.Project.DefaultNamespace.DotJoin(NotifyPropertyLibrary.INotifierName); }
        }

        public NotifyProperty() {

            //var tagName = (new NotifyPropertyChanged_GenAttribute()).TagName;
            _manager = new ManagerType(TagFormat.Xml);
        }
        public ManagerType Manager {
            get {
                return _manager;
            }
        }

        /// <summary>
        /// Create the library file that contains INotifier and Notification extensions
        /// has to be created before EnvDte can add the interface <see cref="RgenLib.Extensions.General.AddInterfaceIfNotExists"/> to classes
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public void RenderLibrary() {
            //Check for existing class
            var prj = ProjectItem.Project;

            const string className = NotifyPropertyLibrary.DefaultClassName;
            var classes = prj.GetCodeElements<CodeClass>();
            var classFullname = prj.DefaultNamespace.DotJoin(className);
            List<CodeClass> matchingClass;
            classes.TryGetValue(classFullname, out matchingClass);

            ProjectItem classItem;
            if (matchingClass == null) {
                //Class not found, generate
                var filePath = Path.Combine(prj.FullPath, className + ".cs");
                //if file exists, warn then quit
                if (File.Exists(filePath)) {
                    MessageBox.Show(string.Format("Trying to add {0} to project, but file {1} already exists", className, filePath));
                    return;
                }
                //Create new empty file
                File.WriteAllText(filePath, "");
                prj.CheckOut();
                //Add it to the project
                classItem = prj.DteObject.ProjectItems.AddFromFile(filePath);
            }
            else {
                //Class found, get corresponding project item
                classItem = matchingClass.First().ProjectItem;
            }

            //Open file for editing
            var wasOpen = classItem.IsOpen[Constants.vsViewKindCode];
            if (!wasOpen) {
                classItem.Open(Constants.vsViewKindCode);
            }
            var textDoc = classItem.Document.ToTextDocument();
            var writer = new ManagerType.Writer(_manager) {
                TargetRange = new TaggedRange { StartPoint = textDoc.StartPoint, EndPoint = textDoc.EndPoint, SegmentType = SegmentTypes.Region },
                InsertStart = textDoc.StartPoint

            };

            if (ManagerType.GeneratedSegment.IsAnyOutdated(writer)) {
                //generate text if outdated
                var code = new NotifyPropertyLibrary(prj.DefaultNamespace).RenderToString();
                writer.Content = code;
                writer.InsertOrReplace(AlwaysInsert);
                classItem.Save();
            }

            //restore to previous state. Close if was not open initially
            if (!wasOpen) {
                classItem.Document.Close();
            }
        }

        /// <summary>
        /// Render within target file, instead of into a separate file
        /// </summary>
        /// <remarks></remarks>
        private void RenderWithinTarget() {

            var undoCtx = Dte.UndoContext;
            undoCtx.Open(OptionAttributeType.Name);
            try {
                //render shared library. It has to be created before the interface can be added to the classes. Otherwise EnvDte would throw exception
                RenderLibrary();

                var validClasses = GetValidClasses();

                var sw = new Stopwatch();
                var hasError = false;
                //!for each class 
                foreach (var cc in validClasses) {
                    sw.Start();

                    var classWriter = _manager.CreateWriter(cc);

                    //!generate
                    GenerateInClass(classWriter);

                    //!if also doing derivedClasses
                    var applyToSubclasses = classWriter.OptionTag.OptionAttribute.ApplyToDerivedClasses;
                    if (applyToSubclasses) {

                        //!for each subclass
                        foreach (var derivedC in cc.GetSubclasses()) {
                            var childInfo = _manager.CreateWriter(cc, derivedC);
                            //generate
                            GenerateInClass(childInfo);
                            //combine status
                            if (childInfo.HasError) {
                                classWriter.HasError = true;
                                classWriter.Status.AppendLine(childInfo.Status.ToString());
                            }
                        }
                    }

                    //if there's error
                    if (classWriter.HasError) {
                        hasError = true;
                        MessageBox.Show(classWriter.Status.ToString());
                    }
                    //finish up
                    sw.Stop();
                    DebugWriteLine(string.Format("Finished {0} in {1}", cc.Name, sw.Elapsed));
                    sw.Reset();
                }


                //if there's error
                if (hasError) {
                    //undo everything
                    undoCtx.SetAborted();
                }
                else {
                    undoCtx.Close();
                    //automatically save, since we are changing the target file
                    var doc = ProjectItem.DteObject.Document;
                    //if anything is changed, save
                    if (doc != null && !doc.Saved) {
                        doc.Save();
                    }
                }

            }
            catch (Exception ex) {
                Debug.DebugHere(ex);
                if (undoCtx.IsOpen) {
                    undoCtx.SetAborted();
                }
            }
        }

        protected virtual CodeClass2[] GetValidClasses() {
            //get only classes marked with the attribute

            var validClasses = (
                from cc in ProjectItem.GetClassesWithAttribute(OptionAttributeType)
                select cc).ToArray();
            return validClasses;
        }

        /// <summary>
        /// Expand Auto properties into a normal properties, so we can insert Notify statement in the setter
        /// </summary>
        /// <param name="writer"></param>
        /// <remarks></remarks>
        public void ExpandAutoProperties(ManagerType.Writer writer) {
            var autoProps = writer.Class.GetAutoProperties()
                                .Where(p => p.AsCodeElement()
                                    .GetAttributeProperty((NotifyPropertyOptionAttribute a) => a.IsIgnored) == false);
            foreach (var p in autoProps) {
                ExpandAutoProperty(p, writer);
            }
        }




        /// <summary>
        /// Expand auto property into a normal property
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="parentWriter"></param>
        /// <remarks></remarks>
        public void ExpandAutoProperty(CodeProperty2 prop, ManagerType.Writer parentWriter) {

            //Save existing elements of the property
            //doc comment
            var comment = prop.AsCodeElement().GetDocComment();
            //attributes
            var propAttrs = prop.GetText(vsCMPart.vsCMPartAttributesWithDelimiter);
            //Interface implementation 
            var interfaceImpl = prop.GetInterfaceImplementation();


            var code = General.GetTemplateOutput(output =>
                GenProperty(output, prop.Name, prop.Type.SafeFullName(), comment, propAttrs, interfaceImpl)
                );
            var writer = new ManagerType.Writer(parentWriter) {
                TargetRange = new TaggedRange { SegmentType = SegmentTypes.Region },
                TagNote = string.Format("{0} expanded by", prop.Name),
                //OptionTag,
                Content = code
            };

            var text = AutoPropertyExpansionIsTagged ? writer.GenText() : code;
            //only do this once, since once it is expanded it will no longer be detected as auto property


            //Replace all code starting from comment to endPoint of the property
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            const int options = (int)(vsEPReplaceTextOptions.vsEPReplaceTextAutoformat |
                                       vsEPReplaceTextOptions.vsEPReplaceTextNormalizeNewlines);
            prop.GetCommentStartPoint()
                .CreateEditPoint()
                .ReplaceText(prop.EndPoint, text, options);

        }

        /// <summary>
        /// Generate code that will notify other propertyName different from the member with the attribute.
        /// </summary>
        /// <param name="optionTag"></param>
        /// <param name="parentWriter"></param>
        /// <remarks>
        /// Example Add NotifyPropertyChanged_GenAttribute with ExtraNotifications="OtherProperty1,OtherProperty2" to SomeProperty.
        /// This method will generate code for Notify("OtherProperty1") and Notify("OtherProperty2") within that member
        /// This is useful for Property that affects other Property, or a method that affects another property.
        /// This has the advantage of generation/compile time verification of the properties
        /// </remarks>
        private string GenInMember_ExtraNotifications(ManagerType.OptionTag optionTag, ManagerType.Writer parentWriter) {

            //Render extra notifications (notifications for other related properties)
            if (string.IsNullOrEmpty(optionTag.OptionAttribute.ExtraNotifications)) {
                return null;
            }

            var extras = GetValidatedExtraNotifications(parentWriter.Clone(optionTag));
            return string.Format(NotifyChangedFormat, string.Join(",", extras.Select(x => x.Quote())));


        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Array of invalid properties</returns>
        /// <remarks></remarks>
        public string[] GetValidatedExtraNotifications(ManagerType.Writer writer) {

            var propNames = new HashSet<string>(writer.Class.GetProperties().Select(x => x.Name));
            var extraNotifications = writer.OptionTag.OptionAttribute.ExtraNotifications
                                        .Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

            var invalids = extraNotifications.Where(x => !(propNames.Contains(x))).ToArray();
            if (invalids.Any()) {
                writer.HasError = true;
                writer.Status.AppendFormat("Properties:{0} to be notified are not found in the class", string.Join(", ", invalids));
            }

            return extraNotifications;
        }

        const string NotifyChangedFormat = "this.NotifyChanged({0});";

        /// <summary>
        /// Generates code in 
        /// </summary>
        /// <param name="optionTag"></param>
        /// <param name="parentWriter"></param>
        /// <remarks></remarks>
        private void GenInMember(ManagerType.OptionTag optionTag, ManagerType.Writer parentWriter) {
            var memberName = optionTag.CodeElement.Name;
            //!Parent can be either CodeFunction(only for ExtraNotifications) or CodeProperty
            string code = null;
            var note = "";

            if (optionTag.CodeElement is CodeProperty) {
                switch (optionTag.OptionAttribute.GenerationType) {
                    case NotifyPropertyOptionAttribute.GenerationTypes.NotifyOnly:

                        //Notify Member name
                        note = string.Format("Notify {0}", memberName);
                        code = string.Format(NotifyChangedFormat, memberName);
                        break;
                    default:
                        //Set and property Member name
                        note = string.Format("SetPropertyAndNotify {0}", memberName);
                        code = string.Format("this.SetPropertyAndNotify(ref _{0}, value, \"{0}\");", memberName);
                        break;
                }
            }

            //Extra notifications
            var extraNotifyCode = GenInMember_ExtraNotifications(optionTag, parentWriter);
            code = code.Conjoin(Environment.NewLine, extraNotifyCode);

            //Code Element, could be property setter or a method
            var prop = (optionTag.CodeElement as CodeProperty2);
            var codeElement = (CodeFunction2)((prop != null) ? prop.Setter : (CodeFunction2)optionTag.CodeElement);
            var tag = optionTag.MemberwiseClone();
            tag.Trigger.Type = TriggerType;

            var memberWriter = new ManagerType.Writer(parentWriter) {
                TagNote = note,
                OptionTag =(ManagerType.OptionTag) tag,
                TargetRange = new TaggedRange { StartPoint = codeElement.StartPoint, EndPoint = codeElement.EndPoint, SegmentType = SegmentTypes.Region },
                Content = code,
            };

            //Find insertion point
            EditPoint insertPoint;
            EditPoint afterClosingBracePoint = null;

            var insertTag = ManagerType.GeneratedSegment.FindInsertionPoint(memberWriter.TargetRange);
            if (insertTag == null) {
                //!No insertion point tag specified, by default insert as last line of setter

                insertPoint = codeElement.GetClosingBracePosition();
                afterClosingBracePoint = insertPoint.CreateEditPoint();
                afterClosingBracePoint.CharRight();
                //always insert new line in case the everything in one line

            }
            else {
                //!InsertPoint Tag found, insert right after it
                insertPoint = insertTag.Range.EndPoint.CreateEditPoint();
                insertPoint.LineDown();
                insertPoint.StartOfLine();
            }

            memberWriter.InsertStart = insertPoint;
            memberWriter.InsertOrReplace(AlwaysInsert, afterClosingBracePoint);

        }
        private void GenInMembers(ManagerType.Writer tsWriter) {
            //!Generate in properties
            var props = tsWriter.Class.GetProperties().ToArray();
            var propOptions = (
                from p in props
                select new ManagerType.OptionTag(p)).ToArray();

            var functions = from f in tsWriter.Class.GetFunctions()
                            where f.AsCodeElement().HasAttribute(OptionAttributeType)
                            select f;
            var funcOptions = (
                from f in functions
                select new ManagerType.OptionTag(f)).ToArray();



            var dpFields = tsWriter.Class.GetDependencyProperties();


            Func<ManagerType.OptionTag, bool> notDpField = x => {
                var res = !(dpFields.Any(dp => dp.Name == x.CodeElement.Name + "Property"));
                return res;
            };
            Func<ManagerType.OptionTag, bool> notIgnored = x => {
                var attr = x.OptionAttribute;
                return attr == null || !attr.IsIgnored;
            };


            var propsWithSetters = propOptions.Where(x => ((CodeProperty2)x.CodeElement).Setter != null);
            //?filter out property for DependencyProperties 
            var validMembers = funcOptions.Concat(propsWithSetters.Where(notDpField).Where(notIgnored));

            foreach (var pa in validMembers) {

                GenInMember(pa, tsWriter);
            }

        }

        private static CodeClass2 GetFirstAncestorImplementing(IEnumerable<CodeClass2> ancestorClasses, string interfaceName) {
            return ancestorClasses.FirstOrDefault(x => x.ImplementedInterfaces.OfType<CodeInterface>().Any(i => i.FullName == interfaceName));

        }
        private void GenerateNotifyFunctions(ManagerType.Writer tsWriter) {
            if (tsWriter.TriggeringBaseClass != null) {
                //if triggered by base class, the notify functions must have already been created
                return;
            }
            if (!tsWriter.Class.Members.Cast<CodeElement>().Any()) //?if there's no member, there won't be any properties. Skip
			{
                return;
            }

            //!If INotify is already implemented by base class, do not generate (only generate tag)
            var ancestorClasses = tsWriter.Class.GetAncestorClasses().ToArray();
            var ancestorImplementingINPC = GetFirstAncestorImplementing(ancestorClasses, INotifyPropertyChangedName);
            var inotifierFullname = string.Format("{0}.{1}", ProjectItem.Project.DefaultNamespace, NotifyPropertyLibrary.INotifierName);
            var ancestorImplementingINotifier = GetFirstAncestorImplementing(ancestorClasses, inotifierFullname);
            string code;
            if (ancestorImplementingINotifier != null) {
                code = string.Format("'{0} already implemented by {1}", NotifyPropertyLibrary.INotifierName, ancestorImplementingINotifier.FullName);
            }
            else if (ancestorImplementingINPC != null) {
                code = string.Format("'{0} already implemented by {1}{2}", ancestorImplementingINPC.FullName, INotifyPropertyChangedName, Environment.NewLine);
            }
            else {
                code = General.GetTemplateOutput(output => GenFunctions(output, tsWriter.Class.FullName, true));
            }
            var insertPoint = tsWriter.Class.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
            insertPoint.StartOfLine();
            //var body = writer.Class.GetText(vsCMPart.vsCMPartBody);
            //var header = writer.Class.GetText(vsCMPart.vsCMPartHeader);
            //var x = writer.Class.GetText(vsCMPart.vsCMPartBodyWithDelimiter);
            //var name = writer.Class.GetText(vsCMPart.vsCMPartName);
            //copy info, instead of using the passed parameter, prevent unintentionally using irrelevant property set 
            // by other code
            var newInfo = new ManagerType.Writer(tsWriter) {
                TargetRange = new TaggedRange { StartPoint = tsWriter.Class.StartPoint, EndPoint = tsWriter.Class.EndPoint, SegmentType = SegmentTypes.Region },
                InsertStart = insertPoint,
                Content = code,
                TagNote = "INotifier Functions",
                OptionTag = { Category = "INotifierFunctions", Trigger = { Type = TriggerType } }
            };

            var isUpdated = newInfo.InsertOrReplace();
            if (isUpdated) {
                var fullname = tsWriter.Class.ProjectItem.GetDefaultNamespace().DotJoin(NotifyPropertyLibrary.INotifierName);
                tsWriter.Class.AddInterfaceIfNotExists(fullname);
            }


        }
        public void GenerateInClass(ManagerType.Writer writer) {
            GenerateNotifyFunctions(writer);
            ExpandAutoProperties(writer);
            GenInMembers(writer);
            AppendWarning(writer);
            DebugWriteLine(writer.Status.ToString());
        }

        private void AppendWarning(ManagerType.Writer writer) {
            var autoProperties = writer.Class.GetAutoProperties().Where(x => !((new ManagerType.OptionTag(x)).OptionAttribute.IsIgnored)).ToArray();
            //?Warn unprocesssed autoproperties
            if (autoProperties.Any()) {
                writer.HasError = true;
                writer.Status.AppendFormat("{0} Autoproperties skipped:", writer.Class.Name).AppendLine();

                foreach (var ap in autoProperties) {
                    writer.Status.AppendIndent(1, ap.Name).AppendLine();
                }


            }
        }

        public override RenderResults Render() {
            Debug.SetTraceListener(OutputPaneTraceListener);
            RenderWithinTarget();
            return null; // new RenderResults();
            //"'Because of the way custom tool works a file has to be generated. This file can be safely ignored.");

        }



    }
}