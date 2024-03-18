using Safin.Plugins.Modules;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Stores.FileStorage
{
    public class PluginFileStore : IAssemblyModuleStore, ICSXScriptStore
    {
        public AssemblyLoadContext CreateLoadContext(string name, bool AllowUnload) => new ModuleLoadContext(name, AllowUnload);

        public AssemblyName CreateAssemblyName(string name) => new(Path.GetFileNameWithoutExtension(name));

        public Task LoadAsync(string name, ICSXScriptBuilder loader)
        {
            var stream = new FileStream(name, FileMode.Open, FileAccess.Read);
            return loader.LoadFromStreamAsync(stream)
                .ContinueWith(t =>
                {
                    stream.Dispose();
                });
        }
    }
}
