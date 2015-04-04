using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _9Rocks
{
    public class OnSquareSelectedChangeEventArgs : EventArgs
    {
        private int row_ = 0;
        private int col_ = 0;

        private bool val_ = false;

        public OnSquareSelectedChangeEventArgs(int row, int col, bool val)
        {
            this.row_ = row;
            this.col_ = col;
            this.val_ = val;
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

        public bool IsSelected
        {
            get
            {
                return this.val_;
            }
        }
    }
}
