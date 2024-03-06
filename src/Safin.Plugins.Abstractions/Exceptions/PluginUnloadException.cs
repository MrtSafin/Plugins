using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Exceptions
{
    public class PluginUnloadException : Exception
    {
        public PluginUnloadException()
        {
        }

        public PluginUnloadException(string? message) : base(message)
        {
        }

        public PluginUnloadException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
