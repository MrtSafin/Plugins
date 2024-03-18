using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Safin.Plugins.Scripts
{
    public record CSXScriptPluginOptions
    {
        public string? Name { get; set; }
        public CSXCommandInfo[]? Commands { get; set; }
    }
    public record CSXCommandInfo
    {
        public string? CommandName { get; set; }
        public string? FileName { get; set; }
    }
}
