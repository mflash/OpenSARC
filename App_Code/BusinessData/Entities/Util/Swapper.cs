using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities.Util
{
    public class Swapper
    {
        public static void Swap<T>(ref T x, ref T y)
        {
            T aux = x;
            x = y;
            y = aux;
        }
    }
}
