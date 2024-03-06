using Safin.Plugins.Stores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Scripts
{
    public class CSXScriptProvider(ICSXScriptStore store, CSXScriptProviderOptions options): IPluginProvider
    {
        private readonly ICSXScriptStore _store = store;
        private readonly CSXScriptProviderOptions _options = options;
        private CSXScript? _script = null;
        public bool IsLoaded => _script != null;

        public void Load()
        {
            if (_script == null)
            {
                throw new PluginLoadException($"Скрипт \"{_options.Name}\" уже загружен.");
            }

            _script = new CSXScript(_store);
        }
        public void Unload()
        {
            _script = null;
        }
    }
}
