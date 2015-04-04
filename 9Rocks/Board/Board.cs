using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace _9Rocks
{
    public delegate void BoardGameStartDelegate(OnGameStartedEventArgs args);
    public delegate void BoardGameEndDelegate(OnGameEndEventArgs args);
    public delegate void BoardMakeMoveDelegate(OnMoveMakeEventArgs args);
    public delegate void BoardMessageOutDelegate(OnMessageOutEventArgs args);
    public delegate void BoardSquareSelectedChangeDelegate(OnSquareSelectedChangeEventArgs args);
    public delegate void BoardGameStatusChangeDelegate(OnGameStatusChange args);

    public partial class Board : UserControl
    {
        private int[,] innerArray_;

        // swuare
        private Square lastSelected_ = null;
        private Square currentSquare_ = null;
        private Square[,] innerSquareArray_;

        private ArrayList innerMoveHistory_ = new ArrayList();

        // bool
        private bool isPlayer1Computer_ = false;
        private bool isPlayer2Computer_ = false;
        private bool isStarted_ = false;
        private bool isFinished_ = false;
        private bool isPaused_ = false;
        private bool isResumed_ = false;
        private bool isFake_ = false;
        private bool throwPiece_ = false;

        // int
        private int totalSquaresOnBoard_ = 0;
        private int currentPlayerTurn_ = 1;
        private int outSquareOfPlayer1_ = 0;
        private int outSquareOfPlayer2_ = 0;
        private int currentRow_ = 0;
        private int currentCol_ = 0;

        //events
        public event BoardGameStartDelegate GameStarted = null;
        public event BoardGameEndDelegate GameEnded = null;
        public event BoardMakeMoveDelegate MoveMade = null;
        public event BoardMessageOutDelegate MessageOut = null;
        public event BoardSquareSelectedChangeDelegate SquareSelectedChange = null;
        public event BoardGameStatusChangeDelegate GameStatusChange = null;

        public Board(bool fake)
        {
            InitializeComponent();

            this.isFake_ = fake;
        }

        #region Public Properties
        public int CurrentPlayer
        {
            get
            {
                return this.currentPlayerTurn_;
            }
        }

        public Square LastSelected
        {
            get
            {
                return this.lastSelected_;
            }
        }

        public Square CurrentSquare
        {
            get
            {
                return this.currentSquare_;
            }
        }

        public Square[,] Squares
        {
            get
            {
                return this.innerSquareArray_;
            }
        }

        public ArrayList MoveHistory
        {
            get
            {
                return this.innerMoveHistory_;
            }
        }

        public int TotalSquaresOnBoard
        {
            get
            {
                return this.totalSquaresOnBoard_;
            }
        }

        public int OutSquareOfPlayer1
        {
            get
            {
                return this.outSquareOfPlayer1_;
            }
        }

        public int OutSquareOfPlayer2
        {
            get
            {
                return this.outSquareOfPlayer2_;
            }
        }

        public int TotalSquaresSoFar
        {
            get
            {
                return this.totalSquaresOnBoard_ + this.outSquareOfPlayer1_ + this.outSquareOfPlayer2_;
            }
        }

        public GameStatus GameStatus
        {
            get
            {
                if (this.isStarted_)
                    return GameStatus.STARTED;
                else if (this.isFinished_)
                    return GameStatus.FINISHED;
                else if (this.isPaused_)
                    return GameStatus.PAUSED;
                else if (this.isResumed_)
                    return GameStatus.RESUMED;
                else
                    return GameStatus.UNKNOWN;
            }
        }

        public bool CanMakeMove
        {
            get
            {
                return this.isStarted_ || this.isFake_ || this.isResumed_;
            }
        }
        #endregion

        #region Public Methods
        public bool StartGame()
        {
            this.isStarted_ = true;
            this.isFinished_ = false;
            this.isPaused_ = false;
            this.isResumed_ = false;

            // init the board
            this.InitializeBoard();

            // fire the events
            if (this.GameStarted != null)
                this.GameStarted(new OnGameStartedEventArgs());
            if (this.GameStatusChange != null)
                this.GameStatusChange(new OnGameStatusChange(this.GameStatus));

            return true;
        }

        public bool EndGame()
        {
            this.isStarted_ = false;
            this.isFinished_ = true;
            this.isPaused_ = false;
            this.isResumed_ = false;

            // fire the event
            if (this.GameEnded != null)
                this.GameEnded(new OnGameEndEventArgs());
            if (this.GameStatusChange != null)
                this.GameStatusChange(new OnGameStatusChange(this.GameStatus));

            return true;
        }

        public bool PauseGame()
        {
            this.isStarted_ = false;
            this.isFinished_ = false;
            this.isPaused_ = true;
            this.isResumed_ = false;

            // fire the event
            if (this.GameStatusChange != null)
                this.GameStatusChange(new OnGameStatusChange(this.GameStatus));

            return true;
        }

        public bool ResumeGame()
        {
            this.isStarted_ = false;
            this.isFinished_ = false;
            this.isPaused_ = false;
            this.isResumed_ = true;

            // fire the event
            if (this.GameStatusChange != null)
                this.GameStatusChange(new OnGameStatusChange(this.GameStatus));

            return true;
        }

        public bool LoadGame()
        {
            return true;
        }

        public bool SaveGame()
        {
            return true;
        }

        public void ShowSquareHint(int row, int col, bool state)
        {
            if (this.CanMakeMove)
            {
                if (Settings.ShowHint)
                {
                    #region search for neighbour in columns (-)
                    int index = -1;
                    int counter = col - 1;
                    bool found = false;
                    while (!found && counter >= 0)
                    {
                        if (this.innerSquareArray_[row, counter].Status == SquareStatus.EMPTY)
                        {
                            index = counter;
                            found = true;
                        }
                        else
                        {
                            if (this.innerSquareArray_[row, counter].Status == SquareStatus.LINE)
                                counter--;
                            else
                                found = true;
                        }
                    }
                    if (index > -1)
                        this.innerSquareArray_[row, index].ShowHint = state;
                    #endregion

                    #region search for neighbour in columns (+)
                    index = -1;
                    counter = col + 1;
                    found = false;
                    while (!found && counter < 9)
                    {
                        if (this.innerSquareArray_[row, counter].Status == SquareStatus.EMPTY)
                        {
                            index = counter;
                            found = true;
                        }
                        else
                        {
                            if (this.innerSquareArray_[row, counter].Status == SquareStatus.LINE)
                                counter++;
                            else
                                found = true;
                        }
                    }
                    if (index > -1)
                        this.innerSquareArray_[row, index].ShowHint = state;
                    #endregion

                    #region search for neighbour in rows (-)
                    index = -1;
                    counter = row - 1;
                    found = false;
                    while (!found && counter >= 0)
                    {
                        if (this.innerSquareArray_[counter, col].Status == SquareStatus.EMPTY)
                        {
                            index = counter;
                            found = true;
                        }
                        else
                        {
                            if (this.innerSquareArray_[counter, col].Status == SquareStatus.LINE)
                                counter--;
                            else
                                found = true;
                        }
                    }
                    if (index > -1)
                        this.innerSquareArray_[index, col].ShowHint = state;
                    #endregion

                    #region search for neighbour in rows (+)
                    index = -1;
                    counter = row + 1;
                    found = false;
                    while (!found && counter < 9)
                    {
                        if (this.innerSquareArray_[counter, col].Status == SquareStatus.EMPTY)
                        {
                            index = counter;
                            found = true;
                        }
                        else
                        {
                            if (this.innerSquareArray_[counter, col].Status == SquareStatus.LINE)
                                counter++;
                            else
                                found = true;
                        }
                    }
                    if (index > -1)
                        this.innerSquareArray_[index, col].ShowHint = state;
                    #endregion
                }
            }
        }

        public bool IsValidMove(int fromx, int fromy, int tox, int toy)
        {
            bool result = true;

            // check if we are in the same row or column
            if (fromx - tox == 0 || fromy - toy == 0)
            {
                // ok same row
                if (fromx == tox)
                {
                    int miny = fromy;
                    int maxy = toy;

                    if (fromy > toy)
                    {
                        miny = toy;
                        maxy = fromy;
                    }

                    int empty = 1;
                    for (int i = miny + 1; i < maxy; i++)
                    {
                        if (this.innerSquareArray_[fromx, i].Status == SquareStatus.BLOCK ||
                            this.innerSquareArray_[fromx, i].Status == SquareStatus.PLAYER1 ||
                            this.innerSquareArray_[fromx, i].Status == SquareStatus.PLAYER2)
                        {
                            result = false;
                            break;
                        }
                        else
                        {
                            if (this.innerSquareArray_[fromx, i].Status == SquareStatus.EMPTY)
                                empty++;
                        }
                    }
                    // there must be only one empty place to go!
                    if (empty > 1)
                        result = false;
                }
                else
                {
                    // ok same column, check also if we have any other piece on the line
                    int minx = fromx;
                    int maxx = tox;

                    if (fromx > tox)
                    {
                        minx = tox;
                        maxx = fromx;
                    }

                    int empty = 1;
                    for (int i = minx + 1; i < maxx; i++)
                    {
                        if (this.innerSquareArray_[fromy, i].Status == SquareStatus.BLOCK ||
                            this.innerSquareArray_[fromy, i].Status == SquareStatus.PLAYER1 ||
                            this.innerSquareArray_[fromy, i].Status == SquareStatus.PLAYER2)
                        {
                            result = false;
                            break;
                        }
                        else
                        {
                            if (this.innerSquareArray_[fromy, i].Status == SquareStatus.EMPTY)
                                empty++;
                        }
                    }
                    // there must be only one empty place to go!
                    if (empty > 1)
                        result = false;
                }
            }
            else
                result = false;

            return result;
        }

        public void MakeMove(int fromx, int fromy, int tox, int toy)
        {
            if (this.CanMakeMove)
            {
                if (!this.throwPiece_)
                {
                    // place a new square on board
                    if (this.TotalSquaresSoFar < Settings.TotalSquaresForEachPlayer * 2)
                    {
                        // current move in string format
                        string move = (tox + 1).ToString() + Settings.ArrayStringMap[toy];
                        // current player
                        int player = this.currentPlayerTurn_;

                        // set square value
                        this.SetSquareValue(tox, toy, player);
                        // add game history
                        this.innerMoveHistory_.Add(player.ToString() + "|>" + move);
                        // increase square counter
                        this.totalSquaresOnBoard_++;
                        // change turn
                        //this.ChangePlayerTurn();
                        // fire the event
                        if (this.MoveMade != null)
                            this.MoveMade(new OnMoveMakeEventArgs(player, fromx, fromy, tox, toy));
                    }
                    else
                    {
                        // make the move
                        if (this.IsValidMove(fromx, fromy, tox, toy))
                        {
                            // current move in string format
                            string move = (fromx + 1).ToString() + Settings.ArrayStringMap[fromy].ToString() + "-" + (tox + 1).ToString() + Settings.ArrayStringMap[toy].ToString();
                            // current player
                            int player = this.currentPlayerTurn_;

                            // chage the status of the previous square
                            this.SetSquareValue(fromx, fromy, (int)SquareStatus.EMPTY);
                            // set the current square value
                            this.SetSquareValue(tox, toy, player);
                            // add game history
                            this.innerMoveHistory_.Add(player.ToString() + "|" + move);
                            // change turn
                            //this.ChangePlayerTurn();
                            // set the last selected square to null
                            if (this.lastSelected_ != null)
                            {
                                this.lastSelected_.IsSelected = false;
                                this.lastSelected_ = null;
                            }
                            // fire the event
                            if (this.MoveMade != null)
                                this.MoveMade(new OnMoveMakeEventArgs(player, fromx, fromy, tox, toy));
                        }
                        else
                        {
                            // a new message to the pipeline
                            if (this.MessageOut != null)
                                this.MessageOut(new OnMessageOutEventArgs(Settings.Strings[1], MessageType.WARNING, true));

                            // show alert
                            if (Settings.ShowAlertsOnMsgBox)
                                MessageBox.Show(Settings.Strings[1]);
                        }
                    }

                    // Check rock conditions
                    if (!CheckMove(tox, toy))
                        this.ChangePlayerTurn();
                    else
                    {
                        this.throwPiece_ = true;

                        // a new message to the pipeline
                        if (this.MessageOut != null)
                            this.MessageOut(new OnMessageOutEventArgs(Settings.Strings[14], MessageType.ERROR, true));
                    }
                }
                else
                {
                    this.throwPiece_ = false;

                    // current move in string format
                    string move = (tox + 1).ToString() + Settings.ArrayStringMap[toy];
                    // current player
                    int player = this.currentPlayerTurn_;

                    // set square value
                    this.SetSquareValue(tox, toy, 0);
                    // add game history
                    this.innerMoveHistory_.Add(player.ToString() + "|<" + move);

                    this.ChangePlayerTurn();
                }
            }
        }
        private bool CheckMove(int tox, int toy)
        {
            int beginX = 0;
            if (toy == 4)
            {
                if (tox > 2)
                    beginX = 6;
            }

            // search for current row
            int totalX = 0;
            while (beginX < 9)
            {
                if(this.innerSquareArray_[beginX, toy].Status == SquareStatus.BLOCK)
                    break;

                if (this.innerSquareArray_[beginX, toy].Status != SquareStatus.EMPTY
                   && this.innerSquareArray_[beginX, toy].Status != SquareStatus.LINE)
                    totalX += this.innerArray_[beginX, toy];

                beginX++;
            }

            if (totalX == this.currentPlayerTurn_ * 3)
                return true;

            int beginY = 0;
            if (tox == 4)
            {
                if (toy > 2)
                    beginY = 6;
            }

            // search for current row
            int totalY = 0;
            while (beginY < 9)
            {
                if (this.innerSquareArray_[tox, beginY].Status == SquareStatus.BLOCK)
                    break;

                if (this.innerSquareArray_[tox, beginY].Status != SquareStatus.EMPTY
                   && this.innerSquareArray_[tox, beginY].Status != SquareStatus.LINE)
                    totalY += this.innerArray_[tox, beginY];

                beginY++;
            }

            if (totalY == this.currentPlayerTurn_ * 3)
                return true;

            return false;
        }
        #endregion

        #region Private Methods
        private void SetSelectedSquare(int row, int col)
        {
            if (this.CanMakeMove)
            {
                if (!this.innerSquareArray_[row, col].IsSelected)
                {
                    // first disselect the last selected square
                    if (this.lastSelected_ != null)
                    {
                        if (this.lastSelected_.Row != row || this.lastSelected_.Column != col)
                            this.lastSelected_.IsSelected = false;
                        else
                            this.lastSelected_ = null;
                    }
                }
                else
                {
                    // first disselect the last selected square
                    if (this.lastSelected_ != null)
                        this.lastSelected_.IsSelected = false;

                    // then set last selected value to the currently selected square
                    this.lastSelected_ = this.innerSquareArray_[row, col];
                }
            }
        }

        private void ChangePlayerTurn()
        {
            if (this.CanMakeMove)
            {
                if (this.currentPlayerTurn_ == 1)
                    this.currentPlayerTurn_ = 2;
                else
                    this.currentPlayerTurn_ = 1;
            }
        }

        private void SetSquareValue(int row, int col, int val)
        {
            if (this.CanMakeMove)
            {
                // set int array value
                this.innerArray_[row, col] = val;
                // set square array status value
                switch (val)
                {
                    case (int)SquareStatus.BLOCK:
                        this.innerSquareArray_[row, col].Status = SquareStatus.BLOCK;
                        break;
                    case (int)SquareStatus.EMPTY:
                        this.innerSquareArray_[row, col].Status = SquareStatus.EMPTY;
                        break;
                    case (int)SquareStatus.LINE:
                        this.innerSquareArray_[row, col].Status = SquareStatus.LINE;
                        break;
                    case (int)SquareStatus.PLAYER1:
                        this.innerSquareArray_[row, col].Status = SquareStatus.PLAYER1;
                        break;
                    case (int)SquareStatus.PLAYER2:
                        this.innerSquareArray_[row, col].Status = SquareStatus.PLAYER2;
                        break;
                    default:
                        this.innerSquareArray_[row, col].Status = SquareStatus.EMPTY;
                        break;
                }
            }
        }

        private void ResetBoard()
        {
            // remove current controls on board
            this.Controls.Clear();

            // add rocks to the board
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Square newSquare = new Square(this);
                    newSquare.Row = i;
                    newSquare.Column = j;
                    newSquare.MessageOut += new SquareMessageOutDelegate(newSquare_OnMessageOut);
                    newSquare.SquareSelectedChange += new SquareSelectedChangeDelegate(newSquare_OnSquareSelectedChange);
                    newSquare.MouseMove += new MouseEventHandler(newSquare_MouseMove);

                    switch (this.innerArray_[i, j])
                    {
                        case 0: newSquare.Status = SquareStatus.EMPTY;
                            break;
                        case 1: newSquare.Status = SquareStatus.PLAYER1;
                            break;
                        case 2: newSquare.Status = SquareStatus.PLAYER2;
                            break;
                        case 7: newSquare.Status = SquareStatus.BLOCK;
                            break;
                        case 9: newSquare.Status = SquareStatus.LINE;
                            break;
                        default:
                            break;
                    }

                    // add current square to the board control
                    this.Controls.Add(newSquare);
                    // add current square to the inner array in order to access it with row and column variables
                    this.innerSquareArray_[i, j] = newSquare;
                }
            }
        }

        private void InitializeBoard()
        {
            // set background image
            this.BackgroundImage = null;
            // set inner square array
            this.innerSquareArray_ = new Square[9, 9];
            // set inner board array
            this.innerArray_ = new int[9, 9] { {0,9,9,9,0,9,9,9,0},
                                               {9,0,9,9,0,9,9,0,9},
                                               {9,9,0,9,0,9,0,9,9},
                                               {9,9,9,7,7,7,9,9,9},
                                               {0,0,0,7,7,7,0,0,0},
                                               {9,9,9,7,7,7,9,9,9},
                                               {9,9,0,9,0,9,0,9,9},
                                               {9,0,9,9,0,9,9,0,9},
                                               {0,9,9,9,0,9,9,9,0}
                                             };
            // and finally reset the board
            this.ResetBoard();
        }
        #endregion

        #region Events
        public void newSquare_OnSquareSelectedChange(OnSquareSelectedChangeEventArgs args)
        {
            // set board selected square
            this.SetSelectedSquare(args.Row, args.Column);
            // fire event
            if (this.SquareSelectedChange != null)
                this.SquareSelectedChange(args);
        }

        public void newSquare_OnMessageOut(OnMessageOutEventArgs args)
        {
            if (this.MessageOut != null)
                this.MessageOut(args);
        }

        private void Board_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Settings.BoardPNG, new Point(0, 0));
        }

        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(Square))
            {
                Square tmpSquare = (Square)sender;

                // set current square
                this.currentSquare_ = tmpSquare;

                // set current row and column
                this.currentRow_ = tmpSquare.Row;
                this.currentCol_ = tmpSquare.Column;
            }
        }

        public void newSquare_MouseMove(object sender, MouseEventArgs e)
        {
            this.Board_MouseMove(sender, e);
        }
        #endregion
    }
}
