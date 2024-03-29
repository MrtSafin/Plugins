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
    internal class ScriptReferenceResolver(MetadataReferenceResolver referenceResolver, Func<string, string> resolver) : MetadataReferenceResolver
    {
        private static readonly MetadataReferenceProperties s_resolvedMissingAssemblyReferenceProperties = MetadataReferenceProperties.Assembly.WithAliases([]);
        private readonly MetadataReferenceResolver _referenceResolver = referenceResolver;
        private readonly Func<string, string> _resolver = resolver;
        public override bool Equals(object? other) => other == this;
        public override int GetHashCode() => 0;
        public override bool ResolveMissingAssemblies => _referenceResolver.ResolveMissingAssemblies;
        public override PortableExecutableReference? ResolveMissingAssembly(MetadataReference definition, AssemblyIdentity referenceIdentity)
        {
            return _referenceResolver.ResolveMissingAssembly(definition, referenceIdentity);
        }
        //public override PortableExecutableReference? ResolveMissingAssembly(MetadataReference definition, AssemblyIdentity referenceIdentity)
        //{
        //    // resolve assemblies from the directory containing the test and from directory containing corlib

        //    string name = referenceIdentity.Name;
        //    string testDir = Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.ManifestModule.FullyQualifiedName)!;
        //    string testDependencyAssemblyPath = Path.Combine(testDir, name + ".dll");
        //    if (File.Exists(testDependencyAssemblyPath))
        //    {
        //        return MetadataReference.CreateFromFile(testDependencyAssemblyPath, s_resolvedMissingAssemblyReferenceProperties);
        //    }

        //    string fxDir = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.ManifestModule.FullyQualifiedName)!;
        //    string fxAssemblyPath = Path.Combine(fxDir, name + ".dll");
        //    if (File.Exists(fxAssemblyPath))
        //    {
        //        return MetadataReference.CreateFromFile(fxAssemblyPath, s_resolvedMissingAssemblyReferenceProperties);
        //    }

        //    return null;
        //}
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
