using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _9Rocks
{
    public class OnMessageOutEventArgs : EventArgs
    {
        private string message_ = "";

        private bool show_ = true;

        private MessageType type_ = MessageType.INFO;

        public OnMessageOutEventArgs(string msg, MessageType type, bool show)
        {
            this.message_ = msg;
            this.type_ = type;
            this.show_ = show;
        }

        public string Message
        {
            get
            {
                return this.message_;
            }
        }

        public MessageType Type
        {
            get
            {
                return this.type_;
            }
        }

        public bool Show
        {
            get
            {
                return this.show_;
            }
        }
    }
}
