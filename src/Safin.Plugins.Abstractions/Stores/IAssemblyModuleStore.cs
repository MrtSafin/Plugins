using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Stores
{
    /// <summary>
    /// Хранилище отвечающее за откомпилированные модули.
    /// </summary>
    public interface IAssemblyModuleStore
    {
        AssemblyLoadContext CreateLoadContext(string name, bool AllowUnload);
        AssemblyName CreateAssemblyName(string name);
    }
}
