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
        [CodeSnippet]
    class NotifyPropertySnippet : NotifyProperty {
            protected override EnvDTE80.CodeClass2[] GetValidClasses() {
                var cls =Dte.GetCodeElementAtCursor(vsCMElement.vsCMElementClass);
                return new CodeClass2[] {(CodeClass2)cls};
            }
    }
}
