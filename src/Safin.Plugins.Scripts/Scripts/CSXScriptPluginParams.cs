using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts
{
    public class CSXScriptPluginParams(Dictionary<string, object> values)
    {
        public IReadOnlyDictionary<string, object> Values { get; } = values;
        public int ValueInt(string key) => (int)Values[key];
        public string ValueString(string key) => (string)Values[key];
        public bool ValueBool(string key) => (bool)Values[key];
        public double ValueDouble(string key) => (double)Values[key];
        public T Value<T>(string key) => (T)Values[key];
    }
}
