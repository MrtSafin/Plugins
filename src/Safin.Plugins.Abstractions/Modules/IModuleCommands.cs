using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public interface IModuleCommands
    {
        void Register(string name, string className);
    }
}
