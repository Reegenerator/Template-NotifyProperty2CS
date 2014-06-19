using EnvDTE80;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RgenLib.TaggedSegment {
    
    /// <summary>
    /// A class holding information of the cause of code generation.
    /// </summary>
    /// <remarks>
    /// created as a class, so it can be easily de/serialized as json
    /// </remarks>
    public class TriggerInfo {
        public TriggerInfo() { }
        public TriggerInfo(TriggerTypes type)
        {
            Type = type;
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public TriggerTypes Type { get; set; }

        [JsonConverter(typeof(CodeClassJsonConverter))]
        public CodeClass2 TriggeringBaseClass {get;set;}

    }
}
