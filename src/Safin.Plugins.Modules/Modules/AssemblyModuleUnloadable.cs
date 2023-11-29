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
    public class AssemblyModuleUnloadable(IAssemblyModuleStore store) : AssemblyModuleBase(store)
    {
        private readonly List<ModuleClassProxy> _proxies = [];

        protected override AssemblyLoadContext CreateLoadContext(string name) => _store.CreateLoadContext(name, true); // new ModuleLoadContext(path)
        public bool Unload()
        {
            if (_loadContext is not null)
            {
                UnloadExec(out var weakRef);

                for (int i = 0; weakRef.IsAlive && (i < 10); i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                return !weakRef.IsAlive;
            }
            else
                return true;
        }
        public IProxy CreateInstance(Func<Assembly, object> factory)
        {
            if (_assembly is null)
            {
                throw new InvalidOperationException("Библиотека не загружена");
            }

            return SafeInstance(factory(_assembly));
        }
        public IProxy<T> CreateInstance<T>(Func<Assembly, T> factory)
            where T : class
        {
            if (_assembly is null)
            {
                throw new InvalidOperationException("Библиотека не загружена");
            }

            return SafeInstance(factory(_assembly));
        }
        public void CallAction(Action<Assembly> action)
        {
            if (_assembly is null)
            {
                throw new InvalidOperationException("Библиотека не загружена");
            }

            action(_assembly);
        }
        public TResult? CallFunc<TResult>(Func<Assembly, TResult?> func)
        {
            if (_assembly is null)
            {
                throw new InvalidOperationException("Библиотека не загружена");
            }

            return func(_assembly);
        }
        public IProxy<TResult>? SafeFunc<TResult>(Func<Assembly, TResult?> func)
            where TResult : class
        {
            if (_assembly is null)
            {
                throw new InvalidOperationException("Библиотека не загружена");
            }

            var result = func(_assembly);
            if (result != null)
            {
                return SafeInstance(result);
            } else
            {
                return null;
            }
        }
        public async Task<IProxy<TResult>?> SafeFuncAsync<TResult>(Func<Assembly, Task<TResult?>> func)
            where TResult : class
        {
            if (_assembly is null)
            {
                throw new InvalidOperationException("Библиотека не загружена");
            }

            var result = await func(_assembly);
            if (result != null)
            {
                return SafeInstance(result);
            }
            else
            {
                return null;
            }
        }
        public IProxy SafeInstance(object instance)
        {
            var result = new ModuleClassProxy(this, instance);
            _proxies.Add(result);
            return result;
        }
        public IProxy<T> SafeInstance<T>(T instance)
            where T : class
        {
            var result = new ModuleClassProxy<T>(this, instance);
            _proxies.Add(result);
            return result;
        }
        internal void ClassDisposed(ModuleClassProxy proxy)
        {
            _proxies.Remove(proxy);
        }
        private void UnloadExec(out WeakReference contextWeakRef)
        {
            foreach(var proxy in _proxies)
            {
                proxy.Clear();
            }
            _proxies.Clear();

            contextWeakRef = new WeakReference(_loadContext, true);

            _loadContext!.Unload();

            _assembly = null;
            _loadContext = null;
        }
    }
}
