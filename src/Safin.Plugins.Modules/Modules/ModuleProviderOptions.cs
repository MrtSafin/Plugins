using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public class ModuleProviderOptions(string name)
    {
        public string Name { get; set; } = name;
        public bool IsUnloadable { get; set; } = true;
    }
}
