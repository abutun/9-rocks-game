using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _9Rocks
{
    public class OnMouseOnSquareEventArgs : EventArgs
    {
        private int row_ = 0;
        private int col_ = 0;

        public OnMouseOnSquareEventArgs(int row, int col)
        {
            this.row_ = row;
            this.col_ = col;
        }

        public int Row
        {
            get
            {
                return this.row_;
            }
        }

        public int Column
        {
            get
            {
                return this.col_;
            }
        }
    }
}
