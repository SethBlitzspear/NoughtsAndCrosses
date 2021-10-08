using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFNoughtsAndCrosses
{
    public class NetVM
    {
        private Net theNet;

        public string Name => TheNet.Name;
        public int Win => TheNet.Win;
        public int Draw => TheNet.Draw;
        public int Loss => TheNet.Loss;
        public string Sequence { get => GameVM.Player2AI ? TheNet.P2Sequence : TheNet.P1Sequence; }
        public int NodeNumber => TheNet.Nodes.Count;

        public Net TheNet { get => theNet; set => theNet = value; }

        public NetVM (Net newNet)
        {
            TheNet = newNet;
        }
    }
}
