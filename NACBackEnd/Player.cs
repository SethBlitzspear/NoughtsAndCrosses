using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACBackEnd
{
    public class Player
    {
        private string name;
        private SquareState marker;
        private bool aI;

        public string Name { get => name; set => name = value; }
        public SquareState Marker { get => marker; set => marker = value; }
        public bool AI { get => aI; set => aI = value; }
    }
}
