using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using B18_Ex02_Eyal_321149296_Daniel_311250336;

namespace DamkaUI
{
    class GamePieceUI : PictureBox
    {
        private const int k_TileHeight = 56;
        private const int k_TileWidth = 55;

        public GamePieceUI(int i_Top, int i_Left, Image i_Img)
        {
            this.Image = i_Img;
            this.Top = i_Top;
            this.Left = i_Left;
            this.Size = new Size(k_TileWidth, k_TileHeight);
        }

        public void MovePiece(PictureBox i_NewPlace)
        {
            this.Left = i_NewPlace.Left;
            this.Top = i_NewPlace.Top;
            this.BackColor = Color.White;
        }

        public void CheckIfBecameKing(Image i_KingPicture, char i_PieceChar, char i_KingChar)
        {
            if (i_PieceChar == i_KingChar)
            {
                this.Image = i_KingPicture;
            }
        }

        public GamePieceUI ChoosePiece(ref BoardPosition i_CurrentPositionChosen, ref bool i_FirstTurnClick)
        {
            i_CurrentPositionChosen.Column = (this.Left - 5) / k_TileWidth;
            i_CurrentPositionChosen.Row = (this.Top - 50) / k_TileHeight;
            this.BackColor = Color.Aqua;
            i_FirstTurnClick = false;

            return this;
        }

    }
}
