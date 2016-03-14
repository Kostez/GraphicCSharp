
using System.Drawing;


namespace CGL3
{
    public class Matrix
    {
        private readonly double[] _contains= new double[9]; 

        public double GetByIndexs(int j,int i)
        {
            return _contains[(j - 1)*3 + i - 1];
        }

        public  Matrix(ThreePoints origPoints,ThreePoints transPoints)
        {


            var o = origPoints.GetArray();
            var t = transPoints.GetArray();
            var xx = (o[1].Y - o[0].Y) /(float) (o[1].X - o[0].X);
            var yy = (t[1].X - t[0].X) / (float)(o[1].X - o[0].X);
            var dx = o[2].X - o[0].X;
            var b = (t[2].X - t[0].X - yy * dx) / (o[2].Y - o[0].Y - xx * dx);
            var a = -xx * b + yy;
            var c = t[0].X - o[0].X * a - o[0].Y * b;
            _contains[0] = a;
            _contains[1] = b;
           _contains[2] = c;

           xx = (o[1].Y - o[0].Y) / (float)(o[1].X - o[0].X);
           yy = (t[1].Y - t[0].Y) / (float)(o[1].X - o[0].X);
            dx = o[2].X - o[0].X;
            b = (t[2].Y - t[0].Y - yy * dx) / (o[2].Y - o[0].Y - xx * dx);
            a = -xx * b + yy;
            c = t[0].Y - o[0].X * a - o[0].Y * b;
            _contains[3] = a;
            _contains[4] = b;
            _contains[5] = c;

            _contains[6] = 0;
            _contains[7] = 0;
            _contains[8] = 1;           
        }

        public static PointF operator *(Matrix matrix, PointF pointF)
        {
            return new PointF
                          {
                              X =
                                  (float) matrix.GetByIndexs(1, 1)*pointF.X + (float) matrix.GetByIndexs(1, 2)*pointF.Y +
                                  (float) matrix.GetByIndexs(1, 3),
                              Y =
                                  (float) matrix.GetByIndexs(2, 1)*pointF.X + (float) matrix.GetByIndexs(2, 2)*pointF.Y +
                                  (float) matrix.GetByIndexs(2, 3)
                          };

            
        }
    }
}
