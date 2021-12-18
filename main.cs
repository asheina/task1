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

            Console.ResetColor();
        }

        static void TestLINQ()
        {
            const int testX = 2;
            const int testY = 3;
            const double testStepX = 0.3;
            const double testStepY = 0.4;

            var collection = new V1MainCollection();

            V1DataArray loadedTestArray = new V1DataArray("", DateTime.Now);
            var testArray = new V1DataArray("test_linq_1_arr", DateTime.Now, testX, testY, testStepX, testStepY, Fdbl.TestComplexF);
            var testArrayToList = new V1DataArray("test_linq_1_list", DateTime.Now, testX, testY, testStepX, testStepY, Fdbl.TestComplexF);

            var testList = testArrayToList.ToV1DataList();

            Thread.Sleep(1000);

            var testArray2 = new V1DataArray("test_linq_2_arr", DateTime.Now, testX - 1, testY - 1, testStepX, testStepY, Fdbl.TestComplexF);
            var testArray2ToList = new V1DataArray("test_linq_2_list", DateTime.Now, testX - 1, testY - 1, testStepX, testStepY, Fdbl.TestComplexF);
            var testList2 = testArray2ToList.ToV1DataList();

            Thread.Sleep(1000);

            var testArrayEmpty = new V1DataArray("test_linq_empty_arr", DateTime.Now);
            var testListEmpty = new V1DataList("test_linq_empty_list", DateTime.Now);

            collection.Add(testArray);
            collection.Add(testList);
            collection.Add(testArray2);
            collection.Add(testList2);
            collection.Add(testArrayEmpty);
            collection.Add(testListEmpty);

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Test MinTime");
            Console.WriteLine(collection.minTime());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Test IEnumerable<V1Data>");
            foreach (var data in (IEnumerable<V1Data>)collection)
            {
                Console.WriteLine(data.ToString());
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Test IEnumerable<V1DataList>");
            foreach (var data in (IEnumerable<V1DataList>)collection)
            {
                Console.WriteLine(data.ToString());
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("All collection");
            Console.WriteLine(collection.ToString());
        }

        static void Main(string[] args)
        {
            try
            {
                TestSaveAndLoad("test_arr.xml", "test_list.dat");
                Console.WriteLine("\n.................................\n");
                TestLINQ();
            }
            catch (Exception e)
            {
                Console.WriteLine($"handles exception Main: {e.Message}");
            }
        }
    }
}
