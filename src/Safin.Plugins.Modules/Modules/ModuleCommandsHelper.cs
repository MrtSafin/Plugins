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
                var task = (Task)method.Invoke(instance, [])!;
                return task;
            }
            else
            {
                throw new CommandException("Method \"ExecuteAsync\" not found");
            }
        }
    }
}
