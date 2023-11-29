using Safin.Plugins.Modules;
using Safin.Plugins.Stores.FileStorage;

namespace Safin.Plugins.Modules.Tests
{
    public class AssemblyModuleTest1
    {
        [Fact]
        public void LoadTest1()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityÑheck\\TestLibrary.dll"));
            var module = new AssemblyModule(new FileStorageModule());
            module.Load(path);
        }
        [Fact]
        public void LoadTest2()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityÑheck\\TestLibrary.dll"));
            var module = new AssemblyModuleUnloadable(new FileStorageModule());
            module.Load(path);

            Assert.True(module.Unload());
        }
        [Fact]
        public void LoadTest3()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityÑheck\\TestLibrary.dll"));
            var module = new AssemblyModuleUnloadable(new FileStorageModule());
            module.Load(path);

            var proxy = module.CreateInstance(assembly =>
            {
                var result = assembly.CreateInstance("TestLibrary.TestClass");
                Assert.NotNull(result);
                return result;
            });

            Assert.True(module.Unload());
        }
        [Fact]
        public void LoadTest4()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityÑheck\\TestLibrary.dll"));
            var module = new AssemblyModuleUnloadable(new FileStorageModule());
            module.Load(path);

            var proxy = module.CreateInstance(assembly =>
            {
                Type type = assembly.GetType("TestLibrary.TestClass", true)!;
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
            var path = Path.GetFullPath(Path.Combine(currentDir, "..\\..\\..\\..\\FunctionalityÑheck\\TestLibrary.dll"));
            var module = new AssemblyModuleUnloadable(new FileStorageModule());
            module.Load(path);

            var proxy = module.CreateInstance(assembly =>
            {
                Type type = assembly.GetType("TestLibrary.TestClass", true)!;
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