using Safin.Plugins.Commands;
using Safin.Plugins.Modules;
using Safin.Plugins.Stores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts
{
    public class CSXScriptPlugin(ICSXScriptStore store, CSXScriptPluginOptions options): IPlugin
    {
        private readonly ICSXScriptStore _store = store;
        private readonly CSXScriptPluginOptions _options = options;
        private readonly Dictionary<string, CSXScriptDetails> _commands = new();
        private bool _isLoaded = false;
        public bool IsLoaded => _isLoaded;

        public Task<ICommand> CreateCommandAsync(string commandName)
        {
            return Task.FromResult<ICommand>(new CSXScriptCommand(_commands[commandName]));
        }

        public Task LoadAsync()
        {
            if (_isLoaded)
            {
                throw new PluginLoadException($"Скрипт \"{_options.Name}\" уже загружен.");
            }

            if (_options.Commands != null)
            {
                foreach (var command in _options.Commands)
                {
                    if (command.CommandName != null && command.FileName != null)
                    {
                        var details = new CSXScriptDetails(command.FileName, new CSXScript(_store, globalValuesType: typeof(CSXScriptPluginParams)));
                        _commands.Add(command.CommandName, details);
                    }
                }
            }
            _isLoaded = true;
            return Task.CompletedTask;
        }
        public void Unload()
        {
            _isLoaded = false;
        }
        // Класс содержащий информацию по команде и скрипту
    }
    internal class CSXScriptCommand(CSXScriptDetails details) : ICommand
    {
        private readonly CSXScriptDetails _details = details;
        private readonly Dictionary<string, object> _parameters = new();
        private object? _result = null;

        public object? Result => _result;

        public Task ExecuteAsync()
        {
            return _details.Script.Execute(_details.FileName, new CSXScriptPluginParams(_parameters))
                .ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        _result = t.Result;
                    }
                });
        }

        public void SetProperty(string paramName, object value)
        {
            _parameters.Add(paramName, value);
        }
    }
    internal record CSXScriptDetails(string FileName, CSXScript Script);
    public record CSXScriptPluginParams(IDictionary<string, object> Values);
}
