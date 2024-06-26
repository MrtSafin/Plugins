using Safin.Plugins.Modules;
using Safin.Plugins.Stores.FileStorage;

namespace Safin.Plugins.Tests.Assembly
{
    public class AssemblyModuleTest1
    {
        [Fact]
        public void LoadTest1()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\Functionality—heck"));
            var module = new AssemblyModule(new PluginFileStore(path));
            module.Load("TestLibrary.dll");
        }
        [Fact]
        public void LoadTest2()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\Functionality—heck"));
            var module = new AssemblyModuleUnloadable(new PluginFileStore(path));
            module.Load("TestLibrary.dll");

            Assert.True(module.Unload());
        }
        [Fact]
        public void LoadTest3()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\Functionality—heck"));
            var module = new AssemblyModuleUnloadable(new PluginFileStore(path));
            module.Load("TestLibrary.dll");

            var proxy = module.CreateInstance(assembly =>
            {
                var result = assembly.CreateInstance("Commands.TestClass");
                Assert.NotNull(result);
                return result;
            });

            Assert.True(module.Unload());
        }
        [Fact]
        public void LoadTest4()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\Functionality—heck"));
            var module = new AssemblyModuleUnloadable(new PluginFileStore(path));
            module.Load("TestLibrary.dll");

            var proxy = module.CreateInstance(assembly =>
            {
                Type type = assembly.GetType("Commands.TestClass", true)!;
                var result = Activator.CreateInstance(type, [5]);
                Assert.NotNull(result);
                return result;
            });

            proxy.CallAction(CheckActionProperty);

            Assert.True(module.Unload());
        }
        [Fact]
        public void LoadTest5()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\Functionality—heck"));
            var module = new AssemblyModuleUnloadable(new PluginFileStore(path));
            module.Load("TestLibrary.dll");

            var proxy = module.CreateInstance(assembly =>
            {
                Type type = assembly.GetType("Commands.TestClass", true)!;
                var result = Activator.CreateInstance(type, [5]);
                Assert.NotNull(result);
                return result;
            });
            var result = proxy.CallFunc(CheckActionFunc);

            Assert.Equal(5, result);

            Assert.True(module.Unload());
        }

        private void CheckActionProperty(object obj)
        {
            Assert.NotNull(obj);

            var type = obj.GetType();
            var prop = type.GetProperty("X");
            Assert.NotNull(prop);
            var XValue = prop.GetValue(obj);
            Assert.NotNull(XValue);

            Assert.Equal(5, (int)XValue);
        }
        private int CheckActionFunc(object obj)
        {
            Assert.NotNull(obj);

            var type = obj.GetType();
            var funcInfo = type.GetMethod("GetX");
            Assert.NotNull(funcInfo);
            var func = funcInfo.CreateDelegate<Func<int>>(obj);
            Assert.NotNull(func);

            return func();
        }
    }
}