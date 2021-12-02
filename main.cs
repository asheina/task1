using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Task1
{
    class Program
    {
        static void TestSaveAndLoad(string fileJSON, string fileBIN)
        {
            const int testX = 2;
            const int testY = 3;
            const double testStepX = 0.3;
            const double testStepY = 0.4;

            V1DataArray loadedTestArray = new V1DataArray("", DateTime.Now);
            var testArray = new V1DataArray("test_save_load", DateTime.Now, testX, testY, testStepX, testStepY, Fdbl.TestComplexF);

            var okSave = V1DataArray.SaveAsText(fileJSON, testArray);
            var okLoad = V1DataArray.LoadAsText(fileJSON, ref loadedTestArray);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Исходный массив: ");
            Console.WriteLine(okSave);
            Console.WriteLine(testArray.ToLongString("{0:f2}"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Восстановленный массив: ");
            Console.WriteLine(okLoad);
            Console.WriteLine(loadedTestArray.ToLongString("{0:f2}"));

            Console.ResetColor();

            V1DataList loadedTestList = new V1DataList("", DateTime.Now);
            var testList = testArray.ToV1DataList();
            okSave = V1DataList.SaveBinary(fileBIN, testList);
            okLoad = V1DataList.LoadBinary(fileBIN, ref loadedTestList);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Исходный список: ");
            Console.WriteLine(okSave);
            Console.WriteLine(testList.ToLongString("{0:f2}"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Восстановленный список: ");
            Console.WriteLine(okLoad);
            Console.WriteLine(loadedTestList.ToLongString("{0:f2}"));
        }
        static void Main(string[] args)
        {
            TestSaveAndLoad("test_arr.json", "test_list.dat");
            // Console.WriteLine("------ 1.");
            // V1DataArray testArray = new V1DataArray("test (array)", System.DateTime.Now, 2, 1, 1.5, 1.0, Fdbl.TestComplexF);
            // Console.WriteLine(testArray.ToLongString("{0:f2}"));
            // Console.WriteLine(String.Format("Count: {0:d}", testArray.Count));
            // Console.WriteLine(String.Format("Avg: {0:f3}", testArray.AverageValue));

            // Console.WriteLine("------");

            // var testList = testArray.ToV1DataList();
            // Console.WriteLine(testList.ToLongString("{0:f2}"));
            // Console.WriteLine(String.Format("Count: {0:d}", testList.Count));
            // Console.WriteLine(String.Format("Avg: {0:f3}", testList.AverageValue));

            // // Thread.Sleep(2000);

            // Console.WriteLine("------ 2.");
            // V1MainCollection testCollection = new V1MainCollection();
            // testCollection.Add(testList);
            // testCollection.Add(testArray);

            // var testList2 = new V1DataList("test2", System.DateTime.Now);
            // testList2.AddDefaults(3, Fdbl.TestComplexF);
            // // Thread.Sleep(2000);
            // var testArray2 = new V1DataArray("test3", System.DateTime.Now, 3, 2, 0.3, 0.1, Fdbl.TestComplexF);
            // // Thread.Sleep(2000);
            // testCollection.Add(testList2);
            // testCollection.Add(testArray2);
            // Console.WriteLine(testCollection.ToLongString("{0:f2}"));

            // Console.WriteLine("------ 3.");
            // for (int i = 0; i < testCollection.Count; i++)
            // {
            //     Console.WriteLine(String.Format("Count: {0:d}", testCollection[i].Count));
            //     Console.WriteLine(String.Format("Avg: {0:f3}", testCollection[i].AverageValue));
            // }

            // Console.WriteLine("------ 4.");
            // Console.WriteLine(testCollection.minTime().ToString());

            // var c = new V1MainCollection();
            // Console.WriteLine(c.minTime() == null);

            // foreach (var data in (IEnumerable<V1Data>)testCollection)
            // {
            //     Console.WriteLine(data.ToString());
            // }

            // foreach (var data in (IEnumerable<V1Data>)c)
            // {
            //     Console.WriteLine(data.ToString());
            // }

            // var ok = V1DataArray.SaveAsText("test.txt", testArray);
            // Console.Write(ok);
        }
    }
}
