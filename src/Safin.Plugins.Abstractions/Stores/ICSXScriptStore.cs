using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Stores
{
    public interface ICSXScriptStore
    {
        /// <summary>
        /// Загрузка скрипта из источника.
        /// </summary>
        /// <param name="name">Имя скрипта в хранилище</param>
        /// <param name="builder">Объект отвечающий за создание объекта который будет работать со скриптом.</param>
        /// <remarks>
        /// Так как источник ни чего не знает о механизме компиляции и выполнении скрипта,
        /// то нужен объект ICSXScriptBuilder который и выполнит создание обекта по работе со скриптом
        /// </remarks>
        Task LoadAsync(string name, ICSXScriptBuilder builder);
    }
}
