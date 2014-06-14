using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes
{
    /// <summary>
    /// BaseClass for Generator Attributes
    /// </summary>
    /// <remarks>
    /// Created in separate Assembly so the render target project does not need to include the Reegenerator library code.
    /// </remarks>
    public class GeneratorOptionAttribute : Attribute
    {

        public Version Version { get; set; }

    }
}
