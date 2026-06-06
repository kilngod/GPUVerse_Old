using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VulkanGen
{
#nullable disable
    public class FuncpointerDefinition
    {
        public string Name;
        public string Type;
        public List<Parameter> Parameters = new List<Parameter>();

        public static FuncpointerDefinition FromXML(XElement elem)
        {
            var proto = elem.Element("proto");

            FuncpointerDefinition funcpointer = new FuncpointerDefinition();
            funcpointer.Name = proto?.Element("name")?.Value
                               ?? elem.Element("name")?.Value
                               ?? throw new InvalidDataException("Funcpointer is missing a name.");

            funcpointer.Type = GetCType(proto);

            foreach (var param in elem.Elements("param"))
                funcpointer.Parameters.Add(Parameter.FromXML(param));

            return funcpointer;
        }

        internal static string GetCType(XElement elem)
        {
            var raw = new StringBuilder();
            foreach (var node in elem.Nodes())
            {
                if (node is XElement { Name.LocalName: "name" })
                    break;

                raw.Append(node is XElement element ? element.Value : node.ToString());
            }

            return raw.ToString()
                .Replace("const ", string.Empty)
                .Replace("struct ", string.Empty)
                .Replace(" *", "*")
                .Replace("* ", "*")
                .Trim();
        }
    }

    public class Parameter
    {
        public string Type;
        public string Name;

        internal static Parameter FromXML(XElement elem) =>
            new Parameter
            {
                Name = elem.Element("name")?.Value
                       ?? throw new InvalidDataException("Funcpointer parameter is missing a name."),
                Type = FuncpointerDefinition.GetCType(elem),
            };
    }
}
