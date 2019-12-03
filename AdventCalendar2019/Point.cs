using System;

namespace AdventCalendar2019
{
    public class Point
    {
        private int X;
        private int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        private bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public Point Move(char dir)
        {
            switch (dir)
            {
                case '>':
                case 'R':
                    return new Point(X+1, Y);
                case '<':
                case 'L':
                    return new Point(X-1, Y);
                case '^':
                case 'U':
                    return new Point(X, Y-1);
                case 'v':
                case 'D':
                    return new Point(X, Y+1);
            }

            return this;
        }
        
        public int ManhattanDist(Point dirLength)
        {
            return Math.Abs(X - dirLength.X) + Math.Abs(Y - dirLength.Y);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}