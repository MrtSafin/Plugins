using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Stores.FileStores
{
    public class FolderProvider(string folder): IPluginStoresProvider
    {
        private readonly string _folder = folder;
        public Task LoadPlugins()
        {
            foreach(var pluginFile in Directory.GetFiles(_folder, "*.plugin.json"))
            {

            }
            return Task.CompletedTask;
        }
    }
}
