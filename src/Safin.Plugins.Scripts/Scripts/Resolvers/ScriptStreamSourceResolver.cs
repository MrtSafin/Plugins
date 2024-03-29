using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts.Resolvers
{
    internal class ScriptStreamSourceResolver(Func<string, Stream> resolver) : SourceReferenceResolver
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
            return resolver(resolvedPath);
        }
        public override bool Equals(object? other) => other == this;
        public override int GetHashCode() => 0;
    }
}
