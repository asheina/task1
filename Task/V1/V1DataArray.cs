using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Numerics;
using System.Runtime.InteropServices;

[Serializable]
public class Body
{
    public string Type { get; set; }

    public DateTime Time { get; set; }

    public int nX { get; set; }

    public int nY { get; set; }

    public double StepX { get; set; }

    public double StepY { get; set; }

    public struct JsonComplexNode
    {
        public int X { get; set; }

        public int Y { get; set; }

        public double Real { get; set; }

        public double Imaginary { get; set; }
    }

    public List<JsonComplexNode> ListedMatrix { get; set; }
}

[Serializable]
public class V1DataArray : V1Data
{
    public int nX { get; private set; }
    public int nY { get; private set; }
    public double stepX { get; private set; }
    public double stepY { get; private set; }
    public Complex[,] matrix { get; private set; }
    public override int Count { get { return nX * nY; } }
    public override double AverageValue
    {
        get
        {
            double avg = 0;
            foreach (Complex item in matrix)
            {
                avg += Complex.Abs(item);
            }

            return avg / nX / nY;
        }
    }

    public V1DataArray(string dataType, DateTime time) : base(dataType, time)
    {
        matrix = new Complex[0, 0];
    }

    public V1DataArray(string dataType,
            DateTime time,
            int x,
            int y,
            double sX,
            double sY,
            FdblComplex F) : base(dataType, time)
    {
        stepX = sX;
        stepY = sY;
        nX = x;
        nY = y;
        matrix = new Complex[nX, nY];

        for (int i = 0; i < nX; i++)
            for (int j = 0; j < nY; j++)
                matrix[i, j] = F(i * stepX, j * stepY);
    }

    public override string ToString()
    {
        return "Collection Type: " + type + "\n" +
                    "n_x: " + nX + ", n_y: " + nY + "\n" +
                    "step_x: " + stepX + ", step_y: " + stepY + "\n" +
                    "Time: " + this.createdAt.ToLongTimeString();
    }

    public override string ToLongString(string format)
    {
        string res = this.ToString();
        for (int i = 0; i < nX; i++)
            for (int j = 0; j < nY; j++)
            {
                res += "(x, y) : (" + String.Format(format, i * stepX) + ", " +
                   String.Format(format, j * stepY) +
                   ")\n";
                res += "value = " + String.Format(format, matrix[i, j].Real) + "+ i*" +
                    String.Format(format, matrix[i, j].Imaginary) +
                    " |value| = " + String.Format(format, System.Numerics.Complex.Abs(matrix[i, j])) +
                    "\n";
            }

        return res;
    }

    public V1DataList ToV1DataList()
    {
        V1DataList list = new V1DataList(this.type, this.createdAt);

        for (int i = 0; i < nX; i++)
            for (int j = 0; j < nY; j++)
                list.Add(new DataItem(i * stepX, j * stepY, matrix[i, j]));

        return list;
    }

    public override IEnumerator<DataItem> GetEnumerator()
    {
        for (int i = 0; i < nX; i++)
            for (int j = 0; j < nY; j++)
                yield return new DataItem(i, j, matrix[i, j]);
    }


    public static bool SaveAsText(string filename, V1DataArray v1)
    {
        var body = new Body
        {
            Type = v1.type,
            Time = v1.createdAt,
            nX = v1.nX,
            nY = v1.nY,
            StepX = v1.stepX,
            StepY = v1.stepY,
            ListedMatrix = new List<Body.JsonComplexNode>(),
        };

        for (int i = 0; i < v1.nX; i++)
            for (int j = 0; j < v1.nY; j++)
            {
                body.ListedMatrix.Add(new Body.JsonComplexNode
                {
                    X = i,
                    Y = j,
                    Real = v1.matrix[i, j].Real,
                    Imaginary = v1.matrix[i, j].Imaginary,
                });
            }

        var input = JsonSerializer.Serialize(body);
        try
        {
            File.WriteAllText(filename, input);
        }
        catch (Exception e)
        {
            Console.WriteLine($"handles exception in V1DataArray.SaveAsText: {e.Message}");
            return false;
        }
        return true;
    }

    public static bool LoadAsText(string filename, ref V1DataArray v1)
    {
        try
        {
            var content = File.ReadAllText(filename);
            var body = JsonSerializer.Deserialize<Body>(content);
            v1.type = body.Type;
            v1.createdAt = body.Time;
            v1.nX = body.nX;
            v1.nY = body.nY;
            v1.stepX = body.StepX;
            v1.stepY = body.StepY;
            v1.matrix = new Complex[v1.nX, v1.nY];

            foreach (var item in body.ListedMatrix)
            {
                v1.matrix[item.X, item.Y] = new Complex(item.Real, item.Imaginary);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"handles exception in V1DataArray.LoadAsText: {e.Message}");
            return false;
        }
        return true;
    }

    Complex? FieldAt(int jx, int jy)
    {
        if (jx >= nX || jy >= nY) 
        {
            return null;
        }
        return matrix[jx, jy];
    }

    bool Max_Field_Re(int jy, ref double min, ref double max)
    {
        if (jy >= nY)
        {
            return false;
        }

        min = matrix[0, jy].Real;
        max = min;
        for (int i = 1; i < nX; i++)
        {
            if (matrix[i, jy].Real > max)
            {
                max = matrix[i, jy].Real;
            }

            if (matrix[i, jy].Real < min)
            {
                min = matrix[i, jy].Real;
            }
        }

        return true;
    }

    [DllImport("MKL.dll")]
    private static extern void makeSplines(double[,] data, int nx, int ny, double stepx, int n, double[,] res);

    public V1DataArray ToSmallerGrid(int ns) 
    {
        var extendedMatrix = new double[nX*ns, nY];
        makeSplines(this.ToDoubleArray(), nX, nY, stepX, ns, extendedMatrix);

        var res =  new V1DataArray(type, createdAt);
        ToComplexArray(extendedMatrix, ref res);

        return res;
    }

    private double[,] ToDoubleArray()
    {
        var res = new double[nX, nY * 2];
        for (int i = 0; i < nX; i++)
        {
            for (int j = 0; j < nY; j += 2)
            {
                res[i, j] = matrix[i, j].Real;
                res[i, j + 1] = matrix[i, j].Imaginary;
            }
        }

        return res;
    }

    private static void ToComplexArray(double[,] doubleMatrix, ref V1DataArray v1)
    {
        int nX = doubleMatrix.GetLength(0);
        int nY = doubleMatrix.GetLength(1) / 2;
        v1.matrix = new Complex[nX, nY];
        v1.nX = nX;
        v1.nY = nY;
        for (int i = 0; i < v1.nX; i++)
        {
            for (int j = 0; j < v1.nY; j++)
            {
                v1.matrix[i, j] = new Complex(doubleMatrix[i, j], doubleMatrix[i, j + 1]);
            }
        }
    }
}
