using Safin.Plugins.Stores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public class AssemblyModule(IAssemblyModuleStore store) : AssemblyModuleBase(store)
    {
        public Assembly? Assembly => _assembly;
        protected override AssemblyLoadContext CreateLoadContext(string name) => _store.CreateLoadContext(name, false); // new ModuleLoadContext(path)
    }
}
