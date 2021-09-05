using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FourInRowGameUI
{
    public class BoardButton : Button
    {
        private readonly Point r_LocationOnBoard;

        public BoardButton(int i_Row, int i_Col)
        {
            r_LocationOnBoard = new Point(i_Row, i_Col);
        }

        public Point LocationOnBoard
        {
            get
            {
                return r_LocationOnBoard;
            }
        }
    }
}
