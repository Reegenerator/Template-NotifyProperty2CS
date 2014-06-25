
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RgenLib.TaggedSegment;

namespace RgenLib
{
    /// <summary>
    /// Mark which properties of GeneratorOptionAttribute are to be written in the Tag by <see cref="Manager{TRenderer,TOptionAttr}.Writer"/> 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
	public class XmlAttributeAttribute : Attribute
	{

        /// <summary>
        /// stores dictionary of attribute name => property info
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _xmlNameToProperty_ByType;
        /// <summary>
        /// stores dictionary of property name => xml attribute name
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string, string>> _propertyToXml_ByType;

        static XmlAttributeAttribute()
        {
            _xmlNameToProperty_ByType = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
            _propertyToXml_ByType = new Dictionary<Type, Dictionary<string, string>>();
        }

      
        public static Dictionary<string, PropertyInfo> GetXmlProperties(Type type)
        {
            Dictionary<string, PropertyInfo> dict;
            //if not found, initialize dictionary
            if (!_xmlNameToProperty_ByType.TryGetValue(type, out dict))
            {
                dict = GenXmlNameToPropertyDict(type);
                _xmlNameToProperty_ByType.Add(type, dict);
            }
            return dict;
        }

        /// <summary>
        /// Create dictionary of Key:XmlAttribute specified Name, Value: PropertyInfo
        /// Properties without attributes are skipped
        /// Properties with IsFlattened will have it's own properties expanded <see cref="IsFlattened"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefix"></param>
        /// <param name="existingDict"></param>
        /// <returns></returns>
        private static Dictionary<string, PropertyInfo> GenXmlNameToPropertyDict(Type type, string prefix = "" , Dictionary<string, PropertyInfo> existingDict = null)
        {
            var typeCache = TypeResolver.ByType(type);
            var dict =existingDict?? new Dictionary<string, PropertyInfo>();
            var members = typeCache.GetProperties().ToArray();
            //add all properties with custom attribute
            foreach (var m in members)
            {
                var xmlAttr = m.GetCustomAttribute<XmlAttributeAttribute>();
                // skip if has no attribute
                if (xmlAttr == null) continue;

                if (xmlAttr.IsFlattened)
                {
                    //recursively process sub properties 
                    GenXmlNameToPropertyDict(m.PropertyType, xmlAttr.Name, dict);
                }
                dict.Add(prefix + "_" + xmlAttr.Name, (PropertyInfo) typeCache[m.Name]);
            }
            return dict;
        }

        public static Dictionary<string, string> GetPropertyToXmlAttributeTranslation(Type type) {
            Dictionary<string, string> propertyToXmlDict;
            //if not found, initialize dictionary
            if (!_propertyToXml_ByType.TryGetValue(type, out propertyToXmlDict)) {
                var typeCache = TypeResolver.ByType(type);
                propertyToXmlDict = new Dictionary<string, string>();
                var members = typeCache.GetProperties().ToArray();
                //add all properties 
                foreach (var m in members)
                {
                    var xmlAttr = m.GetCustomAttribute<XmlAttributeAttribute>();
                    //if there's no xmlAttribute, add the pair with same key and value
                    //so it will be safe to always get the property name
                    propertyToXmlDict.Add(m.Name, xmlAttr == null ? m.Name : xmlAttr.Name);
                }
                _propertyToXml_ByType.Add(type, propertyToXmlDict);
            }
            return propertyToXmlDict;
        }
      
        /// <summary>
        /// Specifies an alternate (usually shorter) name of property name to be written as xml attribute
        /// </summary>
		public string Name {get; set;}

        /// <summary>
        /// "True" means member marked with this attribute should have its properties expanded in this format Name_PropertyName
        /// Used to deserialize sub type a series of prefixed attributes instead of inner xelement
        /// Used for <see cref="TriggerInfo"/>
        /// </summary>
        public bool IsFlattened { get; set; }

        public XmlAttributeAttribute(string attrName)
		{
			Name = attrName;
		}
	}

}