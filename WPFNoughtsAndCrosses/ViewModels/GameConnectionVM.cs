using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFNoughtsAndCrosses
{


    public class GameConnectionVM : BaseVM
    {
        private GameConnection conn;
       
        public ObservableCollection<NetVM> Nets => new ObservableCollection<NetVM>(Conn.TheNets.Select((item) => new NetVM(item)));
        public ObservableCollection<NodeVM> Nodes => new ObservableCollection<NodeVM>(Conn.TheNodes.Select((item) => new NodeVM(item)));
        public ObservableCollection<NodeVM> GameTrail { get => Conn.ActiveNeuralNet == null ? new ObservableCollection<NodeVM>() : new ObservableCollection<NodeVM>(Conn.ActiveNeuralNet.GameTrail.Select((item) => new NodeVM(item.TheNode.SqlNode))); }
        public ObservableCollection<NodeVM> OptionNodes { get => Conn.ActiveNeuralNet == null ? new ObservableCollection<NodeVM>() : new ObservableCollection<NodeVM>(Conn.ActiveNeuralNet.CurrentNode.Options.Select((item) => createNodeVM(item))); }

        private NodeVM createNodeVM(string item)
        {
            if (item == null)
            {
                return new NodeVM();
            }
            else
            {
                return new NodeVM(conn.ActiveNeuralNet.GetNode(new Board(item, null)).SqlNode);
            }

        }


        
        public bool CanStartGame { get => conn.ActiveGame == null ? false : conn.ActiveGame.GameActive == false || conn.ActiveGame.GameStarted ? true : false; }
        public bool CanTrain { get => conn.ActiveGame == null ? false : conn.ActiveGame.GameActive == true && !conn.ActiveGame.GameStarted ? true : false; }
        public Visibility Connected { get => Conn.ActiveNet == null ? Visibility.Hidden : Visibility.Visible; }
        public NeuralNet ActiveNeuralNet => conn.ActiveNeuralNet;
        public string Sequence { get => GameVM.Player2AI ? P2Sequence : P1Sequence; }
        public string P1Sequence { get => conn.ActiveNeuralNet == null ? null : conn.ActiveNeuralNet.P1Sequence; }
        public string P2Sequence { get => conn.ActiveNeuralNet == null ? null : conn.ActiveNeuralNet.P2Sequence; }
        /*public bool Player1AI 
        { 
            get => conn.ActiveGame == null ? false : conn.ActiveGame.Player1.AI;
            set
            {
                conn.ActiveGame.Player1.AI = value;
                conn.ActiveGame.Player2.AI = !value;
                OnPropertyChanged("IsAIMove");
                OnPropertyChanged("Player2AI");
            }
        }*/

        public string P1Type { get => conn.ActiveGame == null ? "MENACE" : conn.ActiveGame.Player1.AI ? "MENACE" : "Human"; }
        public string P2Type { get => conn.ActiveGame == null ? "Human" : conn.ActiveGame.Player1.AI ? "Human" : "MENACE"; }
        public bool Player2AI 
        { 
            get => GameVM.Player2AI;
            set
            {
                if (ActiveGame != null)
                {
                    conn.ActiveGame.Player2.AI = value;
                    conn.ActiveGame.Player1.AI = !value;
                }
                    GameVM.Player2AI = value;
                    OnPropertyChanged("IsAIMove");
                OnPropertyChanged("P1Type");
                OnPropertyChanged("P2Type");
                OnPropertyChanged("Nets");
                OnPropertyChanged("Sequence");

            }
        }
        public Visibility GameActive { get => Conn.ActiveGame == null ? Visibility.Hidden : Visibility.Visible; }
        public Board GameBoard { get => conn.ActiveGame == null ? new Board("         ", null) : conn.ActiveGame.CurrentNode.Theboard; }
        public bool IsAIMove { get => conn.ActiveGame == null ? false : conn.ActiveGame.Activeplayer.AI && conn.ActiveGame.GameActive; }
        public NetVM ActiveNet
        {
            set
            {
                if (value != null)
                {
                    Conn.ActiveNet = value.TheNet;
                    conn.StartGame(GameVM.Player2AI);
                    OnPropertyChanged("Nodes");
                    OnPropertyChanged("Connected");
                    OnPropertyChanged("GameActive");
                    OnPropertyChanged("Player1AI");
                    OnPropertyChanged("Player2AI");
                    OnPropertyChanged("TurnString");
                    OnPropertyChanged("IsAIMove");
                    OnPropertyChanged("CanStartGame");
                    OnPropertyChanged("CanTrain");
                    OnPropertyChanged("Sequence");
                    OnPropertyChanged("OptionNodes");
                }
            }
        }

        internal void CreateNet(string newNetName)
        {
            conn.CreateNet(newNetName);
            OnPropertyChanged("Nets");
        }

        public string TurnString
        {
            get
            {
                if (conn.ActiveGame == null)
                {
                    return "No Game";
                }
                else
                {
                    if (conn.ActiveGame.GameActive)
                    {
                        return conn.ActiveGame.Activeplayer.Name + "'s turn";
                    }
                    else
                    {
                        if (Conn.ActiveGame.GameWon)
                        {
                            if (Conn.ActiveGame.Conceeded)
                            {
                                return "Game Over " + conn.ActiveGame.Activeplayer.Name + " conceeded";
                            }
                            else
                            {
                                return "Game Over " + conn.ActiveGame.Activeplayer.Name + " won";
                            }
                        }
                        else
                        {
                            return "Game Over, it was a Draw";
                        }
                    }
                }
            }
        }

        public void CreateGame()
        {

            ActiveGame.StartGame();
            OnPropertyChanged("GameActive");
            OnPropertyChanged("Player1AI");
            OnPropertyChanged("Player2AI");
            OnPropertyChanged("TurnString");
            OnPropertyChanged("IsAIMove");
            OnPropertyChanged("CanStartGame");
            OnPropertyChanged("CanTrain");
            OnPropertyChanged("GameTrail");
            OnPropertyChanged("OptionNodes");
            OnPropertyChanged("GameBoard");
        }

        public void TrainAI(bool updateAI, bool updateNodes)
        {
            long now = DateTime.Now.Ticks;
            ActiveGame.TrainAI();
            if (updateAI)
            {
                OnPropertyChanged("GameActive");
                OnPropertyChanged("TurnString");
                OnPropertyChanged("IsAIMove");
                OnPropertyChanged("CanStartGame");
                OnPropertyChanged("CanTrain");
                OnPropertyChanged("GameBoard");
                OnPropertyChanged("ActiveGame");
                OnPropertyChanged("Sequence");

                if(updateNodes)
                {
                    OnPropertyChanged("Nodes");
                    OnPropertyChanged("Nets");
                    OnPropertyChanged("GameTrail");
                    OnPropertyChanged("OptionNodes");
                }
            }
             now = now - DateTime.Now.Ticks;
            Console.WriteLine("AI Trained in " + now / 10000 + " " + updateAI);
        }

        public void DoAIMove()
        {
            
            Conn.ActiveGame.AIMove();
            OnPropertyChanged("GameBoard");
            OnPropertyChanged("TurnString");
            OnPropertyChanged("IsAIMove");
            OnPropertyChanged("CanStartGame");
            OnPropertyChanged("CanTrain");
            OnPropertyChanged("GameTrail");
            OnPropertyChanged("ActiveGame");
            if (conn.ActiveGame.GameWon)
            {
                OnPropertyChanged("Nodes");
                OnPropertyChanged("Sequence");
                OnPropertyChanged("Nets");
            }
        }

        public void DoPlayerMove(SquareID square)
        {
            Conn.ActiveGame.GameActive = true;
            Conn.ActiveGame.PlayerMove(square);
            OnPropertyChanged("GameBoard");
            OnPropertyChanged("TurnString");
            OnPropertyChanged("IsAIMove");
            OnPropertyChanged("OptionNodes");
            OnPropertyChanged("CanStartGame");
            OnPropertyChanged("CanTrain");
            OnPropertyChanged("ActiveGame");
            
            if (conn.ActiveGame.GameWon)
            {
                OnPropertyChanged("Nodes");
                OnPropertyChanged("Sequence");
                OnPropertyChanged("Nets");
            }
        }

        public GameConnection Conn { get => conn; set => conn = value; }
        public Game ActiveGame { get => conn.ActiveGame; set => conn.ActiveGame = value; }

        public GameConnectionVM()
        {
            Conn = new GameConnection();
            
        }

        public override void VMUpdated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
