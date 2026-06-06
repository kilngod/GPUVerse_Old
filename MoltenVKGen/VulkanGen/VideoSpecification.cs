using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VulkanGen
{
#nullable disable
    public sealed class VideoSpecification
    {
        public List<VideoConstantDefinition> Constants { get; } = new();
        public List<VideoEnumDefinition> Enums { get; } = new();
        public List<VideoStructDefinition> Structs { get; } = new();

        public IReadOnlyDictionary<string, VideoConstantDefinition> ConstantLookup =>
            Constants.ToDictionary(c => c.Name, c => c);

        public static VideoSpecification FromFile(string xmlFile)
        {
            var doc = XDocument.Load(xmlFile);
            var registry = doc.Element("registry") ??
                throw new InvalidOperationException("video.xml is missing registry root.");

            var spec = new VideoSpecification();

            foreach (var define in registry.Element("types")?.Elements("type")
                         .Where(t => t.Attribute("category")?.Value == "define") ?? [])
            {
                var name = define.Element("name")?.Value;
                if (string.IsNullOrEmpty(name) || name == "VK_MAKE_VIDEO_STD_VERSION")
                    continue;

                var versionMatch = Regex.Match(
                    define.Value,
                    @"VK_MAKE_VIDEO_STD_VERSION\s*\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)");
                if (!versionMatch.Success)
                    continue;

                uint major = uint.Parse(versionMatch.Groups[1].Value);
                uint minor = uint.Parse(versionMatch.Groups[2].Value);
                uint patch = uint.Parse(versionMatch.Groups[3].Value);
                uint version = ((major & 0x3FFu) << 22) | ((minor & 0x3FFu) << 12) | (patch & 0xFFFu);

                spec.Constants.Add(new VideoConstantDefinition
                {
                    Name = name,
                    Value = version.ToString(),
                    Type = "uint32_t",
                });
            }

            foreach (var e in registry.Elements("enums")
                         .Where(e => e.Attribute("type")?.Value == "enum" ||
                                     e.Attribute("type")?.Value == "bitmask"))
            {
                spec.Enums.Add(VideoEnumDefinition.FromXML(e));
            }

            foreach (var s in registry.Element("types")?.Elements("type")
                         .Where(t => t.Attribute("category")?.Value == "struct") ?? [])
            {
                spec.Structs.Add(VideoStructDefinition.FromXML(s));
            }

            foreach (var extension in registry.Element("extensions")?.Elements("extension") ?? [])
            {
                if (extension.Attribute("supported")?.Value == "disabled")
                    continue;

                foreach (var e in extension.Elements("require").Elements("enum"))
                {
                    if (e.Attribute("extends") is not null)
                        continue;

                    var name = e.Attribute("name")?.Value;
                    var value = e.Attribute("value")?.Value;
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                        continue;

                    if (!spec.Constants.Any(c => c.Name == name))
                    {
                        spec.Constants.Add(new VideoConstantDefinition
                        {
                            Name = name,
                            Value = value.Replace("&quot;", "\""),
                            Type = e.Attribute("type")?.Value ?? InferConstantType(value),
                        });
                    }
                }
            }

            return spec;
        }

        private static string InferConstantType(string value)
        {
            if (value.StartsWith("\"", StringComparison.Ordinal))
                return "string";
            return "uint32_t";
        }
    }

    public sealed class VideoConstantDefinition
    {
        public string Name;
        public string Value;
        public string Type;
    }

    public sealed class VideoEnumDefinition
    {
        public string Name;
        public List<VideoEnumValue> Values { get; } = new();

        public static VideoEnumDefinition FromXML(XElement elem)
        {
            var result = new VideoEnumDefinition
            {
                Name = elem.Attribute("name")?.Value ??
                    throw new InvalidOperationException("Video enum is missing a name."),
            };

            foreach (var v in elem.Elements("enum"))
            {
                var name = v.Attribute("name")?.Value;
                var value = v.Attribute("value")?.Value;
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                    continue;

                result.Values.Add(new VideoEnumValue { Name = name, Value = value });
            }

            return result;
        }
    }

    public sealed class VideoEnumValue
    {
        public string Name;
        public string Value;
    }

    public sealed class VideoStructDefinition
    {
        public string Name;
        public List<VideoMemberDefinition> Members { get; } = new();
        public bool IsBitfieldStruct => Members.Count > 0 && Members.All(m => m.BitWidth.HasValue);

        public static VideoStructDefinition FromXML(XElement elem)
        {
            var result = new VideoStructDefinition
            {
                Name = elem.Attribute("name")?.Value ??
                    throw new InvalidOperationException("Video struct is missing a name."),
            };

            foreach (var member in elem.Elements("member"))
                result.Members.Add(VideoMemberDefinition.FromXML(member));

            return result;
        }
    }

    public sealed class VideoMemberDefinition
    {
        public string Name;
        public string Type;
        public int PointerLevel;
        public List<string> ArrayLengths { get; } = new();
        public int? BitWidth;

        public static VideoMemberDefinition FromXML(XElement elem)
        {
            var result = new VideoMemberDefinition
            {
                Name = elem.Element("name")?.Value ??
                    throw new InvalidOperationException("Video struct member is missing a name."),
                Type = elem.Element("type")?.Value ??
                    throw new InvalidOperationException("Video struct member is missing a type."),
            };

            string declaration = GetDeclaration(elem);
            int nameIndex = declaration.IndexOf(result.Name, StringComparison.Ordinal);
            if (nameIndex >= 0)
                result.PointerLevel = declaration[..nameIndex].Count(c => c == '*');

            string declarator = GetDeclaratorSuffix(elem);
            foreach (Match match in Regex.Matches(declarator, @"\[(?:<enum>)?(?<value>[A-Za-z0-9_]+)(?:</enum>)?\]"))
                result.ArrayLengths.Add(match.Groups["value"].Value);

            var bitMatch = Regex.Match(elem.Value, @":\s*(\d+)");
            if (bitMatch.Success)
                result.BitWidth = int.Parse(bitMatch.Groups[1].Value);

            return result;
        }

        private static string GetDeclaration(XElement elem)
        {
            var result = new StringBuilder();
            foreach (var node in elem.Nodes())
            {
                if (node is XElement child)
                {
                    if (child.Name.LocalName == "comment")
                        break;

                    result.Append(child.Value);
                }
                else
                {
                    result.Append(node.ToString());
                }
            }

            return result.ToString();
        }
        private static string GetDeclaratorSuffix(XElement elem)
        {
            var result = new StringBuilder();
            bool afterName = false;

            foreach (var node in elem.Nodes())
            {
                if (node is XElement child)
                {
                    string localName = child.Name.LocalName;
                    if (localName == "name")
                    {
                        afterName = true;
                        continue;
                    }

                    if (!afterName)
                        continue;

                    if (localName == "comment")
                        break;

                    if (localName == "enum")
                    {
                        result.Append("<enum>");
                        result.Append(child.Value);
                        result.Append("</enum>");
                    }
                    else
                    {
                        result.Append(child.Value);
                    }
                }
                else if (afterName)
                {
                    result.Append(node.ToString());
                }
            }

            return result.ToString();
        }
    }
}
