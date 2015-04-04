using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _9Rocks
{
    public partial class MainForm : Form
    {
        private Board board_ = null;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Overcomes the flicker problem on windows forms
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // first load settings
            Settings.LoadSettings();

            // init strings
            InitStrings();

            // init the board
            InitializeBoard();
        }

        private void InitStrings()
        {
            this.player1Label.Text = Settings.Strings[2];
            this.player2Label.Text = Settings.Strings[3];
            this.player1NameLabel.Text = Settings.Strings[6];
            this.player2NameLabel.Text = Settings.Strings[7];
        }

        private void InitializeBoard()
        {
            this.board_ = new Board(false);
            this.board_.Location = new Point(261, 60);

            // add events
            this.board_.GameEnded += new BoardGameEndDelegate(board__OnGameEnded);
            this.board_.GameStarted += new BoardGameStartDelegate(board__OnGameStarted);
            this.board_.MoveMade += new BoardMakeMoveDelegate(board__OnMoveMade);
            this.board_.MessageOut += new BoardMessageOutDelegate(board__OnMessageOut);
            this.board_.SquareSelectedChange += new BoardSquareSelectedChangeDelegate(board__OnSquareSelectedChange);
            this.board_.GameStatusChange += new BoardGameStatusChangeDelegate(board__OnGameStatusChange);
            
            this.Controls.Add(this.board_);
        }

        public void board__OnGameStatusChange(OnGameStatusChange args)
        {
            if (args.Status == GameStatus.STARTED)
                this.SetGameStatus(Settings.Strings[8], MessageType.INFO);
            else if(args.Status== GameStatus.FINISHED)
                this.SetGameStatus(Settings.Strings[11], MessageType.INFO);
            else if (args.Status == GameStatus.PAUSED)
                this.SetGameStatus(Settings.Strings[9], MessageType.INFO);
            else if(args.Status == GameStatus.RESUMED)
                this.SetGameStatus(Settings.Strings[10], MessageType.INFO);
            else
                this.SetGameStatus(Settings.Strings[13], MessageType.INFO);
        }

        public void board__OnSquareSelectedChange(OnSquareSelectedChangeEventArgs args)
        {
            if (args.IsSelected)
                this.SetGameStatus(Settings.Strings[4] + (args.Row + 1).ToString() + Settings.ArrayStringMap[args.Column], MessageType.INFO);
        }

        public void board__OnMessageOut(OnMessageOutEventArgs args)
        {
            if (args.Show)
                this.SetGameStatus(args.Message, args.Type);
        }

        public void board__OnMoveMade(OnMoveMakeEventArgs args)
        {
            string move = "";

            // prepare move string
            if (this.board_.TotalSquaresSoFar < Settings.TotalSquaresForEachPlayer * 2)
                move = (args.ToX + 1).ToString() + Settings.ArrayStringMap[args.ToY];
            else
                move = (args.FromX + 1).ToString() + Settings.ArrayStringMap[args.FromY].ToString() + "-" + (args.ToX + 1).ToString() + Settings.ArrayStringMap[args.ToY].ToString();
            // add it to the history listview
            this.AddGameHistory(args.Player, move);
            // show current turn information
            if (this.board_.CurrentPlayer == 1)
                this.currentTurnBox.Image = Settings.Player1ThumbPNG;
            else
                this.currentTurnBox.Image = Settings.Player2ThumbPNG;
        }

        public void board__OnGameStarted(OnGameStartedEventArgs args)
        {
            // init player information
            this.player1Box.Image = Settings.Player1ThumbPNG;
            this.player2Box.Image = Settings.Player2ThumbPNG;
            if (this.board_.CurrentPlayer == 1)
                this.currentTurnBox.Image = Settings.Player1ThumbPNG;
            else
                this.currentTurnBox.Image = Settings.Player2ThumbPNG;
        }

        public void board__OnGameEnded(OnGameEndEventArgs args)
        {
            // init player information
            this.player1Box.Image = null;
            this.player2Box.Image = null;
            this.currentTurnBox.Image = null;
        }

        public void SetGameStatus(string msg, MessageType type)
        {
            switch (type)
            {
                case MessageType.ERROR: this.gameStatusLabel.ForeColor = Color.Red;
                    break;
                case MessageType.WARNING: this.gameStatusLabel.ForeColor = Color.Blue;
                    break;
                case MessageType.INFO: this.gameStatusLabel.ForeColor = Color.Green;
                    break;
                default: this.gameStatusLabel.ForeColor = Color.Green;
                    break;
            }

            this.gameStatusLabel.Text = msg;
        }

        private void AddGameHistory(int player, string move)
        {
            // init message
            string[] current = new string[3];
            // set move count
            current[0] = (this.gameHistoryListView.Items.Count + 1).ToString();
            // set player name
            if (player == 1)
                current[1] = Settings.Strings[2];
            else
                current[1] = Settings.Strings[3];
            current[2] = move;

            // add to listview
            this.gameHistoryListView.Items.Add(new ListViewItem(current));
            // ensure visible
            this.gameHistoryListView.EnsureVisible(this.gameHistoryListView.Items.Count - 1);
            // add to status label
            if (player == 1)
                this.SetGameStatus(Settings.Strings[2] + " : " + move, MessageType.INFO);
            else
                this.SetGameStatus(Settings.Strings[3] + " : " + move, MessageType.INFO);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // start game
            this.board_.StartGame();
            // enable/disable items
            this.startToolStripMenuItem.Enabled = false;
            this.pauseToolStripMenuItem.Enabled = true;
            this.resumeToolStripMenuItem.Enabled = false;
            this.resignToolStripMenuItem.Enabled = true;
            this.saveAsToolStripMenuItem.Enabled = true;
            this.saveToolStripMenuItem.Enabled = true;
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // pause game
            this.board_.PauseGame();
            // enable/disable items
            this.pauseToolStripMenuItem.Enabled = false;
            this.resumeToolStripMenuItem.Enabled = true;
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // resume game
            this.board_.ResumeGame();
            // enable/disable items
            this.pauseToolStripMenuItem.Enabled = true;
            this.resumeToolStripMenuItem.Enabled = false;
        }
    }
}
