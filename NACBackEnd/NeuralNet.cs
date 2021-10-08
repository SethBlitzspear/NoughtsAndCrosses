using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace NACBackEnd
{
    public class NeuralNet
    {
        Dictionary<string, GameNode> nodes = new Dictionary<string, GameNode>();

        private GameNode baseNode, currentNode;
        private List<Turn> gameTrail = new List<Turn>();
        //private Player player1, player2;
        //private Player activeplayer;
        //bool gameActive;
        //bool gameWon;
       // private string name;
        private Net sqlNet;
        NACDataContext dataContext;
        private bool useNet, learning;

        public GameNode BaseNode { get => baseNode; set => baseNode = value; }
        public GameNode CurrentNode { get => currentNode; set => currentNode = value; }
        public  List<Turn> GameTrail { get => gameTrail; set => gameTrail = value; }
       /* public Player Player1 { get => player1; set => player1 = value; }
          public Player Player2 { get => player2; set => player2 = value; }
          public Player Activeplayer { get => activeplayer; set => activeplayer = value; }
          public bool GameActive { get => gameActive; set => gameActive = value; }
          public bool GameWon { get => gameWon; set => gameWon = value; } */
        public Dictionary<string, GameNode> Nodes { get => nodes; set => nodes = value; }
        public string Name { get => SqlNet.Name; set => SqlNet.Name = value; }
        public Net SqlNet { get => sqlNet; set => sqlNet = value; }

        public int Win => sqlNet.Win;
        public int Loss => sqlNet.Loss;
        public int Draw => sqlNet.Draw;
        public string P1Sequence => SqlNet.P1Sequence;
        public string P2Sequence => SqlNet.P2Sequence;

        public bool UseNet { get => useNet; set => useNet = value; }
        public bool Learning { get => learning; set => learning = value; }

        public NeuralNet(Net net, Node[] newNodes, NACDataContext newContext)
        {
            dataContext = newContext;
            
            NodeOptions.theNet = this;
            SqlNet = net;
            

            if (newNodes.Length == 0)
            {
                Board startingBoard = new Board(null);
                Node newSQLNode = createSQLNode(startingBoard);


                BaseNode = new GameNode(newSQLNode, this);
                startingBoard.TheNode = BaseNode;
                Nodes[startingBoard.ID] = BaseNode;
            }
            else
            {
                foreach(Node node in newNodes)
                {
                    GameNode newGameNode = new GameNode(node, this);
                    AddNode(newGameNode);
                }
            }
            BaseNode = Nodes["         "];
            currentNode = BaseNode;
            UseNet = true;
            Learning = true;
            /*  Player1 = new Player();
              Player1.Name = "Player 1";
              Player1.Marker = SquareState.Cross;
              Player1.AI = false;

              Player2 = new Player();
              Player2.Name = "Player 2";
              Player2.Marker = SquareState.Nought;
              Player2.AI = true;*/

        }

        public void DrawGame(bool P2AI)
        {
            foreach (Turn turn in GameTrail)
            {
                turn.TheNode.Weight++;
                Console.WriteLine("Adding 1 to " + turn.TheNode.SqlNode.Id);
                turn.TheNode.NeedsPersisting = true;
            }
            SqlNet.Draw++;
            if (P2AI)
            {
                SqlNet.P2Sequence += "D";
            }
            else
            {
                SqlNet.P1Sequence += "D";
            }
            dataContext.SubmitChanges();
        }

        public void EndGame(bool win, bool P2AI)
        {
            if (Learning)
            {
                if (win)
                {
                    foreach (Turn turn in GameTrail)
                    {
                        turn.TheNode.Weight+=3;
                        turn.TheNode.NeedsPersisting = true;
                        Console.WriteLine("Adding 3 to " + turn.TheNode.SqlNode.Id);
                    }
                    SqlNet.Win++;
                    if (P2AI)
                    {
                        SqlNet.P2Sequence += "W";
                    }
                    else
                    {
                        SqlNet.P1Sequence += "W";
                    }
                }
                else
                {
                    foreach (Turn turn in GameTrail)
                    {
                        if (turn.TheNode.Weight > 0)
                        {
                            turn.TheNode.Weight--;
                            turn.TheNode.NeedsPersisting = true;
                            Console.WriteLine("subtracting 1 to " + turn.TheNode.SqlNode.Id);

                        }

                    }
                    SqlNet.Loss++;
                    if (P2AI)
                    {
                        SqlNet.P2Sequence += "L";
                    }
                    else
                    {
                        SqlNet.P1Sequence += "L";
                    }
                }
            }
            dataContext.SubmitChanges();
            //string json = JsonConvert.SerializeObject(Nodes, Formatting.Indented);
            //System.IO.File.WriteAllText("neural.net", json);
        }

   /*     public void StartGame()
        {

            CurrentNode = BaseNode;
            GameTrail = new List<Turn>();
            Activeplayer = Player1;
            GameActive = true;
            GameWon = false;

        }*/

        public void AIMove(SquareState AIMarker, bool training)
        {
            SquareID chosenMove = currentNode.decideMove(AIMarker);
            if (currentNode.Options[(int)chosenMove] == null)
            {
                throw new Exception("null option return from AI move");
            }
            currentNode = Nodes[currentNode.Options[(int)chosenMove]];
            if (!training)
            {
                Turn lastTurn = new Turn();
                lastTurn.TheNode = currentNode;
                lastTurn.LastSquare = chosenMove;
                GameTrail.Add(lastTurn);
            }
        }

        public void PlayerMove(SquareID squareid, SquareState marker)
        {
            currentNode = currentNode.DoPlayerTurn(squareid, marker);
            Turn lastTurn = new Turn();
            lastTurn.TheNode = currentNode;
            lastTurn.LastSquare = squareid;
            //GameTrail.Add(lastTurn);
            
        }

       

        public GameNode GetNode(Board theBoard)
        {
            if (!Nodes.ContainsKey(theBoard.ID))
            {
                Node newSQLNode = createSQLNode(theBoard);
                
                GameNode newNode = new GameNode(newSQLNode, this);
                newNode.NewNode = true;
                AddNode(newNode);
            }
            GameNode theNode = Nodes[theBoard.ID];
            while(theBoard.ID != theNode.Theboard.ID)
            {
                theNode.Theboard.rotateBoard();
            }
            return theNode;
        }

        private Node createSQLNode(Board theBoard)
        {
            Node newSQLNode = new Node();
            newSQLNode.Id = theBoard.ID;
            
            newSQLNode.Weight = Convert.ToInt32( newSQLNode.Id.Count(c => c == ' ') / 2);
            if(newSQLNode.Weight < 1)
            {
                newSQLNode.Weight = 1;
            }
            newSQLNode.Turn = 10 - newSQLNode.Id.Count(c => c == ' ');
            newSQLNode.Net1 = SqlNet;
            dataContext.Nodes.InsertOnSubmit(newSQLNode);
            dataContext.SubmitChanges();
            return newSQLNode;
        }

        private void AddNode(GameNode newNode)
        {
            for (int rotationCount = 0; rotationCount < 4; rotationCount++)
            {
                Nodes[RotateNode(newNode.Theboard.ID, rotationCount)] = newNode;
            }
        }

        static public string RotateNode(string id, int rotation)
        {
            string rotatedID = id;
            char[] rotatedIDChar = new char[9];
            for (int rotateCount = 0; rotateCount < rotation; rotateCount++)
            {
                rotatedIDChar[0] = rotatedID[2];
                rotatedIDChar[1] = rotatedID[5];
                rotatedIDChar[2] = rotatedID[8];
                rotatedIDChar[3] = rotatedID[1];
                rotatedIDChar[4] = rotatedID[4];
                rotatedIDChar[5] = rotatedID[7];
                rotatedIDChar[6] = rotatedID[0];
                rotatedIDChar[7] = rotatedID[3];
                rotatedIDChar[8] = rotatedID[6];
                rotatedID = new string(rotatedIDChar);

            }
            return rotatedID;


        }

 /*       private bool CheckDraw()
        {
            if (CurrentNode.Theboard.BoardData.Contains(SquareState.Blank))
            {
                return false;
            }
            else
            {
                GameActive = false;
                return true;
            }
        }

        private bool CheckVictory()
        {

            bool win = false;


            for (int dimension1 = 0; dimension1 < 3; dimension1++)
            {
                if (!win && currentNode.Theboard.BoardData[dimension1] != SquareState.Blank)
                {
                    SquareState winningSquare = currentNode.Theboard.BoardData[dimension1];
                    win = true;

                    for (int dimension2 = 1; dimension2 < 3; dimension2++)
                    {
                        if (winningSquare != currentNode.Theboard.BoardData[dimension2 * 3 + dimension1])
                        {
                            win = false;
                        }
                    }
                }
                if (!win && currentNode.Theboard.BoardData[dimension1 * 3] != SquareState.Blank)
                {
                    SquareState winningSquare = currentNode.Theboard.BoardData[dimension1 * 3];
                    win = true;
                    for (int dimension2 = 1; dimension2 < 3; dimension2++)
                    {
                        if (winningSquare != currentNode.Theboard.BoardData[dimension2 + 3 * dimension1])
                        {
                            win = false;
                        }
                    }

                }
            }
            if (!win && currentNode.Theboard.BoardData[4] != SquareState.Blank)
            {
                if (currentNode.Theboard.BoardData[0] == currentNode.Theboard.BoardData[4] && currentNode.Theboard.BoardData[4] == currentNode.Theboard.BoardData[8] ||
                    currentNode.Theboard.BoardData[2] == currentNode.Theboard.BoardData[4] && currentNode.Theboard.BoardData[4] == currentNode.Theboard.BoardData[6])
                {
                    win = true;
                }
            }
            if (win)
            {
                GameActive = false;
                GameWon = true;
                EndGame(activeplayer.AI);
            }
            return win;
        }*/
    }

}
