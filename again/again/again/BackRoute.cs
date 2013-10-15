using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace again
{
    public class BackRoute
    {
        public int[, ,] myRoute;
        public Stack<int> inOut = new Stack<int>();
        public int[] backDirection=new int[8];
        public BackRoute(int m, int n)
        {
            myRoute = new int[m, n, 8];
            for (int i = 0; i < 8; i++)
            {
                backDirection[i] = -1;
            }
        }
    }
}
