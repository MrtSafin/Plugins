﻿using Safin.Plugins.Scripts;
using Safin.Plugins.Stores;
using Safin.Plugins.Stores.FileStorage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Modules.Tests.Scripts
{
    public class CSXScriptTest1
    {
        [Fact]
        public async Task ExecuteFileTest1()
        {
            ICSXScriptStore store = new PluginFileStore();
            var script = new CSXScript(store, typeof(CSXScriptPluginParams));

            var result = await script.Execute("Scripts\\test1.csx", new CSXScriptPluginParams(new Dictionary<string, object> {
                { "X", 25 },
                {"Y", 75 }
            }));

            Assert.NotNull(result);
            Assert.Equal(100, (int)result);
        }
        [Fact]
        public async Task ExecuteFileTest2()
        {
            ICSXScriptStore store = new PluginFileStore();
            var script = new CSXScript<CSXScriptPluginParams>(store);

            var result = await script.Execute<int>("Scripts\\test1.csx", new CSXScriptPluginParams(new Dictionary<string, object> {
                { "X", 25 },
                {"Y", 75 }
            }));

            Assert.Equal(100, result);
        }
    }
}
