using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoughtsAndCrosses
{
    class Program
    {
        static int gameSize = 3;
        static char[][] boardData;
        static int currentX = 0;
        static int currentY = 0;
        static bool gameOn = false;
        static bool gameWon = false;
        static bool programOn = true;
        static int activePlayer;
        static int gameBoardMargin = 5;
        static int textWindowWidth = 30;
        static char borderCharacter = '#';
        static Player[] players = new Player[2];
        static int winningPlayer;
        

        struct Player
        {

            public string Name;
            public char Marker;

        }
        enum MainMenuChoice
        {
            NO_OPTION = 0,
            LOAD_GAME = 1,
            SAVE_GAME = 2,
            SET_GAME_OPTIONS = 3,
            START_NEW_GAME = 4,
            QUIT_GAME = 5,
            RETURN_TO_GAME = 6


        } 

        static void DrawBoard()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Clear();

            DrawFrame();
            if (gameOn)
            {
                for (int verticalCount = 0; verticalCount < gameSize; verticalCount++)
                {
                    for (int horizontalCount = 0; horizontalCount < gameSize; horizontalCount++)
                    {
                        if (currentX == horizontalCount && currentY == verticalCount)
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {

                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.SetCursorPosition((horizontalCount * 2) + 5, (verticalCount * 2) + 5);
                        Console.Write(boardData[verticalCount][horizontalCount]);
                        if (horizontalCount != gameSize - 1)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("|");
                        }

                    }
                    if (verticalCount != gameSize - 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(5, (verticalCount * 2) + 6);
                        for (int horizontalCount = 0; horizontalCount < gameSize; horizontalCount++)
                        {
                            Console.Write("-");
                            if (horizontalCount != gameSize - 1)
                            {
                                Console.Write("+");
                            }
                        }
                    }
                    Console.SetCursorPosition(5 + (currentX * 2), 5 + (currentY * 2));
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;

                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 2);
                Console.Write("Move the cursor to the");
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 3);
                Console.Write("square you want?");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 5);
                Console.Write("'w' moves you up");
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 6);
                Console.Write("'s' moves you down");
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 7);
                Console.Write("'a' moves you left");
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 8);
                Console.Write("'d' moves you right");
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 9);
                Console.Write("enter key to choose square");
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 10);
                Console.Write("q to return to menu");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 12);
                Console.Write("It is " + players[activePlayer].Name + "'s go");
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 13);
                Console.Write("Place your " + players[activePlayer].Marker);


            }
        }

        private static void DrawFrame()
        {
            int screenSize = (2 * gameBoardMargin) + (gameSize * 2) - 1;
            for (int verticalPosition = 0; verticalPosition < screenSize; verticalPosition++)
            {
                Console.SetCursorPosition(0, verticalPosition);
                if(verticalPosition == 0 || verticalPosition == screenSize - 1)
                {
                    Console.Write(CreateHorizontalBar());
                }
                else
                {
                    Console.Write(borderCharacter);
                    Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) - 1, verticalPosition);
                    Console.Write(borderCharacter);
                    Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + textWindowWidth, verticalPosition);
                    Console.Write(borderCharacter);
                }
            }
        }

        private static string CreateHorizontalBar()
        {
            string bar = "";

            for (int barCount = 0; barCount < 5; barCount++)
            {
                bar += borderCharacter;
            }
            for (int barCount = 0; barCount < gameSize + gameSize - 1; barCount++)
            {
                bar += borderCharacter;
            }
            for (int barCount = 0; barCount < 36; barCount++)
            {
                bar += borderCharacter;
            }

                return bar;
        }

        static void Main(string[] args)
        {
            InitialSetup();
            bool gamePlayed = false;
            DrawBoard();

            while (programOn)
            {
                MainMenuChoice mainMenuOption = GetMainMenuChoice(gamePlayed);

                switch (mainMenuOption)
                {
                    case MainMenuChoice.LOAD_GAME:
                        LoadGame();
                        gameOn = true;
                        break;

                    case MainMenuChoice.SAVE_GAME:
                        SaveGame();
                        break;

                    case MainMenuChoice.SET_GAME_OPTIONS:
                        SetOptions();
                        break;

                    case MainMenuChoice.START_NEW_GAME:
                        InitialiseGame();
                        gamePlayed = true;
                        gameOn = true;
                        DrawBoard();
                        break;

                    case MainMenuChoice.RETURN_TO_GAME:
                        gameOn = true;
                        break;

                    case MainMenuChoice.QUIT_GAME:
                        programOn = false;
                        break;
                }



                char currentCommand = ' ';
                

                while (gameOn)
                {
                    if (currentCommand != ' ')
                    {
                        DrawBoard();
                        currentCommand = ' ';
                    }
                    if (Console.KeyAvailable)
                    {
                        currentCommand = Console.ReadKey().KeyChar;
                    }
                    switch (currentCommand)
                    {
                        case 'w':
                            MoveCursorUp();
                            break;

                        case 'a':
                            MoveCursorLeft();
                            break;

                        case 's':
                            MoveCursorDown();
                            break;

                        case 'd':
                            MoveCursorRight();
                            break;

                        case 'q':
                            gameOn = false;
                            break;

                        case '\r':
                            if (boardData[currentY][currentX] == ' ')
                            {
                                boardData[currentY][currentX] = players[activePlayer].Marker;
                                if (activePlayer == 1)
                                {
                                    activePlayer = 0;
                                }
                                else
                                {
                                    activePlayer = 1;
                                }
                            }
                            break;
                    }
                    if (gameOn && currentCommand != ' ')
                    {
                        gameWon = CheckVictory();
                        if (gameWon)
                        {
                            gameOn = false;
                        }
                    }
                }

                if (gameWon)
                {
                    ClearTextWindow();
                    Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 3);
                    Console.Write("Game over " + players[winningPlayer].Name + " won");
                    Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 4);
                    Console.Write("press any key to continue");
                    Console.In.ReadLine();
                    
                }
              
            }
        }

        private static void SaveGame()
        {
            string[] writeLines = new string[gameSize + 1];

            for (int lineCount = 0; lineCount < gameSize; lineCount++)
            {
                for (int columnCount = 0; columnCount < gameSize; columnCount++)
                {
                    writeLines[lineCount] += boardData[lineCount][columnCount];
                    if (columnCount != gameSize - 1)
                    {
                        writeLines[lineCount] += ",";
                    }
                }
            }
            writeLines[gameSize] = players[0].Name + "," + players[0].Marker + "," + players[1].Name + "," + players[1].Marker + "," + Convert.ToString(gameSize) + "," + Convert.ToString(activePlayer);

            System.IO.File.WriteAllLines(@"SaveGame.csv", writeLines);

        }

        private static void SetOptions()
        {
            DrawBoard();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            SetPlayerOptions(1);
            SetPlayerOptions(2);

            ClearTextWindow();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 3);
            Console.Write("How big would you like");
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 4);
            Console.Write("the game board");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 6);
            Console.Write("please enter a number 2-10");

            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 7);
            string newDimensionString = Console.In.ReadLine();
            int newDimension;
            while (!int.TryParse(newDimensionString, out newDimension) || (newDimension<2 || newDimension > 10))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 6);
                Console.Write("please enter a number 2-10");

                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 7);
                newDimensionString = Console.In.ReadLine();
            }
            gameSize = newDimension;
        }

        private static void SetPlayerOptions(int number)
        {
            ClearTextWindow();
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 3);
            Console.Write("Setup Player " + number);
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 4);
            Console.Write("--------------");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 5);
            Console.Write("Name is " + players[number - 1].Name);
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 6);
            Console.Write("Enter new name?");

            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 7);
            players[number - 1].Name = Console.In.ReadLine();

            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 5);
            Console.Write("Marker is " + players[number - 1].Marker);
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 6);
            Console.Write("Enter new Marker?");

            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 7);
            players[number - 1].Marker = Console.In.ReadLine()[0];
        }

        private static void InitialSetup()
        {
            players[0] = new Player();
            players[1] = new Player();

            players[0].Name = "Player 1";
            players[0].Marker = 'X';
            players[1].Name = "Player 2";
            players[1].Marker = 'O';
            activePlayer = 0;
        
        }

        private static void LoadGame()
        {
            players[0] = new Player();
            players[1] = new Player();

            string[] loadedData = System.IO.File.ReadAllLines(@"SaveGame.csv");

            string[] gameOptions = loadedData[loadedData.Length - 1].Split(',');
            players[0].Name = gameOptions[0];
            players[0].Marker = gameOptions[1][0];
            players[1].Name = gameOptions[2];
            players[1].Marker = gameOptions[3][0];
            gameSize = Convert.ToInt32(gameOptions[4]);
            activePlayer = Convert.ToInt32(gameOptions[5]);

            boardData = new char[gameSize][];
            for (int lineCount = 0; lineCount < gameSize; lineCount++)
            {
                boardData[lineCount] = new char[gameSize];
                string[] lineData = loadedData[lineCount].Split(',');
                for (int itemCount = 0; itemCount < gameSize; itemCount++)
                {
                    boardData[lineCount][itemCount] = lineData[itemCount][0];
                }
            }


        }

        private static void InitialiseGame()
        {
            boardData = new char[gameSize][];

            for (int initCount = 0; initCount < gameSize; initCount++)
            {
                boardData[initCount] = new char[gameSize];
                for (int setCount = 0; setCount < gameSize; setCount++)
                {
                    boardData[initCount][setCount] = ' ';
                }
            }
        }

        private static MainMenuChoice GetMainMenuChoice(bool gameBeingPlayed)
        {
            ClearTextWindow();
            MainMenuChoice chosenOption = MainMenuChoice.NO_OPTION;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 3);
            Console.Write("What would you like to do?");
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 1, 4);
            Console.Write("(Please enter number)");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, 6);
            Console.Write("1) Load Game");
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, 7);
            Console.Write("2) Save Game");
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, 8);
            Console.Write("3) Set Game Options");
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, 9);
            Console.Write("4) Start New Game");
            Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, 10);
            Console.Write("5) Quit Game");
            if (gameBeingPlayed)
            {
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, 11);
                Console.Write("6) Return To Game");
            }
            

            while (chosenOption == MainMenuChoice.NO_OPTION)
            {
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, 12);
                string optionChosen = Console.In.ReadLine();
                int convertOption;
                try
                {
                    convertOption = Convert.ToInt32(optionChosen);
                    if (convertOption < (int)MainMenuChoice.LOAD_GAME || convertOption > (int)MainMenuChoice.RETURN_TO_GAME)
                    {
                        Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, (2 * gameBoardMargin) + (gameSize * 2) - 3);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Must be a number above!");
                    }
                    else
                    {
                        chosenOption = (MainMenuChoice) convertOption;
                    }
                }
                catch (Exception e)
                {
                    Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1) + 3, (2 * gameBoardMargin) + (gameSize * 2) - 3);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Please enter a number!");

                }
            }
            return chosenOption;
        }

        private static void ClearTextWindow()
        {
            Console.BackgroundColor = ConsoleColor.Black;

            
            for (int verticalCount = 0; verticalCount < (2 * gameBoardMargin) + (2 * gameSize) - 3; verticalCount++)
            {
                Console.SetCursorPosition((2 * gameBoardMargin) + (2 * gameSize - 1), 1 + verticalCount);
                for (int horizontalCount = 0; horizontalCount < textWindowWidth; horizontalCount++)
                {
                    Console.Write(" ");
                }
            }
        }

        private static bool CheckVictory()
        {
            bool win = false;
            for (int playerCount = 0; playerCount < 2; playerCount++)
            {

                for (int dimension1 = 0; dimension1 < gameSize; dimension1++)
                {
                    if (!win)
                    {
                        if (boardData[0][dimension1] == players[playerCount].Marker)
                        {
                            win = true;

                            for (int dimension2 = 0; dimension2 < gameSize; dimension2++)
                            {
                                if (players[playerCount].Marker != boardData[dimension2][dimension1])
                                {
                                    win = false;
                                }
                            }
                            if (win)
                            {
                                winningPlayer = playerCount;
                            }
                        }
                    }
                    if (!win)
                    {
                        if (boardData[dimension1][0] == players[playerCount].Marker)
                        {
                            win = true;
                            for (int dimension2 = 0; dimension2 < gameSize; dimension2++)
                            {
                                if (players[playerCount].Marker != boardData[dimension1][dimension2])
                                {
                                    win = false;
                                }
                            }
                            if (win)
                            {
                                winningPlayer = playerCount;
                            }
                        }
                    }
                 }
                if (!win)
                {
                    win = true;
                    for (int dimension = 0; dimension < gameSize; dimension++)
                    {
                        if(players[playerCount].Marker != boardData[dimension][dimension])
                        {
                            win = false;
                        }
                    }
                    if (win)
                    {
                        winningPlayer = playerCount;
                    }
                }
                if (!win)
                {
                    win = true;
                    for (int dimension = 0; dimension < gameSize; dimension++)
                    {
                        if (players[playerCount].Marker != boardData[dimension][gameSize - 1 - dimension])
                        {
                            win = false;
                        }
                    }
                    if(win)
                    {
                        winningPlayer = playerCount;
                    }
                }

            }
            if (win)
            {
                gameOn = false;
            }
            return win;
        }

        private static void MoveCursorRight()
        {
            if(currentX < gameSize - 1)
            {
                currentX++;
            }
        }

        private static void MoveCursorDown()
        {
            if (currentY < gameSize - 1)
            {
                currentY++;
            }
        }

        private static void MoveCursorLeft()
        {
            if (currentX > 0)
            {
                currentX--;
            }
        }

        private static void MoveCursorUp()
        {
            if (currentY > 0)
            {
                currentY--;
            }
        }
    }
}
