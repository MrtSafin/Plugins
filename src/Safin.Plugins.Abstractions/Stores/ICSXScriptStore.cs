﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins.Stores
{
    public interface ICSXScriptStore
    {
        Task LoadAsync(string name, Func<Stream, Task> loadFromStream, Func<string, Task> loadFromString);
    }
}
