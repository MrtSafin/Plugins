using Safin.Plugins.Modules;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins
{
    /// <summary>
    /// Работа с одним плагином
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Загрузить Plugin
        /// </summary>
        Task LoadAsync();
        /// <summary>
        /// Выгрузить Plugin
        /// </summary>
        void Unload();
        /// <summary>
        /// Создать команду
        /// </summary>
        Task<ICommand> CreateCommandAsync(string commandName);
    }
}
