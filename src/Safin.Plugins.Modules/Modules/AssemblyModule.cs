using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public class AssemblyModule: AssemblyModuleBase
    {
        public Assembly? Assembly => _assembly;
        protected override AssemblyLoadContext CreateLoadContext(string path) => new ModuleLoadContext(path);
    }
}
