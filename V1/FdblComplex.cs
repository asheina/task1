using System;
public delegate System.Numerics.Complex FdblComplex(double x, double y);

public static class Fdbl
{
    public static System.Numerics.Complex TestComplexF(double x, double y)
    {
        var r = new Random();
        return new System.Numerics.Complex(r.NextDouble() * 10, r.NextDouble() * 10);
    }
}