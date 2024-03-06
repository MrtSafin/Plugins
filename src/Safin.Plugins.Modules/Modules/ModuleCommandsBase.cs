using Safin.Plugins.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public abstract class ModuleCommandsBase()
    {
        protected Dictionary<string, ModuleCommandBuilder> _commands = [];
        public void Register(string name, string className)
        {
            _commands.Add(name, CreateCommandBuilder(className));
        }
        protected abstract ModuleCommandBuilder CreateCommandBuilder(string className);
    }
    public abstract class ModuleCommandBuilder
    {
        public abstract ICommand BuildCommand();
    }
}
