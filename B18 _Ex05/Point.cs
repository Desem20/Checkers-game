using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18__Ex05
{
    internal class Point
    {
        private int m_Row;
        private int m_Column;

        public Point(int i_Row, int i_Column)
        {
            m_Row = i_Row;
            m_Column = i_Column;
        }

        public Point() : this(new int(), new int())
        {
        }

        public void SetPointTo(Point i_SettingPoint)
        {
            m_Row = i_SettingPoint.Row;
            m_Column = i_SettingPoint.Column;
        }

        public int Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int Column
        {
            get
            {
                return m_Column;
            }

            set
            {
                m_Column = value;
            }
        }

        public bool CheckIfPointIsOnBoard(int i_BoardSize)
        {
            bool isOnBoard = m_Row < i_BoardSize && m_Column < i_BoardSize && m_Row >= 0 && m_Column >= 0;
            return isOnBoard;
        }

        public bool SamePoint(Point i_Point)
        {
            return m_Row == i_Point.m_Row && m_Column == i_Point.m_Column;
        }

        public Point DiagonalPlusOneRight()
        {
            return Coordinate(this.Row - 1, this.Column + 1);
        }

        public Point DiagonalPlusOneLeft()
        {
            return Coordinate(this.Row - 1, this.Column - 1);
        }

        public Point DiagonalMinusOneRight()
        {
            return Coordinate(this.Row + 1, this.Column + 1);
        }

        public Point DiagonalMinusOneLeft()
        {
            return Coordinate(this.Row + 1, this.Column - 1);
        }

        public Point DiagonalPlusTwoRight()
        {
            return Coordinate(this.Row - 2, this.Column + 2);
        }

        public Point DiagonalPlusTwoLeft()
        {
            return Coordinate(this.Row - 2, this.Column - 2);
        }

        public Point DiagonalMinusTwoRight()
        {
            return Coordinate(this.Row + 2, this.Column + 2);
        }

        public Point DiagonalMinusTwoLeft()
        {
            return Coordinate(this.Row + 2, this.Column - 2);
        }

        private Point Coordinate(int i_Row, int i_Column)
        {
            return new Point(i_Row, i_Column);
        }
    }
}