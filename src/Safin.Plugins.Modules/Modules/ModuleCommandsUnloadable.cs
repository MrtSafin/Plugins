using Safin.Plugins.Commands;
using Safin.Plugins.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public class ModuleCommandsUnloadable(AssemblyModuleUnloadable assemblyModule) : ModuleCommandsBase, IAssemblyCommands
    {
        private readonly AssemblyModuleUnloadable _assemblyModule = assemblyModule;
        public override IModuleCommandBuilder CreateCommandBuilder(string className)
        {
            return new ModuleCommandBuilderUnloadable(_assemblyModule, className);
        }
        private class ModuleCommandBuilderUnloadable(AssemblyModuleUnloadable assemblyModule, string className): IModuleCommandBuilder
        {
            private readonly AssemblyModuleUnloadable _assemblyModule = assemblyModule;
            private readonly string _className = className;

            public ICommand BuildCommand()
            {
                return new ModuleCommand(_assemblyModule.CreateInstance(assembly =>
                {
                    var type = ModuleCommandsHelper.GetClassType(assembly, _className);
                    return Activator.CreateInstance(type)!;
                }));
            }
        }
        private class ModuleCommand(IProxy proxy): ICommand
        {
            private readonly IProxy _proxy = proxy;
            private object? _result = null;
            public object? Result => _result;

            public Task ExecuteAsync()
            {
                return _proxy
                    .CallFunc(instance => ModuleCommandsHelper.ExecuteAsync(instance.GetType(), instance))
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
                _proxy.CallAction(instance => ModuleCommandsHelper.SetProperty(instance.GetType(), instance, paramName, value));
            }
        }
    }
}
