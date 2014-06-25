using EnvDTE;
using EnvDTE80;
using Kodeo.Reegenerator.Generators;
using RgenLib.Extensions;
using RgenLib.TaggedSegment;

namespace Templates {
    /// <summary>
    /// Created as a separate class because currently there's no way to tell if the template is triggered as CodeSnippet or attached renderer
    /// </summary>
    [CodeSnippet]
    class NotifyPropertySnippet : NotifyProperty {
        protected override CodeClass2[] GetValidClasses() {
            var cls = Dte.GetCodeElementAtCursor(vsCMElement.vsCMElementClass);
            
            return cls == null ? new CodeClass2[] { } : new[] { (CodeClass2)cls };
        }

        protected override bool AlwaysInsert {
            get {
                return true;
            }
        }

        protected override bool AutoPropertyExpansionIsTagged {
            get {
                return false;
            }
        }

        protected override RgenLib.TaggedSegment.TriggerTypes TriggerType {
            get {
                return TriggerTypes.CodeSnippet;
            }
        }
    }
}
