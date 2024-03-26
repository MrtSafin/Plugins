using Safin.Plugins.Modules;
using Safin.Plugins.Stores.FileStorage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Tests.Assembly
{
    public class ModulePluginTest1
    {
        [Fact]
        public async Task LoadTest1()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityСheck\\TestLibrary.dll"));
            var store = new PluginFileStore();
            IPlugin plugin = new ModulePlugin(store, new ModulePluginOptions(path)
            {
                IsUnloadable = false
            });

            await plugin.LoadAsync();
            var command = await plugin.CreateCommandAsync("TestLibrary.Test1");

            command.SetProperty("X", 25);
            command.SetProperty("Y", 75);
            await command.ExecuteAsync();

            Assert.Equal(100, command.Result);
        }
    }
}
