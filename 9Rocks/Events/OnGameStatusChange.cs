using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _9Rocks
{
    public class OnGameStatusChange : EventArgs
    {
        private GameStatus status_ = GameStatus.UNKNOWN;

        public OnGameStatusChange(GameStatus status)
        {
            this.status_ = status;
        }

        public GameStatus Status
        {
            get
            {
                return this.status_;
            }
        }
    }
}
