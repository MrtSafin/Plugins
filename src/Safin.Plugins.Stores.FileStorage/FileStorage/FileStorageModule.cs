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
    public class FileStorageModule : IAssemblyModuleStore, ICSXScriptStore
    {
        public AssemblyLoadContext CreateLoadContext(string name, bool AllowUnload)
        {
            return new ModuleLoadContext(name, AllowUnload);
        }

        public AssemblyName GetAssemblyName(string name)
        {
            return new AssemblyName(Path.GetFileNameWithoutExtension(name));
        }

        public Task LoadAsync(string name, Action<Stream> loadFromStream, Action<string> loadFromString)
        {
            using var stream = new FileStream(name, FileMode.Open, FileAccess.Read);
            loadFromStream(stream);
            stream.Close();

            return Task.CompletedTask;
        }
    }
}
