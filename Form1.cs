using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CGL3
{
    internal delegate Color FilteringDelegate(PointF point, int highSize, int lowSize);
    public partial class Form1 : Form
    {
        readonly Graphics _origG;
        readonly ThreePoints _origPoints = new ThreePoints();
        readonly Graphics _transG;
        readonly ThreePoints _transPoints = new ThreePoints();
        private Bitmap _bitmapImage;
       
        private FilteringDelegate _filtering;
        private float _distortion;
        private readonly Bitmap[] _mipmap = new Bitmap[2];

        public Form1()
        {
            InitializeComponent();

            _origG = originalImage.CreateGraphics();
            _transG = transformedImage.CreateGraphics();
            originalImage.BackColor = Const.BackgroundColor;
            transformedImage.BackColor = Const.BackgroundColor;            
            _bitmapImage = new Bitmap(originalImage.Width, originalImage.Height);
            originalImage.Image = _bitmapImage;

        }

        private void OpenImageButtonClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = Const.Filter };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            _origG.Clear(Const.BackgroundColor);
            _origPoints.Clear();
            _transPoints.Clear();
            _transG.Clear(Const.BackgroundColor);
            var image = Image.FromFile(ofd.FileName);
            _bitmapImage = new Bitmap(originalImage.Width, originalImage.Height);
             
            double arx, ary;
            var coof =  image.Height/(double) image.Width;
            if (image.Height>image.Width)
            {
                arx = originalImage.Height / coof;
                ary = originalImage.Height;
            } else
            {
                arx = originalImage.Width;
                ary = originalImage.Height *coof;
            }

            var graphics = Graphics.FromImage(_bitmapImage);

            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, new Rectangle(0, 0, (int)(arx), (int)(ary)));
            graphics.Dispose();

            originalImage.Image = _bitmapImage;
        }

        public void GetMipmap(int lowSize)
        {                       
                for (var lvl = 0; lvl < 2; lvl++)
                {
                    
                    var width = (int)Math.Ceiling(Convert.ToDouble(originalImage.Width) / lowSize);
                    var height = (int)Math.Ceiling(Convert.ToDouble(originalImage.Height) / lowSize);
                    _mipmap[lvl] = new Bitmap(width, height);
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            // записываем усреднённое значение цвета для каждого пикселя нового уровня детализации
                            _mipmap[lvl].SetPixel(j, i, GetFromImage(j * lowSize, i * lowSize, lowSize));
                        }
                    }
                    lowSize *= 2;
                }            
        }

        private Color BiPixel(PointF pointF, int highSize, int lowSize)
        {            
            var botx = (int)Math.Floor(pointF.X);
            var boty = (int)Math.Floor(pointF.Y);
            var topx = botx+1;
            var topy = boty+1;
           
            var x = pointF.X;
            var y = pointF.Y;

            var tt = _bitmapImage.GetPixel(topx, topy);
            var tb = _bitmapImage.GetPixel(topx, boty);
            var bt = _bitmapImage.GetPixel(botx, topy);
            var bb = _bitmapImage.GetPixel(botx, boty);


            var red = (byte)((bb.R * (topx - x) + tb.R * (x - botx)) * (topy - y) +
                      (bt.R * (topx - x) + tt.R * (x - botx)) * (y - boty));
            var green = (byte)((bb.G * (topx - x) + tb.G * (x - botx)) * (topy - y) +
                       (bt.G * (topx - x) + tt.G * (x - botx)) * (y - boty));
            var blue = (byte)((bb.B * (topx - x) + tb.B * (x - botx)) * (topy - y) +
                       (bt.B * (topx - x) + tt.B * (x - botx)) * (y - boty));
            return Color.FromArgb(red, green, blue);
          
        }

        private Color TriPixel(PointF pointF, int highSize, int lowSize)
        {       
            var c1 = _mipmap[0].GetPixel((int)Math.Floor(pointF.X) / lowSize, (int)Math.Floor(pointF.Y) / lowSize);
            var c2 = _mipmap[1].GetPixel((int)Math.Floor(pointF.X) / highSize, (int)Math.Floor(pointF.Y) / highSize);

            var red = (byte)((c1.R * (highSize - _distortion) + c2.R * (_distortion - lowSize)) / lowSize);
            var green = (byte)((c1.G * (highSize - _distortion) + c2.G * (_distortion - lowSize)) / lowSize);
            var blue = (byte)((c1.B * (highSize - _distortion) + c2.B * (_distortion - lowSize)) / lowSize);
            return Color.FromArgb(red, green, blue);
            
        }

        public void FiltrationTransform(Matrix matrix)
        { 
             _distortion = GetDistortion(matrix, transformedImage.Width, transformedImage.Height);
            int lowLevel=0, highSize=0, lowSize =0;

            if (_distortion < 1)
            {
                _filtering = BiPixel;
            }
            else
            {                
                _filtering = TriPixel;
                lowLevel = -1;
                //меньший уровень детализации (более глубокий)
                highSize = 2;
                while (_distortion > highSize)
                {
                    highSize = highSize * 2;
                    lowLevel++;
                }
                //больший уровень
                lowSize = highSize / 2;
                GetMipmap(lowSize);
            }        
            var pointF = new PointF(0, 0);
            Color color;
            for (var i = 0; i < transformedImage.Height; i++)
            {
                for (var j = 0; j < transformedImage.Width; j++)
                {
                    pointF.X = j;
                    pointF.Y = i;
               
                    var originalPoint = matrix * pointF;
                   
                    if (originalPoint.X < 0 || Math.Ceiling(originalPoint.X )>= originalImage.Width || originalPoint.Y < 0 || Math.Ceiling(originalPoint.Y)>= originalImage.Height)
                 
                    {
                        color = Const.BackgroundColor;
                    }
                    else
                    {
                        color = _filtering(originalPoint,highSize,lowSize);
                    }
                    _transG.FillRectangle(new SolidBrush(color), new Rectangle(j, i, 1, 1));
                }
            }
            // отрисовать точки
            _transPoints.Draw(_transG);

        }

        static float PointsLength(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private static float GetDistortion( Matrix matrix, int wight, int height)
        {
            // искажение пространства по горизонтали
            var a = new PointF();
            var b = new PointF { X = wight };
     
            a = matrix * a; b = matrix * b;
            var distortionH = PointsLength(a, b) / wight;

            // по вертикали
            a = new PointF();
            b = new PointF {  Y = height};
            a = matrix * a; b = matrix * b;
            var distortionV = PointsLength(a, b) / height;

            return Math.Max(distortionH, distortionV);
        }

        private Color GetFromImage(int x, int y, int k)
        {
            int green, blue;
            var red = blue = green = 0;
            // пока в пределах пикселя и не выходим за рамки изображения
            for (var i = x; i < k+x && i  < originalImage.Width; i++)
            {
                for (var j = y; j < k+y && j  < originalImage.Height; j++)
                {
                    red += _bitmapImage.GetPixel(i , j).R;
                    green += _bitmapImage.GetPixel(i , j ).G;
                    blue += _bitmapImage.GetPixel(i , j ).B;
                }
            }
            return Color.FromArgb(red / k / k, green / k / k, blue / k / k);            
        }
        
        private void OriginalImageClick(object sender, MouseEventArgs e)
        {
            
            _origPoints.Add(new Point(e.X,e.Y));
            _origG.Clear(Const.BackgroundColor);
         
            _origG.DrawImage(_bitmapImage, 0, 0);
            _origPoints.Draw(_origG);        
        }

        private void TransformedImageClick(object sender, MouseEventArgs e)
        {
         
                _transPoints.Add(new Point(e.X, e.Y));
                _transG.Clear(Const.BackgroundColor);
             _transPoints.Draw(_transG);
            
        }

        private void TransformButtonClick(object sender, EventArgs e)
        {
            if (!_origPoints.IsFool() || !_transPoints.IsFool()) return;          
            FiltrationTransform(new Matrix(_transPoints, _origPoints));
        }                  
    }
}
