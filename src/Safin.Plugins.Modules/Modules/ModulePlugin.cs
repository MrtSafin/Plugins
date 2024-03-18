using Safin.Plugins.Commands;
using Safin.Plugins.Exceptions;
using Safin.Plugins.Stores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public class ModulePlugin(IAssemblyModuleStore store, ModuleProviderOptions options): IPlugin
    {
        private readonly IAssemblyModuleStore _store = store;
        private readonly ModuleProviderOptions _options = options;
        private AssemblyModuleBase? _module = null;
        private IAssemblyCommands? _moduleCommands = null;
        
        public bool IsLoaded => _module != null && _module.IsLoaded;

        public Task LoadAsync()
        {
            if (_module != null)
            {
                throw new PluginLoadException($"Модуль \"{_options.Name}\" уже загружен.");
            }

            _moduleCommands = _module switch
            {
                AssemblyModule assemblyModule => new ModuleCommands(assemblyModule.Assembly!),
                AssemblyModuleUnloadable moduleUnloadable => new ModuleCommandsUnloadable(moduleUnloadable),
                _ => throw new InvalidOperationException("Данный тип модуля не потдерживаеться")
            };

            if (_options.IsUnloadable)
            {
                var module = new AssemblyModuleUnloadable(_store);
                _moduleCommands = new ModuleCommandsUnloadable(module);
                module.Load(_options.Name);
                try
                {
                    module.CallAction(assembly => AssemblyInitialize(assembly, _moduleCommands));
                }
                catch
                {
                    _moduleCommands = null;
                    module.Unload();
                    throw;
                }

                _module = module;
            }
            else
            {
                var module = new AssemblyModule(_store);
                try
                {
                    module.Load(_options.Name);
                    _moduleCommands = new ModuleCommands(module.Assembly!);

                    AssemblyInitialize(module.Assembly!, _moduleCommands);
                }
                catch
                {
                    _moduleCommands = null;
                    throw;
                }
                _module = module;
            }
            return Task.CompletedTask;
        }
        private void AssemblyInitialize(Assembly assembly, IModuleCommands moduleCommands)
        {

        }
        public void Unload()
        {
            if (_module == null)
            {
                throw new PluginUnloadException("Медуль не загружен.");
            }
            if (!_options.IsUnloadable)
            {
                throw new PluginUnloadException("Модуль должен быть выгружаемым.");
            }

            var module = (AssemblyModuleUnloadable)_module;
            if (!module.Unload())
            {
                throw new PluginUnloadException("Модуль не можут быть выгружен. Проверьте возможно остались ссылки на объекты в модуле.");
            }
        }

        public Task<ICommand> CreateCommandAsync(string commandName)
        {
            if (_moduleCommands == null)
            {
                throw new PluginLoadException("Модуль не загружен.");
            }
            
            var builder = _moduleCommands.CreateCommandBuilder(commandName);
            return Task.FromResult(builder.BuildCommand());
        }
    }
}
