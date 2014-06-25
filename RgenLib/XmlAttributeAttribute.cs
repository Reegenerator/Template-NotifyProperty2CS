
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RgenLib.Extensions;
using RgenLib.TaggedSegment;

namespace RgenLib {
    /// <summary>
    /// Mark which properties of GeneratorOptionAttribute are to be written in the Tag by <see cref="Manager{TRenderer,TOptionAttr}.Writer"/> 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlAttributeAttribute : Attribute {
        public class NestedPropertyInfo {
            public NestedPropertyInfo ParentInfo{ get; set; }
            public XmlAttributeAttribute Attribute { get; set; }
            public PropertyInfo Info{ get; set; }

            private object GetParentValue(object rootObj)
            {
                if (rootObj == null) Debug.DebugHere();
                var obj = ParentInfo == null ? rootObj : ParentInfo.GetValue(rootObj);
                return obj;
            }
            public object GetValue(object rootObj) {
                if (rootObj == null) Debug.DebugHere();
                var parent = GetParentValue(rootObj);
                if (parent == null) return null;
                return Info.GetValue(parent, null);
            }
            public void SetValue(object rootObj, object value) {
                var obj = ParentInfo == null ? rootObj : ParentInfo.GetValue(rootObj);
                Info.SetValue(GetParentValue(rootObj), value);
            }
        }
        /// <summary>
        /// stores dictionary of attribute name => property info
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string, NestedPropertyInfo>> _xmlNameToProperty_ByType;
        /// <summary>
        /// stores dictionary of property name => xml attribute name
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string, string>> _propertyToXml_ByType;

        static XmlAttributeAttribute() {
            _xmlNameToProperty_ByType = new Dictionary<Type, Dictionary<string, NestedPropertyInfo>>();
            _propertyToXml_ByType = new Dictionary<Type, Dictionary<string, string>>();
        }


        public static Dictionary<string, NestedPropertyInfo> GetXmlProperties(Type type) {
            Dictionary<string, NestedPropertyInfo> dict;
            //if not found, initialize dictionary
            if (!_xmlNameToProperty_ByType.TryGetValue(type, out dict)) {
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
        /// <param name="existingDict"></param>
        /// <returns></returns>
        private static Dictionary<string, NestedPropertyInfo> GenXmlNameToPropertyDict(Type type,  Dictionary<string, NestedPropertyInfo> existingDict = null, NestedPropertyInfo parentInfo = null) {
            var typeCache = TypeResolver.ByType(type);
            var dict = existingDict ?? new Dictionary<string, NestedPropertyInfo>();
            var members = typeCache.GetProperties().ToArray();
            //add all properties with custom attribute
            foreach (var m in members) {
                var xmlAttr = m.GetCustomAttribute<XmlAttributeAttribute>();
                // skip if has no attribute
                if (xmlAttr == null) continue;
                var prop = new NestedPropertyInfo { ParentInfo = parentInfo, Info = (PropertyInfo)typeCache[m.Name], Attribute = xmlAttr };
                if (xmlAttr.IsFlattened)
                {
                    //recursively process sub properties 
                    GenXmlNameToPropertyDict(m.PropertyType, existingDict: dict, parentInfo: prop);
                }
                else {
                    
                    var completeName = (prop.ParentInfo== null ? "" : prop.ParentInfo.Attribute.Name + "_") + xmlAttr.Name;
                    dict.Add(completeName, prop);
                }

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
                foreach (var m in members) {
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
        /// Parse Attribute Argument into the actual string value
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="value"></param>
        /// <remarks>
        /// Attribute argument is presented exactly as it was typed
        /// Ex: SomeArg="Test" would result in the Argument.Value "Test" (with quote)
        /// Ex: SomeArg=("Test") would result in the Argument.Value ("Test") (with parentheses and quote)
        /// </remarks>
        public static object ParsePropertyValue(NestedPropertyInfo propInfo, string value) {

            object parsed;
            var propType = propInfo.Info.PropertyType;
            if (propType.IsEnum) {
                //if enum, remove the Enum qualifier (e.g TagTypes.InsertPoint becomes InserPoint)
                parsed = Enum.Parse(propType, value.StripQualifier());
            }
            else if (propType == typeof(DateTime) || propType == typeof(DateTime?)) {
                parsed = DateTime.ParseExact(value, Constants.TagDateFormat, Constants.TagDateCulture);
            }
            else if (propType == typeof(string)) {
                //remove quotes
                parsed = value.Trim('\"');
            }

            else {
                parsed = value;
            }
            return parsed;
        }


        /// <summary>
        /// Specifies an alternate (usually shorter) name of property name to be written as xml attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// "True" means member marked with this attribute should have its properties expanded in this format Name_PropertyName
        /// Used to deserialize sub type a series of prefixed attributes instead of inner xelement
        /// Used for <see cref="TriggerInfo"/>
        /// </summary>
        public bool IsFlattened { get; set; }

        public XmlAttributeAttribute(string attrName) {
            Name = attrName;
        }
    }

}