using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFNoughtsAndCrosses
{
    public class NodeVM
    {
        Node theNode;

        public string Name { get => theNode == null ? "XOXOXOXOX" : theNode.Id; }
        public int Weight { get => theNode == null ? -1: theNode.Weight; }
        public int Turn { get => theNode == null ? -1 : theNode.Turn; }
        public bool NodeValid { get => theNode == null ? false : true; }
        public int WieghtFontSize { get => theNode.Weight > 99 ? 10 : 15; }
        public NodeVM()
        {

        }
        public NodeVM(Node newNode)
        {
            theNode = newNode;
        }
    }
}
