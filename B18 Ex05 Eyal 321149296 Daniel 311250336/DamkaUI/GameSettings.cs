using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DamkaUI
{
    public class GameSettings : Form
    {
        private readonly Label r_BoardSettings;
        private readonly Label r_Players;
        private readonly Label r_PlayerOne;
        private readonly Label r_PlayerTwo;

        private RadioButton m_SixOnSix = new RadioButton();
        private RadioButton m_EightOnEight = new RadioButton();
        private RadioButton m_TenOnTen = new RadioButton();
        private CheckBox m_PlayerTwoCheckBox = new CheckBox();
        private TextBox m_PlayerOneName = new TextBox();
        private TextBox m_PlayerTwoName = new TextBox();
        private Button m_DoneButton = new Button();

        private bool m_IsAgainstComputer = true;

        private int m_BoardSize;

        public GameSettings()
        {
            r_BoardSettings = new Label();
            r_Players = new Label();
            r_PlayerOne = new Label();
            r_PlayerTwo = new Label();
            initializeGameSettingWindow();
        }

        private void done_clicked(object sender, EventArgs e)
        {
            this.Hide();
            startGame();
               
        }

        private void startGame()
        {
            Damka game = new Damka(m_PlayerOneName.Text, m_PlayerTwoName.Text, m_BoardSize, m_IsAgainstComputer);
            game.FormClosed += new FormClosedEventHandler(on_close);
            game.Start();

        }

        private void on_close(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void enable_done(object sender, EventArgs e)
        {
            m_DoneButton.Enabled = true;
            if ((sender as RadioButton).Text == "6x6")
            {
                m_BoardSize = 6;
            }
            else if ((sender as RadioButton).Text == "8x8")
            {
                m_BoardSize = 8;
            }
            else if ((sender as RadioButton).Text == "10x10")
            {
                m_BoardSize = 10;
            }
        }

        private void checkBox_change(object sender, EventArgs e)
        {
            m_PlayerTwoName.Enabled = !m_PlayerTwoName.Enabled;
            m_IsAgainstComputer = !m_PlayerTwoCheckBox.Checked;
        }

        private void textBox_disabled(object sender, EventArgs e)
        {
            if (m_PlayerTwoName.Enabled == false)
            {
                m_PlayerTwoName.Text = "[Computer]";
            }
        }

        private void initializeGameSettingWindow()
        {
            this.Icon = Icon.FromHandle(DamkaUI.Properties.Resources.checkrsIcon.GetHicon());
            this.Size = new Size(280, 210);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Controls.Add(m_SixOnSix);
            this.Controls.Add(m_EightOnEight);
            this.Controls.Add(m_TenOnTen);

            m_SixOnSix.Font = new Font("Arial", 10);
            m_SixOnSix.Text = "6x6";
            m_SixOnSix.Top = 30;
            m_SixOnSix.Left = 20;
            m_SixOnSix.Size = new Size(60, 20);
            m_SixOnSix.Click += new EventHandler(enable_done);

            m_EightOnEight.Font = new Font("Arial", 10);
            m_EightOnEight.Text = "8x8";
            m_EightOnEight.Top = 30;
            m_EightOnEight.Left = 90;
            m_EightOnEight.Size = new Size(60, 20);
            m_EightOnEight.Click += new EventHandler(enable_done);

            m_TenOnTen.Font = new Font("Arial", 10);
            m_TenOnTen.Text = "10x10";
            m_TenOnTen.Top = 30;
            m_TenOnTen.Left = 160;
            m_TenOnTen.Size = new Size(80, 20);
            m_TenOnTen.Click += new EventHandler(enable_done);


            r_BoardSettings.Font = new Font("Arial", 10);
            r_BoardSettings.Text = "Board Size:";
            r_BoardSettings.Top = 5;
            r_BoardSettings.Left = 5;
            this.Controls.Add(r_BoardSettings);


            r_Players.Font = new Font("Arial", 10);
            r_Players.Text = "Players:";
            r_Players.Top = 50;
            r_Players.Left = 5;
            this.Controls.Add(r_Players);


            r_PlayerOne.Font = new Font("Arial", 10);
            r_PlayerOne.Text = "Player 1:";
            r_PlayerOne.Top = 75;
            r_PlayerOne.Left = 20;
            r_PlayerOne.Size = new Size(70, 20);
            this.Controls.Add(r_PlayerOne);

            r_PlayerTwo.Font = new Font("Arial", 10);
            r_PlayerTwo.Text = "Player 2:";
            r_PlayerTwo.Top = 100;
            r_PlayerTwo.Left = 40;
            r_PlayerTwo.Size = new Size(70, 20);
            this.Controls.Add(r_PlayerTwo);

            m_PlayerOneName.Top = 75;
            m_PlayerOneName.Left = 110;
            this.Controls.Add(m_PlayerOneName);

            m_PlayerTwoName.Enabled = false;
            m_PlayerTwoName.Text = "[Computer]";
            m_PlayerTwoName.Top = 100;
            m_PlayerTwoName.Left = 110;
            this.Controls.Add(m_PlayerTwoName);
            m_PlayerTwoName.EnabledChanged += new EventHandler(textBox_disabled);

            m_PlayerTwoCheckBox.Checked = false;
            m_PlayerTwoCheckBox.Top = 98;
            m_PlayerTwoCheckBox.Left = 20;
            this.Controls.Add(m_PlayerTwoCheckBox);

            m_PlayerTwoCheckBox.CheckedChanged += new EventHandler(checkBox_change);

            m_DoneButton.Enabled = false;
            m_DoneButton.Text = "Done";
            m_DoneButton.Top = this.ClientSize.Height - m_DoneButton.Height;
            m_DoneButton.Left = this.ClientSize.Width - m_DoneButton.Width;
            this.Controls.Add(m_DoneButton);
            m_DoneButton.Click += new EventHandler(done_clicked);

            this.Text = "Game Settings";
        }
    }
}
