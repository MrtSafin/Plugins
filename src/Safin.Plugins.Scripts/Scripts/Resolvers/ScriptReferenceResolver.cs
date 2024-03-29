using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts.Resolvers
{
    internal sealed class ScriptReferenceResolver(MetadataReferenceResolver referenceResolver, Func<string, string> resolver) : MetadataReferenceResolver
    {
        private readonly MetadataReferenceResolver _referenceResolver = referenceResolver;
        private readonly Func<string, string> _resolver = resolver;
        public override bool Equals(object? other) => other == this;
        public override int GetHashCode() => 0;
        public override bool ResolveMissingAssemblies => _referenceResolver.ResolveMissingAssemblies;
        public override PortableExecutableReference? ResolveMissingAssembly(MetadataReference definition, AssemblyIdentity referenceIdentity)
        {
            return _referenceResolver.ResolveMissingAssembly(definition, referenceIdentity);
        }
        public override ImmutableArray<PortableExecutableReference> ResolveReference(string reference, string? baseFilePath, MetadataReferenceProperties properties)
        {
            // Note: currently not handling relative paths, since we don't have tests that use them

            var fileName = _resolver(reference);

            if (File.Exists(fileName))
            {
                return [MetadataReference.CreateFromFile(fileName, properties)];
            }

            return _referenceResolver.ResolveReference(reference, baseFilePath, properties);
        }
    }
}
