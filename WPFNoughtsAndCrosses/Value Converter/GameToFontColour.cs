using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFNoughtsAndCrosses.Value_Converter
{
    class GameToFontColour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Game theGame = (Game)value;
            int SquareID = System.Convert.ToInt32(parameter);
            if (theGame != null)
            {
                Board GameBoard = theGame.CurrentNode.Theboard;
                if (theGame.GameWon)
                {
                    if (theGame.Conceeded)
                    {
                        if (GameBoard.BoardData[SquareID] == theGame.Activeplayer.Marker)
                        {
                            return Brushes.Red;
                        }
                        else
                        {
                            return Brushes.Green;
                        }
                    }
                    else
                    {
                        if (GameBoard.BoardData[SquareID] == theGame.Activeplayer.Marker)
                        {
                            return Brushes.Green;
                        }
                        else
                        {
                            return Brushes.Red;
                        }
                    }
                }
                else
                {
                    return Brushes.Black;
                }
            }
            else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
