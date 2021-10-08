using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Linq;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace NACBackEnd
{
    public class GameConnection
    {
        NACDataContext NACDB = new NACDataContext();
        private Net activenet;
        private NeuralNet activeNeuralNet;
        private Game activeGame;

        public List<Net> TheNets => NACDB.Nets.ToList();
        public List<Node> TheNodes
        {
            get => activeGame == null || activeGame.Player1.AI ? NACDB.Nodes.Where(node => (node.Net1 == activenet && (node.Turn == 0 || node.Turn == 2 || node.Turn == 4 || node.Turn == 6 || node.Turn == 8 || node.Turn == 10))).OrderBy(node => node.Turn).ToList() 
                : NACDB.Nodes.Where(node => (node.Net1 == activenet && (node.Turn == 0 || node.Turn == 1 || node.Turn == 3 || node.Turn == 5 || node.Turn == 7 || node.Turn == 9))).OrderBy(node => node.Turn).ToList();
        }
       
        public GameConnection()
        {

        }

        public NeuralNet getNet(string netName)
        {
            if(NACDB.Nets.Count(net => net.Name == netName) == 0)

            {
                CreateNet(netName);
                
            }
            Net theNet = NACDB.Nets.First(net => net.Name == netName);
            activenet = theNet;
            Node[] theNodes = theNet.Nodes.ToArray();
            return new NeuralNet(theNet, theNodes, NACDB);
        }
        
        public string[] GetNetNames
        {
            get
            {
                List<string> NetNames = new List<string>();
                foreach(Net net in NACDB.Nets)
                {
                    NetNames.Add(net.Name);
                }
                return NetNames.ToArray();
            }
        }

        public Net ActiveNet 
        { 
            get => activenet;
            set
            {
                activenet = value;
                ActiveNeuralNet = getNet(activenet.Name);
            }
        }

        public NeuralNet ActiveNeuralNet { get => activeNeuralNet; set => activeNeuralNet = value; }
        public Game ActiveGame { get => activeGame; set => activeGame = value; }

        public void CreateNet(string netName)
        {
            Net newNet = new Net();
            newNet.Name = netName;
            Node BaseNode = new Node();
            BaseNode.Id = "         ";
            BaseNode.Net1 = newNet;
            NACDB.Nodes.InsertOnSubmit(BaseNode);
            NACDB.Nets.InsertOnSubmit(newNet);
            NACDB.SubmitChanges();
        }


        public void StartGame(bool player2AI)
        {
            ActiveGame = new Game(ActiveNeuralNet, player2AI);
            
        }

        
    }
}
