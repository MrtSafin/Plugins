using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Text;

using Safin.Plugins.Helpers;
using Safin.Plugins.Stores;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts
{
    public class CSXScript(ICSXScriptStore store, ICSXBuilderOptions? builderOptions = null, Type? globalValuesType = null)
    {
        protected readonly ICSXScriptStore _store = store;
        protected readonly Type? _globalValuesType = globalValuesType;
        protected readonly ICSXBuilderOptions? _builderOptions = builderOptions;

        public CSXScript(ICSXScriptStore store): this(store, null, null) { }
        public CSXScript(ICSXScriptStore store, ICSXBuilderOptions builderOptions): this(store, builderOptions: builderOptions, globalValuesType: null) { }
        public CSXScript(ICSXScriptStore store, Type globalValuesType) : this(store, builderOptions: null, globalValuesType: globalValuesType) { }

        public async Task<object> Execute(string name, object? globalValues)
        {
            var options = BuildOptions();
            var script = await CreateScript(name, options);
            
            var compileResult = script.Compile();
            if (compileResult.Length == 0)
            {
                var runResult = await script.RunAsync(globals: globalValues);
                return runResult.ReturnValue;
            } else
            {
                throw new CompilationErrorException(
                    compileResult.Aggregate((string?)null,
                        (result, value) => string.IsNullOrEmpty(result) ? value.GetMessage() : result + Environment.NewLine + value.GetMessage()),
                    compileResult);
            }
        }
        protected virtual ScriptOptions BuildOptions()
        {
            var result = ScriptOptions.Default
                        .AddImports("System", "System.IO", "System.Collections.Generic",
                            "System.Console", "System.Diagnostics", "System.Dynamic",
                            "System.Linq", "System.Text",
                            "System.Threading.Tasks")
                        .AddReferences("System", "System.Core"); //.AddReferences(currentAssembly)
            result = _builderOptions?.BuildOptions(result) ?? result;
            
            return result;
        }
        // var script = CSharpScript.Create("", options, globalsType: _globalValuesType);
        // script = script.ContinueWith("", options);
        protected virtual async Task<Script> CreateScript(string name, ScriptOptions options)
        {
            CSXScriptLoader loader = new(_globalValuesType, options);
            await _store.LoadAsync(name, loader);
            if (loader.Script == null)
                throw new ScriptNotFoundException($"Скрипт {name} не найден");

            return loader.Script;
        }
    }
    public class CSXScript<GlobalValuesType>(ICSXScriptStore store, ICSXBuilderOptions? builderOptions = null) : CSXScript(store, builderOptions, typeof(GlobalValuesType))
        where GlobalValuesType: class
    {
        public CSXScript(ICSXScriptStore store) : this(store, null) { }
        public async Task<TResult> Execute<TResult>(string name, GlobalValuesType globalValues)
        {
            var options = BuildOptions();
            var script = await CreateScript<TResult>(name, options);

            var compileResult = script.Compile();
            if (compileResult.Length == 0)
            {
                var runResult = await script.RunAsync(globals: globalValues);
                return runResult.ReturnValue;
            }
            else
            {
                throw new CompilationErrorException(
                    compileResult.Aggregate((string?)null,
                        (result, value) => string.IsNullOrEmpty(result) ? value.GetMessage() : result + Environment.NewLine + value.GetMessage()),
                    compileResult);
            }
        }
        protected virtual async Task<Script<TResult>> CreateScript<TResult>(string name, ScriptOptions options)
        {
            CSXScriptLoader<TResult> loader = new(_globalValuesType, options);
            await _store.LoadAsync(name, loader);
            if (loader.Script == null)
                throw new ScriptNotFoundException($"Скрипт {name} не найден");

            return loader.Script;
        }
    }
    public class CSXScriptLoader<TResult>(Type? globalValuesType, ScriptOptions options) : ICSXScriptBuilder
    {
        public Script<TResult>? Script { get; private set; } = null;
        public Task LoadFromStreamAsync(Stream stream, Func<string, Stream> resolver)
        {
            var resolverOptions = options.WithSourceResolver(new ScriptStreamSourceResolver(resolver));
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

        public Task LoadFromStringAsync(string code, Func<string, string> resolver)
        {
            var resolverOptions = options.WithSourceResolver(new ScriptCodeSourceResolver(resolver));
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
        public Task LoadFromFileAsync(string fileName, Func<string, string> resolver)
        {
            var resolverOptions = options.WithSourceResolver(new ScriptFileSourceResolver(resolver));
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

        public Task LoadFromFileAsync(string fileName, Func<string, string> resolver)
        {
            var resolverOptions = options.WithSourceResolver(new ScriptFileSourceResolver(resolver));
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

        public Task LoadFromStreamAsync(Stream stream, Func<string, Stream> resolver)
        {
            var resolverOptions = options.WithSourceResolver(new ScriptStreamSourceResolver(resolver));
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

        public Task LoadFromStringAsync(string code, Func<string, string> resolver)
        {
            var resolverOptions = options.WithSourceResolver(new ScriptCodeSourceResolver(resolver));
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
    internal class ScriptFileSourceResolver(Func<string, string> resolver) : SourceReferenceResolver
    {
        public override string NormalizePath(string path, string? baseFilePath)
        {
            return path;
        }

        public override string ResolveReference(string path, string? baseFilePath)
        {
            return resolver(path);
        }

        public override Stream OpenRead(string resolvedPath)
        {
            return new FileStream(resolvedPath, FileMode.Open, FileAccess.Read);
        }

        public override bool Equals(object? other)
        {
            return ReferenceEquals(this, this);
        }

        public override int GetHashCode()
        {
            return (ToString() ?? string.Empty).GetHashCode();
        }
    }
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

        public override bool Equals(object? other)
        {
            return ReferenceEquals(this, this);
        }

        public override int GetHashCode()
        {
            return (ToString() ?? string.Empty).GetHashCode();
        }
    }
    internal class ScriptCodeSourceResolver(Func<string, string> resolver) : SourceReferenceResolver
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

        public override bool Equals(object? other)
        {
            return ReferenceEquals(this, this);
        }

        public override int GetHashCode()
        {
            return (ToString() ?? string.Empty).GetHashCode();
        }
    }
}
