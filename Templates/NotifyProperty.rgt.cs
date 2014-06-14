using EnvDTE80;
using Kodeo.Reegenerator.Wrappers;
using RgenLib.Extensions;

namespace Templates {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Kodeo.Reegenerator;

    public partial class NotifyProperty {
        /// <summary>
        /// Method that gets called prior to calling <see cref="Render"/>.
        /// Use this method to initialize the properties to be used by the render process.
        /// You can access the project item attached to this generator by using the <see cref="ProjectItem"/> property.
        /// </summary>
        public override void PreRender() {
            base.PreRender();
            // var projectItem = base.ProjectItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Array of invalid properties</returns>
        /// <remarks></remarks>
        public string[] ValidateExtraNotifications(NotifyPropertyOptionAttribute optionAttr ,CodeClass2 cc, string[] extras) {

            var propNames = new HashSet<string>(cc.GetProperties().Select((x) => x.Name));
            var invalids = extras.Where((x) => !(propNames.Contains(x))).ToArray();
            if (invalids.Any()) {
                return invalids;
            }

            ExtraNotifications = string.Join(", ", extras);
            return new string[0]; //return empty array
        }

        public override bool AreArgumentsEquals(GeneratorAttribute other) {
            var otherNPC = (NotifyPropertyChanged_GenAttribute)other;
            return base.AreArgumentsEquals(other) && this.ExtraNotifications == otherNPC.ExtraNotifications;
        }
    }
}