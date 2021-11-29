using System;
using System.Collections;
using System.Collections.Generic;

public class V1DataArray : V1Data
{
    public int nX { get; private set; }
    public int nY { get; private set; }

    public double stepX { get; private set; }
    public double stepY { get; private set; }
    public System.Numerics.Complex[,] matrix { get; private set; }
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


    static bool SaveAsText(string filename) //экземплярный должен быть
    {

    }

    bool LoadAsText(string filename, ref V1DataArray v1) //too
    {

    }
}