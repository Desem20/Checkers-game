using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace B18__Ex05
{
    internal class SquareButton : Button
    {
        private const string k_WhitePieces = " O ";
        private const string k_BlackPieces = " X ";
        private const string k_WhiteKing = " U ";
        private const string k_BlackKing = " K ";
        private const string k_EmptyPieces = "   ";
        private Square m_Square;

        public SquareButton(Square i_Square, System.Drawing.Point i_LocationOfButton)
        {
            m_Square = i_Square;
            
            if (m_Square.ColourSquare == Square.eColourSquare.BlackSquare)
            {
                this.Enabled = false;
                this.BackColor = Color.SaddleBrown;
            }
            else
            {
                this.BackColor = Color.Moccasin;
            }

            m_Square.Chosen += square_Chosen;
            m_Square.IsTakenByPiece += square_IsTakenByPiece;
            this.Size = new Size(50, 50);
            this.Location = i_LocationOfButton;
            this.Show();
        }

        private void square_Chosen(Square i_Square)
        {
            if (i_Square.SquareAlreadyChosen)
            {
                this.BackColor = Color.Moccasin;
                i_Square.SquareAlreadyChosen = false;
            }
            else
            {
                this.BackColor = Color.LightBlue;
                i_Square.SquareAlreadyChosen = true;
            }

            this.Show();
        }

        private void square_IsTakenByPiece(Square i_BoardPiece)
        {
            if (m_Square.TakenByPiece == Square.eTakenByPiece.RegularWhite)
            {
                this.Text = k_WhitePieces;
            }
            else if (m_Square.TakenByPiece == Square.eTakenByPiece.RegularBlack)
            {
                this.Text = k_BlackPieces;
            }
            else if (m_Square.TakenByPiece == Square.eTakenByPiece.KingBlack)
            {
                this.Text = k_BlackKing;
            }
            else if (m_Square.TakenByPiece == Square.eTakenByPiece.KingWhite)
            {
                this.Text = k_WhiteKing;
            }
            else
            {
                this.Text = k_EmptyPieces;
            }

            this.Show();
        }

        public Point SquareLocationInBoard
        {
            get
            {
                return m_Square.SquareLocationInBoard;
            }
        }

        public Square RegularSquare
        {
            get
            {
                return m_Square;
            }
        }
    }
}
