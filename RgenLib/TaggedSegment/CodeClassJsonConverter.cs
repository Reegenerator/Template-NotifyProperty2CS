using System;
using EnvDTE80;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RgenLib.TaggedSegment {
    class CodeClassJsonConverter : CustomCreationConverter<CodeClass2>
    {
        public override CodeClass2 Create(Type objectType)
        {
            throw new NotImplementedException();
        }
   
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            //only write the name
            var cls = (CodeClass2)value;
            base.WriteJson(writer, cls.Name, serializer);
        }
    }
}
