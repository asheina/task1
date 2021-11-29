using System;
using System.Collections;
using System.Collections.Generic;

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
                    "Count: " + list.Count + "\n" +
                    "Time: " + this.createdAt.ToLongTimeString() + "\n";
        foreach (DataItem item in list)
            res += item.ToString();
        return res;
    }

    public override string ToLongString(string format)
    {
        string res = "Collection Type: " + type + "\n";
        foreach (DataItem item in list)
            res += item.ToLongString(format);

        res += "Count: " + list.Count + "\n" + "Time: " + this.createdAt.ToLongTimeString(); ;
        return res;
    }

    public override IEnumerator<DataItem> GetEnumerator()
    {
        return this.list.GetEnumerator();
    }

    // bool SaveBinary(string filename)
    // {

    // }

    // bool LoadBinary(string filename, ref V1DataList v1)
    // {

    // }
}

