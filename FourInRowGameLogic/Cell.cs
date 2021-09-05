using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FourInRowGameLogic
{
    public class Cell
    {
        private readonly Point r_LocationOnBoard;
        private eCellValue m_CellValue;

        public event Action<Cell> CellChanged;

        public Cell(int i_Row, int i_Col)
        {
            r_LocationOnBoard = new Point(i_Row, i_Col);
            m_CellValue = eCellValue.Empty;
        }

        public eCellValue Value
        {
            get
            {
                return m_CellValue;
            }

            set
            {
                m_CellValue = value;
                OnCellChanged();
            }
        }

        public Point Location
        {
            get
            {
                return r_LocationOnBoard;
            }
        }

        protected virtual void OnCellChanged()
        {
            if (CellChanged != null)
            {
                CellChanged.Invoke(this);
            }
        }

        public bool HasValue()
        {
            return m_CellValue != eCellValue.Empty;
        }
    }
}
