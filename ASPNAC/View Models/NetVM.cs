using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNAC
{
    public class NetVM
    {
        private Net theNet;

        public string Name => TheNet.Name;
        public int Win => TheNet.Win;
        public int Draw => TheNet.Draw;
        public int Loss => TheNet.Loss;
        public string Sequence => TheNet.P1Sequence;

        public Net TheNet { get => theNet; set => theNet = value; }

        public NetVM (Net newNet)
        {
            TheNet = newNet;
        }
    }
}
