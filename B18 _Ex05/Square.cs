using System;
using System.Collections.Generic;
using System.Text;

namespace B18__Ex05
{
    internal class Square
    {
        public enum eTakenByPiece
        {
            RegularBlack = 1,
            RegularWhite = 2,
            KingBlack = 4,
            KingWhite = 8,
            NonePiece = 16
        }

        public enum eColourSquare
        {
            BlackSquare = 1,
            WhiteSquare = 2,
        }

        private Point m_SquareLocationInBoard;
        private eTakenByPiece m_TakenByPiece;
        private eColourSquare m_ColourSquare;
        private BoardPiece m_BoardPiece;
        private bool m_SquareAlreadyChosen;

        // $G$ SFN-012 (+15) Bonus: Events in the Logic layer are handled by the UI.

        public event Action<Square> Chosen;

        public event Action<Square> IsTakenByPiece;

        public Square(int i_Row, int i_Column, eTakenByPiece i_Taken, eColourSquare i_ColourSquare)
        {
            m_SquareLocationInBoard = new Point(i_Row, i_Column);
            m_TakenByPiece = i_Taken;
            m_ColourSquare = i_ColourSquare;
        }

        public bool EmptySquare()
        {
            return m_TakenByPiece == eTakenByPiece.NonePiece;
        }

        public eColourSquare ColourSquare
        {
            get
            {
                return m_ColourSquare;
            }
        }

        public eTakenByPiece TakenByPiece
        {
            get
            {
                return m_TakenByPiece;
            }

            set
            {
                m_TakenByPiece = value;
            }
        }

        public BoardPiece BoardPiece
        {
            get
            {
                return m_BoardPiece;
            }

            set
            {
                m_BoardPiece = value;
            }
        }

        public void OnChosen()
        {
            if (Chosen != null)
            {
                Chosen(this);
            }
        }

        public void OnSquareIsTakenByPiece()
        {
            if (IsTakenByPiece != null)
            {
                IsTakenByPiece(this);
            }
        }

        public Point SquareLocationInBoard
        {
            get
            {
                return m_SquareLocationInBoard;
            }
        }

        public bool SquareAlreadyChosen
        {
            get
            {
                return m_SquareAlreadyChosen;
            }

            set
            {
                m_SquareAlreadyChosen = value;
            }
        }

        public void RemovePieceOfSquare()
        {
            m_TakenByPiece = eTakenByPiece.NonePiece;
        }
    }
}