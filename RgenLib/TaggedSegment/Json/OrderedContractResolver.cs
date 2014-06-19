﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RgenLib.Extensions;

namespace RgenLib.TaggedSegment.Json {

    /// <summary>
    /// Sort Property Generation based on orderFunc parameter
    /// Skips enum property with default values
    /// </summary>
    public class OrderedContractResolver : DefaultContractResolver  {
        public OrderedContractResolver(Func<JsonProperty, Object>orderFunc)
        {
            OrderFunc = orderFunc;
        }

        public Func<JsonProperty, object> OrderFunc { get; set; }

        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization) {
            //Template property has to be the first one
            var res= base.CreateProperties(type, memberSerialization).OrderBy(OrderFunc).ToList();
            return res;
        }

        /// <summary>
        /// overriden to skip property enum that's default (=0)
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization) {
            var prop= base.CreateProperty(member, memberSerialization);
            if (prop.PropertyType.IsEnum)
            {
                
                prop.ShouldSerialize = p =>
                {
                    var propValue = ((PropertyInfo) member).GetValue(p);

                    return (int)propValue != 0;
                };
            }
            return prop;
        }
    }
}

