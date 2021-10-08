using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NACBackEnd
{
    public class GameNode
    {
        Board theboard;

        string[] options = new string[9];
        
        Node sqlNode;
        bool needsPersisting;
        bool newNode;
       
        NeuralNet theNet;

        public Board Theboard { get => theboard; set => theboard = value; }
        public int Weight { get => SqlNode.Weight; set => SqlNode.Weight = value; }
        public string[] Options { get => options; set => options = value; }
        [JsonIgnore]
        public NeuralNet TheNet { get => theNet; set => theNet = value; }
        public bool NeedsPersisting { get => needsPersisting; set => needsPersisting = value; }
        public bool NewNode { get => newNode; set => newNode = value; }
        public Node SqlNode { get => sqlNode; set => sqlNode = value; }

        public GameNode(Node state,  NeuralNet baseNet)
        {
            SqlNode = state;
            Theboard = new Board(SqlNode.Id, this);
            //Weight = 3;
            TheNet = baseNet;
        }

        private SquareState GetOtherState(SquareState oldState)
        {
            if (oldState == SquareState.Cross)
            {
                return SquareState.Nought;
            }
            else if (oldState == SquareState.Nought)
            {
                return SquareState.Cross;
            }
            else
            {
                throw new Exception("Invalid other state");
            }
        }

        public void BuildOptions(SquareState state)
        {
            for (int squareCount = 0; squareCount < 9; squareCount++)
            {
                if (Theboard.getSquareState((SquareID)squareCount) == SquareState.Blank)
                {
                    if (Options[squareCount] == null)
                    {
                        Options[squareCount] = CreateNode((SquareID)squareCount, state).Theboard.ID;
                    }
                   
                }
            }
        }

        public SquareID decideMove(SquareState state)
        {
            List<SquareID> AIOptions = new List<SquareID>();
            int totalWeight = 0;
            for (int squareCount = 0; squareCount < 9; squareCount++)
            {
                if (Theboard.getSquareState((SquareID) squareCount) == SquareState.Blank)
                {
                    if(Options[squareCount] == null)
                    {
                        Options[squareCount] = CreateNode((SquareID)squareCount, state).Theboard.ID;
                    }
                    totalWeight += TheNet.UseNet ? TheNet.Nodes[Options[squareCount]].Weight : 1;
                    for (int squareIDCount = 0; squareIDCount < TheNet.Nodes[Options[squareCount]].Weight; squareIDCount++)
                    {
                        AIOptions.Add((SquareID)squareCount);
                    }
                }
            }
            if(totalWeight == 0)
            {
                throw new Exception("No Options Available");
            }
            int randomNumber = new Random().Next(totalWeight);


            return AIOptions[randomNumber];
        }

        private GameNode CreateNode(SquareID id, SquareState state)
        {
            Board newBoard = new Board(Theboard.ID, this);
            newBoard.setSquareState(id, state);
            return TheNet.GetNode(newBoard);
            
        }

        public GameNode DoPlayerTurn(SquareID id, SquareState state)
        {
            if (Options[(int)id] == null)
            {                
                Options[(int)id] = CreateNode(id, state).Theboard.ID;
            }
            return TheNet.Nodes[Options[(int)id]];
        }

    }
}
