using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    internal interface IAssemblyCommands: IModuleCommands
    {
        IModuleCommandBuilder CreateCommandBuilder(string className);
    }
}
