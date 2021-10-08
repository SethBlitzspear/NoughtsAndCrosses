using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFNoughtsAndCrosses
{
    public class GameVM : BaseVM
    {

        public static bool Player2AI = false;
        private Game theGame;

        public override void VMUpdated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public GameVM(Game newGame)
        {
            theGame = newGame;
        }
    }
}
