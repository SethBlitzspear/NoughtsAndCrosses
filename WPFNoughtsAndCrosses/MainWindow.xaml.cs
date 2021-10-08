using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFNoughtsAndCrosses
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameConnectionVM gameConnectionVM;
        public MainWindow()
        {
            InitializeComponent();
            gameConnectionVM = new GameConnectionVM();
            DataContext = gameConnectionVM;
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            gameConnectionVM.CreateGame();
        }
        private void TrainingGameButton_Click(object sender, RoutedEventArgs e)
        {
            gameConnectionVM.TrainAI(true, true);
        }
        private void AIMoveButton_Click(object sender, RoutedEventArgs e)
        {
            gameConnectionVM.DoAIMove();
        }

        private void TextBlock_MouseUp(object sender, RoutedEventArgs e)
        {
            if(gameConnectionVM.IsAIMove == false)
            {
                Button clicked = (Button)sender;
                if((string)clicked.Content != "X" && (string)clicked.Content != "O")
                {
                    SquareID clickedSquare = SquareID.TopLeft;
                    switch(clicked.Name)
                    {
                        case "TL":
                            clickedSquare = SquareID.TopLeft;
                            break;
                        case "TC":
                            clickedSquare = SquareID.TopCenter;
                            break;
                        case "TR":
                            clickedSquare = SquareID.TopRight;
                            break;
                        case "CL":
                            clickedSquare = SquareID.CenterLeft;
                            break;
                        case "CC":
                            clickedSquare = SquareID.CenterCenter;
                            break;
                        case "CR":
                            clickedSquare = SquareID.CenterRight;
                            break;
                        case "BL":
                            clickedSquare = SquareID.BottomLeft;
                            break;
                        case "BC":
                            clickedSquare = SquareID.BottomCenter;
                            break;
                        case "BR":
                            clickedSquare = SquareID.BottomRight;
                            break;
                    }
                    gameConnectionVM.DoPlayerMove(clickedSquare);
                }
                
            }
        }

        private void NewNet_Click(object sender, RoutedEventArgs e)
        {
            if (NewNetNameTextBox.Text != "")
            {
                gameConnectionVM.CreateNet(NewNetNameTextBox.Text);
            }
        }

        private void AITrainMultiple_Click(object sender, RoutedEventArgs e)
        {
            Button clicker = (Button)sender;
            int max = Convert.ToInt32(((string)clicker.Content).Substring(0, 2));
            Thread trainThread = new Thread(() => MultiTrain(max));
            trainThread.Start();
        }

        private void MultiTrain(int max)
        {
            for (int trainCount = 0; trainCount < max; trainCount++)
            {
                bool updateUI = false, updateNodes = false;
                if (trainCount == max - 1 )
                {
                    updateUI = true;
                    updateNodes = true;
                }
                if(trainCount % 10 == 0)
                {
                    updateUI = true;
                }
                gameConnectionVM.TrainAI(true, updateNodes);
            }
        }
    }
}
