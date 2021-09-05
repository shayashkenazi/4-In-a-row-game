using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FourInRowGameLogic;

namespace FourInRowGameUI
{
    public partial class FormGame : Form
    {
        private const string k_X = "X";
        private const string k_O = "O";
        private const string k_Empty = " ";
        private const int k_CellWidth = 40;
        private const int k_Space = 10;
        private readonly FourInRowGame r_LogicGameManager;

        private readonly BoardButton[,] r_GameBoard;
        private readonly int r_BoardSizeRows;
        private readonly int r_BoardSizeCols;

        private readonly string r_FirstPlayerNameStr;
        private readonly string r_SecondPlayerNameStr;

        private Label m_LabelFirstPlayer;
        private Label m_LabelSecondPlayer;

        public FormGame(int i_BoardSizeRows, int i_BoardSizeCols, string i_FirstPlayerName, string i_SecondPlayerName)
        {
            r_BoardSizeRows = i_BoardSizeRows;
            r_BoardSizeCols = i_BoardSizeCols;
            r_GameBoard = new BoardButton[i_BoardSizeRows + 1, i_BoardSizeCols];
            r_LogicGameManager = new FourInRowGame(i_BoardSizeRows, i_BoardSizeCols);
            r_FirstPlayerNameStr = i_FirstPlayerName;
            r_SecondPlayerNameStr = i_SecondPlayerName;
            initializeComponent();
            subscribeUiToLogicBoardCells();
            setLogicAsListenerToUiBoardCells();
            r_LogicGameManager.GameOver += gameOver_Occurred;
        }

        public BoardButton this[int i_Row, int i_Col]
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

        public FourInRowGame LogicGameManager
        {
            get
            {
                return r_LogicGameManager;
            }
        }

        private void initializeComponent()
        {
            for(int col = 0; col < r_BoardSizeCols; col++)
            {
                r_GameBoard[0, col] = new BoardButton(0, col);
                r_GameBoard[0, col].Text = (col + 1).ToString();
                r_GameBoard[0, col].Font = new Font(r_GameBoard[0, col].Font, FontStyle.Bold);
                r_GameBoard[0, col].BackColor = Color.Aquamarine;

            }

            for(int row = 1; row < r_BoardSizeRows + 1; row++)
            {
                for(int col = 0; col < r_BoardSizeCols; col++)
                {
                    r_GameBoard[row, col] = new BoardButton(row, col);
                }
            }

            this.m_LabelFirstPlayer = new Label();
            this.m_LabelSecondPlayer = new Label();
            this.SuspendLayout();

            int positionX = k_Space;
            int positionY = k_Space;

            for(int row = 0; row < r_BoardSizeRows + 1; row++)
            {
                positionX = k_Space; /* initial space from beginning */
                for(int col = 0; col < r_BoardSizeCols; col++)
                {
                    r_GameBoard[row, col].Location = new System.Drawing.Point(positionX, positionY);
                    r_GameBoard[row, col].Size = new System.Drawing.Size(k_CellWidth, k_CellWidth);
                    positionX += k_CellWidth + k_Space;
                }

                positionY += k_CellWidth + k_Space;
            }

            int formWidth = (k_CellWidth + k_Space) * (r_BoardSizeCols + 1);
            int formHeight = (k_CellWidth + k_Space ) * (r_BoardSizeRows + 1);
            int middle = formWidth / 2;

            // labelFirstPlayerName
            this.m_LabelFirstPlayer.AutoSize = true;
            this.m_LabelFirstPlayer.Location = new System.Drawing.Point(middle - 70, positionY);
            this.m_LabelFirstPlayer.Name = "labelFirstPlayerName";
            this.m_LabelFirstPlayer.Size = new System.Drawing.Size(69, 20);
            this.m_LabelFirstPlayer.Text = r_FirstPlayerNameStr + ": 0";

            // labelSecondPlayerName
            this.m_LabelSecondPlayer.AutoSize = true;
            this.m_LabelSecondPlayer.Location = new System.Drawing.Point(middle + 5, positionY);
            this.m_LabelSecondPlayer.Name = "labelSecondPlayerName";
            this.m_LabelSecondPlayer.Size = new System.Drawing.Size(83, 20);
            this.m_LabelSecondPlayer.Text = r_SecondPlayerNameStr + ": 0";

            // GameBoard
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(formWidth - k_Space, formHeight + 30);
            this.Controls.Add(this.m_LabelSecondPlayer);
            this.Controls.Add(this.m_LabelFirstPlayer);

            foreach(BoardButton cell in r_GameBoard)
            {
                this.Controls.Add(cell);
            }

            this.Name = "GameBoard";

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "4 in a Raw !!";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public void ResetBoard()
        {
            for(int col = 0; col < r_BoardSizeCols; col++)
            {
                r_GameBoard[0, col].Enabled = true;
            }
        }

        public void UpdateScore()
        {
            int firstPlayerScore = LogicGameManager.Player1.Score;
            int secondPlayerScore = LogicGameManager.Player2.Score;
            this.m_LabelFirstPlayer.Text = string.Format("{0}{1}{2}", r_FirstPlayerNameStr, ": ", firstPlayerScore.ToString());
            this.m_LabelSecondPlayer.Text = string.Format("{0}{1}{2}", r_SecondPlayerNameStr, ": ", secondPlayerScore.ToString());
        }

        private void gameOver_Occurred(FourInRowGame.eGameStatus i_GameStatus)
        {
            if(i_GameStatus == FourInRowGame.eGameStatus.Tie)
            {
                tieHandler();
            }
            else if (i_GameStatus == FourInRowGame.eGameStatus.Player1Won || i_GameStatus == FourInRowGame.eGameStatus.Player2Won)
            {
                winHandler();
            }
        }

        private void tieHandler()
        {
            string message = "Tie!!" + Environment.NewLine
                                          + "Another Round?";
            if (MessageBox.Show(message, "Tie", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                LogicGameManager.StartNewGame();
                ResetBoard();
            }
            else
            {
                this.Close();
            }
        }

        private void winHandler()
        {
            UpdateScore();
            string name = LogicGameManager.PlayerTurn == FourInRowGame.ePlayerTurn.Player1Turn
                              ? r_FirstPlayerNameStr
                              : r_SecondPlayerNameStr;
       
            string message = name + " Won!!" + Environment.NewLine
                             + "Another Round?";
            if (MessageBox.Show(message, "Win", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                LogicGameManager.StartNewGame();
                ResetBoard();
            }
            else
            {
                this.Close();
            }
        }

        private void setCellInUiBoard(Cell i_CellCalled)
        {
            BoardButton curCell = r_GameBoard[i_CellCalled.Location.X + 1, i_CellCalled.Location.Y];
            curCell.Text = getCellValueAsString(i_CellCalled.Value);
            curCell.ForeColor = getCellColor();
            curCell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));

        }

        private Color getCellColor()
        {
            Color color;
            if(LogicGameManager.PlayerTurn == FourInRowGame.ePlayerTurn.Player1Turn)
            {
                color = System.Drawing.Color.DarkOrange;
            }
            else
            {
                color = System.Drawing.Color.Red;   
            }

            return color;
        }
        private string getCellValueAsString(eCellValue i_CellValue)
        {
            string cellValueAsString = string.Empty;

            switch (i_CellValue)
            {
                case eCellValue.Empty:
                    cellValueAsString = k_Empty;
                    break;
                case eCellValue.X:
                    cellValueAsString = k_X;
                    break;
                case eCellValue.O:
                    cellValueAsString = k_O;
                    break;
                default:
                    break;
            }

            return cellValueAsString;
        }

        private void subscribeUiToLogicBoardCells()
        {
            for (int row = 0; row < LogicGameManager.BoardRowSize; row++)
            {
                for (int col = 0; col < LogicGameManager.BoardCoulumnSize; col++)
                {
                    LogicGameManager.Board[row, col].CellChanged += setCellInUiBoard;
                }
            }
        }

        private void setLogicAsListenerToUiBoardCells()
        {
            for(int col = 0; col < LogicGameManager.BoardCoulumnSize; col++)
            {
                BoardButton curCell = r_GameBoard[0, col];
                curCell.Click += setCellInLogicBoard;
            }
        }

        private void setCellInLogicBoard(object sender, EventArgs e)
        {
            BoardButton buttonClicked = sender as BoardButton;
            int columnClicked = buttonClicked.LocationOnBoard.Y;
            LogicGameManager.SetCell(columnClicked);
            disableButtonWhenColIsFull(columnClicked);
            LogicGameManager.UpdateGameStatus(columnClicked);
            if (LogicGameManager.ComputerModeAndTurn())
            {
                LogicGameManager.ComputerMove(out columnClicked);
                disableButtonWhenColIsFull(columnClicked);
                LogicGameManager.UpdateGameStatus(columnClicked);
            }
        }

        private void disableButtonWhenColIsFull(int i_ColumnClicked)
        {
            if(LogicGameManager.GetLastRowInCol(i_ColumnClicked) == 0)
            {
                BoardButton curCell = r_GameBoard[0, i_ColumnClicked];
                curCell.Enabled = false;
            }
        }
    }
}
