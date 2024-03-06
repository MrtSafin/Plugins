using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

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
            Script? script = null;
            await _store.LoadAsync(name, stream =>
            {
                if (script == null)
                {
                    script = CSharpScript.Create(stream, options, globalsType: _globalValuesType);
                }
                else
                {
                    script = script.ContinueWith(stream, options);
                }
                return Task.CompletedTask;
            }, code =>
            {
                if (script == null)
                {
                    script = CSharpScript.Create(code, options, globalsType: _globalValuesType);
                }
                else
                {
                    script = script.ContinueWith(code, options);
                }
                return Task.CompletedTask;
            });
            if (script == null)
                throw new ScriptNotFoundException($"Скрипт {name} не найден");

            return script;
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
            Script<TResult>? script = null;
            await _store.LoadAsync(name, stream =>
            {
                if (script == null)
                {
                    script = CSharpScript.Create<TResult>(stream, options, globalsType: _globalValuesType);
                }
                else
                {
                    script = script.ContinueWith<TResult>(stream, options);
                }
                return Task.CompletedTask;
            }, code =>
            {
                if (script == null)
                {
                    script = CSharpScript.Create<TResult>(code, options, globalsType: _globalValuesType);
                }
                else
                {
                    script = script.ContinueWith<TResult>(code, options);
                }
                return Task.CompletedTask;
            });
            if (script == null)
                throw new ScriptNotFoundException($"Скрипт {name} не найден");

            return script;
        }
    }
}
