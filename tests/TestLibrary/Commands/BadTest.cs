using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class BadTest
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
