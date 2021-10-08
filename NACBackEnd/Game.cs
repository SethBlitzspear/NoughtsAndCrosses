using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACBackEnd
{
    public class Game
    {
        NeuralNet theNet;
        private Player player1, player2;
        private Player activeplayer;
        bool gameActive, gameStarted;
        bool gameWon;
        bool training = false;
        bool conceeded = false;

        public Player Player1 { get => player1; set => player1 = value; }
        public Player Player2 { get => player2; set => player2 = value; }
        public Player Activeplayer { get => activeplayer; set => activeplayer = value; }
        public bool GameActive { get => gameActive; set => gameActive = value; }
        public bool GameWon { get => gameWon; set => gameWon = value; }
            public GameNode CurrentNode { get => theNet.CurrentNode; }
        public bool Conceeded { get => conceeded; set => conceeded = value; }
        public bool GameStarted { get => gameStarted; set => gameStarted = value; }

        public Game (NeuralNet newNet, bool player2AI)
        {
            theNet = newNet;

            Player1 = new Player();
            Player1.Name = "Player 1";
            Player1.Marker = SquareState.Cross;
            Player1.AI = !player2AI;

            Player2 = new Player();
            Player2.Name = "Player 2";
            Player2.Marker = SquareState.Nought;
            Player2.AI = player2AI;

            StartGame();

        }

        public void TrainAI()
        {
            StartGame();
            training = true;
            theNet.CurrentNode.BuildOptions(Activeplayer.Marker);
            while (GameActive)
            {
                training = !Activeplayer.AI;
                    AIMove();
                    if (GameActive)
                    {
                        theNet.CurrentNode.BuildOptions(Activeplayer.Marker);
                    }
            }
        }
        public void StartGame()
        {

            theNet.CurrentNode = theNet.BaseNode;
            theNet.GameTrail = new List<Turn>();
            Activeplayer = Player1;
            GameActive = true;
            GameWon = false;
            Conceeded = false;
            GameStarted = false;
            theNet.CurrentNode.BuildOptions(Activeplayer.Marker);
        }

        public void PlayerMove(SquareID squareID)
        {
            theNet.PlayerMove(squareID, Activeplayer.Marker);
            changePlayer();
            theNet.CurrentNode.BuildOptions(Activeplayer.Marker);
            GameStarted = true;
        /*    if(Activeplayer.AI)
            {
                theNet.AIMove(Activeplayer.Marker);
                changePlayer();
            }*/

        }

        private void changePlayer()
        {
            if (!CheckVictory() && !CheckDraw())
            {
                if (Activeplayer == Player1)
                {
                    Activeplayer = Player2;
                }
                else
                {
                    Activeplayer = Player1;
                }
            }
        }

        public void AIMove()
        {
            try
            {
                theNet.AIMove(Activeplayer.Marker, training);
                changePlayer();
                gameStarted = true;
            }
            catch (Exception e)
            {
                GameActive = false;
                GameWon = true;
                theNet.EndGame(false, Player2.AI);
                Conceeded = true;
            }

        }

        private bool CheckDraw()
        {
            if (CurrentNode.Theboard.BoardData.Contains(SquareState.Blank))
            {
                return false;
            }
            else
            {
                GameActive = false;
                theNet.DrawGame(Player2.AI);
                return true;
            }
        }
        


        private bool CheckVictory()
        {

            bool win = false;


            for (int dimension1 = 0; dimension1 < 3; dimension1++)
            {
                if (!win && CurrentNode.Theboard.BoardData[dimension1] != SquareState.Blank)
                {
                    SquareState winningSquare = CurrentNode.Theboard.BoardData[dimension1];
                    win = true;

                    for (int dimension2 = 1; dimension2 < 3; dimension2++)
                    {
                        if (winningSquare != CurrentNode.Theboard.BoardData[dimension2 * 3 + dimension1])
                        {
                            win = false;
                        }
                    }
                }
                if (!win && CurrentNode.Theboard.BoardData[dimension1 * 3] != SquareState.Blank)
                {
                    SquareState winningSquare = CurrentNode.Theboard.BoardData[dimension1 * 3];
                    win = true;
                    for (int dimension2 = 1; dimension2 < 3; dimension2++)
                    {
                        if (winningSquare != CurrentNode.Theboard.BoardData[dimension2 + 3 * dimension1])
                        {
                            win = false;
                        }
                    }

                }
            }
            if (!win && CurrentNode.Theboard.BoardData[4] != SquareState.Blank)
            {
                if (CurrentNode.Theboard.BoardData[0] == CurrentNode.Theboard.BoardData[4] && CurrentNode.Theboard.BoardData[4] == CurrentNode.Theboard.BoardData[8] ||
                    CurrentNode.Theboard.BoardData[2] == CurrentNode.Theboard.BoardData[4] && CurrentNode.Theboard.BoardData[4] == CurrentNode.Theboard.BoardData[6])
                {
                    win = true;
                }
            }
            if (win)
            {
                GameActive = false;
                GameWon = true;
                theNet.EndGame(activeplayer.AI, Player2.AI);
            }
            return win;
        }
    }
}
