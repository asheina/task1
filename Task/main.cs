using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Task1
{
    class Program
    {
        static void TestMKL()
        {
            var testArray = new V1DataArray("test", DateTime.Now, 4, 4, 1.0, 1.0, Fdbl.Polynom3);

            var newRes = testArray.ToSmallerGrid(8);
        }

        static void Main(string[] args)
        {
            try
            {
                TestMKL();
            }
            catch (Exception e)
            {
                Console.WriteLine($"handles exception Main: {e.Message}");
            }
        }
    }
}
