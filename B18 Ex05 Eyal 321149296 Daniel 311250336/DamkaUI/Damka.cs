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
        private const int k_TileHeight = 56;
        private const int k_TileWidth = 55;
        private PictureBox[,] m_GameBoardGraphics;
        private GameBoard m_GameBoardData;
        private Player m_PlayerOne;
        private Player m_PlayerTwo;
        private Player m_CurrentPlayer;
        private Player m_NextPlayer;
        private Label m_PlayerOneLabel = new Label();
        private Label m_PlayerTwoLabel = new Label();
        private List<PictureBox> m_PlayerOnePieces;
        private List<PictureBox> m_PlayerTwoPieces;
        private PictureBox m_PieceTaken;
        private BoardPosition m_CurrentPosition = new BoardPosition();
        private BoardPosition m_NewPlace = new BoardPosition();
        private bool m_FirstTurnClick = true;

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
            this.Size = new Size(((i_BoardSize * k_TileWidth) + 25), ((i_BoardSize * k_TileHeight) + 90));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Icon = Icon.FromHandle(DamkaUI.Properties.Resources.checkrsIcon.GetHicon());

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
                    m_GameBoardGraphics[i, j] = initializeTilePictureBox(top, left, i, j);
                    this.Controls.Add(m_GameBoardGraphics[i, j]);
                    m_GameBoardGraphics[i, j].SendToBack();
                    left += k_TileWidth;
                }
                left = 5;
                top += k_TileHeight;

            }

            initializePieceGraphics();
        }

        private void initializePlayerData()
        {
            m_PlayerOnePieces = new List<PictureBox>((m_GameBoardData.GameBoardSize / 2) * ((m_GameBoardData.GameBoardSize / 2) - 1));
            m_PlayerOneLabel.Text = m_PlayerOne.Name + ": " + m_PlayerOne.Score.ToString();
            m_PlayerOneLabel.Font = new Font("Arial", 11);
            m_PlayerOneLabel.Top = 10;
            m_PlayerOneLabel.Left = 50;
            m_PlayerOneLabel.AutoSize = true;
            this.Controls.Add(m_PlayerOneLabel);
            m_PlayerTwoPieces = new List<PictureBox>((m_GameBoardData.GameBoardSize / 2) * ((m_GameBoardData.GameBoardSize / 2) - 1));
            m_PlayerTwoLabel.Text = m_PlayerTwo.Name + ": " + m_PlayerTwo.Score.ToString();
            m_PlayerTwoLabel.Font = new Font("Arial", 11);
            m_PlayerTwoLabel.Top = 10;
            m_PlayerTwoLabel.Left = m_PlayerOneLabel.Right + 50;
            m_PlayerTwoLabel.AutoSize = true;
            this.Controls.Add(m_PlayerTwoLabel);
        }

        private void initializePieceGraphics()
        {
            int top = 50;
            int left = 5;

            for (int i = 0; i < m_GameBoardData.GameBoardSize; i++)
            {
                for (int j = 0; j < m_GameBoardData.GameBoardSize; j++)
                {
                    if (m_GameBoardData.GetCellSymbol(i, j) == (char) PieceSymbol.eGameSymbols.PlayerOneRegular)
                    {
                        m_PlayerOnePieces.Add(initializePiecePictureBox(top, left, DamkaUI.Properties.Resources.white, i, j));
                    }
                    else if (m_GameBoardData.GetCellSymbol(i, j) == (char) PieceSymbol.eGameSymbols.PlayerTwoRegular)
                    {
                        m_PlayerTwoPieces.Add(initializePiecePictureBox(top, left, DamkaUI.Properties.Resources.black, i, j));
                    }

                    left += k_TileWidth;
                }

                left = 5;
                top += k_TileHeight;
            }
        }

        private PictureBox initializeTilePictureBox(int i_Top, int i_Left, int i_Row, int i_Column)
        {
            PictureBox pic = new PictureBox();

            pic.Size = new Size(k_TileWidth, k_TileHeight);
            pic.Top = i_Top;
            pic.Left = i_Left;
            if (i_Row % 2 == 0)
            {
                if (i_Column % 2 == 0)
                {
                    pic.Image = DamkaUI.Properties.Resources.BlackTile;
                }
                else
                {
                    pic.Image = DamkaUI.Properties.Resources.WhiteTile;
                }
            }
            else
            {
                if (i_Column % 2 == 0)
                {
                    pic.Image = DamkaUI.Properties.Resources.WhiteTile;
                }
                else
                {
                    pic.Image = DamkaUI.Properties.Resources.BlackTile;
                }
            }

            pic.Click += new EventHandler(tile_click);

            return pic;
        }

        private void tile_click(object i_Sender, EventArgs e)
        {
            if (m_FirstTurnClick == false)
            {
                m_NewPlace.Column = ((i_Sender as PictureBox).Left - 5) / k_TileWidth;
                m_NewPlace.Row = ((i_Sender as PictureBox).Top - 50) / k_TileHeight;
                if (m_CurrentPlayer.CheckMoveAvailabillityAndMove(m_CurrentPosition, m_NewPlace,
                    m_CurrentPlayer.PiecesThatMustCapture()) == true)
                {
                    m_PieceTaken.Left = (i_Sender as PictureBox).Left;
                    m_PieceTaken.Top = (i_Sender as PictureBox).Top;
                    m_PieceTaken.BackColor = Color.Transparent;
                    updateIfCaptured();
                    m_FirstTurnClick = true;
                    swapActivePlayer(ref m_CurrentPlayer, ref m_NextPlayer);
                }
                else
                {
                    m_PieceTaken.BackColor = Color.Transparent;
                    MessageBox.Show("Illegal move");
                    m_FirstTurnClick = true;
                }
            }
            else
            {
                MessageBox.Show("Invalid selection.");
            }
        }

        private void updateIfCaptured()
        {
            if (m_CurrentPlayer.CapturedAPiece)
            {
                BoardPosition capturedPiece = new BoardPosition();

                capturedPiece = m_NewPlace - m_CurrentPosition;
                capturedPiece.Column = capturedPiece.Column / 2;
                capturedPiece.Row = capturedPiece.Row / 2;
                capturedPiece = capturedPiece + m_CurrentPosition;
                int top = Math.Abs(capturedPiece.Row * k_TileHeight) + 50;
                int left = Math.Abs(capturedPiece.Column * k_TileWidth) + 5;
                if (m_CurrentPlayer == m_PlayerOne)
                {
                    removePieceFromBoard(top, left, m_PlayerTwoPieces);
                }
                else
                {
                    removePieceFromBoard(top, left, m_PlayerOnePieces);
                }
            }
        }

        private void removePieceFromBoard(int i_Top, int i_Left, List<PictureBox> i_PieceToSearch)
        {
            PictureBox toRemove = new PictureBox();

            foreach (PictureBox pic in i_PieceToSearch)
            {
                if ((pic.Top == i_Top) && (pic.Left == i_Left))
                {
                    toRemove = pic;
                }
            }

            i_PieceToSearch.Remove(toRemove);
            this.Controls.Remove(toRemove);

        }

        private PictureBox initializePiecePictureBox(int i_Top, int i_Left, Image i_Img, int i_Row, int i_Column)
        {
            PictureBox picture = new PictureBox();

            picture.Image = i_Img;
            picture.Top = i_Top;
            picture.Left = i_Left;
            picture.Size = new Size(k_TileWidth, k_TileHeight);
            this.Controls.Add(picture);
            picture.BringToFront();
            picture.Click += new EventHandler(move_piece);

            return picture;
        }

        private void move_piece(object i_Sender, EventArgs i_E)
        {
            if (m_FirstTurnClick == true)
            {
                m_CurrentPlayer.UpdatePiecesMoves();
                if (m_CurrentPlayer == m_PlayerOne)
                {
                    if (m_PlayerOnePieces.Contains(i_Sender as PictureBox))
                    {
                        pieceToMove(i_Sender as PictureBox);
                    }
                    else
                    {
                        MessageBox.Show("Invalid selection.");
                    }
                }
                else
                {
                    if (m_PlayerTwoPieces.Contains(i_Sender as PictureBox))
                    {
                        pieceToMove(i_Sender as PictureBox);
                    }
                    else
                    {
                        MessageBox.Show("Invalid selection.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid selection.");
            }
        }

        private void pieceToMove(PictureBox i_Piece)
        {
            m_CurrentPosition.Column = (i_Piece.Left - 5) / k_TileWidth;
            m_CurrentPosition.Row = (i_Piece.Top - 50) / k_TileHeight;
            i_Piece.BackColor = Color.Aqua;
            m_FirstTurnClick = false;
            m_PieceTaken = i_Piece;
        }

        private void swapActivePlayer(ref Player io_ActivePlayer, ref Player io_NextPlayer)
        {
            Player temp;

            temp = io_ActivePlayer;
            io_ActivePlayer = io_NextPlayer;
            io_NextPlayer = temp;
            io_ActivePlayer.UpdatePiecesMoves();
        }
    }
}
