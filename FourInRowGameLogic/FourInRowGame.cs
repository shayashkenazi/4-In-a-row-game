using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInRowGameLogic
{
    public class FourInRowGame
    {
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private readonly Board r_GameBoard;
        private eGameMode m_GameMode;
        private eGameStatus m_CurGameStatus = eGameStatus.DuringGame;
        private ePlayerTurn m_PlayerTurn = ePlayerTurn.Player1Turn;

        public event Action<eGameStatus> GameOver;

        public enum eGameMode
        {
            HumanVsHuman = 1,
            HumanVsComputer
        }

        public enum eGameStatus
        {
            Player1Won = 1,
            Player2Won,
            Tie,
            DuringGame,
            Quit
        }

        public eGameMode GameMode
        {
            get
            {
                return m_GameMode;
            }

            set
            {
                m_GameMode = value;
            }
        }

        public enum ePlayerTurn
        {
            Player1Turn = 1,
            Player2Turn
        }

        public FourInRowGame(int i_Row, int i_Col)
        {
            r_Player1 = new Player();
            r_Player2 = new Player();
            r_GameBoard = new Board(i_Row, i_Col);
        }

        public Board Board
        {
            get
            {
                return r_GameBoard;
            }
        }

        public int BoardRowSize
        {
            get
            {
                return Board.BoardRowSize;
            }
        }

        public int BoardCoulumnSize
        {
            get
            {
                return Board.BoardColumnSize;
            }
        }

        public Player Player1
        {
            get
            {
                return r_Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return r_Player2;
            }
        }

        public ePlayerTurn PlayerTurn
        {
            get
            {
                return m_PlayerTurn;
            }

            set
            {
                m_PlayerTurn = value;
            }
        }

        public eGameStatus GameStatus
        {
            get
            {
                return m_CurGameStatus;
            }

            set
            {
                m_CurGameStatus = value;
            }
        }

        protected virtual void OnGameOver()
        {
            if(GameOver != null)
            {
                GameOver.Invoke(this.GameStatus);
            }
        }

        public void SetCell(int i_UserColumn)
        {
            bool spaceLeft = false;
            eCellValue symbol = PlayerTurn == ePlayerTurn.Player1Turn ? eCellValue.X : eCellValue.O;
            for(int row = BoardRowSize - 1; row >= 0 && spaceLeft == false; row--)
            {
                if(Board[row, i_UserColumn].Value == eCellValue.Empty)
                {
                    spaceLeft = true;
                    Board[row, i_UserColumn].Value = symbol;
                }
            }
        }

        public void SetSpecificCell(int i_UserRow, int i_UserColumn, eCellValue i_Symbol)
        {
            Board[i_UserRow, i_UserColumn].Value = i_Symbol;
        }

        public void RemoveCell(int i_UserColumn)
        {
            int currentRowRemove = GetLastRowInCol(i_UserColumn);
            Board[currentRowRemove, i_UserColumn].Value = eCellValue.Empty;
        }

        public bool CheckColumnWin(int i_ColumnCheck, eCellValue i_Symbol)
        {
            bool isWin = false;
            for(int row = BoardRowSize - 1; row >= 3 && isWin == false; row--)
            {
                if((Board[row, i_ColumnCheck].Value == i_Symbol) && Board[row - 1, i_ColumnCheck].Value == i_Symbol
                                                                 && Board[row - 2, i_ColumnCheck].Value == i_Symbol
                                                                 && Board[row - 3, i_ColumnCheck].Value == i_Symbol)
                {
                    isWin = true;
                }
            }

            return isWin;
        }

        public bool CheckRowWin(eCellValue i_Symbol)
        {
            bool isWin = false;
            for(int col = BoardCoulumnSize - 1; col >= 3 && isWin == false; col--)
            {
                for(int row = BoardRowSize - 1; row >= 0 && isWin == false; row--)
                {
                    if((Board[row, col].Value == i_Symbol) && Board[row, col - 1].Value == i_Symbol
                                                           && Board[row, col - 2].Value == i_Symbol
                                                           && Board[row, col - 3].Value == i_Symbol)
                    {
                        isWin = true;
                    }
                }
            }

            return isWin;
        }

        public bool CheckRightDiagonal(eCellValue i_Symbol)
        {
            bool isWin = false;
            for(int col = 0; col <= BoardCoulumnSize - 1 - 3 && isWin == false; col++)
            {
                for(int row = BoardRowSize - 1; row >= 3 && isWin == false; row--)
                {
                    if((Board[row, col].Value == i_Symbol) && Board[row - 1, col + 1].Value == i_Symbol
                                                           && Board[row - 2, col + 2].Value == i_Symbol
                                                           && Board[row - 3, col + 3].Value == i_Symbol)
                    {
                        isWin = true;
                    }
                }
            }

            return isWin;
        }

        public bool CheckLeftDiagonal(eCellValue i_Symbol)
        {
            bool isWin = false;
            for(int col = BoardCoulumnSize - 1; col >= 3 && isWin == false; col--)
            {
                for(int row = BoardRowSize - 1; row >= 3 && isWin == false; row--)
                {
                    if((Board[row, col].Value == i_Symbol) && Board[row - 1, col - 1].Value == i_Symbol
                                                           && Board[row - 2, col - 2].Value == i_Symbol
                                                           && Board[row - 3, col - 3].Value == i_Symbol)
                    {
                        isWin = true;
                    }
                }
            }

            return isWin;
        }

        public bool CheckVictory(int i_ColumnCheck, eCellValue i_Symbol)
        {
            return CheckRowWin(i_Symbol) || CheckColumnWin(i_ColumnCheck, i_Symbol) || CheckRightDiagonal(i_Symbol)
                   || CheckLeftDiagonal(i_Symbol);
        }

        public bool ComputerModeAndTurn()
        {
            bool computerGame = GameMode == FourInRowGame.eGameMode.HumanVsComputer;
            bool computerTurn = PlayerTurn == ePlayerTurn.Player2Turn;
            return computerGame && computerTurn;
        }

        public void ComputerMove(out int io_BestComputerColumn)
        {
            int bestCol = 0, maxSequence = 0, CurSequence;
            for(int col = 0; col < BoardCoulumnSize; col++)
            {
                if(!Board[0, col].HasValue())
                {
                    SetCell(col);
                    if(CheckVictory(col, eCellValue.O))
                    {
                        bestCol = col;
                        RemoveCell(col);
                        break;
                    }

                    if(CheckIfRivalHasVictory(col))
                    {
                        bestCol = col;
                        RemoveCell(col);
                        break;
                    }
                    else
                    {
                        CurSequence = MaxSequenceAtCell(col);
                        if(maxSequence < CurSequence)
                        {
                            bestCol = col;
                            maxSequence = CurSequence;
                        }
                    }

                    RemoveCell(col);
                }
            }

            SetCell(bestCol);
            io_BestComputerColumn = bestCol;
        }

        public int CountRightRowSequence(int i_ColumnCheck, int i_CurrentRowCheck, eCellValue i_Symbol)
        {
            int curSequence = 0, indexCol = 0;
            while((indexCol + i_ColumnCheck < BoardCoulumnSize)
                  && Board[i_CurrentRowCheck, i_ColumnCheck + indexCol].Value == i_Symbol)
            {
                curSequence++;
                indexCol++;
            }

            return curSequence;
        }

        public int CountLeftRowSequence(int i_ColumnCheck, int i_CurrentRowCheck, eCellValue i_Symbol)
        {
            int curSequence = 0, indexCol = 0;
            while((i_ColumnCheck - indexCol > 0)
                  && Board[i_CurrentRowCheck, i_ColumnCheck - indexCol].Value == i_Symbol)
            {
                curSequence++;
                indexCol++;
            }

            return curSequence;
        }

        public int CountDownColSequence(int i_ColumnCheck, int i_CurrentRowCheck, eCellValue i_Symbol)
        {
            int curSequence = 0, indexRow = 0;
            while(i_CurrentRowCheck + indexRow < BoardRowSize
                  && Board[i_CurrentRowCheck + indexRow, i_ColumnCheck].Value == i_Symbol)
            {
                curSequence++;
                indexRow++;
            }

            return curSequence;
        }

        public int CountLeftUpDiagonalSequence(int i_ColumnCheck, int i_CurrentRowCheck, eCellValue i_Symbol)
        {
            int curSequence = 0, indexRow = 0, indexCol = 0;
            while(i_CurrentRowCheck - indexRow >= 0 && i_ColumnCheck - indexCol >= 0
                                                    && Board[i_CurrentRowCheck - indexRow, i_ColumnCheck - indexCol]
                                                        .Value == i_Symbol)
            {
                curSequence++;
                indexRow++;
                indexCol++;
            }

            return curSequence;
        }

        public int CountLeftDownDiagonalSequence(int i_ColumnCheck, int i_CurrentRowCheck, eCellValue i_Symbol)
        {
            int curSequence = 0, indexRow = 0, indexCol = 0;
            while(i_CurrentRowCheck + indexRow < BoardRowSize && i_ColumnCheck - indexCol >= 0
                                                              && Board[i_CurrentRowCheck + indexRow,
                                                                  i_ColumnCheck - indexCol].Value == i_Symbol)
            {
                curSequence++;
                indexRow++;
                indexCol++;
            }

            return curSequence;
        }

        public int CountRightUpDiagonalSequence(int i_ColumnCheck, int i_CurrentRowCheck, eCellValue i_Symbol)
        {
            int curSequence = 0, indexRow = 0, indexCol = 0;
            while(i_CurrentRowCheck - indexRow >= 0 && i_ColumnCheck + indexCol < BoardCoulumnSize
                                                    && Board[i_CurrentRowCheck - indexRow, i_ColumnCheck + indexCol]
                                                        .Value == i_Symbol)
            {
                curSequence++;
                indexRow++;
                indexCol++;
            }

            return curSequence;
        }

        public int CountRightDownDiagonalSequence(int i_ColumnCheck, int i_CurrentRowCheck, eCellValue i_Symbol)
        {
            int curSequence = 0, indexRow = 0, indexCol = 0;
            while(i_CurrentRowCheck + indexRow < BoardRowSize && i_ColumnCheck + indexCol < BoardCoulumnSize
                                                              && Board[i_CurrentRowCheck + indexRow,
                                                                  i_ColumnCheck + indexCol].Value == i_Symbol)
            {
                curSequence++;
                indexRow++;
                indexCol++;
            }

            return curSequence;
        }

        public int MaxSequenceAtCell(int i_ColumnCheck)
        {
            int currentRowCheck = GetLastRowInCol(i_ColumnCheck);
            int maxSequence;
            eCellValue symbol = eCellValue.O;
            int sequenceInRightRow = CountRightRowSequence(i_ColumnCheck, currentRowCheck, symbol);
            int sequenceInLeftRow = CountLeftRowSequence(i_ColumnCheck, currentRowCheck, symbol);
            maxSequence = Math.Max(sequenceInRightRow, sequenceInLeftRow);
            int sequenceInDownCol = CountDownColSequence(i_ColumnCheck, currentRowCheck, symbol);
            maxSequence = Math.Max(sequenceInDownCol, maxSequence);
            int sequenceInLeftUpDiagonal = CountLeftUpDiagonalSequence(i_ColumnCheck, currentRowCheck, symbol);
            maxSequence = Math.Max(sequenceInLeftUpDiagonal, maxSequence);
            int sequenceInLeftDownDiagonal = CountLeftDownDiagonalSequence(i_ColumnCheck, currentRowCheck, symbol);
            maxSequence = Math.Max(sequenceInLeftDownDiagonal, maxSequence);
            int sequenceInRightUpDiagonal = CountRightUpDiagonalSequence(i_ColumnCheck, currentRowCheck, symbol);
            maxSequence = Math.Max(sequenceInRightUpDiagonal, maxSequence);
            int sequenceInRightDownDiagonal = CountRightDownDiagonalSequence(i_ColumnCheck, currentRowCheck, symbol);
            maxSequence = Math.Max(sequenceInRightDownDiagonal, maxSequence);

            return maxSequence;
        }

        public bool CheckIfRivalHasVictory(int i_ColumnCheck)
        {
            int currentRowCheck = GetLastRowInCol(i_ColumnCheck);
            SetSpecificCell(currentRowCheck, i_ColumnCheck, eCellValue.X);
            bool isWin = CheckVictory(i_ColumnCheck, eCellValue.X);
            SetSpecificCell(currentRowCheck, i_ColumnCheck, eCellValue.O);

            return isWin;
        }

        public int GetLastRowInCol(int i_Column)
        {
            int currentRow = 0;
            while(!Board[currentRow, i_Column].HasValue() && currentRow < BoardRowSize)
            {
                currentRow++;
            }

            return currentRow;
        }

        public void UpdateScore()
        {
            if(m_CurGameStatus == eGameStatus.Player1Won)
            {
                Player1.Score++;
            }
            else
            {
                Player2.Score++;
            }
        }

        public void SwitchPlayerTurn()
        {
            PlayerTurn = PlayerTurn == ePlayerTurn.Player1Turn ? ePlayerTurn.Player2Turn : ePlayerTurn.Player1Turn;
        }

        public void StartNewGame()
        {
            Board.StartNewBoard();
            GameStatus = FourInRowGame.eGameStatus.DuringGame;
            PlayerTurn = FourInRowGame.ePlayerTurn.Player1Turn;
        }

        public void UpdateGameStatus(int i_Column)
        {
            eCellValue symbol = PlayerTurn == ePlayerTurn.Player1Turn ? eCellValue.X : eCellValue.O;
            if (CheckVictory(i_Column, symbol))
            {
                GameStatus = m_PlayerTurn == ePlayerTurn.Player1Turn ? eGameStatus.Player1Won : eGameStatus.Player2Won;
                UpdateScore();
                OnGameOver();
            }
            else if(r_GameBoard.IsFull())
            {
                GameStatus = eGameStatus.Tie;
                OnGameOver();
            }
            else
            {
                GameStatus = eGameStatus.DuringGame;
                SwitchPlayerTurn();
            }
        }
    }
}
