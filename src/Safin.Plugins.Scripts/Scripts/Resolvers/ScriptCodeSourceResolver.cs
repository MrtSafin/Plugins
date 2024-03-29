using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts.Resolvers
{
    internal sealed class ScriptCodeSourceResolver(Func<string, string> resolver) : SourceReferenceResolver
    {
        public override string NormalizePath(string path, string? baseFilePath)
        {
            return path;
        }

        public override string ResolveReference(string path, string? baseFilePath)
        {
            return path;
        }

        public override Stream OpenRead(string resolvedPath)
        {
            var code = resolver(resolvedPath);
            var bytes = Encoding.UTF8.GetBytes(code);
            return new MemoryStream(bytes);
        }
        public override bool Equals(object? other) => other == this;
        public override int GetHashCode() => 0;
    }
}
