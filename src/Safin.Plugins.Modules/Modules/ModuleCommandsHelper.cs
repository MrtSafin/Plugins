using Safin.Plugins.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    internal static class ModuleCommandsHelper
    {
        public static Type GetClassType(Assembly assembly, string className)
        {
            Type type = assembly.GetType(className, true)!;
            if (!type.IsClass)
            {
                throw new CommandException($"Command type \"{className}\" is not class ");
            }
            return type;
        }
        public static void SetProperty(Type type, object instance, string paramName, object value)
        {
            var property = type.GetProperty(paramName, BindingFlags.Instance | BindingFlags.Public);
            property?.SetValue(instance, value);
        }
        public static Task ExecuteAsync(Type type, object instance)
        {
            var method = type.GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes);
            if (method != null)
            {
                if (method.ReturnType != typeof(Task))
                {
                    throw new CommandException("Method \"ExecuteAsync\" declaration error. \"public Task ExecuteAsync()\"");
                }
                try
                {
                    var result = method.Invoke(instance, []);
                    return result == null ? Task.CompletedTask : (Task)result;
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException!;
                }
            }
            else
            {
                method = type.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes);

                if (method != null)
                {
                    if (method.ReturnType != typeof(void))
                    {
                        throw new CommandException("Method \"Execute\" declaration error. \"public void Execute()\"");
                    }
                    method.Invoke(instance, []);
                    return Task.CompletedTask;
                }
                else
                {
                    throw new CommandException("Method \"ExecuteAsync\" not found");
                }
            }
        }
        public static object? GetResult(Type type, object instance)
        {
            var property = type.GetProperty("Result", BindingFlags.Instance | BindingFlags.Public);
            return property?.GetValue(instance);
        }
    }
}
