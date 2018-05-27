using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using B18_Ex02_Eyal_321149296_Daniel_311250336;

namespace DamkaUI
{
    public class Damka : Form
    {
        private PictureBox[,] m_GameBoardGraphics;
        private GameBoard m_GameBoardData;
        private Player m_PlayerOne;
        private Player m_PlayerTwo;
        private Player m_CurrentPlayer;
        private Player m_NextPlayer;
        private Label m_PlayerOneLabel = new Label();
        private Label m_PlayerTwoLabel = new Label();
        private Label m_PlayerOneScore = new Label();
        private Label m_PlayerTwoScore = new Label();

        private const bool v_IsComputerPlayer = true;

        public Damka(string i_PlayerOneName, string i_PlayerTwoName, int i_BoardSize, bool i_IsAgainstComputer)
        {
            m_GameBoardData = new GameBoard(i_BoardSize);

            m_PlayerOne = new Player(i_PlayerOneName,!v_IsComputerPlayer,PieceSymbol.eGameSymbols.PlayerOneRegular,m_GameBoardData);
            if (i_IsAgainstComputer == true)
            {
                m_PlayerTwo = new Player(i_PlayerTwoName, v_IsComputerPlayer, PieceSymbol.eGameSymbols.PlayerTwoRegular, m_GameBoardData);
            }
            else
            {
                m_PlayerTwo = new Player(i_PlayerTwoName, !v_IsComputerPlayer, PieceSymbol.eGameSymbols.PlayerTwoRegular, m_GameBoardData);
            }

            m_PlayerOne.MoveDirection = Player.eMoveDirection.Up;
            m_PlayerTwo.MoveDirection = Player.eMoveDirection.Down;

            m_CurrentPlayer = m_PlayerTwo;
            m_NextPlayer = m_PlayerOne;

            m_GameBoardGraphics = new PictureBox[i_BoardSize, i_BoardSize];

            this.Text = "Damka";
            this.Size = new Size(((i_BoardSize * 55) + 15), ((i_BoardSize * 56) + 85));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

        }

        public void Start()
        {
            initializeGameBoardGraphics();
            ShowDialog();
        }

        private void initializeGameBoardGraphics()
        {
            int top = 50;
            int left = 5;

            initializePlayerData();
            for (int i = 0; i < m_GameBoardData.GameBoardSize; i++)
            {
               
                for (int j = 0; j < m_GameBoardData.GameBoardSize; j++)
                {
                    m_GameBoardGraphics[i, j] = new PictureBox();
                    m_GameBoardGraphics[i, j].Size = new Size(55, 56);
                    m_GameBoardGraphics[i, j].Top = top;
                    m_GameBoardGraphics[i, j].Left = left;
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            m_GameBoardGraphics[i, j].Image = DamkaUI.Properties.Resources.BlackTile;
                        }
                        else
                        {
                            m_GameBoardGraphics[i, j].Image = DamkaUI.Properties.Resources.WhiteTile;
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            m_GameBoardGraphics[i, j].Image = DamkaUI.Properties.Resources.WhiteTile;
                        }
                        else
                        {
                            m_GameBoardGraphics[i, j].Image = DamkaUI.Properties.Resources.BlackTile;
                        }
                    }
                    this.Controls.Add(m_GameBoardGraphics[i, j]);
                    left += 55;
                }
                left = 5;
                top += 56;

            }
        }

        private void initializePlayerData()
        {
            m_PlayerOneLabel.Text = m_PlayerOne.Name +": "+m_PlayerOne.Score.ToString();
            m_PlayerOneLabel.Font = new Font("Arial", 11);
            m_PlayerOneLabel.Top = 10;
            m_PlayerOneLabel.Left = 50;
            m_PlayerOneLabel.AutoSize = true;
            //m_PlayerOneLabel.Size = new Size(m_PlayerOneLabel.Text.Length * 8, 20);
            this.Controls.Add(m_PlayerOneLabel);

            //m_PlayerOneScore.Text = m_PlayerOne.Score.ToString();
            //m_PlayerOneScore.Font = new Font("Arial", 10);
            //m_PlayerOneScore.Top = 10;
            //m_PlayerOneScore.Left = m_PlayerOneLabel.Width + m_PlayerOneLabel.Left;
            //this.Controls.Add(m_PlayerOneScore);
        }
    }
}
