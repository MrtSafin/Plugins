using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Stores
{
    public interface IAssemblyModuleStore
    {
        AssemblyLoadContext CreateLoadContext(string name, bool AllowUnload);
        AssemblyName GetAssemblyName(string name);
    }
}
