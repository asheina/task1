using System.Collections.Generic;
public abstract class V1Data
{
    public string type { get; set; }
    public System.DateTime createdAt { get; set; }

    public abstract int Count { get; }
    public abstract double AverageValue { get; }
    public abstract string ToLongString(string format);
    public V1Data(string dataType, System.DateTime time)
    {
        type = dataType;
        createdAt = time;
    }

    public override string ToString()
    {
        return "";
    }
}

public class V1DataList : V1Data
{
    public List<DataItem> value { get; }
    public override int Count { get { return 0; } }
    public override double AverageValue { get { return 0.0; } }
    public override string ToLongString(string format)
    {
        return "";
    }

    public V1DataList(string dataType, System.DateTime time) : base(dataType, time)
    {
        value = new List<DataItem>();
    }

    public bool Add(DataItem newItem)
    {
        return false;
    }

    public delegate System.Numerics.Complex FdblComplex(int x, int y);
    int AddDefaults(int nitems, FdblComplex F)
    {
        return 0;
    }

    public override string ToString()
    {
        return "";
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
    public override double AverageValue { get { return 0.0; } }
    public override string ToLongString(string format)
    {
        return "";
    }

    public V1DataArray(string dataType, System.DateTime time) : base(dataType, time)
    {
        matrix = new System.Numerics.Complex[0, 0];
    }

    public delegate System.Numerics.Complex FdblComplex(int x, int y);
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
            for (int j = 0; j < nX; j++)
                matrix[i, j] = F(i, j);
    }

    public override string ToString()
    {
        return "";
    }

    public V1DataList ToV1DataList(V1DataArray array)
    {
        V1DataList list = new V1DataList(array.type, array.createdAt);

        for (int i = 0; i < nX; i++)
            for (int j = 0; j < nX; j++)
                list.Add(new DataItem(i, j, matrix[i, j]));

        return list;
    }
}

public class V1MainCollection
{
    private List<V1Data> objects;
    public int Count { get { return 0; } }

    public bool Contains(string ID)
    {
        return false;
    }
    public bool Add(V1Data v1Data)
    {
        objects.Add(v1Data);
        return false;
    }

    public string ToLongString(string format)
    {
        string res = "";
        foreach (V1Data i in objects)
            res += i.ToLongString(format) + "\n";

        return res;
    }

    public override string ToString()
    {
        string res = "";
        foreach (V1Data i in objects)
            res += i.ToString() + "\n";

        return res;
    }
}