using System.Collections.Generic;
using Attributes;
using EnvDTE80;

namespace RgenLib.TaggedSegment {
    /// <summary>
    /// Parse and generate code wrapped with xml information, so it can be easily found and replaced 
    /// </summary>
    /// <remarks></remarks>
    public partial class Manager<TRenderer> where TRenderer : TaggedCodeRenderer, new() {
       
        public Manager(TRenderer renderer, TagFormat tagFormat)
        {
            _tagFormat = tagFormat;
            _renderer = renderer;
            _propertyToXml = XmlAttributeAttribute.GetPropertyToXmlAttributeTranslation(_renderer.OptionAttributeType);

        }
        private readonly TagFormat _tagFormat;

        public TagFormat TagFormat {
            get { return _tagFormat; }
        }

        private readonly Dictionary<string, string> _propertyToXml;
        private readonly TRenderer _renderer;

        public TRenderer Renderer {
            get { return _renderer; }
        }

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
            get { return TypeResolver.ByType(_renderer.OptionAttributeType); }
        }


    }
}