using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
    public class Test1
    {
        public int X { get; set; }
        public int Y { get; set; }
        public object? Result { get; private set; }
        public Task ExecuteAsync() {
            Result = X + Y;
            return Task.CompletedTask;
        }
    }
}
