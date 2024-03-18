﻿using Safin.Plugins.Commands;
using Safin.Plugins.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public class ModuleCommands(Assembly assembly) : ModuleCommandsBase, IAssemblyCommands
    {
        private readonly Assembly _assembly = assembly;
        public override IModuleCommandBuilder CreateCommandBuilder(string className)
        {
            Type type = ModuleCommandsHelper.GetClassType(_assembly, className);
            return new TypeCommandBuilder(type);
        }
        private class TypeCommandBuilder(Type type): IModuleCommandBuilder
        {
            private readonly Type _type = type;
            public ICommand BuildCommand()
            {
                return new AssemblyCommand(Activator.CreateInstance(_type)!);
            }
        }
        private class AssemblyCommand(object instance): ICommand
        {
            private readonly object _instance = instance;
            private readonly Type _type = instance.GetType();
            private object? _result = null;
            public object? Result => _result;

            public void SetProperty(string paramName, object value) => ModuleCommandsHelper.SetProperty(_type, _instance, paramName, value);
            public Task ExecuteAsync() => 
                ModuleCommandsHelper.ExecuteAsync(_type, _instance)
                    .ContinueWith(t =>
                    {
                        if (!t.IsFaulted)
                        {
                            _result = t.Result;
                        }
                    });
        }
    }
}
