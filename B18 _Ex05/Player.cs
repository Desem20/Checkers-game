using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18__Ex05
{
    internal class Player
    {
        private const string k_Black = "Black";
        private const string k_White = "White";
        private string m_NameOfPlayer;
        private int m_ScoreOfPlayerIncurrentGame;
        private int m_ScoreOfPlayerInAllGames;
        private int m_NumberOfPieces;
        private Board m_BoardOfPlayer;
        private List<BoardPiece> m_PiecesOfPlayer;
        private string m_BlackOrWhitePlayer;
        private Move m_PlayerCurrentMove;
        private Point m_PlayerMustMakeThisPiece;
        private bool m_AnotherMoveToEat;
       
        public void PlayerChoseValidPiece(Point i_Source)
        {
            bool PlayerChoseValidPiece;
            if (m_BlackOrWhitePlayer.Equals(k_Black))
            {
                PlayerChoseValidPiece = m_BoardOfPlayer.BlackPieceExist(i_Source);
            }
            else
            {
                PlayerChoseValidPiece = m_BoardOfPlayer.WhitePieceExist(i_Source);
            }

            if(PlayerChoseValidPiece)
            {
                m_BoardOfPlayer.GetSquare(i_Source).OnChosen();
            }
        }

        public Player(string i_Name, Board i_Board, string i_BlackOrWhite)
        {
            m_NameOfPlayer = i_Name;
            m_BoardOfPlayer = i_Board;
            m_BlackOrWhitePlayer = i_BlackOrWhite;
            m_PlayerCurrentMove = new Move();
            checkWhichList();
            m_NumberOfPieces = m_PiecesOfPlayer.Count;
            m_ScoreOfPlayerIncurrentGame = 0;
            m_AnotherMoveToEat = false;
        }

        public Move RandomMove()
        {
            List<Move> randomMoves = GetAllThePossibleMoves();
            Random randomNumber = new Random();
            int indexOfRandomMove = randomNumber.Next(0, randomMoves.Count);
            return randomMoves[indexOfRandomMove];
        }

        public bool CheckIfPlayerHasNoValidMoves()
        {
            List<Move> possibleMoves = GetAllThePossibleMoves();
            return possibleMoves.Count == 0;
        }

        public List<Move> GetAllThePossibleMoves()
        {
            List<Move> randomMoves = new List<Move>(m_NumberOfPieces);
            if (m_AnotherMoveToEat)
            {
                foreach (BoardPiece indexBoardPiece in m_PiecesOfPlayer)
                {
                    if (m_PlayerMustMakeThisPiece.SamePoint(indexBoardPiece.PieceLocationInBoard))
                    {
                        indexBoardPiece.CanEat(randomMoves);
                    }
                }
            }
            else if (MustEat())
            {
                foreach (BoardPiece IndexBoardPiece in m_PiecesOfPlayer)
                {
                    IndexBoardPiece.CanEat(randomMoves);
                }
            }
            else
            {
                foreach (BoardPiece IndexBoardPiece in m_PiecesOfPlayer)
                {
                    IndexBoardPiece.CanRegularMove(randomMoves);
                }
            }

            return randomMoves;
        }

        public bool MustEat()
        {
            List<Move> moves = new List<Move>();
            bool mustEat = false;
            foreach (BoardPiece indexBoardPiece in m_PiecesOfPlayer)
            {
                mustEat = indexBoardPiece.CanEat(moves);
                if (mustEat)
                {
                    break;
                }
            }

            return mustEat;
        }

        public bool CheckIfMoveIsLegal(Move i_Move)
        {
            List<Move> moves = GetAllThePossibleMoves();
            bool moveIsLegal = false;
            foreach (Move indexBoardPiece in moves)
            {
                moveIsLegal = indexBoardPiece.SameMove(i_Move);
                if (moveIsLegal)
                {
                    m_BoardOfPlayer.GetSquare(i_Move.Source).OnChosen();
                    break;
                }
            }

            return moveIsLegal;
        }

        public void MakeMove()
        {
            List<Move> moves = new List<Move>();
            Move currentMove = new Move(m_PlayerCurrentMove.Source, m_PlayerCurrentMove.Destination);
            m_BoardOfPlayer.GetSquare(m_PlayerCurrentMove.Source).BoardPiece.MoveThePiece(currentMove);
            if (m_PlayerCurrentMove.EatingMove())
            {
                if (m_BlackOrWhitePlayer.Equals(k_White))
                {
                    m_BoardOfPlayer.RemoveBlackPieceFromList(m_BoardOfPlayer.GetSquare(m_PlayerCurrentMove.LocationPieceToBeEaten()).BoardPiece);
                }
                else if (m_BlackOrWhitePlayer.Equals(k_Black))
                {
                    m_BoardOfPlayer.RemoveWhitePieceFromList(m_BoardOfPlayer.GetSquare(m_PlayerCurrentMove.LocationPieceToBeEaten()).BoardPiece);
                }

                m_AnotherMoveToEat = m_BoardOfPlayer.GetSquare(m_PlayerCurrentMove.Destination).BoardPiece.CanEat(moves);
                if (m_AnotherMoveToEat)
                {
                    m_PlayerMustMakeThisPiece = m_PlayerCurrentMove.Destination;
                }
            }

            m_NumberOfPieces = m_PiecesOfPlayer.Count;
        }

        public void UpdateScoreOfPlayerInCurrentRound()
        {
            int sumOfCurrentScore = 0;
            foreach (BoardPiece boardPieces in m_PiecesOfPlayer)
            {
                if (boardPieces.TypePiece == BoardPiece.eTypePieceAndColour.RegularBlack || boardPieces.TypePiece == BoardPiece.eTypePieceAndColour.RegularWhite)
                {
                    sumOfCurrentScore++;
                }
                else
                {
                    sumOfCurrentScore += 4;
                }
            }

            m_ScoreOfPlayerIncurrentGame = sumOfCurrentScore;
        }

        private void checkWhichList()
        {
            if (m_BlackOrWhitePlayer.Equals(k_Black))
            {
                m_PiecesOfPlayer = m_BoardOfPlayer.BoardBlackPieces;
            }
            else
            {
                m_PiecesOfPlayer = m_BoardOfPlayer.BoardWhitePieces;
            }
        }

        public List<BoardPiece> PiecesOfPlayer
        {
            get
            {
                return m_PiecesOfPlayer;
            }
        }

        public Move PlayerCurrentMove
        {
            get
            {
                return m_PlayerCurrentMove;
            }

            set
            {
                m_PlayerCurrentMove = value;
            }
        }

        public string Name
        {
            get
            {
                return m_NameOfPlayer;
            }

            set
            {
                m_NameOfPlayer = value;
            }
        }

        public int ScoreOfPlayerInCurrentGame
        {
            get
            {
                return m_ScoreOfPlayerIncurrentGame;
            }

            set
            {
                m_ScoreOfPlayerIncurrentGame = value;
            }
        }

        public int ScoreOfPlayerAllGames
        {
            get
            {
                return m_ScoreOfPlayerInAllGames;
            }

            set
            {
                m_ScoreOfPlayerInAllGames = value;
            }
        }

        public bool AnotherMoveToEat
        {
            get
            {
                return m_AnotherMoveToEat;
            }

            set
            {
                m_AnotherMoveToEat = value;
            }
        }
    }
}