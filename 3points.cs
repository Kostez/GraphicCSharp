
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace CGL3
{
    public class ThreePoints
    {
        private readonly List<Point> _points = new List<Point>();

        public void Add(Point point)
        {
            _points.Add(point);
            if( _points.Count()>3){_points.RemoveAt(0);}
        }

        public void Draw(Graphics graphics)
        {
            
            for(int i = 0; i<_points.Count(); i++)
            {
                graphics.FillRectangle(Const.Brashes[i], _points[i].X - Const.Size / 2, _points[i].Y - Const.Size / 2, Const.Size, Const.Size);
            }

        }

        public bool IsFool()
        {
            return _points.Count() == 3;
        }
        public Point[] GetArray()
        {
            return _points.ToArray();
        }
        public void Clear()
        {
            _points.Clear();
        }
    }

    internal class Const
    {
      
        public static float Size = 6;
        public static Color BackgroundColor = Color.Black;
        public static string Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
        public static Brush[] Brashes = new Brush[] { new SolidBrush(Color.Red), new SolidBrush(Color.Green), new SolidBrush(Color.Blue) };
    }
}
