using System;
using System.Collections;
using System.Collections.Generic;

public abstract class V1Data : IEnumerable<DataItem>
{
    public string type { get; protected set; }
    public System.DateTime createdAt { get; protected set; }
    public abstract int Count { get; }
    public abstract double AverageValue { get; }
    public abstract string ToLongString(string format);
    public V1Data(string type, System.DateTime createdAt)
    {
        this.type = type;
        this.createdAt = createdAt;
    }

    public abstract override string ToString();

    public abstract IEnumerator<DataItem> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}