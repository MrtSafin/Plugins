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
    public abstract class AssemblyModuleBase
    {
        //static string AssemblyIsNotLoad = "Библиотека не загружена";
        protected AssemblyLoadContext? _loadContext = null;
        protected Assembly? _assembly = null;

        //public delegate void FindTypeEventHandler(AssemblyModuleBase source, FindTypeEventArgs args);
        //public event FindTypeEventHandler? FindTypeEvent;

        public void Load(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            if (_assembly is not null)
            {
                throw new InvalidOperationException("Повторная загрузка библиотеки");
            }

            _loadContext = CreateLoadContext(path);
            _assembly = _loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }
        protected abstract AssemblyLoadContext CreateLoadContext(string path);
    }
}
