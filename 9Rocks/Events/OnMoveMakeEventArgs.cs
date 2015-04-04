using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _9Rocks
{
    public class OnMoveMakeEventArgs : EventArgs
    {
        private int player_ = 0;
        private int fromx_ = 0;
        private int fromy_ = 0;
        private int tox_ = 0;
        private int toy_ = 0;

        public OnMoveMakeEventArgs(int player, int fromx, int fromy, int tox, int toy)
        {
            this.player_ = player;
            this.fromx_ = fromx;
            this.fromy_ = fromy;
            this.tox_ = tox;
            this.toy_ = toy;
        }

        public int Player
        {
            get
            {
                return this.player_;
            }
        }

        public int FromX
        {
            get
            {
                return this.fromx_;
            }
        }

        public int FromY
        {
            get
            {
                return this.fromy_;
            }
        }

        public int ToX
        {
            get
            {
                return this.tox_;
            }
        }

        public int ToY
        {
            get
            {
                return this.toy_;
            }
        }
    }
}
