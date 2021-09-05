using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FourInRowGameUI
{
    public partial class FormGameSettings : Form
    {
        public string FirstPlayerName
        {
            get
            {
                return textBoxPlayer1.Text;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return textBoxPlayer2.Text;
            }

            set
            {
                textBoxPlayer2.Text = value;
            }
        }

        public bool IsGameAgainstComputer
        {
            get
            {
                return !checkBoxPlayer2.Checked;
            }
        }

        public int GameBoardSizeRows
        {
            get
            {
                return (int)numericUpDownRows.Value;
            }
        }

        public int GameBoardSizeCols
        {
            get
            {
                return (int)numericUpDownCols.Value;
            }
        }

        public string Player1Name
        {
            get
            {
                string name = textBoxPlayer1.Text;
                if (name == string.Empty)
                {
                    name = "Player 1";
                }

                return name;
            }
        }

        public string Player2Name
        {
            get
            {
                string name = textBoxPlayer2.Text;
                if (textBoxPlayer2.Enabled == false)
                {
                    name = "Computer";
                }
                else
                {
                    if (name == string.Empty)
                    {
                        name = "Player 2";
                    }
                }

                return name;
            }
        }

        public FormGameSettings()
        {
            InitializeComponent();
        }

        private void labelPlayers_Click(object sender, EventArgs e)
        {
        }

        private void textBoxPlayer1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxPlayer2_TextChanged(object sender, EventArgs e)
        {
        }

        private void labelBoardSize_Click(object sender, EventArgs e)
        {
        }

        private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayer2.Checked == true)
            {
                textBoxPlayer2.Text = string.Empty;
                textBoxPlayer2.Enabled = true;
            }
            else
            {
                textBoxPlayer2.Enabled = false;
                this.textBoxPlayer2.Text = "[Computer]";
            }
        }

        private void labelPlayer1_Click(object sender, EventArgs e)
        {
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void labelRows_Click(object sender, EventArgs e)
        {
        }

        private void labelCols_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDownRows_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDownCols_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
