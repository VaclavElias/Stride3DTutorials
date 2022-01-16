using System.Reflection;
using System.Text.RegularExpressions;

namespace PerformanceTests;

public class YamlTest
{
    public TextReader YamlFile(string name)
    {
        //var test = GetType().GetTypeInfo().Assembly.GetManifestResourceNames();
        //var qualifiedName = typeof(YamlTest).AssemblyQualifiedName;
        //var elementType = Type.GetType(qualifiedName);

        var fromType = typeof(YamlTest);
        var assembly = Assembly.GetAssembly(fromType);
        var stream = assembly.GetManifestResourceStream(name) ??
                     assembly.GetManifestResourceStream(fromType.Namespace + ".files." + name);
        return new StreamReader(stream);
    }

    public TextReader YamlText(string yaml)
    {
        var lines = yaml
            .Split('\n')
            .Select(l => l.TrimEnd('\r', '\n'))
            .SkipWhile(l => l.Trim(' ', '\t').Length == 0)
            .ToList();

        while (lines.Count > 0 && lines[lines.Count - 1].Trim(' ', '\t').Length == 0)
        {
            lines.RemoveAt(lines.Count - 1);
        }

        if (lines.Count > 0)
        {
            var indent = Regex.Match(lines[0], @"^(\t+)");
            if (!indent.Success)
            {
                throw new ArgumentException("Invalid indentation");
            }

            lines = lines
                .Select(l => l.Substring(indent.Groups[1].Length))
                .ToList();
        }

        return new StringReader(string.Join("\n", lines.ToArray()));
    }
}