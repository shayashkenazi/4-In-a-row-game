using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInRowGameLogic
{
    public class Board
    {
        private readonly Cell[,] r_GameBoard;
        private readonly int r_Row;
        private readonly int r_Col;

        public Board(int i_Row, int i_Col)
        {
            r_GameBoard = new Cell[i_Row, i_Col];
            r_Row = i_Row;
            r_Col = i_Col;

            for (int row = 0; row < r_Row; row++)
            {
                for (int col = 0; col < r_Col; col++)
                {
                    r_GameBoard[row, col] = new Cell(row, col);
                }
            }
        }

        public Cell this[int i_Row, int i_Col]
        {
            get
            {
                return r_GameBoard[i_Row, i_Col];
            }

            set
            {
                r_GameBoard[i_Row, i_Col] = value;
            }
        }

        public int BoardColumnSize
        {
            get
            {
                return r_Col;
            }
        }

        public int BoardRowSize
        {
            get
            {
                return r_Row;
            }
        }

        public void StartNewBoard()
        {
            foreach (Cell cell in r_GameBoard)
            {
                cell.Value = eCellValue.Empty;
            }
        }

        public bool IsFull()
        {
            bool isBoardFull = true;

            foreach (Cell cell in r_GameBoard)
            {
                if (!cell.HasValue())
                {
                    isBoardFull = false;
                }
            }

            return isBoardFull;
        }
    }
}
