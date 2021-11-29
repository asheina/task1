using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class V1MainCollection : IEnumerable<V1DataList>, IEnumerable<V1Data>
{
    private List<V1Data> _values = new List<V1Data>();
    public V1Data this[int i]
    {
        get { return _values[i]; }
    }
    public int Count { get { return _values.Count; } }
    public bool Contains(string ID)
    {
        foreach (V1Data item in _values)
            if (item.type == ID)
                return true;
        return false;
    }
    public bool Add(V1Data v1Data)
    {
        if (this.Contains(v1Data.type))
            return false;
        _values.Add(v1Data);
        return true;
    }

    public DateTime? minTime()
    {
        if (this.Count != 0)
        {
            var query = (from i in this._values select i.createdAt).Min();
            return query;
        }
        return null;
    }

    public string ToLongString(string format)
    {
        string res = "";
        foreach (V1Data i in this._values)
            res += i.ToLongString(format) + "\n";

        return res;
    }

    public override string ToString()
    {
        string res = "";
        foreach (V1Data i in this._values)
            res += i.ToString() + "\n";

        return res;
    }

    IEnumerator<V1DataList> IEnumerable<V1DataList>.GetEnumerator()
    {
        var list = new List<V1Data>();
        foreach (var data in this._values)
        {
            if (data is V1DataList)
            {
                list.Add(data);
            }
        }
        if (list.Count == 0)
        {
            yield return null;
        }
        list.Sort(delegate (V1Data x, V1Data y)
        {
            if (x.Count == 0 && y.Count == 0) return 0;
            else if (x.Count == 0) return -1;
            else if (y.Count == 0) return 1;
            else return x.AverageValue.CompareTo(y.AverageValue);
        });

        foreach (var data in this._values)
        {
            yield return (V1DataList)data;
        }
    }

    IEnumerator<V1Data> IEnumerable<V1Data>.GetEnumerator()
    {
        var maxValue = this._values.Max(data => data.Count);
        foreach (var data in this._values)
        {
            if (data.Count == maxValue)
                yield return data;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var data in this._values)
        {
            yield return data;
        }
    }
}
