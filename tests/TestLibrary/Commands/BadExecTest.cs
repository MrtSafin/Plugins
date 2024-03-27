using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class BadExecTest
    {
        public int TestParam { get; set; }
        public Task ExecuteAsync()
        {
            //await Task.Delay(10);
            throw new ArgumentOutOfRangeException("TestParam", "ArgumentOutOfRange");
        }
    }
}
