using System.Text;

namespace RgenLib.Extensions {
    public static class CodeRenderer {

        public static string RenderToString(this Kodeo.Reegenerator.Generators.CodeRenderer renderer) {
            return Encoding.ASCII.GetString(renderer.Render().GeneratedCode);
        }

    }
}
