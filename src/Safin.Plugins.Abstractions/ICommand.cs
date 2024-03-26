using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins
{
    /// <summary>
    /// Interface for a command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Set the value of the parameter
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        void SetProperty(string paramName, object value);
        /// <summary>
        /// Execute the command
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();
        /// <summary>
        /// Get the result of the command
        /// </summary>
        object? Result { get; }
    }
}
