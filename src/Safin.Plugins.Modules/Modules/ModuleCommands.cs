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
    public class ModuleCommands(Assembly assembly) : ModuleCommandsBase, IModuleCommands
    {
        private readonly Assembly _assembly = assembly;
        protected override ModuleCommandBuilder CreateCommandBuilder(string className)
        {
            Type type = ModuleCommandsHelper.GetClassType(_assembly, className);
            return new TypeCommandBuilder(type);
        }
        private class TypeCommandBuilder(Type type): ModuleCommandBuilder
        {
            private readonly Type _type = type;
            public override ICommand BuildCommand()
            {
                return new AssemblyCommand(Activator.CreateInstance(_type)!);
            }
        }
        private class AssemblyCommand(object instance): ICommand
        {
            private readonly object _instance = instance;
            private readonly Type _type = instance.GetType();

            public void SetProperty(string paramName, object value) => ModuleCommandsHelper.SetProperty(_type, _instance, paramName, value);
            public Task ExecuteAsync() => ModuleCommandsHelper.ExecuteAsync(_type, _instance);
        }
    }
}
