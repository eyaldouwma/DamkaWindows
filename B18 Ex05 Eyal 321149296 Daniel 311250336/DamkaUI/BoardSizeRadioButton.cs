using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DamkaUI
{
    public class BoardSizeRadioButton : RadioButton
    {
        public BoardSizeRadioButton(string i_ButtonText, int i_Top, int i_Left)
        {
            this.Font = new Font("Arial", 10);
            this.Text = i_ButtonText;
            this.Top = i_Top;
            this.Left = i_Left;
            this.Size = new Size(60, 20);
            this.BackColor = Color.Transparent;
        }
    }
}
