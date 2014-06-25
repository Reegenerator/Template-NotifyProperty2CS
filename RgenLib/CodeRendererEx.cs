using System;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Kodeo.Reegenerator.Generators;

namespace RgenLib {
    public abstract class CodeRendererEx : CodeRenderer {

        public readonly string Newline = Environment.NewLine;
        public DTE Dte {
            get {
                return ProjectItem.DteObject.DTE;
            }
        }

        public DTE2 Dte2 {
            get {
                return (DTE2)Dte;
            }
        }

        public StringBuilder _OutputBuilder;
        public StringBuilder OutputBuilder {
            get { return _OutputBuilder ?? (_OutputBuilder = Output.GetStringBuilder()); }
        }


        public TextSelection GetTextSelection() {
            return (TextSelection)Dte.ActiveDocument.Selection;
        }


        public void DebugWrite(string text) {
            OutputPaneTraceListener.Write(text);
        }
        public void DebugWriteLine(string text) {
            OutputPaneTraceListener.WriteLine(text);
        }



    }
}
