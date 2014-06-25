using System;
using EnvDTE80;
using RgenLib.Extensions;

namespace RgenLib.TaggedSegment {
    public partial class Manager<TRenderer, TOptionAttr>
        where TRenderer : TaggedCodeRenderer, new()
        where TOptionAttr : Attribute, new() {

        public class OptionTag : Tag {
            private CodeElement2 _codeElement;


            public OptionTag() {
                //do nothing
            }
            public CodeElement2 CodeElement {
                get { return _codeElement; }
                set {
                    _codeElement = value;
                    //set OptionAttribute if exists
                    var codeAttr = value.GetCustomAttribute(typeof(TOptionAttr));
                    if (codeAttr != null) OptionAttribute = codeAttr.ToAttribute<TOptionAttr>();

                }
            }

            public OptionTag(CodeProperty2 codeProperty2) {
                CodeElement = (CodeElement2)codeProperty2;
            }

            public OptionTag(CodeFunction2 codeFunction) {
                CodeElement = (CodeElement2)codeFunction;
            }
        }
    }
}