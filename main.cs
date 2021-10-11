using System;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------ 1.");
            V1DataArray testArray = new V1DataArray("test (array)", System.DateTime.Now, 2, 1, 1.5, 1.0, Fdbl.TestComplexF);
            Console.WriteLine(testArray.ToLongString("{0:f2}"));
            Console.WriteLine(String.Format("Count: {0:d}", testArray.Count));
            Console.WriteLine(String.Format("Avg: {0:f3}", testArray.AverageValue));

            Console.WriteLine("------");

            var testList = testArray.ToV1DataList();
            Console.WriteLine(testList.ToLongString("{0:f2}"));
            Console.WriteLine(String.Format("Count: {0:d}", testList.Count));
            Console.WriteLine(String.Format("Avg: {0:f3}", testList.AverageValue));
            
            Console.WriteLine("------ 2.");
            V1MainCollection testCollection = new V1MainCollection();
            testCollection.Add(testList);
            testCollection.Add(testArray);

            var testList2 = new V1DataList("test2", System.DateTime.Now);
            testList2.AddDefaults(3, Fdbl.TestComplexF);

            var testArray2 = new V1DataArray("test3", System.DateTime.Now, 3, 2, 0.3, 0.1, Fdbl.TestComplexF);

            testCollection.Add(testList2);
            testCollection.Add(testArray2);
            Console.WriteLine(testCollection.ToLongString("{0:f2}"));

            Console.WriteLine("------ 3.");
            for (int i = 0; i < testCollection.Count; i++)
            {
                Console.WriteLine(String.Format("Count: {0:d}", testCollection[i].Count));
                Console.WriteLine(String.Format("Avg: {0:f3}", testCollection[i].AverageValue));
            }
        }
    }
}
