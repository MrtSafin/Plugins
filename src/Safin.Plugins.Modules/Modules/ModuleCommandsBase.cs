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
        protected Dictionary<string, IModuleCommandBuilder> _commands = [];
        public void Register(string name, string className)
        {
            _commands.Add(name, CreateCommandBuilder(className));
        }
        public abstract IModuleCommandBuilder CreateCommandBuilder(string className);
    }
    public interface IModuleCommandBuilder
    {
        ICommand BuildCommand();
    }
}
