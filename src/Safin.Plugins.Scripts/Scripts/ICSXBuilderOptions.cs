using Microsoft.CodeAnalysis.Scripting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts
{
    public interface ICSXBuilderOptions
    {
        ScriptOptions BuildOptions(ScriptOptions options);
    }
}
