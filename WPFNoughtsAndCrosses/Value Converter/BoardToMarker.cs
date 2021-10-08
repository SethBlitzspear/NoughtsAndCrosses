using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFNoughtsAndCrosses.Value_Converter
{
    class BoardToMarker : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Board GameBoard = (Board)value;
            int SquareID = System.Convert.ToInt32(parameter);
            string ret = "";
            if(GameBoard.BoardData[SquareID] == SquareState.Cross)
            {
                ret = "X";
            }
            else if (GameBoard.BoardData[SquareID] == SquareState.Nought)
            {
                ret = "O";
            }
            else
            {
                ret = " ";
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
