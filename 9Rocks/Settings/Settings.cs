using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;

namespace _9Rocks
{
    public static class Settings
    {
        // images
        private static Image boardPNG_;
        private static Image emptyPNG_;
        private static Image player1PNG_;
        private static Image player1ThumbPNG_;
        private static Image player1upPNG_;
        private static Image player2PNG_;
        private static Image player2ThumbPNG_;
        private static Image player2upPNG_;

        private static List<string> innerStrings_ = new List<string>();

        // strings
        private static string arrayStringMap_;

        // ints
        private static int totalSquareForEachPlayer_;

        private static string currentTheme_ = "";

        private static bool showHint_ = true;
        private static bool showAlertsOnMsgBox_ = false;

        public static void LoadSettings()
        {
            // set theme
            currentTheme_ = "default"; // sex
            // load images
            boardPNG_ = Image.FromFile(Environment.CurrentDirectory + @"\Themes\" + currentTheme_ + @"\Board\Board.png");
            emptyPNG_ = Image.FromFile(Environment.CurrentDirectory + @"\Themes\" + currentTheme_ + @"\Rocks\Empty.png");
            player1PNG_ = Image.FromFile(Environment.CurrentDirectory + @"\Themes\" + currentTheme_ + @"\Rocks\Player1.png");
            player2PNG_ = Image.FromFile(Environment.CurrentDirectory + @"\Themes\" + currentTheme_ + @"\Rocks\Player2.png");
            player1upPNG_ = Image.FromFile(Environment.CurrentDirectory + @"\Themes\" + currentTheme_ + @"\Rocks\Player1Up.png");
            player2upPNG_ = Image.FromFile(Environment.CurrentDirectory + @"\Themes\" + currentTheme_ + @"\Rocks\Player2Up.png");
            // prepare image thumbs
            PrepareThumbs();
            // load strings
            innerStrings_.Add("No square selected! Please select one first!");  //0
            innerStrings_.Add("Not a valid move!");                             //1
            innerStrings_.Add("Player 1");                                      //2
            innerStrings_.Add("Player 2");                                      //3
            innerStrings_.Add("Selected item : ");                              //4
            innerStrings_.Add("Game Information");                              //5
            innerStrings_.Add("Jerry Seinfeld");                                //6
            innerStrings_.Add("George Costanza");                               //7
            innerStrings_.Add("Game started...");                               //8
            innerStrings_.Add("Game paused...");                                //9
            innerStrings_.Add("Game resumed...");                               //10
            innerStrings_.Add("Game finished...");                              //11
            innerStrings_.Add("wins the game!");                                //12
            innerStrings_.Add("Unknown status...");                             //13
            innerStrings_.Add("Select a piece from the oppenent :)");           //14
            // set number-letter string map
            arrayStringMap_ = "ABCDEFGHIJ";
            showHint_ = true;
            showAlertsOnMsgBox_ = false;
            totalSquareForEachPlayer_ = 9;
        }

        private static bool Test()
        {
            return true;
        }

        private static void PrepareThumbs()
        {
            player1ThumbPNG_ = player1PNG_.GetThumbnailImage(25, 25, new Image.GetThumbnailImageAbort(Test), System.IntPtr.Zero);
            player2ThumbPNG_ = player2PNG_.GetThumbnailImage(25, 25, new Image.GetThumbnailImageAbort(Test), System.IntPtr.Zero);
        }

        public static string Theme
        {
            get
            {
                return currentTheme_;
            }
        }

        public static int TotalSquaresForEachPlayer
        {
            get
            {
                return totalSquareForEachPlayer_;
            }
        }

        public static string ArrayStringMap
        {
            get
            {
                return arrayStringMap_;
            }
        }

        public static List<string> Strings
        {
            get
            {
                return innerStrings_;
            }
        }

        public static bool ShowHint
        {
            get
            {
                return showHint_;
            }
        }

        public static bool ShowAlertsOnMsgBox
        {
            get
            {
                return showAlertsOnMsgBox_;
            }
        }

        public static Image BoardPNG
        {
            get
            {
                return boardPNG_;
            }
        }

        public static Image EmptyPNG
        {
            get
            {
                return emptyPNG_;
            }
        }

        public static Image Player1PNG
        {
            get
            {
                return player1PNG_;
            }
        }

        public static Image Player1ThumbPNG
        {
            get
            {
                return player1ThumbPNG_;
            }
        }

        public static Image Player1UpPNG
        {
            get
            {
                return player1upPNG_;
            }
        }

        public static Image Player2PNG
        {
            get
            {
                return player2PNG_;
            }
        }

        public static Image Player2ThumbPNG
        {
            get
            {
                return player2ThumbPNG_;
            }
        }

        public static Image Player2UpPNG
        {
            get
            {
                return player2upPNG_;
            }
        }
    }
}
