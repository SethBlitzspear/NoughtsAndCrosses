using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFNoughtsAndCrosses.Value_Converter
{
    public class SequenceToBitmap : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sequence = (string)value;
            int lineWidth = System.Convert.ToInt32((string)parameter);
            int width = 512;
            int height = 160;
            byte[] pixels = new byte[width * height * 4];
            //byte[] pixels = Enumerable.Repeat((byte)0x0F, 100 * 100 * 4).ToArray();
            BitmapSource source = BitmapSource.Create(width, height, 3, 3, PixelFormats.Pbgra32, null, pixels, width * 4);
            DrawingVisual visual = new DrawingVisual();

            SolidColorBrush background = Brushes.LightGreen;
            SolidColorBrush black = Brushes.Black;
            Pen pen = new Pen(black, lineWidth);

            byte[] block = new byte[1];
            block[0] = 0xDB;

            if (sequence != null)
            {

                List<double> Y = new List<double>();
                double currentY = 0;
                Y.Add(currentY);

                foreach (char item in sequence)
                {
                    switch (item)
                    {
                        case 'W':
                            currentY++;
                            break;
                        case 'L':
                            currentY--;
                            break;
                    }
                    Y.Add(currentY);
                }

                double XScale = 1;
                if (sequence.Length > width)
                {
                    XScale = (double)width / (double)sequence.Length;
                }

                double maxY = Y.Max() > 50? Y.Max() : 50;
                double minY = Y.Min() < -25? Y.Min() : -25;

                double YScale = (height - 20) / (maxY - minY);
                double YZero = (maxY * YScale) + 10;
                Y = Y.Select(y => y =  (maxY * YScale) - (y * YScale) + 10).ToList();

                double oldStartX = 10, oldStartY = YZero;
                double newStartX = 10, newStartY = YZero;
                Point oldPoint = new Point(oldStartX, oldStartY);
                Point newPoint = new Point(newStartX, newStartY);

                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(background, pen, new Rect(0, 0, width, height));
                    context.DrawLine(pen, new Point(10, 10), new Point(10, 150));
                    context.DrawLine(pen, oldPoint, new Point(502, YZero));

                    foreach (double newY in Y)
                    {
                        newStartX += XScale;
                        oldPoint = newPoint;
                        newPoint.X = newStartX;
                        newPoint.Y = newY;

                        context.DrawLine(pen, oldPoint, newPoint);
                    }

                    context.Close();
                }

            }
            else
            {
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(background, pen, new Rect(0, 0, width, height));
                    context.DrawLine(pen, new Point(10, 10), new Point(10, 150));
                    context.DrawLine(pen, new Point(10, 80), new Point(502, 80));
                }
            }

            DrawingImage board = new DrawingImage(visual.Drawing);
            return board;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
