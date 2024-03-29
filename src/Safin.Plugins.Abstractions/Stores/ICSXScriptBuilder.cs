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
        /// <param name="stream">Поток содержащий скрипт</param>
        Task LoadFromStreamAsync(Stream stream, Func<string, Stream> resolve);
        /// <summary>
        /// Создаёт объект скрипта из строки
        /// </summary>
        /// <param name="str">текст скрипта</param>
        Task LoadFromStringAsync(string str, Func<string, string> resolve);
        /// <summary>
        /// Создаёт объект скрипта из файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        Task LoadFromFileAsync(string fileName, Func<string, string> resolve);
    }
}
