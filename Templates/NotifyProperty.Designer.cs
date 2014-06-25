// -------------------------------------------------------
// Automatically generated with Kodeo's Reegenerator for NON-COMMERCIAL USE
// Generator: RgenTemplate (internal)
// Generation date: 2014-06-23 08:58
// Generated by: GTS-DEV\Pranolo
//-------------------------------------------------------
namespace Templates
{
    using System;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reegenerator", "2.0.6.0")]
    [Kodeo.Reegenerator.Generators.TemplateDisplayAttribute(DisplayName="NotifyProperty", Description="NotifyPropertyChanged Code Generator", HideInDialog=false)]
    public partial class NotifyProperty : RgenLib.TaggedCodeRenderer
    {
        
        /// <summary>
        ///Renders the code as defined in the source script file.
        ///</summary>
        ///<param name="output">The instance where the rendered code will be written to.</param>
        ///<param name="className"></param>
        ///<param name="IncludeNPC"></param>
        public virtual void GenFunctions(System.IO.StringWriter output, String className, Boolean IncludeNPC)
        {
 
	if (IncludeNPC) { 

            output.Write(" \tpublic event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;" +
                    "");

	} 
            output.Write("\r\n    void ");
            output.Write( INotifierFullName );
            output.Write(".Notify(string propertyName) {  \r\n\t\tif (PropertyChanged != null) PropertyChanged(" +
                    "this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));\r\n    }");
            output.WriteLine();
        }
        
        /// <summary>
        ///Renders the code as defined in the source script file.
        ///</summary>
        ///<param name="output">The instance where the rendered code will be written to.</param>
        ///<param name="propName"></param>
        ///<param name="propType"></param>
        ///<param name="existingComment"></param>
        ///<param name="attributes"></param>
        ///<param name="interfaceImpl"></param>
        public virtual void GenProperty(System.IO.StringWriter output, String propName, String propType, String existingComment, String attributes, String interfaceImpl)
        {
            output.Write("\tprivate ");
            output.Write( propType );
            output.Write(" _");
            output.Write( propName );
            output.Write("; ");
            output.Write(
	 existingComment );
            output.Write(
	 attributes 	);
            output.Write("\r\n\tpublic ");
            output.Write( propType );
            output.Write(" ");
            output.Write( propName );
            output.Write(" {\r\n        get {\r\n            return _");
            output.Write( propName );
            output.Write(";\r\n        }\r\n        set {");
 //Left empty to be filled by Generated code
            output.Write("\r\n\t\t}\r\n    }");
            output.WriteLine();
        }
    }
}
