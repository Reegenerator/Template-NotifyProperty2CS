using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgenLib.Extensions {
    public static class CodeRenderer {

        public static string RenderToString(this Kodeo.Reegenerator.Generators.CodeRenderer renderer) {
            return System.Text.Encoding.ASCII.GetString(renderer.Render().GeneratedCode);
        }

    }
}
