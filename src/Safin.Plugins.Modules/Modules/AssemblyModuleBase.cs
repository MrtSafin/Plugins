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
    //public class FindTypeEventArgs(IEnumerable<Type> types, string typeName) : EventArgs
    //{
    //    public IEnumerable<Type> Types { get; } = types;
    //    public string TypeName { get; } = typeName;
    //    public Type? Result { get; set; } = null;
    //}
    public abstract class AssemblyModuleBase(IAssemblyModuleStore store)
    {
        //static string AssemblyIsNotLoad = "Библиотека не загружена";
        protected AssemblyLoadContext? _loadContext = null;
        protected Assembly? _assembly = null;
        protected IAssemblyModuleStore _store = store;

        public void Load(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            if (_assembly is not null)
            {
                throw new InvalidOperationException("Повторная загрузка библиотеки");
            }

            _loadContext = CreateLoadContext(name);
            _assembly = _loadContext.LoadFromAssemblyName(_store.GetAssemblyName(name)); //new AssemblyName(Path.GetFileNameWithoutExtension(path)
        }
        protected abstract AssemblyLoadContext CreateLoadContext(string path);
    }
}
