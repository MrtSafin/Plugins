using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Commands
{
    public interface ICommand
    {
        void SetProperty(string paramName, object value);
        Task ExecuteAsync();
    }
}
