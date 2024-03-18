using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Stores
{
    /// <summary>
    /// Загрузсчик скриптов. Отвечает за загрузку скрипта из потока или строки.
    /// </summary>
    public interface ICSXScriptBuilder
    {
        /// <summary>
        /// Создаёт объект скрипта из потока.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task LoadFromStreamAsync(Stream stream);
        /// <summary>
        /// Создаёт объект скрипта из строки
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        Task LoadFromStringAsync(string str);
    }
}
