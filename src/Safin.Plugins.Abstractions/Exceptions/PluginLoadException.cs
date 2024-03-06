using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins
{
    public class PluginLoadException : Exception
    {
        public PluginLoadException()
        {
        }

        public PluginLoadException(string? message) : base(message)
        {
        }

        public PluginLoadException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
