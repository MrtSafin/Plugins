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
        private readonly string _folder;
        public PluginFileStore(string folder)
        {
            _folder = folder;
        }
        public PluginFileStore()
        {
            _folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        }
        public AssemblyLoadContext CreateLoadContext(string name, bool AllowUnload)
        {
            var fileName = GetFileName(name);
            return new ModuleLoadContext(fileName, AllowUnload);
        }

        public AssemblyName CreateAssemblyName(string name)
        {
            var fileName = GetFileName(name);
            return new(Path.GetFileNameWithoutExtension(fileName));
        }

        public Task LoadAsync(string name, ICSXScriptBuilder loader)
        {
            var fileName = GetFileName(name);
            return loader.LoadFromFileAsync(fileName, GetFileName);
        }
        public virtual string GetFileName(string name)
        {
            return Path.IsPathFullyQualified(name) ? name : Path.Combine(_folder, name);
        }
    }
}
