using System;

namespace NACBackEnd
{
    public enum SquareState
    {
        Blank = 0,
        Nought = 1,
        Cross = 2

    }

    public enum SquareID
    {
        TopLeft = 0,
        TopCenter = 1,
        TopRight = 2,
        CenterLeft = 3,
        CenterCenter = 4,
        CenterRight = 5,
        BottomLeft = 6,
        BottomCenter = 7,
        BottomRight = 8
    }
    public class Board
    {
        private SquareState[] boardData;
        private GameNode theNode;

        public SquareState[] BoardData { get => boardData; set => boardData = value; }
        public string ID
        {

            get
            {
                string id = "";
                for (int squareCount = 0; squareCount < 9; squareCount++)
                {
                   
                        id = id + getSquareID((SquareID)squareCount);
                   
                }
                return id;
            }
        }

        public GameNode TheNode { get => theNode; set => theNode = value; }

        public string getSquareID(SquareID id)
        {
            switch(BoardData[(int)id])
            {
                case SquareState.Blank:
                    return " ";

                case SquareState.Nought:
                    return "O";

                case SquareState.Cross:
                    return "X";
            }
            return "";
        }

        public SquareState getSquareState(SquareID id)
        {
            return BoardData[(int)id];
        }

        public SquareState getSquareState(char state)
        {
            switch(state)
            {
                case 'O':
                    return SquareState.Nought;

                case 'X':
                    return SquareState.Cross;

                case ' ':
                    return SquareState.Blank;

                default:
                    throw new Exception("invalid sqare state '" + state + "'");

            }
        }

        public void setSquareState(SquareID id, SquareState newState)
        {
            if (BoardData[(int)id] != SquareState.Blank)
            {
                throw new Exception("Square not blank");
            }
            BoardData[(int)id] = newState;
        }

        public Board(GameNode newNode)
        {
            TheNode = newNode;
            boardData = new SquareState[9];
            for (int squareCount = 0; squareCount < 9; squareCount++)
            {
                BoardData[squareCount] = SquareState.Blank;
            }
        }

        public Board(string boardState, GameNode newNode)
        {
            TheNode = newNode;
            boardData = new SquareState[9];
            for (int squareCount = 0; squareCount < 9; squareCount++)
            {
                BoardData[squareCount] = getSquareState(boardState[squareCount]);
            }
        }

        public void rotateBoard()
        {
            
            SquareState[] rotatedBoard = new SquareState[9];
           
                rotatedBoard[0] = boardData[2];
                rotatedBoard[1] = boardData[5];
                rotatedBoard[2] = boardData[8];
                rotatedBoard[3] = boardData[1];
                rotatedBoard[4] = boardData[4];
                rotatedBoard[5] = boardData[7];
                rotatedBoard[6] = boardData[0];
                rotatedBoard[7] = boardData[3];
                rotatedBoard[8] = boardData[6];


            boardData = rotatedBoard;
        }
    }
}
