using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace DamkaUI
{
    public class GameSettings : Form
    {
        private readonly Label LabelBoardSettings;
        private readonly Label LabelPlayers;
        private readonly Label LabelPlayerOne;
        private readonly Label LabelPlayerTwo;

        private BoardSizeRadioButton BoardSizeRadioButtonSixOnSix;
        private BoardSizeRadioButton BoardSizeRadioButtonEightOnEight;
        private BoardSizeRadioButton BoardSizeRadioButtonTenOnTen;
        private CheckBox CheckBoxPlayerTwoHuman = new CheckBox();
        private TextBox LabelPlayerOneName = new TextBox();
        private TextBox LabelPlayerTwoName = new TextBox();
        private PictureBox PictureBoxDoneButton = new PictureBox();
        private Thread m_GameThread;

        private bool m_IsAgainstComputer = true;
        private string m_PlayerOneName;
        private string m_PlayerTwoName;
        private int m_BoardSize;

        public GameSettings()
        {
            LabelBoardSettings = new Label();
            LabelPlayers = new Label();
            LabelPlayerOne = new Label();
            LabelPlayerTwo = new Label();
            initializeGameSettingWindow();
        }

        private void done_clicked(object sender, EventArgs e)
        {
            m_PlayerOneName = LabelPlayerOneName.Text;
            m_PlayerTwoName = LabelPlayerTwoName.Text;
            this.Close();
            m_GameThread = new Thread(startGame);
            m_GameThread.SetApartmentState(ApartmentState.STA);
            m_GameThread.Start();
        }

        private void startGame()
        {
            Damka game = new Damka(m_PlayerOneName, m_PlayerTwoName, m_BoardSize, m_IsAgainstComputer);
            game.Start();
        }

        private void on_close(object i_Sender, FormClosedEventArgs i_E)
        {
            this.Close();
        }

        private void enable_done(object i_Sender, EventArgs i_E)
        {
            PictureBoxDoneButton.Enabled = true;
            PictureBoxDoneButton.Image = DamkaUI.Properties.Resources.doneButton;
            if ((i_Sender as BoardSizeRadioButton).Text == "6x6")
            {
                m_BoardSize = 6;
            }
            else if ((i_Sender as BoardSizeRadioButton).Text == "8x8")
            {
                m_BoardSize = 8;
            }
            else if ((i_Sender as BoardSizeRadioButton).Text == "10x10")
            {
                m_BoardSize = 10;
            }
        }

        private void checkBox_change(object i_Sender, EventArgs i_E)
        {
            LabelPlayerTwoName.Enabled = !LabelPlayerTwoName.Enabled;
            m_IsAgainstComputer = !CheckBoxPlayerTwoHuman.Checked;
        }

        private void textBox_disabled(object i_Sender, EventArgs i_E)
        {
            if (LabelPlayerTwoName.Enabled == false)
            {
                LabelPlayerTwoName.Text = "[Computer]";
            }
        }

        private void initializeGameSettingWindow()
        {
            this.BackgroundImage = DamkaUI.Properties.Resources.MarbleBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Icon = Icon.FromHandle(DamkaUI.Properties.Resources.checkrsIcon.GetHicon());
            this.Size = new Size(280, 210);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            

            BoardSizeRadioButtonSixOnSix = new BoardSizeRadioButton("6x6", 30, 20);
            BoardSizeRadioButtonSixOnSix.Click += new EventHandler(enable_done);

            BoardSizeRadioButtonEightOnEight = new BoardSizeRadioButton("8x8", BoardSizeRadioButtonSixOnSix.Top, BoardSizeRadioButtonSixOnSix.Left + 70);
            BoardSizeRadioButtonEightOnEight.Click += new EventHandler(enable_done);

            BoardSizeRadioButtonTenOnTen = new BoardSizeRadioButton("10x10", BoardSizeRadioButtonSixOnSix.Top, BoardSizeRadioButtonEightOnEight.Left + 70);
            BoardSizeRadioButtonTenOnTen.Click += new EventHandler(enable_done);
            this.Controls.Add(BoardSizeRadioButtonSixOnSix);
            this.Controls.Add(BoardSizeRadioButtonEightOnEight);
            this.Controls.Add(BoardSizeRadioButtonTenOnTen);

            LabelBoardSettings.Font = new Font("Arial", 10);
            LabelBoardSettings.Text = "Board Size:";
            LabelBoardSettings.Top = 5;
            LabelBoardSettings.Left = 5;
            LabelBoardSettings.BackColor = Color.Transparent;
            this.Controls.Add(LabelBoardSettings);


            LabelPlayers.Font = new Font("Arial", 10);
            LabelPlayers.Text = "Players:";
            LabelPlayers.Top = 50;
            LabelPlayers.Left = 5;
            LabelPlayers.BackColor = Color.Transparent;
            this.Controls.Add(LabelPlayers);


            LabelPlayerOne.Font = new Font("Arial", 10);
            LabelPlayerOne.Text = "Player 1:";
            LabelPlayerOne.Top = 75;
            LabelPlayerOne.Left = 20;
            LabelPlayerOne.Size = new Size(70, 20);
            LabelPlayerOne.BackColor = Color.Transparent;
            this.Controls.Add(LabelPlayerOne);

            LabelPlayerTwo.Font = new Font("Arial", 10);
            LabelPlayerTwo.Text = "Player 2:";
            LabelPlayerTwo.Top = 100;
            LabelPlayerTwo.Left = 40;
            LabelPlayerTwo.Size = new Size(70, 20);
            LabelPlayerTwo.BackColor = Color.Transparent;
            this.Controls.Add(LabelPlayerTwo);

            LabelPlayerOneName.Top = LabelPlayerOne.Top;
            LabelPlayerOneName.Left = LabelPlayerOne.Right + 20;
            this.Controls.Add(LabelPlayerOneName);

            LabelPlayerTwoName.Enabled = false;
            LabelPlayerTwoName.Text = "[Computer]";
            LabelPlayerTwoName.Top = LabelPlayerTwo.Top;
            LabelPlayerTwoName.Left = LabelPlayerTwo.Right + 20;
            this.Controls.Add(LabelPlayerTwoName);
            LabelPlayerTwoName.EnabledChanged += new EventHandler(textBox_disabled);

            CheckBoxPlayerTwoHuman.Checked = false;
            CheckBoxPlayerTwoHuman.Top = 98;
            CheckBoxPlayerTwoHuman.Left = 20;
            CheckBoxPlayerTwoHuman.BackColor = Color.Transparent;
            this.Controls.Add(CheckBoxPlayerTwoHuman);
            CheckBoxPlayerTwoHuman.CheckedChanged += new EventHandler(checkBox_change);

            PictureBoxDoneButton.Enabled = false;
            PictureBoxDoneButton.Text = "Done";
            PictureBoxDoneButton.Top = this.ClientSize.Height - PictureBoxDoneButton.Height;
            PictureBoxDoneButton.Left = this.ClientSize.Width - PictureBoxDoneButton.Width;
            PictureBoxDoneButton.Image = DamkaUI.Properties.Resources.doneButtonDisabled;
            PictureBoxDoneButton.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(PictureBoxDoneButton);
            PictureBoxDoneButton.Click += new EventHandler(done_clicked);

            this.Text = "Game Settings";
        }
    }
}
