using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Safin.Plugins.Scripts.Resolvers;
using Safin.Plugins.Stores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts
{
    public class CSXScriptLoader<TResult>(Type? globalValuesType, ScriptOptions options) : ICSXScriptBuilder
    {
        public Script<TResult>? Script { get; private set; } = null;
        public Task LoadFromStreamAsync(Stream stream, Func<string, Stream> resolver, Func<string, string> referenceResolver)
        {
            var resolverOptions = options
                .WithSourceResolver(new ScriptStreamSourceResolver(resolver))
                .WithMetadataResolver(new ScriptReferenceResolver(options.MetadataResolver, referenceResolver))
                ;
            if (Script == null)
            {
                Script = CSharpScript.Create<TResult>(stream, resolverOptions, globalsType: globalValuesType);
            }
            else
            {
                Script = Script.ContinueWith<TResult>(stream, resolverOptions);
            }
            return Task.CompletedTask;
        }

        public Task LoadFromStringAsync(string code, Func<string, string> resolver, Func<string, string> referenceResolver)
        {
            var resolverOptions = options
                .WithSourceResolver(new ScriptCodeSourceResolver(resolver))
                .WithMetadataResolver(new ScriptReferenceResolver(options.MetadataResolver, referenceResolver))
                ;
            if (Script == null)
            {
                Script = CSharpScript.Create<TResult>(code, resolverOptions, globalsType: globalValuesType);
            }
            else
            {
                Script = Script.ContinueWith<TResult>(code, resolverOptions);
            }
            return Task.CompletedTask;
        }
        public Task LoadFromFileAsync(string fileName, Func<string, string> resolver, Func<string, string> referenceResolver)
        {
            var resolverOptions = options
                .WithSourceResolver(new ScriptFileSourceResolver(resolver))
                .WithMetadataResolver(new ScriptReferenceResolver(options.MetadataResolver, referenceResolver))
                ;
            using Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            if (Script == null)
            {
                Script = CSharpScript.Create<TResult>(stream, resolverOptions, globalsType: globalValuesType);
            }
            else
            {
                Script = Script.ContinueWith<TResult>(stream, resolverOptions);
            }
            return Task.CompletedTask;
        }
    }
    public class CSXScriptLoader(Type? globalValuesType, ScriptOptions options) : ICSXScriptBuilder
    {
        public Script? Script { get; private set; } = null;

        public Task LoadFromFileAsync(string fileName, Func<string, string> resolver, Func<string, string> referenceResolver)
        {
            var resolverOptions = options
                .WithSourceResolver(new ScriptFileSourceResolver(resolver))
                .WithMetadataResolver(new ScriptReferenceResolver(options.MetadataResolver, referenceResolver))
                ;
            using Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            if (Script == null)
            {
                Script = CSharpScript.Create(stream, resolverOptions, globalsType: globalValuesType);
            }
            else
            {
                Script = Script.ContinueWith(stream, resolverOptions);
            }
            return Task.CompletedTask;
        }

        public Task LoadFromStreamAsync(Stream stream, Func<string, Stream> resolver, Func<string, string> referenceResolver)
        {
            var resolverOptions = options
                .WithSourceResolver(new ScriptStreamSourceResolver(resolver))
                .WithMetadataResolver(new ScriptReferenceResolver(options.MetadataResolver, referenceResolver))
                ;
            if (Script == null)
            {
                Script = CSharpScript.Create(stream, resolverOptions, globalsType: globalValuesType);
            }
            else
            {
                Script = Script.ContinueWith(stream, resolverOptions);
            }
            return Task.CompletedTask;
        }

        public Task LoadFromStringAsync(string code, Func<string, string> resolver, Func<string, string> referenceResolver)
        {
            var resolverOptions = options
                .WithSourceResolver(new ScriptCodeSourceResolver(resolver))
                .WithMetadataResolver(new ScriptReferenceResolver(options.MetadataResolver, referenceResolver))
                ;
            if (Script == null)
            {
                Script = CSharpScript.Create(code, resolverOptions, globalsType: globalValuesType);
            }
            else
            {
                Script = Script.ContinueWith(code, resolverOptions);
            }
            return Task.CompletedTask;
        }
    }
}
