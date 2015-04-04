using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace _9Rocks
{
    public delegate void SquareMessageOutDelegate(OnMessageOutEventArgs args);
    public delegate void SquareSelectedChangeDelegate(OnSquareSelectedChangeEventArgs args);

    public partial class Square : UserControl
    {
        private SquareStatus innerStatus_ = SquareStatus.EMPTY;

        // bool
        private bool isDragging_ = false;
        private bool showHint_ = false;
        private bool isSelected_ = false;

        // int
        private int col_ = 0;
        private int row_ = 0;
        private int currentX_, currentY_;

        // board
        private Board board_ = null;

        // events
        public event SquareMessageOutDelegate MessageOut = null;
        public event SquareSelectedChangeDelegate SquareSelectedChange = null;

        public Square(Board board)
        {
            InitializeComponent();

            // set owner board
            this.board_ = board;
        }

        public bool ShowHint
        {
            get
            {
                return this.showHint_;
            }
            set
            {
                this.showHint_ = value;

                this.Invalidate(false);
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected_;
            }
            set
            {
                this.isSelected_ = value;

                // fire event
                if(this.SquareSelectedChange!=null)
                    this.SquareSelectedChange(new OnSquareSelectedChangeEventArgs(this.row_, this.col_, value));

                this.Invalidate(false);
            }
        }

        /// <summary>
        /// Shows the current state of the square
        /// </summary>
        public SquareStatus Status
        {
            get
            {
                return this.innerStatus_;
            }
            set
            {
                this.innerStatus_ = value;

                this.Invalidate(false);
            }
        }

        /// <summary>
        /// Current row of the square
        /// </summary>
        public int Row
        {
            get
            {
                return this.row_;
            }
            set
            {
                this.row_ = value;

                this.Location = new Point(this.Column * 50, value * 50);
            }
        }

        /// <summary>
        /// Current column of the square
        /// </summary>
        public int Column
        {
            get
            {
                return this.col_;
            }
            set
            {
                this.col_ = value;

                this.Location = new Point(value * 50, this.Row * 50);
            }
        }

        public Board Board 
        {
            get
            {
                return this.board_;
            }
        }

        private void Square_MouseEnter(object sender, EventArgs e)
        {
            if (this.board_.CanMakeMove)
            {
                if (this.innerStatus_ == SquareStatus.PLAYER1 || this.innerStatus_ == SquareStatus.PLAYER2)
                {
                    // if the mouse is on the current player's square
                    if (this.Board.CurrentPlayer - (int)this.innerStatus_ == 0)
                    {
                        // set cursor
                        this.Cursor = Cursors.Hand;
                        // show hint if only we are playing with pieces (not placing)
                        if (this.board_.TotalSquaresSoFar >= Settings.TotalSquaresForEachPlayer * 2)
                        {
                            if (Settings.ShowHint)
                                this.Board.ShowSquareHint(this.row_, this.col_, true);
                        }
                    }
                }
                else if (this.innerStatus_ == SquareStatus.EMPTY)
                    this.Cursor = Cursors.Hand;
            }
        }

        private void Square_MouseLeave(object sender, EventArgs e)
        {
            if (this.board_.CanMakeMove)
            {
                if (this.innerStatus_ == SquareStatus.PLAYER1 || this.innerStatus_ == SquareStatus.PLAYER2)
                {
                    // clear cursor
                    this.Cursor = Cursors.Default;
                    // show hint if only we are playing with pieces (not placing)
                    if (this.board_.TotalSquaresSoFar >= Settings.TotalSquaresForEachPlayer * 2)
                    {
                        if (Settings.ShowHint)
                            this.Board.ShowSquareHint(this.row_, this.col_, false);
                    }
                }
            }
        }

        private void Square_Paint(object sender, PaintEventArgs e)
        {
            this.BackgroundImage = null;

            if (!this.showHint_)
            {
                switch (this.innerStatus_)
                {
                    case SquareStatus.PLAYER1:
                        if (this.isSelected_)
                            e.Graphics.DrawImage(Settings.Player1UpPNG, new Point(0, 0));
                        else
                            e.Graphics.DrawImage(Settings.Player1PNG, new Point(0,0));
                        break;

                    case SquareStatus.PLAYER2:
                        if (this.isSelected_)
                            e.Graphics.DrawImage(Settings.Player2UpPNG, new Point(0,0));
                        else
                            e.Graphics.DrawImage(Settings.Player2PNG, new Point(0,0));
                        break;

                    default:
                        break;
                }
            }
            else
                e.Graphics.DrawImage(Settings.EmptyPNG, new Point(0,0));
        }

        private void Square_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.board_.CanMakeMove)
            {
                this.currentX_ = (this.Top + e.X) / 50;
                this.currentY_ = (this.Left + e.Y) / 50;

                if (this.innerStatus_ == SquareStatus.PLAYER1 || this.innerStatus_ == SquareStatus.PLAYER2)
                {
                    // if the mouse is on the current player's square
                    if (this.Board.CurrentPlayer - (int)this.innerStatus_ == 0)
                    {
                        // we can only select squares if the players have all placed their squares on board
                        if (this.board_.TotalSquaresSoFar >= Settings.TotalSquaresForEachPlayer * 2)
                        {
                            // property must be set in order to repaint
                            this.IsSelected = !this.isSelected_;
                        }
                    }
                }
                else if (this.innerStatus_ == SquareStatus.EMPTY)
                {
                    if (this.board_.TotalSquaresSoFar >= Settings.TotalSquaresForEachPlayer * 2)
                    {
                        if (board_.LastSelected != null)
                        {
                            if (this.board_.IsValidMove(this.board_.LastSelected.Row, this.board_.LastSelected.Column, this.currentX_, this.currentY_))
                            {
                                // make the move
                                this.board_.MakeMove(this.board_.LastSelected.Row, this.board_.LastSelected.Column, this.currentX_, this.currentY_);
                            }
                            else
                            {
                                if (this.MessageOut != null)
                                    this.MessageOut(new OnMessageOutEventArgs(Settings.Strings[1], MessageType.WARNING, true));

                                // show alert
                                if (Settings.ShowAlertsOnMsgBox)
                                    MessageBox.Show(Settings.Strings[1]);
                            }
                        }
                        else
                        {
                            if (this.MessageOut != null)
                                this.MessageOut(new OnMessageOutEventArgs(Settings.Strings[0], MessageType.WARNING, true));

                            // show alerts
                            if (Settings.ShowAlertsOnMsgBox)
                                MessageBox.Show(Settings.Strings[0]);
                        }
                    }
                    else
                        this.board_.MakeMove(0, 0, this.currentX_, this.currentY_);
                }
            }
        }

        private void Square_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging_ = true;
        }

        private void Square_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging_ = false;
        }

        private void Square_MouseMove(object sender, MouseEventArgs e)
        {
            //if (this.isSelected_ && this.isDragging_)
            //{
            //}
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (this.board_.CanMakeMove)
            {
                if (this.board_.TotalSquaresSoFar >= Settings.TotalSquaresForEachPlayer * 2)
                {
                    // check current square type
                    if (this.innerStatus_ != SquareStatus.BLOCK && this.innerStatus_ != SquareStatus.LINE)
                    {
                        // disable items
                        this.moveToHereToolStripMenuItem.Enabled = false;
                        this.selectToolStripMenuItem.Enabled = false;
                        this.disselectToolStripMenuItem.Enabled = false;

                        if (this.innerStatus_ == SquareStatus.EMPTY)
                        {
                            if (this.board_.LastSelected != null)
                                this.moveToHereToolStripMenuItem.Enabled = true;
                        }
                        else
                        {
                            if (this.board_.CurrentPlayer == (int)this.innerStatus_)
                            {
                                if (this.board_.LastSelected != null)
                                {
                                    if (this.board_.LastSelected.Row == this.row_ &&
                                       this.board_.LastSelected.Column == this.col_)
                                        this.disselectToolStripMenuItem.Enabled = true;
                                    else
                                        this.selectToolStripMenuItem.Enabled = true;
                                }
                                else
                                    this.selectToolStripMenuItem.Enabled = true;
                            }
                            else
                                e.Cancel = true;
                        }
                    }
                    else
                        e.Cancel = true;
                }
                else
                    e.Cancel = true;
            }
            else
                e.Cancel = true;
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.IsSelected = true;
        }

        private void disselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.IsSelected = false;
        }

        private void moveToHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.board_.LastSelected != null)
                this.board_.MakeMove(this.board_.LastSelected.Row, this.board_.LastSelected.Column, this.row_, this.col_);
        }
    }
}
