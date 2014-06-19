using System;
using System.Collections.Generic;
using Attributes;
using EnvDTE80;

namespace RgenLib.TaggedSegment {
    /// <summary>
    /// Parse and generate code wrapped with xml information, so it can be easily found and replaced 
    /// </summary>
    /// <remarks>
    /// Factory for writer and tag
    /// OptionAttribute cannot be more specific than System.Attribute, so the library does not rely on specific GeneratorOptionAttribute library
    /// GeneratorOptionAttribute library has to be code free, so it will be as small as possible, 
    /// as it will need to be deployed along with the application using template that refers to RgenLib
    /// ></remarks>
    public partial class Manager<TRenderer, TOptionAttr>
        where TRenderer : TaggedCodeRenderer, new()
        where TOptionAttr : Attribute, new() {
       
        public Manager(TagFormat tagFormat)
        {
            _tagFormat = tagFormat;
            _propertyToXml = XmlAttributeAttribute.GetPropertyToXmlAttributeTranslation(typeof(TOptionAttr));

        }
        private readonly TagFormat _tagFormat;

        public TagFormat TagFormat {
            get { return _tagFormat; }
        }

        private readonly Dictionary<string, string> _propertyToXml;

     
        public Writer CreateWriter() {
            return new Writer(this);
        }
        public Writer CreateWriter(CodeClass2 cc) {
            return new Writer(this, cc);
        }
        public Writer CreateWriter(CodeClass2 cc, CodeClass2 triggeringBase) {
            return new Writer(this, cc, triggeringBase);
        }


    

        public void Remove(Writer info) {
            var taggedRanges = GeneratedSegment.FindSegments(info);
            foreach (var t in taggedRanges) {
                t.Range.DeleteText();
            }

        }


        public TypeCache OptionAttributeTypeCache {
            get { return TypeResolver.ByType(typeof(TOptionAttr)); }
        }


    }
}