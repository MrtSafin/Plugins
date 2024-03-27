using Safin.Plugins.Exceptions;
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
            var command = await plugin.CreateCommandAsync("Commands.Test1");

            command.SetProperty("X", 25);
            command.SetProperty("Y", 75);
            await command.ExecuteAsync();

            Assert.Equal(100, command.Result);
        }
        [Fact]
        public async Task ExecuteTest()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityСheck\\TestLibrary.dll"));
            var store = new PluginFileStore();
            IPlugin plugin = new ModulePlugin(store, new ModulePluginOptions(path)
            {
                IsUnloadable = false
            });

            await plugin.LoadAsync();
            var command = await plugin.CreateCommandAsync("Commands.ExecuteTest");

            await command.ExecuteAsync();

            Assert.Null(command.Result);
        }
        [Fact]
        public async Task BadTest()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityСheck\\TestLibrary.dll"));
            var store = new PluginFileStore();
            IPlugin plugin = new ModulePlugin(store, new ModulePluginOptions(path)
            {
                IsUnloadable = false
            });

            await plugin.LoadAsync();
            var command = await plugin.CreateCommandAsync("Commands.BadTest");

            await Assert.ThrowsAsync<CommandException>(command.ExecuteAsync);
        }
        [Fact]
        public async Task BadExecuteTest()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityСheck\\TestLibrary.dll"));
            var store = new PluginFileStore();
            IPlugin plugin = new ModulePlugin(store, new ModulePluginOptions(path)
            {
                IsUnloadable = false
            });

            await plugin.LoadAsync();
            var command = await plugin.CreateCommandAsync("Commands.BadExecTest");

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await command.ExecuteAsync());
        }
    }
}
