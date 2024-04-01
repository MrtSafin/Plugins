#load "subfile.csx"
#r "..\..\..\..\..\FunctionalityCheck\TestLibrary.dll"

Commands.Test1 test = new Commands.Test1();
test.X = 1;
test.Y = 2;
test.ExecuteAsync().Wait();

return add(ValueInt("X"), ValueInt("Y"));