using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.Threading;
using B18_Ex02_Eyal_321149296_Daniel_311250336;
using DamkaGraphics = DamkaUI.Properties.Resources;
using BoardSymbol = B18_Ex02_Eyal_321149296_Daniel_311250336.PieceSymbol.eGameSymbols;
using DelayTimer = System.Timers.Timer;

namespace DamkaUI
{
    public class Damka : Form
    {
        private const int k_TileHeight = 56;
        private const int k_TileWidth = 55;
        private const int k_TopOffset = 50;
        private const int k_LeftOffset = 5;
        private const bool v_GameIsOver = true;
        private PictureBox[,] m_GameBoardGraphics;
        private GamePiece m_PieceThatCaptured;
        private GameBoard m_GameBoardData;
        private Player m_PlayerOne;
        private Player m_PlayerTwo;
        private Player m_CurrentPlayer;
        private Player m_NextPlayer;
        private Label LabelPlayerOne = new Label();
        private Label LabelPlayerTwo = new Label();
        private List<GamePieceUI> m_PlayerOnePieces;
        private List<GamePieceUI> m_PlayerTwoPieces;
        private GamePieceUI GamePieceUIPieceTaken;
        private BoardPosition m_CurrentPosition = new BoardPosition();
        private BoardPosition m_NewPlace = new BoardPosition();
        private bool m_FirstTurnClick = true;
        private Cursor m_DefaultCursor;
        private Image m_PieceTakenImage;
        private bool m_GameOver = !v_GameIsOver;
        private bool m_SamePieceCanCapture = false;

        private const bool v_IsComputerPlayer = true;

        public Damka(string i_PlayerOneName, string i_PlayerTwoName, int i_BoardSize, bool i_IsAgainstComputer)
        {
            m_GameBoardData = new GameBoard(i_BoardSize);

            m_PlayerOne = new Player(i_PlayerOneName,!v_IsComputerPlayer, BoardSymbol.PlayerOneRegular,m_GameBoardData);
            if (i_IsAgainstComputer)
            {
                m_PlayerTwo = new Player(i_PlayerTwoName, v_IsComputerPlayer, BoardSymbol.PlayerTwoRegular, m_GameBoardData);
            }
            else
            {
                m_PlayerTwo = new Player(i_PlayerTwoName, !v_IsComputerPlayer, BoardSymbol.PlayerTwoRegular, m_GameBoardData);
            }

            m_PlayerOne.MoveDirection = Player.eMoveDirection.Up;
            m_PlayerTwo.MoveDirection = Player.eMoveDirection.Down;

            m_CurrentPlayer = m_PlayerOne;
            m_NextPlayer = m_PlayerTwo;

            m_GameBoardGraphics = new PictureBox[i_BoardSize, i_BoardSize];

            this.Text = "Damka";
            this.BackgroundImage = DamkaGraphics.MarbleBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Size = new Size(((i_BoardSize * k_TileWidth) + 25), ((i_BoardSize * k_TileHeight) + 90));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Icon = Icon.FromHandle(DamkaGraphics.checkrsIcon.GetHicon());
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
            int top = k_TopOffset;
            int left = k_LeftOffset;

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
                left = k_LeftOffset;
                top += k_TileHeight;

            }

            initializePieceGraphics();
        }
        
        private void convertBoardPositionToFormPosition(BoardPosition i_Position, out int i_Top, out int i_Left)
        {
            i_Top = ((i_Position.Row * k_TileHeight) + k_TopOffset);
            i_Left = ((i_Position.Column * k_TileWidth) + k_LeftOffset);
        }

        private void initializePlayerData()
        {
            m_PlayerOnePieces = new List<GamePieceUI>((m_GameBoardData.GameBoardSize / 2) * ((m_GameBoardData.GameBoardSize / 2) - 1));
            LabelPlayerOne.Text = m_PlayerOne.Name + ": " + m_PlayerOne.Score.ToString();
            LabelPlayerOne.Font = new Font("Arial", 11);
            LabelPlayerOne.Top = 10;
            LabelPlayerOne.Left = 50;
            LabelPlayerOne.AutoSize = true;
            LabelPlayerOne.BackColor = Color.Transparent;
            this.Controls.Add(LabelPlayerOne);
            m_PlayerTwoPieces = new List<GamePieceUI>((m_GameBoardData.GameBoardSize / 2) * ((m_GameBoardData.GameBoardSize / 2) - 1));
            LabelPlayerTwo.Text = m_PlayerTwo.Name + ": " + m_PlayerTwo.Score.ToString();
            LabelPlayerTwo.Font = new Font("Arial", 11);
            LabelPlayerTwo.Top = 10;
            LabelPlayerTwo.Left = LabelPlayerOne.Right + 50;
            LabelPlayerTwo.AutoSize = true;
            LabelPlayerTwo.BackColor = Color.Transparent;
            this.Controls.Add(LabelPlayerTwo);
        }

        private void initializePieceGraphics()
        {
            int top = k_TopOffset;
            int left = k_LeftOffset;

            for (int i = 0; i < m_GameBoardData.GameBoardSize; i++)
            {
                for (int j = 0; j < m_GameBoardData.GameBoardSize; j++)
                {
                    if (m_GameBoardData.GetCellSymbol(i, j) == (char)BoardSymbol.PlayerOneRegular)
                    {
                        m_PlayerOnePieces.Add(initializePiecePictureBox(top, left, DamkaGraphics.white, i, j));
                    }
                    else if (m_GameBoardData.GetCellSymbol(i, j) == (char)BoardSymbol.PlayerTwoRegular)
                    {
                        m_PlayerTwoPieces.Add(initializePiecePictureBox(top, left, DamkaGraphics.black, i, j));
                    }

                    left += k_TileWidth;
                }

                left = k_LeftOffset;
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
                    pic.Image = DamkaGraphics.BlackTile;
                    pic.Enabled = false;
                }
                else
                {
                    pic.Image = DamkaGraphics.WhiteTile;
                }
            }
            else
            {
                if (i_Column % 2 == 0)
                {
                    pic.Image = DamkaGraphics.WhiteTile;
                }
                else
                {
                    pic.Image = DamkaGraphics.BlackTile;
                    pic.Enabled = false;
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
                GamePieceUIPieceTaken.Image = m_PieceTakenImage;
                m_NewPlace = convertFormPositionToBoardPosition(i_Sender as PictureBox);
                if (m_CurrentPlayer.CheckMoveAvailabillityAndMove(m_CurrentPosition, m_NewPlace,
                    m_CurrentPlayer.PiecesThatMustCapture()) == true)
                {
                    playTurn(i_Sender as PictureBox);
                }
                else
                {
                    GamePieceUIPieceTaken.BackColor = Color.White;
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

            m_CurrentPosition = convertFormPositionToBoardPosition(GamePieceUIPieceTaken);
            currentSymbolInCellOnBoard = m_GameBoardData.GetCellSymbol(m_CurrentPosition.Row, m_CurrentPosition.Column);
            if (m_CurrentPlayer == m_PlayerOne)
            {
                GamePieceUIPieceTaken.CheckIfBecameKing(DamkaGraphics.whiteKing, currentSymbolInCellOnBoard, (char)BoardSymbol.PlayerOneKing);
            }
            else
            {
                GamePieceUIPieceTaken.CheckIfBecameKing(DamkaGraphics.blackKing, currentSymbolInCellOnBoard, (char)BoardSymbol.PlayerTwoKing);
            }
        }

        private void updateIfCaptured()
        {
            if (m_CurrentPlayer.CapturedAPiece)
            {
                BoardPosition capturedPiece = CalculateCapturedBoardPosition();

                int top = Math.Abs(capturedPiece.Row * k_TileHeight) + k_TopOffset;
                int left = Math.Abs(capturedPiece.Column * k_TileWidth) + k_LeftOffset;
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
            if (m_FirstTurnClick)
            {
                GamePieceUI pieceChosen = i_Sender as GamePieceUI;
                BoardPosition position = convertFormPositionToBoardPosition(pieceChosen);

                if (m_SamePieceCanCapture == true)
                {
                    if ((position.Column == m_PieceThatCaptured.Column) && (position.Row == m_PieceThatCaptured.Row))
                    {
                        GamePieceUIPieceTaken = pieceChosen.ChoosePiece(ref m_CurrentPosition, ref m_FirstTurnClick);
                        changeCursorAndPieceTaken(new Cursor(((Bitmap)(pieceChosen.Image)).GetHicon()),
                            DamkaGraphics.transparentTile);
                    }
                    else
                    {
                        MessageBox.Show("Invalid selection.");
                    }
                }
                else
                {
                    if (m_CurrentPlayer == m_PlayerOne)
                    {
                        if (m_PlayerOnePieces.Contains(i_Sender as GamePieceUI))
                        {
                            GamePieceUIPieceTaken = pieceChosen.ChoosePiece(ref m_CurrentPosition, ref m_FirstTurnClick);
                            changeCursorAndPieceTaken(new Cursor(((Bitmap)(pieceChosen.Image)).GetHicon()),
                                DamkaGraphics.transparentTile);
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
                            GamePieceUIPieceTaken = pieceChosen.ChoosePiece(ref m_CurrentPosition, ref m_FirstTurnClick);
                            changeCursorAndPieceTaken(new Cursor(((Bitmap)(pieceChosen.Image)).GetHicon()),
                                DamkaGraphics.transparentTile);
                        }
                        else
                        {
                            MessageBox.Show("Invalid selection.");
                        }
                    }
                }
            }
            else
            {
                if (GamePieceUIPieceTaken == (i_Sender as GamePieceUI))
                {
                    GamePieceUIPieceTaken.BackColor = Color.White;
                    m_FirstTurnClick = true;
                    changeCursorAndPieceTaken(m_DefaultCursor, m_PieceTakenImage);
                }
                else
                {
                    MessageBox.Show("Invalid selection.");
                }
            }
        }

        private BoardPosition convertFormPositionToBoardPosition(PictureBox i_PieceChosen)
        {
            BoardPosition result = new BoardPosition();

            result.Row = ((i_PieceChosen.Top - k_TopOffset) / k_TileHeight);
            result.Column = ((i_PieceChosen.Left - k_LeftOffset) / k_TileWidth);

            return result;
        }

        private void changeCursorAndPieceTaken(Cursor i_NewCursor, Image i_ImageToReplacePieceTaken)
        {
            //this will change the cursor to piece/default and simulate grabbing or returning the piece.
            m_PieceTakenImage = GamePieceUIPieceTaken.Image; //save the image of the piece taken.
            this.Cursor = i_NewCursor;
            GamePieceUIPieceTaken.Image = i_ImageToReplacePieceTaken;
        }

        private void swapActivePlayer(ref Player io_ActivePlayer, ref Player io_NextPlayer)
        {
            Player temp;

            m_SamePieceCanCapture = false;
            temp = io_ActivePlayer;
            io_ActivePlayer = io_NextPlayer;
            io_NextPlayer = temp;
            io_ActivePlayer.UpdatePiecesMoves();
        }

        private void doComputerMove()
        {
            DelayTimer waitTimer = new DelayTimer();

            waitTimer.AutoReset = false;
            waitTimer.Interval = 300;
            waitTimer.SynchronizingObject = this;
            waitTimer.Elapsed += new ElapsedEventHandler(playComputerMove);
            if ((m_CurrentPlayer.AvailablePieces.Count > 0) && (m_GameOver == !v_GameIsOver))
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
            convertBoardPositionToFormPosition(m_CurrentPosition, out top, out left);
            foreach(GamePieceUI piece in m_PlayerTwoPieces)
            {
                if((piece.Left == left) && (piece.Top == top))
                {
                    GamePieceUIPieceTaken = piece;
                    break;
                }
            }

            convertBoardPositionToFormPosition(m_NewPlace, out top, out left);

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
            LabelPlayerOne.Text = m_PlayerOne.Name + ": " + m_PlayerOne.Score.ToString();
            LabelPlayerTwo.Text = m_PlayerTwo.Name + ": " + m_PlayerTwo.Score.ToString();
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
            m_GameOver = !v_GameIsOver;
            m_CurrentPlayer.UpdatePiecesMoves();
        }

        private void playTurn(PictureBox i_NewPlace)
        {
            GamePieceUIPieceTaken.MovePiece(i_NewPlace);
            updateIfCaptured();
            m_CurrentPlayer.CanCapture = false;
            if (m_CurrentPlayer.CapturedAPiece)
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

            checkIfGameOver();
            m_FirstTurnClick = true;
            if(m_CurrentPlayer.IsComputer)
            {
                doComputerMove();
            }
        }

        private void checkIfPieceThatCapturedCanCaptureFurther()
        {
            BoardPosition currentPiece = convertFormPositionToBoardPosition(GamePieceUIPieceTaken);
            m_SamePieceCanCapture = false;

            foreach (GamePiece piece in m_CurrentPlayer.PiecesThatMustCapture())
            {
                if ((piece.Column == currentPiece.Column) && (piece.Row == currentPiece.Row))
                { 
                    m_SamePieceCanCapture = true;
                    m_PieceThatCaptured = piece;
                }
            }

            m_CurrentPlayer.CanCapture = m_SamePieceCanCapture;
            
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
                m_GameOver = v_GameIsOver;
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
