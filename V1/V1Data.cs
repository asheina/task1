using System;
using System.Collections.Generic;
public abstract class V1Data
{
    public string type { get; set; }
    public System.DateTime createdAt { get; set; }
    public abstract int Count { get; }
    public abstract double AverageValue { get; }
    public abstract string ToLongString(string format);
    public V1Data(string type, System.DateTime createdAt)
    {
        this.type = type;
        this.createdAt = createdAt;
    }

    public abstract override string ToString();
}

public class V1DataList : V1Data
{
    public List<DataItem> list { get; }
    public override int Count { get { return list.Count; } }
    public override double AverageValue
    {
        get
        {
            double avg = 0;
            foreach (DataItem item in list)
                avg += System.Numerics.Complex.Abs(item.value);

            return avg / list.Count;
        }
    }
    public V1DataList(string dataType, System.DateTime time) : base(dataType, time)
    {
        list = new List<DataItem>();
    }
    public bool Add(DataItem newItem)
    {
        foreach (DataItem item in list)
        {
            if (item.x == newItem.x
                && item.y == newItem.y)
                return false;
        }
        list.Add(newItem);
        return true;
    }
    private static List<(double x, double y)> getPoints(int n)
    {
        var rand = new System.Random();
        var points = new List<(double x, double y)>();

        while (points.Count < n)
            points.Add((rand.NextDouble() * 100, rand.NextDouble() * 100));

        return points;
    }

    public int AddDefaults(int nitems, FdblComplex F)
    {
        int countn = 0;
        var points = getPoints(nitems);
        foreach ((double x, double y) point in points)
            if (this.Add(new DataItem(point.x, point.y, F(point.x, point.y))))
                countn++;

        return countn;
    }

    public override string ToString()
    {
        string res = "Collection Type: " + type + "\n" +
                    "Count: " + list.Count + "\n";
        foreach (DataItem item in list)
            res += item.ToString();
        return res;
    }

    public override string ToLongString(string format)
    {
        string res = "Collection Type: " + type + "\n";
        foreach (DataItem item in list)
            res += item.ToLongString(format);

        res += "Count: " + list.Count + "\n";
        return res;
    }
}

public class V1DataArray : V1Data
{
    public int nX { get; }
    public int nY { get; }

    public double stepX { get; }
    public double stepY { get; }
    public System.Numerics.Complex[,] matrix { get; set; }
    public override int Count { get { return nX * nY; } }
    public override double AverageValue
    {
        get
        {
            double avg = 0;
            foreach (System.Numerics.Complex item in matrix)
                avg += System.Numerics.Complex.Abs(item);

            return avg / nX / nY;
        }
    }

    public V1DataArray(string dataType, System.DateTime time) : base(dataType, time)
    {
        matrix = new System.Numerics.Complex[0, 0];
    }

    public V1DataArray(string dataType,
            System.DateTime time,
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
        matrix = new System.Numerics.Complex[nX, nY];

        for (int i = 0; i < nX; i++)
            for (int j = 0; j < nY; j++)
                matrix[i, j] = F(i * stepX, j * stepY);
    }

    public override string ToString()
    {
        return "Collection Type: " + type + "\n" +
                    "n_x: " + nX + ", n_y: " + nY + "\n" +
                    "step_x: " + stepX + ", step_y: " + stepY + "\n";
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
}