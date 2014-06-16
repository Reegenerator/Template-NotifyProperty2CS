using EnvDTE80;
using RgenLib.Extensions;

namespace RgenLib.TaggedSegment
{
    public partial class Manager<TRenderer> where TRenderer : TaggedCodeRenderer, new()
    {

        public class OptionTag : Tag
        {
            private CodeElement2 _codeElement;


            static OptionTag()
            {
            

            }

            public CodeElement2 CodeElement
            {
                get { return _codeElement; }
                set
                {
                    _codeElement = value; 
                    OptionAttribute = value.GetCustomAttribute(typeof(OptionAttribute))
                }
            }

            public OptionTag(CodeProperty2 codeProperty2, TypeCache attrType)
            {
                CodeElement =(CodeElement2) codeProperty2;
            }
            public OptionTag(CodeFunction2 codeFunction)
            {
                CodeElement = (CodeElement2)codeFunction;
            }
        }
    }
}