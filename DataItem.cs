using System;

public struct DataItem
{
    public double x { get; set; }
    public double y { get; set; }
    public System.Numerics.Complex value { get; set; }
    public DataItem(double x, double y, System.Numerics.Complex val)
    {
        this.x = x;
        this.y = y;
        this.value = val;
    }
    public string ToLongString(string format)
    {
        string res = "(x, y) : (" + String.Format(format, x) + ", " +
                String.Format(format, y) +
                ")\n";
        res += "value = " + value.ToString() +
                " |value| = " + System.Numerics.Complex.Abs(value) +
                "\n";
        return res;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}