using System.Data.SQLite;
using System.Data;
using System.Text;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;

namespace ChessProject
{

    public partial class Form1 : Form
    {
        private Button[,] Board = new Button[8, 8];

        private int countdown1 = 10 * 60; //the seconds of the timer1
        private int countdown2 = 10 * 60; //the seconds of the timer3

        int dur1 = 0; int dur2 = 0; //duration of the game
        string Game_Review = " "; //all the game
        private bool botison = false; //bot is on
        public bool bot = false; //bot bool also useful
        private bool game = true; //game is on
        bool select = false; //to see if a button is selected
        string text = string.Empty; //to transfer the text
        Button prev = null; //to erase the text from previous button
        bool color = true; //whites play first

        SQLiteConnection con = new SQLiteConnection($"Data source={Path.Combine(Application.StartupPath, "Chess.db")};Version=3;");

        public Form1()
        {
            InitializeComponent();
            InitializeChessBoard();
            //set the date
            timer2.Interval = 1000;
            timer2.Tick += timer2_Tick;
            timer2.Enabled = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string dbPath = Path.Combine(Application.StartupPath, "Chess.db");
            SQLiteConnection con = new SQLiteConnection($"Data source={dbPath};Version=3;");
            try
            {
                con.Open();
                // Check if the main table exists
                var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='YourTableName';", con);
                var name = cmd.ExecuteScalar();
                if (name == null)
                {
                    // Table does not exist, create it
                    cmd = new SQLiteCommand("CREATE TABLE YourTableName (Column1 TEXT, Column2 INTEGER, ...);", con);
                    cmd.ExecuteNonQuery();
                    // Initialize with any default data as necessary
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize database: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private void seeChoices(Button butt)
        {
            int row = -1; int col = -1; //we search the button's row and column
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] == butt)
                    {
                        row = i;
                        col = j;
                        break; //when it's found we exit the loop
                    }
                }
            }
            //now that we have the row and col, we can put some little x's in the buttons the piece can move
            //rook's move:
            if (butt.Text == "♜" || butt.Text == "♖")
            {
                for (int i = col - 1; i > -1; i--)
                {
                    if (Board[row, i].Text == "")
                    {
                        Board[row, i].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = col + 1; i < 8; i++)
                {
                    if (Board[row, i].Text == "")
                    {
                        Board[row, i].Text = "●";
                    }
                    else { break; }
                }
                for (int i = row - 1; i > -1; i--)
                {
                    if (Board[i, col].Text == "")
                    {
                        Board[i, col].Text = "●";
                    }
                    else { break; }
                }
                for (int i = row + 1; i < 8; i++)
                {
                    if (Board[i, col].Text == "")
                    {
                        Board[i, col].Text = "●";
                    }
                    else { break; }
                }
            }
            //pawn's move:
            if (butt.Text == "♙" && col == 1)
            {
                if (Board[row, 2].Text == "") { Board[row, 2].Text = "●"; }
                if (Board[row, 3].Text == "") { Board[row, 3].Text = "●"; }

            }
            else if (butt.Text == "♙" && col == 6)
            {
                if (Board[row, 7].Text == "") { Board[row, 7].Text = "Q"; }

            }
            else if (butt.Text == "♙" && col < 6)
            {
                if (Board[row, col + 1].Text == "") { Board[row, col + 1].Text = "●"; }
            }
            if (butt.Text == "♟" && col == 6)
            {
                if (Board[row, 5].Text == "") { Board[row, 5].Text = "●"; }
                if (Board[row, 4].Text == "") { Board[row, 4].Text = "●"; }

            }
            else if (butt.Text == "♟" && col == 1)
            {
                if (Board[row, 0].Text == "") { Board[row, 0].Text = "Q"; }

            }
            else if (butt.Text == "♟" && col > 1)
            {
                if (Board[row, col - 1].Text == "") { Board[row, col - 1].Text = "●"; }
            }
            //knight's move:
            if (butt.Text == "♞" || butt.Text == "♘")
            {
                int i = 0; int j = 0;
                //tried to make these with a loop. (no success)
                i = row + 1; j = col - 2;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }

                i = row + 2; j = col - 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }

                j = col + 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }

                i = row + 1; j = col + 2;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }

                i = row - 1; j = col - 2;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }

                i = row - 2; j = col - 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }

                j = col + 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }

                i = row - 1; j = col + 2;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }

            }
            //bishop's move:
            if (butt.Text == "♝" || butt.Text == "♗")
            {
                //same as the rook but with diagonals
                //top left
                for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (Board[i, j].Text == "")
                    {
                        Board[i, j].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }
                //top right
                for (int i = row - 1, j = col + 1; i >= 0 && j < 8; i--, j++)
                {
                    if (Board[i, j].Text == "")
                    {
                        Board[i, j].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }
                //bottom left
                for (int i = row + 1, j = col - 1; i < 8 && j >= 0; i++, j--)
                {
                    if (Board[i, j].Text == "")
                    {
                        Board[i, j].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }
                //bottom right
                for (int i = row + 1, j = col + 1; i < 8 && j < 8; i++, j++)
                {
                    if (Board[i, j].Text == "")
                    {
                        Board[i, j].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //queen's move:
            if (butt.Text == "♛" || butt.Text == "♕")
            {
                //just put rook's and bishop's code together
                //bishop's:
                for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (Board[i, j].Text == "")
                    {
                        Board[i, j].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = row - 1, j = col + 1; i >= 0 && j < 8; i--, j++)
                {
                    if (Board[i, j].Text == "")
                    {
                        Board[i, j].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = row + 1, j = col - 1; i < 8 && j >= 0; i++, j--)
                {
                    if (Board[i, j].Text == "")
                    {
                        Board[i, j].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = row + 1, j = col + 1; i < 8 && j < 8; i++, j++)
                {
                    if (Board[i, j].Text == "")
                    {
                        Board[i, j].Text = "●";
                    }
                    else
                    {
                        break;
                    }
                }

                //rook's:
                for (int i = col - 1; i > -1; i--)
                {
                    if (Board[row, i].Text == "")
                    {
                        Board[row, i].Text = "●";
                    }
                    else { break; }
                }
                for (int i = col + 1; i < 8; i++)
                {
                    if (Board[row, i].Text == "")
                    {
                        Board[row, i].Text = "●";
                    }
                    else { break; }
                }
                for (int i = row - 1; i > -1; i--)
                {
                    if (Board[i, col].Text == "")
                    {
                        Board[i, col].Text = "●";
                    }
                    else { break; }
                }
                for (int i = row + 1; i < 8; i++)
                {
                    if (Board[i, col].Text == "")
                    {
                        Board[i, col].Text = "●";
                    }
                    else { break; }
                }
            }
            //king's move:
            if (butt.Text == "♚" || butt.Text == "♔")
            {
                int i = row; int j = col;
                i = i + 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }
                i = i - 2;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }
                j = j - 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }
                i = i + 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }
                i = i + 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }
                j = j + 2;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }
                i = i - 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }
                i = i - 1;
                if (i < 8 && j < 8 && i >= 0 && j >= 0)
                {
                    if (Board[i, j].Text == "") { Board[i, j].Text = "●"; }
                }
            }
        }
        //hide the choices
        private void hideChoices()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j].Text == "●") { Board[i, j].Text = ""; }
                }
            }
        }

        //set the board for the match
        private void setBoard()
        {
            //setting the fonts, sizes, alignments of all the squares
            int newSize = 36;
            for (int col = 0; col < 8; col++)
            {
                for (int row = 0; row < 8; row++)
                {
                    Board[row, col].TextAlign = ContentAlignment.MiddleCenter;
                    Font newFont = new Font(Board[row, col].Font.FontFamily, newSize, Board[row, col].Font.Style);
                    Board[row, col].Font = newFont;
                    Board[row, col].Text = "";

                }
            }
            //setting the black pieces
            a8.Text = "♜"; b8.Text = "♞";
            c8.Text = "♝"; d8.Text = "♛";
            e8.Text = "♚"; f8.Text = "♝";
            g8.Text = "♞"; h8.Text = "♜";

            //setting the pawns
            for (int row = 0; row < 8; row++)
            {
                Board[row, 1].Text = "♙";
                Board[row, 6].Text = "♟";
            }

            //setting the white pieces
            a1.Text = "♖"; b1.Text = "♘";
            c1.Text = "♗"; d1.Text = "♕";
            e1.Text = "♔"; f1.Text = "♗";
            g1.Text = "♘"; h1.Text = "♖";
        }

        private void Move(object sender, EventArgs e) // it is the click event of all the buttons
        {
            if (game == false) { return; }
            //timers
            if (color) { timer1.Start(); timer3.Stop(); } else { timer3.Start(); timer1.Stop(); }

            Button butt = (Button)sender;
            if (!select)
            {
                if (butt.Text != "")
                {
                    hideChoices(); //hide the choices to see the choices of a new one maybe
                    if (color == true)
                    {
                        if (butt.Text == "♜" ||
                            butt.Text == "♞" ||
                            butt.Text == "♝" ||
                            butt.Text == "♛" ||
                            butt.Text == "♚" ||
                            butt.Text == "♟")
                        { return; }
                    }
                    else
                    {
                        if (butt.Text == "♖" ||
                            butt.Text == "♘" ||
                            butt.Text == "♗" ||
                            butt.Text == "♕" ||
                            butt.Text == "♔" ||
                            butt.Text == "♙")
                        { return; }
                    } //see what color is playing
                    select = true; //select is enabled
                    text = butt.Text; //copy the piece
                    prev = butt; //save the button to erase the piece
                    seeChoices(butt); //show the choices
                }
            }
            else
            {
                select = false; //select is disabled
                hideChoices(); //second time it is pressed, the choices are gone
                if (prev == butt)
                {
                    return; //if you press the same button 2 time, nothing happens
                }
                butt.Text = text; //paste the piece to the new button
                prev.Text = ""; //erase the piece from the last
                color = !color; //now the other color is playing
                //the only way to select another piece to play, is to press the same button again 
                if (color) { timer1.Start(); timer3.Stop(); } else { timer3.Start(); timer1.Stop(); } //timers
                if (bot && botison) { MoveBot(); } //if bot is true then the bot plays
                botison = true;
                Game_Review = Game_Review + butt.Text + butt.Name + ".";
            }
        }

        private void InitializeChessBoard()
        {
            //set the Board with all the buttons inside
            char letter = 'a'; //char because we need letter++; later
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string name = letter.ToString() + (j + 1).ToString(); //we "fix" the button's name
                    Board[i, j] = Controls.Find(name, true).FirstOrDefault() as Button; //and we put it in the 2d array
                }
                letter++; //here
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //here i change the border color cause i like to see the limits
            base.OnPaint(e);
            Color borderColor = Color.Black;
            int borderWidth = 2;

            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "MyChess: \nA Chess Application made by Unipi Students! \nEnjoy ;D",
                "Welcome", MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly,
                false);
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            game = true;
            select = false; //select is disabled
            color = true; //color is white
            if (Player1.Text == "" && Player2.Text == "")
            {
                MessageBox.Show("Fill the names please", "Error");
                return;
            }
            setBoard(); //set for the match
        }

        private void Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Type.SelectedItem == "Blitz") { countdown1 = 3 * 60; countdown2 = 3 * 60; }
            if (Type.SelectedItem == "Bullet") { countdown1 = 1 * 60; countdown2 = 1 * 60; }
            if (Type.SelectedItem == "Rapid") { countdown1 = 10 * 60; countdown2 = 10 * 60; }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label69.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            countdown1--;

            if (countdown1 <= 0)
            {
                timer1.Stop();
                game = false;
                WhiteR.PerformClick();
            }
            else
            {
                p1timer.Text = TimeSpan.FromSeconds(countdown1).ToString(@"mm\:ss");
                dur1++;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            countdown2--;

            if (countdown2 <= 0)
            {
                timer3.Stop();
                game = false;
                BlackR.PerformClick();

            }
            else
            {
                p2timer.Text = TimeSpan.FromSeconds(countdown2).ToString(@"mm\:ss");
                dur2++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer3.Stop();
            game = false;
            bot = false;
            botison = false;
            if (Type.SelectedItem == "Blitz") { countdown1 = 3 * 60; countdown2 = 3 * 60; }
            if (Type.SelectedItem == "Bullet") { countdown1 = 1 * 60; countdown2 = 1 * 60; }
            if (Type.SelectedItem == "Rapid") { countdown1 = 10 * 60; countdown2 = 10 * 60; }
            MessageBox.Show("Black Wins!");
            int dur = dur1 + dur2;
            AddInBase(Player2.Text, dur);
            dur1 = 0; dur2 = 0;
            Game_Review = " ";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer3.Stop();
            game = false;
            bot = false;
            botison = false;
            if (Type.SelectedItem == "Blitz") { countdown1 = 3 * 60; countdown2 = 3 * 60; }
            if (Type.SelectedItem == "Bullet") { countdown1 = 1 * 60; countdown2 = 1 * 60; }
            if (Type.SelectedItem == "Rapid") { countdown1 = 10 * 60; countdown2 = 10 * 60; }
            MessageBox.Show("White Wins!");
            int dur = dur1 + dur2;
            AddInBase(Player2.Text, dur);
            dur1 = 0; dur2 = 0;
            Game_Review = " ";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer3.Stop();
            game = false;
            bot = false;
            botison = false;
            if (Type.SelectedItem == "Blitz") { countdown1 = 3 * 60; countdown2 = 3 * 60; }
            if (Type.SelectedItem == "Bullet") { countdown1 = 1 * 60; countdown2 = 1 * 60; }
            if (Type.SelectedItem == "Rapid") { countdown1 = 10 * 60; countdown2 = 10 * 60; }
            MessageBox.Show("It's A Tie!");
            int dur = dur1 + dur2;
            AddInBase("Tie", dur);
            dur1 = 0; dur2 = 0;
            Game_Review = " ";
        }

        private void PlayBotButton_Click(object sender, EventArgs e)
        {
            select = false; //select is disabled
            color = true; //color is white
            Player2.Text = "Bot";
            if (Player1.Text == "")
            {
                MessageBox.Show("Fill the name please", "Error");
                return;
            }
            setBoard(); //set for the match
            bot = true;
            botison = true;
        }

        private void MoveBot()
        {
            bool pl = true;
            int row = 0; int col = 0;
            int rowd = 0; int cold = 0;
            Random random = new Random();
            do
            {
                do
                {
                    row = random.Next(0, 8); //find the row and col of a black piece
                    col = random.Next(0, 8);
                } while (!(Board[row, col].Text == "♜" || Board[row, col].Text == "♞" ||
                    Board[row, col].Text == "♝" || Board[row, col].Text == "♛" ||
                    Board[row, col].Text == "♚" || Board[row, col].Text == "♟"));

                Move(Board[row, col], EventArgs.Empty);


                bool k = true;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Board[i, j].Text == "●")
                        {
                            rowd = i; cold = j; break;
                            k = false;
                        }
                    }
                }
                if (k)
                {
                    Move(Board[row, col], EventArgs.Empty); pl = false;
                }
            } while (pl);
            botison = false;
            Move(Board[row, col], EventArgs.Empty);
            Move(Board[rowd, cold], EventArgs.Empty);

        }

        private void button6_Click(object sender, EventArgs e)
        {

            string helpMessage = "Welcome to MyChess!\n\n" +
                "1. To play the game, click on a piece and then click on the destination square.\n" +
                "2. Click the same square to cancel the move.\n" +
                "3. Valid moves will be highlighted with '●'.\n" +
                "4. Timers indicate the remaining time for each player.\n" +
                "5. You can choose to play against the bot.\n" +
                "6. Enjoy!";

            MessageBox.Show(helpMessage, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ArchiveButton_Click(object sender, EventArgs e)
        {
            // Create and configure the Archive Form
            Form Archive = new Form
            {
                Text = "Game Archive",
                Size = new Size(850, 500), // Adjust the size as needed
                StartPosition = FormStartPosition.CenterScreen // Center the form on screen
            };

            // Create and configure the ListView
            ListView listView = new ListView
            {
                FullRowSelect = true,
                View = View.Details,
                Dock = DockStyle.Fill, // Make the ListView fill the entire form
            };

            // Add columns to the ListView
            listView.Columns.Add("Player1", 120, HorizontalAlignment.Left);
            listView.Columns.Add("Player2", 120, HorizontalAlignment.Left);
            listView.Columns.Add("Result", 80, HorizontalAlignment.Left);
            listView.Columns.Add("Date", 120, HorizontalAlignment.Left);
            listView.Columns.Add("Duration", 80, HorizontalAlignment.Left);
            listView.Columns.Add("Game_Review", 300, HorizontalAlignment.Left); // Adjust column widths as needed

            string datasource = Path.Combine(Application.StartupPath, "Chess.db");
            string connectionString = $"Data source={datasource};Version=3;";

            // Use using statements for automatic disposal of database objects
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT * FROM Chess", con))
                    using (SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader())
                    {
                        while (sQLiteDataReader.Read())
                        {
                            ListViewItem item = new ListViewItem(sQLiteDataReader["Player1"].ToString());
                            item.SubItems.Add(sQLiteDataReader["Player2"].ToString());
                            item.SubItems.Add(sQLiteDataReader["Result"].ToString());
                            item.SubItems.Add(sQLiteDataReader["Date"].ToString());
                            item.SubItems.Add(sQLiteDataReader["Duration"].ToString());
                            item.SubItems.Add(sQLiteDataReader["Game_Review"].ToString());
                            listView.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load the database: {ex.Message}");
                }
            }

            // Add the ListView to the Archive form and show it
            Archive.Controls.Add(listView);
            Archive.ShowDialog(); // Use ShowDialog to make the form modal if preferred
        }

        private void AddInBase(string Result, int Duration)
        {
            string connectionString = $"Data source={Path.Combine(Application.StartupPath, "Chess.db")};Version=3;";
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand sQLiteCommand = con.CreateCommand())
                    {
                        sQLiteCommand.CommandText = "INSERT INTO Chess([Player1], [Player2], [Result], [Date], [Duration], [Game_Review]) VALUES (@Player1, @Player2, @Result, @Date, @Duration, @Game_Review)";

                        sQLiteCommand.Parameters.AddWithValue("@Player1", Player1.Text); // Assuming Player1 is a control that holds a string
                        sQLiteCommand.Parameters.AddWithValue("@Player2", Player2.Text); // Assuming Player2 is a control that holds a string
                        sQLiteCommand.Parameters.AddWithValue("@Result", Result);
                        // For the date, using current date as example. Adjust accordingly.
                        sQLiteCommand.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); // Correct format for SQLite
                        sQLiteCommand.Parameters.AddWithValue("@Duration", Duration);
                        sQLiteCommand.Parameters.AddWithValue("@Game_Review", Game_Review);

                        int count = sQLiteCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to insert data into database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}