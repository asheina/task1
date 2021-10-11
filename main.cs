using System;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            V1DataArray testArray = new V1DataArray("test", System.DateTime.Now, 2, 1, 1.5, 1.0, Fdbl.TestComplexF);
            Console.WriteLine(testArray.ToLongString("{0:f2}"));
            Console.WriteLine("Count: " + testArray.Count);
            Console.WriteLine("Avg: " + testArray.AverageValue);

            var testList = testArray.ToV1DataList();
            Console.WriteLine(testList.ToLongString("{0:f2}"));
            Console.WriteLine("Count: " + testList.Count);
            Console.WriteLine("Avg: " + testList.AverageValue);
        }
    }
}
