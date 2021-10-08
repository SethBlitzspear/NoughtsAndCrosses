using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACBackEnd
{
    public class NodeOptions
    {
        private string[] indexes = new string[9];
        public static NeuralNet theNet;
        public GameNode this[int i]
        {
            get
            {
                return theNet.Nodes[indexes[i]];
            }
            set
            {
                theNet.Nodes[indexes[i]] = value;
            }
        }

    }
}
