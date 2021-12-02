using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Numerics;
using System.Text.Json.Serialization;

[Serializable]
public class Body
{
    public int nX { get; set; }

    public int nY { get; set; }

    public double stepX { get; set; }

    public double stepY { get; set; }
}

[Serializable]
public class V1DataArray : V1Data
{
    public int nX { get; set; }
    public int nY { get; set; }

    public double stepX { get; set; }
    public double stepY { get; set; }
    public Complex[,] matrix { get; set; }
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
            nX = v1.nX,
            nY = v1.nY,
            stepX = v1.stepX,
            stepY = v1.stepY,
        };
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
            v1.nX = body.nX;
            v1.nY = body.nY;
            v1.stepX = body.stepX;
            v1.stepY = body.stepY;
        }
        catch (Exception e)
        {
            Console.WriteLine($"handles exception in V1DataArray.LoadAsText: {e.Message}");
            return false;
        }
        return true;
    }
}
