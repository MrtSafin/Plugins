using Safin.Plugins.Scripts;
using Safin.Plugins.Stores;
using Safin.Plugins.Stores.FileStorage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules.Tests.Scripts
{
    public record GlobalValues(int X, int Y);
    public class CSXScriptTest1
    {
        [Fact]
        public async Task ExecuteFileTest1()
        {
            ICSXScriptStore store = new PluginFileStore();
            var script = new CSXScript(store, typeof(GlobalValues));

            var result = await script.Execute("Scripts\\test1.csx", new GlobalValues(25, 75));

            Assert.NotNull(result);
            Assert.Equal(100, (int)result);
        }
        [Fact]
        public async Task ExecuteFileTest2()
        {
            ICSXScriptStore store = new PluginFileStore();
            var script = new CSXScript<GlobalValues>(store);

            var result = await script.Execute<int>("Scripts\\test1.csx", new GlobalValues(25, 75));

            Assert.Equal(100, result);
        }
    }
}
