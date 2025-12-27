using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.CLI.Extensions
{
    public static class ProjectPathExtensions
    {
        public static readonly char DirectorySeparator = Path.DirectorySeparatorChar;

        public static string TrimProjectPath(this string path)
        {
            var normalizedPath = Path.GetFullPath(path);

            return normalizedPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static string GetCsProjFileName(this string path)
        {
            var normalizedPath = Path.GetFullPath(path);
            var directoryName = Path.GetFileName(normalizedPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            return $"{directoryName}.csproj";
        }

        public static string GetProjectDllFileName(this string projectPath, string csprojFileName)
        {
            var normalizedProjectPath = projectPath
                .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            var csprojFullPath = Path.GetFullPath(Path.Combine(normalizedProjectPath, csprojFileName));

            var doc = new XmlDocument
            {
                XmlResolver = null
            };

            using (var stream = new FileStream(csprojFullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                doc.Load(stream);
            }

            var elements = doc.GetElementsByTagName(nameof(System.Reflection.AssemblyName));
            var dllName = elements?.Cast<XmlNode>()?.FirstOrDefault()?.InnerText;

            return string.IsNullOrWhiteSpace(dllName)
                ? csprojFileName.Replace(".csproj", ".dll", StringComparison.OrdinalIgnoreCase)
                : $"{dllName}.dll";
        }

        public static string GetProjectCompiledPath(this string path, string configuration, string targetFramework)
        {
            return $"{path.TrimEnd(DirectorySeparator)}{DirectorySeparator}bin{DirectorySeparator}{configuration}{DirectorySeparator}{targetFramework}";
        }

        public static string GetProjectCompiledDllPath(this string compiledPath, string dllFileName)
        {
            return $"{compiledPath}{DirectorySeparator}{dllFileName}";
        }

        public static string GetProjectHostJsonPath(this string compiledPath)
        {
            return $"{compiledPath}{DirectorySeparator}host.json";
        }

        public static string GetOutputPath(this string output, string compiledPath)
        {
            return !Path.IsPathFullyQualified(output)
                ? $"{compiledPath}{DirectorySeparator}{output}"
                : output;
        }
    }
}
