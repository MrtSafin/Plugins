using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules
{
    public static class ModuleProviderHelper
    {
        public static void InitializeStaticSturtup(Assembly assembly, string nameClass, string nameMethod, object[]? args)
        {
            // var startupType = assembly.GetType("Sturtup", false, true);
            var name = assembly.GetName();
            var startupType = assembly.GetType($"{name.Name}.{nameClass}", false, true);
            if (startupType != null)
            {
                var method = startupType.GetMethod(nameMethod, BindingFlags.Static, 
                    args == null || args.Length == 0 ? [] :
                    args.Select(x => x.GetType()).ToArray());
                if (method != null && method.IsStatic)
                {
                    method.Invoke(null, args);
                }
            }
        }
        public static void InitializeStaticSturtup(Assembly assembly, string nameClass, object[]? args) =>
            InitializeStaticSturtup(assembly, nameClass, "Configure", args);
        public static void InitializeStaticSturtup(Assembly assembly, object[]? args) =>
            InitializeStaticSturtup(assembly, "Sturtup", args);
    }
}
