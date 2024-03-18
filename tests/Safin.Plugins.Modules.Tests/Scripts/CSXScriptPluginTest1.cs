using Safin.Plugins.Scripts;
using Safin.Plugins.Stores.FileStorage;
using Safin.Plugins.Stores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules.Tests.Scripts
{
    public class CSXScriptPluginTest1
    {
        // Тест класса CSXScriptPlugin
        [Fact]
        public async Task Test1()
        {
            ICSXScriptStore store = new PluginFileStore();
            CSXScriptPluginOptions options = new()
            {
                Name = "Test1",
                Commands =
                [
                    new CSXCommandInfo { CommandName = "test", FileName = "Scripts\\test1.csx" }
                ]
            };
            var script = new CSXScriptPlugin(store, options);
            await script.LoadAsync();
            var command = await script.CreateCommandAsync("test");
            command.SetProperty("X", 25);
            command.SetProperty("Y", 75);
            await command.ExecuteAsync();
            Assert.NotNull(command.Result);
            Assert.Equal(100, (int)command.Result);
        }
    }
}
