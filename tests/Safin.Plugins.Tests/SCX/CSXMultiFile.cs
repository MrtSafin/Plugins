using Safin.Plugins.Scripts;
using Safin.Plugins.Stores;
using Safin.Plugins.Stores.FileStorage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Tests.SCX
{
    public class CSXMultiFile
    {
        [Fact]
        public async Task Test1()
        {
            ICSXScriptStore store = new PluginFileStore();
            var script = new CSXScript(store, typeof(CSXScriptPluginParams));

            var result = await script.Execute("test-multifile.csx", new CSXScriptPluginParams(new Dictionary<string, object>
            {
                { "X", 25 },
                { "Y", 75 }
            }));

            Assert.NotNull(result);
            Assert.Equal(100, (int)result);
        }
    }
}
