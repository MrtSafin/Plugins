using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Helpers
{
    public class ScriptNotFoundException: Exception
    {
        public ScriptNotFoundException() { }
        public ScriptNotFoundException(string message) : base(message) { }
        public ScriptNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
