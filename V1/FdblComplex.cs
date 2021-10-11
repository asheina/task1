public delegate System.Numerics.Complex FdblComplex(double x, double y);

public static class Fdbl
{
    public static System.Numerics.Complex TestComplexF(double x, double y)
    {
        return new System.Numerics.Complex(x, y);
    }
}