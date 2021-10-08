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
    public class BoardIDToBitmap : IValueConverter
    {
        public BoardIDToBitmap()
        {
            bool no = false;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rotation =  System.Convert.ToInt32((string)parameter);
            int cell = 5;

            Encoding cp437 = Encoding.GetEncoding(437);
            byte[] block = new byte[1];
            block[0] = 0xDB;


            if (rotation == 0)
            {
                 cell = 12;
            }
           
            int width = cell * 3 + 4;
            string ID = value as string;
            ID = NeuralNet.RotateNode(ID, rotation);
            byte[] pixels = new byte[width * width * 4];
            //byte[] pixels = Enumerable.Repeat((byte)0x0F, 100 * 100 * 4).ToArray();
            BitmapSource source = BitmapSource.Create(width, width, 3, 3, PixelFormats.Pbgra32, null, pixels, width * 4);
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext context = visual.RenderOpen())
            {
                SolidColorBrush background = rotation == 0?Brushes.White:Brushes.MintCream;

                SolidColorBrush black = Brushes.Black;
               
                Pen pen = new Pen(black, 1);
                context.DrawRectangle(background, pen, new Rect(0, 0, width, width));

                context.DrawRectangle(black, pen, new Rect(cell + 1, 0, 1, width));
                context.DrawRectangle(black, pen, new Rect(cell * 2 + 2, 0, 1, width));
                context.DrawRectangle(black, pen, new Rect(0, cell + 1, width, 1));
                context.DrawRectangle(black, pen, new Rect(0, cell * 2 + 2, width, 1));


                for (int letterCount = 0; letterCount < 9; letterCount++)
                {
                    if (ID[letterCount] != ' ')
                    {
                        char let = rotation == 0? ID[letterCount] : cp437.GetString(block)[0];
                        double xPos = (letterCount % 3) * cell + (2 + letterCount % 3);
                        double yPos = Math.Floor((double)letterCount / 3) * cell + (1 + Math.Floor((double)letterCount / 3));
                        FormattedText marker = new FormattedText(let.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), cell - 1, ID[letterCount] == 'X' ? Brushes.Red : Brushes.Green, VisualTreeHelper.GetDpi(visual).PixelsPerDip);
                        context.DrawText(marker, new Point(xPos, yPos));
                    }
                }
                
                context.Close();
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
