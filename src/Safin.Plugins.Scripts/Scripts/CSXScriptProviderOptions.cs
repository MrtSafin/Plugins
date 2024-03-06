using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Safin.Plugins.Scripts
{
    public class CSXScriptProviderOptions(string name)
    {
        public string Name { get; set; } = name;
    }
}
