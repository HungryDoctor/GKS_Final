using System.Collections.Generic;

namespace GKS
{
    public struct PointInt
    {
        public int X;
        public int Y;

        public PointInt(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public class MatrixWithHeaders
    {
        public Dictionary<int, string> Headers { get; set; }
        public int[,] Matrix { get; set; }

        public MatrixWithHeaders(int[,] matrix, Dictionary<int, string> headers)
        {
            this.Matrix = matrix;
            this.Headers = headers;
        }
    }

}