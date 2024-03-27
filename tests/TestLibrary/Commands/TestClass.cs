namespace Commands
{
    public class TestClass
    {
        public int X { get; set; }
        public TestClass() { }
        public TestClass(int x)
        {
            X = x;
        }
        public int GetX()
        {
            return X;
        }
    }
}
