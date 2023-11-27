using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public class ModuleClassProxy : DisposableBase, IProxy
    {
        protected const string InstanceIsNull = "Объект был выгружен";

        protected readonly AssemblyModuleUnloadable _owner;
        private object? _instance;
        public bool IsAllowDisposed { get; set; } = true;

        internal ModuleClassProxy(AssemblyModuleUnloadable owner, object instance)
        {
            _owner = owner;
            _instance = instance;
        }
        public void CallAction(Action<object> action)
        {
            if (_instance == null)
            {
                throw new InvalidOperationException(InstanceIsNull);
            }

            action(_instance);
        }
        public TResult? CallFunc<TResult>(Func<object, TResult?> func)
        {
            if (_instance == null)
            {
                throw new InvalidOperationException(InstanceIsNull);
            }

            return func(_instance);
        }
        public IProxy<TResult>? SafeFunc<TResult>(Func<object, TResult?> func)
            where TResult : class
        {
            var result = CallFunc(func);
            if (result != null)
            {
                return _owner.SafeInstance(result);
            } else
            {
                return null;
            }
        }
        public async Task<IProxy<TResult>?> SafeFuncAsync<TResult>(Func<object, Task<TResult?>> func)
            where TResult : class
        {
            if (_instance == null)
            {
                throw new InvalidOperationException(InstanceIsNull);
            }

            var result = await func(_instance);
            if (result != null)
            {
                return _owner.SafeInstance(result);
            } else
            {
                return null;
            }
        }

        internal virtual void Clear()
        {
            if (_instance != null)
            {
                if (_instance is IDisposable disposable && IsAllowDisposed)
                    disposable.Dispose();
                _instance = null;
            }
        }
        protected override void DisposeManaged()
        {
            Clear();
            _owner.ClassDisposed(this);
            base.DisposeManaged();
        }
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is ModuleClassProxy proxy && _instance != null)
            {
                return _instance.Equals(proxy._instance);
            }
            else
            {
                return base.Equals(obj);
            }
        }
        public override int GetHashCode()
        {
            if (_instance != null)
            {
                return _instance.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }
        public override string? ToString()
        {
            if (_instance != null)
            {
                return _instance.ToString();
            }
            else
            {
                return base.ToString();
            }
        }
    }
    public class ModuleClassProxy<T> : ModuleClassProxy, IProxy<T>
        where T : class
    {
        private T? _instance;
        internal ModuleClassProxy(AssemblyModuleUnloadable owner, T instance): base(owner, instance)
        {
            _instance = instance;
        }
        public void CallAction(Action<T> action)
        {
            if (_instance == null)
            {
                throw new InvalidOperationException(InstanceIsNull);
            }
            action(_instance);
        }

        public TResult? CallFunc<TResult>(Func<T, TResult?> func)
        {
            if (_instance == null)
            {
                throw new InvalidOperationException(InstanceIsNull);
            }
            return func(_instance);
        }
        public IProxy<TResult>? SafeFunc<TResult>(Func<T, TResult?> func)
            where TResult : class
        {
            var result = CallFunc(func);
            if (result != null)
            {
                return _owner.SafeInstance(result);
            }
            else
            {
                return null;
            }
        }
        public async Task<IProxy<TResult>?> SafeFuncAsync<TResult>(Func<T, Task<TResult?>> func)
            where TResult : class
        {
            if (_instance == null)
            {
                throw new InvalidOperationException(InstanceIsNull);
            }

            var result = await func(_instance);

            if (result != null)
            {
                return _owner.SafeInstance(result);
            }
            else
            {
                return null;
            }
        }
        internal override void Clear()
        {
            base.Clear();
            _instance = null;
        }
    }
}
