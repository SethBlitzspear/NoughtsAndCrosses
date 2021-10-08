using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPFNoughtsAndCrosses.Value_Converter
{
    class SquareOpenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Board GameBoard = (Board)value;
            int SquareID = System.Convert.ToInt32(parameter);

            if (GameBoard.BoardData[SquareID] == SquareState.Cross)
            {
                return false;
            }
            else if (GameBoard.BoardData[SquareID] == SquareState.Nought)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
