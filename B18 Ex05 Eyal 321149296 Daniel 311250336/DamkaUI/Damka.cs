using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using B18_Ex02_Eyal_321149296_Daniel_311250336;
using BoardSymbol = B18_Ex02_Eyal_321149296_Daniel_311250336.PieceSymbol.eGameSymbols;

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
        private List<GamePieceUI> m_PlayerOnePieces;
        private List<GamePieceUI> m_PlayerTwoPieces;
        private GamePieceUI m_PieceTaken;
        private BoardPosition m_CurrentPosition = new BoardPosition();
        private BoardPosition m_NewPlace = new BoardPosition();
        private bool m_FirstTurnClick = true;
        private Cursor m_DefaultCursor;
        private Image m_PieceTakenImage;

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

            m_CurrentPlayer = m_PlayerOne;
            m_NextPlayer = m_PlayerTwo;

            m_GameBoardGraphics = new PictureBox[i_BoardSize, i_BoardSize];

            this.Text = "Damka";
            this.BackgroundImage = DamkaUI.Properties.Resources.MarbleBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Size = new Size(((i_BoardSize * k_TileWidth) + 25), ((i_BoardSize * k_TileHeight) + 90));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Icon = Icon.FromHandle(DamkaUI.Properties.Resources.checkrsIcon.GetHicon());
            m_DefaultCursor = this.Cursor;
        }

        public void Start()
        {
            initializeGameBoardGraphics();
            m_CurrentPlayer.UpdatePiecesMoves();
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
            m_PlayerOnePieces = new List<GamePieceUI>((m_GameBoardData.GameBoardSize / 2) * ((m_GameBoardData.GameBoardSize / 2) - 1));
            m_PlayerOneLabel.Text = m_PlayerOne.Name + ": " + m_PlayerOne.Score.ToString();
            m_PlayerOneLabel.Font = new Font("Arial", 11);
            m_PlayerOneLabel.Top = 10;
            m_PlayerOneLabel.Left = 50;
            m_PlayerOneLabel.AutoSize = true;
            m_PlayerOneLabel.BackColor = Color.Transparent;
            this.Controls.Add(m_PlayerOneLabel);
            m_PlayerTwoPieces = new List<GamePieceUI>((m_GameBoardData.GameBoardSize / 2) * ((m_GameBoardData.GameBoardSize / 2) - 1));
            m_PlayerTwoLabel.Text = m_PlayerTwo.Name + ": " + m_PlayerTwo.Score.ToString();
            m_PlayerTwoLabel.Font = new Font("Arial", 11);
            m_PlayerTwoLabel.Top = 10;
            m_PlayerTwoLabel.Left = m_PlayerOneLabel.Right + 50;
            m_PlayerTwoLabel.AutoSize = true;
            m_PlayerTwoLabel.BackColor = Color.Transparent;
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

            pic.Click += new EventHandler(click_tile);

            return pic;
        }

        private void click_tile(object i_Sender, EventArgs e)
        {
            if (m_FirstTurnClick == false)
            {
                this.Cursor = m_DefaultCursor;
                m_PieceTaken.Image = m_PieceTakenImage;
                m_NewPlace.Column = ((i_Sender as PictureBox).Left - 5) / k_TileWidth;
                m_NewPlace.Row = ((i_Sender as PictureBox).Top - 50) / k_TileHeight;
                if (m_CurrentPlayer.CheckMoveAvailabillityAndMove(m_CurrentPosition, m_NewPlace,
                    m_CurrentPlayer.PiecesThatMustCapture()) == true)
                {
                    playTurn(i_Sender as PictureBox);
                }
                else
                {
                    m_PieceTaken.BackColor = Color.White;
                    MessageBox.Show("Illegal move");
                    m_FirstTurnClick = true;
                }
            }
            else
            {
                MessageBox.Show("Invalid selection.");
            }
        }

        private bool isTie(out bool o_NextPlayerStillHasMove)
        {
            bool tie = true;
            o_NextPlayerStillHasMove = false;

            foreach(GamePiece piece in m_CurrentPlayer.AvailablePieces)
            {
                if(piece.MoveList.Count > 0)
                {
                    tie = false;
                    break;
                }
            }

            if (tie == true)
            {
                foreach (GamePiece piece in m_NextPlayer.AvailablePieces)
                {
                    if (piece.MoveList.Count > 0)
                    {
                        tie = false;
                        o_NextPlayerStillHasMove = true;
                        break;
                    }
                }
            }

            return tie;
        }

        private void updateIfBecameKing()
        {
            char currentSymbolInCellOnBoard;

            m_CurrentPosition.Column = (m_PieceTaken.Left - 5) / k_TileWidth;
            m_CurrentPosition.Row = (m_PieceTaken.Top - 50) / k_TileHeight;
            currentSymbolInCellOnBoard = m_GameBoardData.GetCellSymbol(m_CurrentPosition.Row, m_CurrentPosition.Column);
            if (m_CurrentPlayer == m_PlayerOne)
            {
                m_PieceTaken.CheckIfBecameKing(DamkaUI.Properties.Resources.whiteKing, currentSymbolInCellOnBoard, (char)BoardSymbol.PlayerOneKing);
            }
            else
            {
                m_PieceTaken.CheckIfBecameKing(DamkaUI.Properties.Resources.blackKing, currentSymbolInCellOnBoard, (char)BoardSymbol.PlayerTwoKing);
            }
        }

        private void updateIfCaptured()
        {
            if (m_CurrentPlayer.CapturedAPiece)
            {
                BoardPosition capturedPiece = CalculateCapturedBoardPosition();

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

        private BoardPosition CalculateCapturedBoardPosition()
        {
            BoardPosition capturedPiece = new BoardPosition();

            capturedPiece = m_NewPlace - m_CurrentPosition;
            capturedPiece.Column = capturedPiece.Column / 2;
            capturedPiece.Row = capturedPiece.Row / 2;
            capturedPiece = capturedPiece + m_CurrentPosition;

            return capturedPiece;
        }

        private void removePieceFromBoard(int i_Top, int i_Left, List<GamePieceUI> i_PieceToSearch)
        {
            GamePieceUI toRemove = null;
            
            foreach (GamePieceUI pic in i_PieceToSearch)
            {
                if ((pic.Top == i_Top) && (pic.Left == i_Left))
                {
                    toRemove = pic;
                    break;
                }
            }

            i_PieceToSearch.Remove(toRemove);
            this.Controls.Remove(toRemove);
        }

        private GamePieceUI initializePiecePictureBox(int i_Top, int i_Left, Image i_Img, int i_Row, int i_Column)
        {
            GamePieceUI picture = new GamePieceUI(i_Top, i_Left, i_Img);

            this.Controls.Add(picture);
            picture.BringToFront();
            picture.Click += new EventHandler(click_piece);

            return picture;
        }

        private void click_piece(object i_Sender, EventArgs i_E)
        {
            if (m_FirstTurnClick == true)
            {
                GamePieceUI pieceChosen;
                if (m_CurrentPlayer == m_PlayerOne)
                {
                    if (m_PlayerOnePieces.Contains(i_Sender as GamePieceUI))
                    {
                        pieceChosen = i_Sender as GamePieceUI;
                        m_PieceTaken = pieceChosen.ChoosePiece(ref m_CurrentPosition, ref m_FirstTurnClick);
                        this.Cursor = new Cursor(((Bitmap)((i_Sender as GamePieceUI).Image)).GetHicon());
                        m_PieceTakenImage = m_PieceTaken.Image;
                        m_PieceTaken.Image = DamkaUI.Properties.Resources.transparentTile;
                    }
                    else
                    {
                        MessageBox.Show("Invalid selection.");
                    }
                }
                else
                {
                    if (m_PlayerTwoPieces.Contains(i_Sender as GamePieceUI))
                    {
                        pieceChosen = i_Sender as GamePieceUI;
                        m_PieceTaken = pieceChosen.ChoosePiece(ref m_CurrentPosition, ref m_FirstTurnClick);
                        m_PieceTakenImage = m_PieceTaken.Image;
                        this.Cursor = new Cursor(((Bitmap)(pieceChosen.Image)).GetHicon());
                        m_PieceTaken.Image = DamkaUI.Properties.Resources.transparentTile;
                    }
                    else
                    {
                        MessageBox.Show("Invalid selection.");
                    }
                }
            }
            else
            {
                if (m_PieceTaken == (i_Sender as GamePieceUI))
                {
                    m_PieceTaken.BackColor = Color.White;
                    m_FirstTurnClick = true;
                    m_PieceTaken.Image = m_PieceTakenImage;
                    this.Cursor = m_DefaultCursor;
                }
                else
                {
                    MessageBox.Show("Invalid selection.");
                }
            }
        }

        private void swapActivePlayer(ref Player io_ActivePlayer, ref Player io_NextPlayer)
        {
            Player temp;
            
            temp = io_ActivePlayer;
            io_ActivePlayer = io_NextPlayer;
            io_NextPlayer = temp;
            io_ActivePlayer.UpdatePiecesMoves();
        }

        private void doComputerMove()
        {
            System.Timers.Timer waitTimer = new System.Timers.Timer();

            waitTimer.AutoReset = false;
            waitTimer.Interval = 300;
            waitTimer.SynchronizingObject = this;
            waitTimer.Elapsed += new ElapsedEventHandler(playComputerMove);
            if (m_CurrentPlayer.AvailablePieces.Count > 0)
            {
                waitTimer.Start();
            }
        }

        private void playComputerMove(object i_Sender, EventArgs i_E)
        {
            string computerMove = m_CurrentPlayer.ComputerPlayerMove(m_CurrentPlayer.PiecesThatMustCapture());
            int left, top;
            PictureBox tileToMoveTo = new PictureBox();

            m_CurrentPosition = getPositionFromChars(computerMove[0], computerMove[1]);
            m_NewPlace = getPositionFromChars(computerMove[3], computerMove[4]);
            left = (m_CurrentPosition.Column * k_TileWidth) + 5;
            top = (m_CurrentPosition.Row * k_TileHeight) + 50;
            foreach(GamePieceUI piece in m_PlayerTwoPieces)
            {
                if((piece.Left == left) && (piece.Top == top))
                {
                    m_PieceTaken = piece;
                    break;
                }
            }

            left = (m_NewPlace.Column * k_TileWidth) + 5;
            top = (m_NewPlace.Row * k_TileHeight) + 50;

            foreach (PictureBox piece in m_GameBoardGraphics)
            {
                if ((piece.Left == left) && (piece.Top == top))
                {
                    tileToMoveTo = piece;
                    break;
                }
            }

            playTurn(tileToMoveTo);

        }

        private BoardPosition getPositionFromChars(char i_Column, char i_Row)
        {
            BoardPosition pos = new BoardPosition();

            pos.Column = i_Column - 'A';
            pos.Row = i_Row - 'a';

            return pos;
        }

        private void calculatePlayersScore()
        {
            int score = 0;

            score = m_NextPlayer.Score + (m_NextPlayer.CalculateScore() - m_CurrentPlayer.CalculateScore());
            m_NextPlayer.Score = score;
            m_PlayerOneLabel.Text = m_PlayerOne.Name + ": " + m_PlayerOne.Score.ToString();
            m_PlayerTwoLabel.Text = m_PlayerTwo.Name + ": " + m_PlayerTwo.Score.ToString();
        }

        private void startNewGame()
        {
            calculatePlayersScore();
            foreach (GamePieceUI piece in m_PlayerOnePieces)
            {
                this.Controls.Remove(piece);
            }

            m_PlayerOnePieces.Clear();
            foreach (GamePieceUI piece in m_PlayerTwoPieces)
            {
                this.Controls.Remove(piece);
            }

            m_PlayerTwoPieces.Clear();
            m_GameBoardData.InitiliazeGamePiecesOnBoard();
            m_PlayerOne.ConnectThePiecesToThePlayer();
            m_PlayerTwo.ConnectThePiecesToThePlayer();
            initializePieceGraphics();
            m_CurrentPlayer = m_PlayerOne;
            m_NextPlayer = m_PlayerTwo;
            m_PlayerOne.CanCapture = false;
            m_PlayerTwo.CanCapture = false;
            m_CurrentPlayer.UpdatePiecesMoves();
        }

        private void playTurn(PictureBox i_NewPlace)
        {
            m_PieceTaken.MovePiece(i_NewPlace);
            updateIfCaptured();
            m_CurrentPlayer.CanCapture = false;
            if (m_CurrentPlayer.CapturedAPiece == true)
            {
                m_NextPlayer.RemovePieces();
                m_CurrentPlayer.UpdatePiecesMoves();
                m_CurrentPlayer.CapturedAPiece = false;
                if (m_CurrentPlayer.CanCapture)
                {
                    checkIfPieceThatCapturedCanCaptureFurther();
                }
            }

            updateIfBecameKing();
            if (m_CurrentPlayer.CanCapture == false)
            {
                swapActivePlayer(ref m_CurrentPlayer, ref m_NextPlayer);
                m_CurrentPlayer.UpdatePiecesMoves();
            }

            m_FirstTurnClick = true;
            if(m_CurrentPlayer.IsComputer)
            {
                doComputerMove();
            }

            checkIfGameOver();
        }

        private void checkIfPieceThatCapturedCanCaptureFurther()
        {
            BoardPosition currentPiece = new BoardPosition();
            bool samePieceCanCapture = false;
            GamePiece saveGamePieceThatCanCapture;

            currentPiece.Column = (m_PieceTaken.Left - 5) / k_TileWidth;
            currentPiece.Row = (m_PieceTaken.Top - 50) / k_TileHeight;
            foreach (GamePiece piece in m_CurrentPlayer.PiecesThatMustCapture())
            {
                if ((piece.Column == currentPiece.Column) && (piece.Row == currentPiece.Row))
                {
                    samePieceCanCapture = true;
                    saveGamePieceThatCanCapture = piece;
                }
            }

            m_CurrentPlayer.CanCapture = samePieceCanCapture;
        }

        private void checkIfGameOver()
        {
            bool forceWin;

            if (isTie(out forceWin) == true)
            {
                string message = string.Format("Tie!{1}Another Round?", System.Environment.NewLine);
                DialogResult result = MessageBox.Show(message, "Damka", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    startNewGame();
                }
                else
                {
                    this.Close();
                }
            }
            else if (m_CurrentPlayer.AvailablePieces.Count == 0 || forceWin)
            {
                string message = string.Format("{0} Won!{1}Another Round?", m_NextPlayer.Name, System.Environment.NewLine);
                DialogResult result = MessageBox.Show(message, "Damka", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    startNewGame();
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}
