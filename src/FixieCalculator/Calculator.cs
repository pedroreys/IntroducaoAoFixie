namespace FixieCalculator
{
    using System.Threading.Tasks;

    public class Calculator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        public int Subtract(int x, int y)
        {
            return x - y;
        }

        public int Multiply(int x, int y)
        {
            return x*y;
        }

        public int Divide(int x, int y)
        {
            return x/y;
        }

        public Task<int> AddAsync(int x, int y)
        {
            return Task.Run(() => x + y);
        } 
    }
}
