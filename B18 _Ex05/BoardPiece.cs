using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18__Ex05
{
    internal class BoardPiece
    {
        public enum eTypePieceAndColour
        {
            RegularBlack = 1,
            RegularWhite = 2,
            KingBlack = 4,
            KingWhite = 8,
        }

        private eTypePieceAndColour m_TypePiece;
        private Point m_LocationInBoard;
        private Board m_BoardOfPiece;

        public BoardPiece(eTypePieceAndColour i_TypePiece, int i_Row, int i_Column, Board i_BoardOfPiece)
        {
            m_TypePiece = i_TypePiece;
            m_LocationInBoard = new Point(i_Row, i_Column);
            m_BoardOfPiece = i_BoardOfPiece;
        }

        public void MoveThePiece(Move i_PieceMove)
        {
            if (i_PieceMove.EatingMove())
            {
                m_BoardOfPiece.GetSquare(i_PieceMove.LocationPieceToBeEaten()).RemovePieceOfSquare();
                m_BoardOfPiece.GetSquare(i_PieceMove.LocationPieceToBeEaten()).OnSquareIsTakenByPiece();
            }

            m_BoardOfPiece.GetSquare(i_PieceMove.Source).TakenByPiece = Square.eTakenByPiece.NonePiece;
            m_BoardOfPiece.GetSquare(i_PieceMove.Source).OnSquareIsTakenByPiece();
            m_BoardOfPiece.GetSquare(i_PieceMove.Destination).TakenByPiece = (Square.eTakenByPiece)m_TypePiece;            
            m_BoardOfPiece.GetSquare(i_PieceMove.Destination).BoardPiece = this;
            m_LocationInBoard.SetPointTo(i_PieceMove.Destination);
            turnToKingIfNecessary(i_PieceMove);
            m_BoardOfPiece.GetSquare(i_PieceMove.Destination).OnSquareIsTakenByPiece();
        }

        private void turnToKingIfNecessary(Move i_PieceMove)
        {
            if (i_PieceMove.Destination.Row == 0 && m_TypePiece == eTypePieceAndColour.RegularBlack)
            {
                m_TypePiece = eTypePieceAndColour.KingBlack;
            }
            else if (i_PieceMove.Destination.Row == m_BoardOfPiece.SizeOfBoard - 1 && m_TypePiece == eTypePieceAndColour.RegularWhite)
            {
                m_TypePiece = eTypePieceAndColour.KingWhite;
            }

            m_BoardOfPiece.GetSquare(m_LocationInBoard).TakenByPiece = (Square.eTakenByPiece)m_TypePiece;
        }

        private bool squareInBoardAndEmpty(Point i_PossibleMovement)
        {
            bool squareInBoard = m_BoardOfPiece.InsideTheBoard(i_PossibleMovement);
            bool squareEmpty = false;
            if (squareInBoard)
            {
                squareEmpty = m_BoardOfPiece.GetSquare(i_PossibleMovement).EmptySquare();
            }

            return squareInBoard && squareEmpty;
        }

        public bool CanRegularMove(List<Move> i_RandomMoves)
        {
            bool canMove;
            if ((int)this.TypePiece == (int)BoardPiece.eTypePieceAndColour.RegularWhite)
            {
                canMove = whiteRegularPieceCanMove(i_RandomMoves);
            }
            else if ((int)this.TypePiece == (int)BoardPiece.eTypePieceAndColour.RegularBlack)
            {
                canMove = blackRegularPieceCanMove(i_RandomMoves);
            }
            else
            {
                canMove = kingRegularPieceCanMove(i_RandomMoves);
            }

            return canMove;
        }

        private bool whiteRegularPieceCanMove(List<Move> i_RandomMoves)
        {
            Point pieceLocationInBoard = new Point(m_LocationInBoard.Row, m_LocationInBoard.Column);
            bool leftMove = squareInBoardAndEmpty(pieceLocationInBoard.DiagonalMinusOneLeft());
            bool rightMove = squareInBoardAndEmpty(pieceLocationInBoard.DiagonalMinusOneRight());
            if (leftMove)
            {
                i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalMinusOneLeft()));
            }

            if (rightMove)
            {
                i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalMinusOneRight()));
            }

            return leftMove || rightMove;
        }

        private bool blackRegularPieceCanMove(List<Move> i_RandomMoves)
        {
            Point pieceLocationInBoard = new Point(m_LocationInBoard.Row, m_LocationInBoard.Column);
            bool leftMove = squareInBoardAndEmpty(pieceLocationInBoard.DiagonalPlusOneLeft());
            bool rightMove = squareInBoardAndEmpty(pieceLocationInBoard.DiagonalPlusOneRight());
            if (leftMove)
            {
                i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalPlusOneLeft()));
            }

            if (rightMove)
            {
                i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalPlusOneRight()));
            }

            return leftMove || rightMove;
        }

        private bool kingRegularPieceCanMove(List<Move> i_RandomMoves)
        {
            bool forwordMove = blackRegularPieceCanMove(i_RandomMoves);
            bool backwardMove = whiteRegularPieceCanMove(i_RandomMoves);
            return forwordMove || backwardMove;
        }

        public bool CanEat(List<Move> i_RandomMoves)
        {
            bool canEat;
            if ((int)this.TypePiece == (int)BoardPiece.eTypePieceAndColour.RegularWhite)
            {
                canEat = whiteRegularPieceCanEat(i_RandomMoves);
            }
            else if ((int)this.TypePiece == (int)BoardPiece.eTypePieceAndColour.RegularBlack)
            {
                canEat = blackRegularPieceCanEat(i_RandomMoves);
            }
            else if ((int)this.TypePiece == (int)BoardPiece.eTypePieceAndColour.KingWhite)
            {
                canEat = whiteKingPieceCanEat(i_RandomMoves);
            }
            else
            {
                canEat = blackKingPieceCanEat(i_RandomMoves);
            }

            return canEat;
        }

        private bool whiteRegularPieceCanEat(List<Move> i_RandomMoves)
        {
            Point pieceLocationInBoard = new Point(m_LocationInBoard.Row, m_LocationInBoard.Column);
            bool leftMove = false;
            bool rightMove = false;
            if (squareInBoardAndEmpty(pieceLocationInBoard.DiagonalMinusTwoLeft()))
            {
                leftMove = m_BoardOfPiece.BlackPieceExist(pieceLocationInBoard.DiagonalMinusOneLeft());
                if (leftMove)
                {
                    i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalMinusTwoLeft()));
                }
            }

            if (squareInBoardAndEmpty(pieceLocationInBoard.DiagonalMinusTwoRight()))
            {
                rightMove = m_BoardOfPiece.BlackPieceExist(pieceLocationInBoard.DiagonalMinusOneRight());
                if (rightMove)
                {
                    i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalMinusTwoRight()));
                }
            }

            return leftMove || rightMove;
        }

        private bool blackRegularPieceCanEat(List<Move> i_RandomMoves)
        {
            Point pieceLocationInBoard = new Point(m_LocationInBoard.Row, m_LocationInBoard.Column);
            bool leftMove = false;
            bool rightMove = false;
            if (squareInBoardAndEmpty(pieceLocationInBoard.DiagonalPlusTwoLeft()))
            {
                leftMove = m_BoardOfPiece.WhitePieceExist(pieceLocationInBoard.DiagonalPlusOneLeft());
                if (leftMove)
                {
                    i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalPlusTwoLeft()));
                }
            }

            if (squareInBoardAndEmpty(pieceLocationInBoard.DiagonalPlusTwoRight()))
            {
                rightMove = m_BoardOfPiece.WhitePieceExist(pieceLocationInBoard.DiagonalPlusOneRight());
                if (rightMove)
                {
                    i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalPlusTwoRight()));
                }
            }

            return leftMove || rightMove;
        }

        private bool whiteKingPieceCanEat(List<Move> i_RandomMoves)
        {
            Point pieceLocationInBoard = new Point(m_LocationInBoard.Row, m_LocationInBoard.Column);
            bool leftMoveBack = false;
            bool rightMoveBack = false;
            bool forwardMove = whiteRegularPieceCanEat(i_RandomMoves);
            if (squareInBoardAndEmpty(pieceLocationInBoard.DiagonalPlusTwoLeft()))
            {
                leftMoveBack = m_BoardOfPiece.BlackPieceExist(pieceLocationInBoard.DiagonalPlusOneLeft());
                if (leftMoveBack)
                {
                    i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalPlusTwoLeft()));
                }
            }

            if (squareInBoardAndEmpty(pieceLocationInBoard.DiagonalPlusTwoRight()))
            {
                rightMoveBack = m_BoardOfPiece.BlackPieceExist(pieceLocationInBoard.DiagonalPlusOneRight());
                if (rightMoveBack)
                {
                    i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalPlusTwoRight()));
                }
            }

            return leftMoveBack || rightMoveBack || forwardMove;
        }

        private bool blackKingPieceCanEat(List<Move> i_RandomMoves)
        {
            Point pieceLocationInBoard = new Point(m_LocationInBoard.Row, m_LocationInBoard.Column);
            bool leftMoveBack = false;
            bool rightMoveBack = false;
            bool forwardMove = blackRegularPieceCanEat(i_RandomMoves);
            if (squareInBoardAndEmpty(pieceLocationInBoard.DiagonalMinusTwoLeft()))
            {
                leftMoveBack = m_BoardOfPiece.WhitePieceExist(pieceLocationInBoard.DiagonalMinusOneLeft());
                if (leftMoveBack)
                {
                    i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalMinusTwoLeft()));
                }
            }

            if (squareInBoardAndEmpty(pieceLocationInBoard.DiagonalMinusTwoRight()))
            {
                rightMoveBack = m_BoardOfPiece.WhitePieceExist(pieceLocationInBoard.DiagonalMinusOneRight());
                if (rightMoveBack)
                {
                    i_RandomMoves.Add(new Move(pieceLocationInBoard, pieceLocationInBoard.DiagonalMinusTwoRight()));
                }
            }

            return leftMoveBack || rightMoveBack || forwardMove;
        }

        public eTypePieceAndColour TypePiece
        {
            get
            {
                return m_TypePiece;
            }

            set
            {
                m_TypePiece = value;
            }
        }

        public Point PieceLocationInBoard
        {
            get
            {
                return m_LocationInBoard;
            }

            set
            {
                m_LocationInBoard = value;
            }
        }

        public bool SamePiece(eTypePieceAndColour i_Piece)
        {
            return (int)m_TypePiece == (int)i_Piece;
        }
    }
}