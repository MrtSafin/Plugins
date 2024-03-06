using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public interface IProxy
    {
        bool IsAllowDisposed { get; set; }
        void CallAction(Action<object> action);
        TResult CallFunc<TResult>(Func<object, TResult> func);
        IProxy<TResult>? SafeFunc<TResult>(Func<object, TResult?> func) where TResult: class;
        Task<IProxy<TResult>?> SafeFuncAsync<TResult>(Func<object, Task<TResult?>> func) where TResult : class;
    }
    public interface IProxy<T>: IProxy
    {
        void CallAction(Action<T> action);
        TResult CallFunc<TResult>(Func<T, TResult> func);
        IProxy<TResult>? SafeFunc<TResult>(Func<T, TResult?> func) where TResult : class;
        Task<IProxy<TResult>?> SafeFuncAsync<TResult>(Func<T, Task<TResult?>> func) where TResult : class;
    }
}
