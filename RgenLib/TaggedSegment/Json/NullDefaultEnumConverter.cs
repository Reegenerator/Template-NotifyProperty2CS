using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RgenLib.Extensions;

namespace RgenLib.TaggedSegment.Json {

    /// <summary>
    /// An enum converter that writes the value without quotes
    /// </summary>
    /// <remarks>
    /// This is a hacky workaround, by overriding StringEnumConverter and replace WriteValue with WriteRawValue
    /// </remarks>
    class NullDefaultEnumConverter : StringEnumConverter {


        /// <summary>
        /// If the enum value is Default, do not write the json
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            if ((int)value == 0) return;
            base.WriteJson(writer, value, serializer);

        }

    }
}
