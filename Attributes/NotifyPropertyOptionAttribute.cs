using System;


namespace Attributes {
    /// <summary>
    /// Classes marked with this attribute will have it's NotifyPropertyChanged code generated automatically
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
    public class NotifyPropertyOptionAttribute : GeneratorOptionAttribute {
        public NotifyPropertyOptionAttribute() {
            //To regenerate all OnVersionChanged generated code, increment the version number
            Version = new Version(1, 1, 0, 17);
        }



        public enum GenerationTypes {
            Default,
            /// <summary>
            /// The generated code will set the property and notify the changes
            /// </summary>
            SetAndNotify =Default,
            /// <summary>
            /// The generated code will only notify the changes
            /// </summary>
            NotifyOnly
        }


        public GenerationTypes GenerationType { get; set; }


        /// <summary>
        /// A simple comma delimited string, since its in attribute we cannot use expression as parameters
        /// But it will be checked against the type during generation, if the property does not exists there will be a warning
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ExtraNotifications { get; set; }

    }
}
