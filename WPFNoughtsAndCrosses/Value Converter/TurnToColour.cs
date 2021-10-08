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
    public class TurnToColour : IValueConverter
    {
        public TurnToColour()
        {
            
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int Turn = (int)value;
            switch (Turn)
            {
                case 1:
                    return Brushes.Khaki;
                case 2:
                    return Brushes.Chocolate;
                case 3:
                    return Brushes.GreenYellow;
                case 4:
                    return Brushes.SteelBlue;
                case 5:
                    return Brushes.Purple;
                case 6:
                    return Brushes.Gold;
                case 7:
                    return Brushes.DeepSkyBlue;
                case 8:
                    return Brushes.LightGreen;
                case 9:
                    return Brushes.PaleVioletRed;

                default:
                    return Brushes.Black;
            }

        }

       
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
