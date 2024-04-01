#load "subfile.csx"
#r "..\..\..\..\..\FunctionalityCheck\TestLibrary.dll"

using System;

Commands.Test1 test = new Commands.Test1();
test.X = 1;
test.Y = 2;
await test.ExecuteAsync();
if ((int)test.Result != 3)
{
    throw new Exception("Test failed");
}

return add(ValueInt("X"), ValueInt("Y"));