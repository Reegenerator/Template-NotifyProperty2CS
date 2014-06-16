using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Kodeo.Reegenerator.Generators;

namespace RgenLib {
    public abstract class CodeRendererEx : CodeRenderer {
        public readonly string Newline = Environment.NewLine;
        public EnvDTE.DTE Dte {
            get {
                return base.ProjectItem.DteObject.DTE;
            }
        }

        public EnvDTE80.DTE2 Dte2 {
            get {
                return (EnvDTE80.DTE2)Dte;
            }
        }

        public StringBuilder _OutputBuilder;
        public StringBuilder OutputBuilder {
            get { return _OutputBuilder ?? (_OutputBuilder = this.Output.GetStringBuilder()); }
        }


        public TextSelection GetTextSelection() {
            return (TextSelection)Dte.ActiveDocument.Selection;
        }


        public void DebugWrite(string text) {
            this.OutputPaneTraceListener.Write(text);
        }
        public void DebugWriteLine(string text) {
            this.OutputPaneTraceListener.WriteLine(text);
        }



    }
}
