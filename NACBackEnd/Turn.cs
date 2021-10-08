using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACBackEnd
{
   public class Turn
    {
        private GameNode theNode;
        private SquareID lastSquare;

        public GameNode TheNode { get => theNode; set => theNode = value; }
        public SquareID LastSquare { get => lastSquare; set => lastSquare = value; }
    }
}
