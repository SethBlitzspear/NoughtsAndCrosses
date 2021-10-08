using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNAC
{
    public class NodeVM
    {
        Node theNode;

        public string Name { get => theNode == null ? "XOXOXOXOX" : theNode.Id; }
        public int Weight { get => theNode == null ? -1: theNode.Weight; }
        public bool NodeValid { get => theNode == null ? false : true; }
        public NodeVM()
        {

        }
        public NodeVM(Node newNode)
        {
            theNode = newNode;
        }
    }
}
