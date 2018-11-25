using System;
using System.Collections.Generic;
using System.Text;

namespace B18__Ex05
{
    internal class Board
    {
        private const int k_Two = 2;
        private readonly int r_BoardSize;
        private readonly int r_NumberLinesToInitWithPieces;
        private Square[,] m_BoardMatrix;
        private List<BoardPiece> m_BoardWhitePieces;
        private List<BoardPiece> m_BoardBlackPieces;

        public Board(int i_Size)
        {
            m_BoardMatrix = new Square[i_Size, i_Size];
            r_NumberLinesToInitWithPieces = (i_Size / k_Two) - 1;
            m_BoardWhitePieces = new List<BoardPiece>();
            m_BoardBlackPieces = new List<BoardPiece>();
            r_BoardSize = i_Size;
            InitBoard();
        }

        public bool InsideTheBoard(Point i_Point)
        {
            return i_Point.CheckIfPointIsOnBoard(r_BoardSize);
        }

        public void InitBoard()
        {
            InitSquaresOnBoard();
            InitBoardPiecesOnBoard();
        }

        public void SetBoardForNextRound()
        {
            m_BoardWhitePieces.RemoveRange(0, m_BoardWhitePieces.Count);
            m_BoardBlackPieces.RemoveRange(0, m_BoardBlackPieces.Count);
            makeEmptyBoard();
            InitBoardPiecesOnBoard();
        }

        private void makeEmptyBoard()
        {
            for (int indexRow = 0; indexRow < r_BoardSize; indexRow++)
            {
                for (int indexColumn = 0; indexColumn < r_BoardSize; indexColumn++)
                {
                    m_BoardMatrix[indexRow, indexColumn].TakenByPiece = Square.eTakenByPiece.NonePiece;
                }
            }
        }

        public void InitSquaresOnBoard()
        {
            Square.eColourSquare BlackOrWhite = Square.eColourSquare.BlackSquare;
            for (int indexRow = 0; indexRow < r_BoardSize; indexRow++)
            {
                for (int indexColumn = 0; indexColumn < r_BoardSize; indexColumn++)
                {
                    m_BoardMatrix[indexRow, indexColumn] = new Square(indexRow, indexColumn, Square.eTakenByPiece.NonePiece, BlackOrWhite);
                    BlackOrWhite = checkIfIsBlackOrWhiteSquare(indexRow, indexColumn);
                }

                BlackOrWhite = checkIfIsBlackOrWhiteSquare(indexRow, r_BoardSize - k_Two);
            }
        }

        private Square.eColourSquare checkIfIsBlackOrWhiteSquare(int i_IndexRowInput, int i_IndexColumnInput)
        {
            Square.eColourSquare blackOrWhite;
            if ((i_IndexRowInput % k_Two == 0 && i_IndexColumnInput % k_Two == 0) || (i_IndexRowInput % k_Two == 1 && i_IndexColumnInput % k_Two == 1))
            {            
                blackOrWhite = Square.eColourSquare.WhiteSquare;
            }
            else
            {
                blackOrWhite = Square.eColourSquare.BlackSquare;
            }

            return blackOrWhite;
        }

        public void InitBoardPiecesOnBoard()
        {
            InitWhitePiecesOnBoard();
            InitBlackPiecesOnBoard();
        }

        public void InitWhitePiecesOnBoard()
        {
            for (int indexRow = 0; indexRow < r_NumberLinesToInitWithPieces; indexRow++)
            {
                for (int indexColumn = 0; indexColumn < r_BoardSize; indexColumn++)
                {
                    if (m_BoardMatrix[indexRow, indexColumn].ColourSquare.Equals(Square.eColourSquare.WhiteSquare))
                    {
                        m_BoardMatrix[indexRow, indexColumn].BoardPiece = new BoardPiece(BoardPiece.eTypePieceAndColour.RegularWhite, indexRow, indexColumn, this);
                        m_BoardMatrix[indexRow, indexColumn].TakenByPiece = Square.eTakenByPiece.RegularWhite;
                        m_BoardWhitePieces.Add(m_BoardMatrix[indexRow, indexColumn].BoardPiece);
                    }
                }
            }
        }

        public void InitBlackPiecesOnBoard()
        {
            for (int indexRow = r_BoardSize - r_NumberLinesToInitWithPieces; indexRow < r_BoardSize; indexRow++)
            {
                for (int indexColumn = 0; indexColumn < r_BoardSize; indexColumn++)
                {
                    if (m_BoardMatrix[indexRow, indexColumn].ColourSquare.Equals(Square.eColourSquare.WhiteSquare))
                    {
                        m_BoardMatrix[indexRow, indexColumn].BoardPiece = new BoardPiece(BoardPiece.eTypePieceAndColour.RegularBlack, indexRow, indexColumn, this);
                        m_BoardMatrix[indexRow, indexColumn].TakenByPiece = Square.eTakenByPiece.RegularBlack;
                        m_BoardBlackPieces.Add(m_BoardMatrix[indexRow, indexColumn].BoardPiece);
                    }
                }
            }
        }

        public bool BlackPieceExist(Point i_Point)
        {
            return PieceExist(i_Point, Square.eTakenByPiece.RegularBlack) || PieceExist(i_Point, Square.eTakenByPiece.KingBlack);
        }

        public bool WhitePieceExist(Point i_Point)
        {
            return PieceExist(i_Point, Square.eTakenByPiece.RegularWhite) || PieceExist(i_Point, Square.eTakenByPiece.KingWhite);
        }

        public bool PieceExist(Point i_Point, Square.eTakenByPiece i_Piece)
        {
            return (int)GetSquare(i_Point).TakenByPiece == (int)i_Piece;
        }

        public void RemoveBlackPieceFromList(BoardPiece i_BlackPiece)
        {
            m_BoardBlackPieces.Remove(i_BlackPiece);
        }

        public void RemoveWhitePieceFromList(BoardPiece i_WhitePiece)
        {
            m_BoardWhitePieces.Remove(i_WhitePiece);
        }

        public Square GetSquare(Point i_Location)
        {
            return m_BoardMatrix[i_Location.Row, i_Location.Column];
        }

        public Square GetSquare(int i_Row, int i_Column)
        {
            return m_BoardMatrix[i_Row, i_Column];
        }

        public List<BoardPiece> BoardWhitePieces
        {
            get
            {
                return m_BoardWhitePieces;
            }
        }

        public List<BoardPiece> BoardBlackPieces
        {
            get
            {
                return m_BoardBlackPieces;
            }
        }

        public int SizeOfBoard
        {
            get
            {
                return r_BoardSize;
            }
        }
    }
}