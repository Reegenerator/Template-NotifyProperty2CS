using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Kodeo.Reegenerator.Generators;
using RgenLib.Extensions;

namespace Templates {
    /// <summary>
    /// Created as a separate class because currently there's no way to tell if the template is triggered as CodeSnippet or attached renderer
    /// </summary>
        [CodeSnippet]
    class NotifyPropertySnippet : NotifyProperty {
            protected override EnvDTE80.CodeClass2[] GetValidClasses() {
                var cls =Dte.GetCodeElementAtCursor(vsCMElement.vsCMElementClass);
                return new CodeClass2[] {(CodeClass2)cls};
            }

            protected override bool _alwaysInsert {
                get {
                    return true;
                }
            }

            protected override bool _autoPropertyExpansionIsTagged {
                get {
                    return false;
                }
            }
    }
}
