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
    public class CSXScript(ICSXScriptStore store, Type? globalValuesType)
    {
        protected readonly ICSXScriptStore _store = store;
        protected readonly Type? _globalValuesType = globalValuesType;
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
        protected virtual ScriptOptions BuildOptions() => ScriptOptions.Default
                        .AddImports("System", "System.IO", "System.Collections.Generic",
                            "System.Console", "System.Diagnostics", "System.Dynamic",
                            "System.Linq", "System.Text",
                            "System.Threading.Tasks")
                        .AddReferences("System", "System.Core"); //.AddReferences(currentAssembly)
        // var script = CSharpScript.Create("", options, globalsType: _globalValuesType);
        // script = script.ContinueWith("", options);
        protected virtual async Task<Script<object>> CreateScript(string name, ScriptOptions options)
        {
            Script<object>? script = null;
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
            });
            if (script == null)
                throw new ScriptNotFoundException($"Скрипт {name} не найден");

            return script;
        }
    }
}
