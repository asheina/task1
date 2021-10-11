using System;
using System.Collections.Generic;

public class V1MainCollection
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
}