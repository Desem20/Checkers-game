using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18__Ex05
{
    internal class Move
    {
        private Point m_Source;
        private Point m_Destination;

        public Move(Point i_Source, Point i_Destination)
        {
            m_Source = i_Source;
            m_Destination = i_Destination;
        }

        public Move() : this(new Point(), new Point())
        {
        }

        public bool SameMove(Move i_Move)
        {
            return m_Source.Row == i_Move.Source.Row && m_Source.Column == i_Move.Source.Column
                && m_Destination.Row == i_Move.Destination.Row && m_Destination.Column == i_Move.Destination.Column;
        }

        public bool EatingMove()
        {
            return eatingUpMove() || eatingDownMove();
        }

        private bool eatingUpMove()
        {
            bool rowIsOK = m_Destination.Row - m_Source.Row == -2;
            bool columnIsOK = m_Destination.Column - m_Source.Column == 2 || m_Destination.Column - m_Source.Column == -2;
            return rowIsOK && columnIsOK;
        }

        private bool eatingDownMove()
        {
            bool rowIsOK = m_Destination.Row - m_Source.Row == 2;
            bool columnIsOK = m_Destination.Column - m_Source.Column == 2 || m_Destination.Column - m_Source.Column == -2;
            return rowIsOK && columnIsOK;
        }

        public Point LocationPieceToBeEaten()
        {
            Point locationPieceToBeEat = new Point(m_Source.Row + ((m_Destination.Row - m_Source.Row) / 2), m_Source.Column + ((m_Destination.Column - m_Source.Column) / 2));
            return locationPieceToBeEat;
        }

        public Point Source
        {
            get
            {
                return m_Source;
            }

            set
            {
                m_Source = value;
            }
        }

        public Point Destination
        {
            get
            {
                return m_Destination;
            }

            set
            {
                m_Destination = value;
            }
        }
    }
}