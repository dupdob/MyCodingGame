using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/**
Squares have been drawn in ASCII characters in a grid.
The objective is to find each of their sizes and in which order they have been drawn.
(Each square overlaps another one.)
Entrée
Line 1 : h the height and w the width of the grid.
Line 2 : nb the number of squares.
h following lines : the content of the grid. '.' for an empty spot,
and a label (between 1 and nb) representing the border of the squares.
Sortie
nb lines giving in drawing order :
The label and size of each square, separated by a space, in order from the first one drawn to the last.
Contraintes
2 ≤ h, w ≤ 10
1 ≤ nb, label ≤ 5
2 ≤ size ≤ 10
 **/
class SolutionSquare
{

    class Square: IEnumerable<Tuple<int, int>>
    {
        public List<int> Overlays = new List<int>();
        public List<Tuple<int, int>> points = new List<Tuple<int, int>>();
        public int MinX = int.MaxValue;
        public int MaxX = int.MinValue;
        public int MinY = int.MaxValue;
        public int MaxY = int.MinValue;

        public void UpdateCorners(int x, int y)
        {
            this.points.Add(new Tuple<int, int>(x,y));
            if (x < this.MinX)
            {
                this.MinX = x;
            }
            if (x>this.MaxX)
            {
                this.MaxX = x;
            }
            if (y < this.MinY)
            {
                this.MinY = y;
            }
            if (y>this.MaxY)
            {
                this.MaxY = y;
            }
        }

        public List<Square> IdentifyPotentialSquares()
        {
            // confirm edges
            bool confirmLeft = false;
            bool confirmRight = false;
            bool confirmTop = false;
            bool confirmBottom = false;
            foreach (var point in this.points)
            {
                if (point.Item1 == this.MinX && point.Item2 != this.MinY && point.Item2 != this.MaxY)
                {
                    confirmLeft = true;
                }
                if (point.Item1 == this.MaxX && point.Item2 != this.MinY && point.Item2 != this.MaxY)
                {
                    confirmRight = true;
                }
                if (point.Item1 != this.MinX && point.Item2 == this.MinY && point.Item1 != this.MaxX)
                {
                    confirmTop = true;
                }
                if (point.Item1 != this.MaxX && point.Item2 == this.MaxY && point.Item1 != this.MinX)
                {
                    confirmBottom = true;
                }
            }
            int minDim = Math.Max(this.MaxX - this.MinX + 1, this.MaxY - this.MinY);
            var result = new List<Square>();
            // if all edges are sure, no questions
            if (confirmBottom && confirmLeft && confirmLeft && confirmRight)
            {
                result.Add(this);
            }
            else if (confirmRight && confirmLeft && confirmBottom)
            {
                Square altSquare = new Square();
                altSquare.MaxX = this.MaxX;
                altSquare.MinX = this.MaxX - minDim;
                altSquare.MaxY = this.MaxY;
                altSquare.MinY = this.MinY;
                altSquare.points = this.points;
                result.Add(altSquare);
            }
            else if (confirmLeft && confirmRight && confirmTop)
            {
                Square altSquare = new Square();
                altSquare.MaxX = this.MinX + minDim;
                altSquare.MinX = this.MinX;
                altSquare.MaxY = this.MaxY;
                altSquare.MinY = this.MinY;
                altSquare.points = this.points;
                result.Add(altSquare);
            }
            return result;
        }

        public IEnumerator<Tuple<int, int>> GetEnumerator()
        {
            return new BorderEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return new BorderEnumerator(this);
        }

        public class BorderEnumerator : IEnumerator<Tuple<int, int>>
        {
            private Square pathSquare;
            private Tuple<int, int> current = new Tuple<int, int>(0, 0);

            public BorderEnumerator(Square square)
            {
                this.pathSquare = square;
                Reset();
            }

            public Tuple<int, int> Current
            {
                get
                {
                    return this.current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            public void Dispose()
            {
                return;
            }

            public bool MoveNext()
            {
                var X = this.current.Item1;
                var Y = this.current.Item2;
                if (Y == this.pathSquare.MinY)
                {
                    if (X < this.pathSquare.MaxX)
                    {
                        X++;
                    }
                    else
                    {
                        Y++;
                    }
                }
                else if (Y == this.pathSquare.MaxY)
                {
                    if (X > this.pathSquare.MinX)
                    {
                        X--;
                    }
                    else
                    {
                        Y--;
                    }
                }
                else if (X == this.pathSquare.MaxX)
                {
                    Y++;
                }
                else if (X == this.pathSquare.MinX)
                {
                    Y--;
                    if (Y == this.pathSquare.MinY)
                        return false;
                }
                this.current = new Tuple<int, int>(X, Y);
                return true;
            }

            public void Reset()
            {
                this.current = new Tuple<int, int>(this.pathSquare.MinX, this.pathSquare.MinY);
            }
        }
    }

    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int h = int.Parse(inputs[0]);
        int w = int.Parse(inputs[1]);
        int nb = int.Parse(Console.ReadLine());
        List<string> map = new List<string>();
        Dictionary<int, Square> squares = new Dictionary<int, Square>(nb);
        // we capture inputs and identify squares coordinates
        for (int i = 0; i < h; i++)
        {
            string line = Console.ReadLine();
            map.Add(line);
            Console.Error.WriteLine(line);
            for (int l = 0; l < line.Length; l++)
            {
                if (line[l] != '.')
                {
                    int id = line[l] - '0';
                    if (!squares.ContainsKey(id))
                    {
                        squares[id] = new Square();
                    }
                    squares[id].UpdateCorners(l, i);
                }
            }
        }
        // now we identify overlapping.
        foreach (var square in squares)
        {
            Console.Error.WriteLine("scanning {0} = {1}:{2} - {3} {4}", square.Key, square.Value.MinX, square.Value.MinY,
                square.Value.MaxX, square.Value.MaxY);
            foreach (var coords in square.Value)
            {
                var car = map[coords.Item2][coords.Item1];
                if (car == '.')
                {
                    Console.Error.WriteLine("Unexpected dot at {0}:{1}", coords.Item1, coords.Item2);
                    // rectangles was not properly identified
                    return;
                }
                int id = car - '0';
                if (id != square.Key)
                {
                    if (!square.Value.Overlays.Contains(id))
                    {
                        square.Value.Overlays.Add(id);
                        Console.Error.WriteLine("Square {0} overlaps square {1}", id, square.Key);
                    }
                }
            }
        }
        Console.Error.WriteLine("Ordering");
        var squareList = new List<int>(squares.Keys);
        var orderedSquares = new Stack<int>();
        // we still have to identify the order
        while (squareList.Count != 0)
        {
            for (int i=0;i<squareList.Count; i++)
            {
                var overlaysFound = true;
                // we can add a square to the list if all the overlays are in the list
                foreach (var overlay in squares[squareList[i]].Overlays)
                {
                    if (!orderedSquares.Contains(overlay))
                    {
                        overlaysFound = false;
                        break;
                    }
                }
                if (overlaysFound)
                {
                    orderedSquares.Push(squareList[i]);
                    squareList.RemoveAt(i--);
                }
            }
        }
        // now we dump

        foreach (var orderedSquare in orderedSquares)
        {
            var square = squares[orderedSquare];
            Console.WriteLine("{0} {1}", orderedSquare, square.MaxX-square.MinX+1);
        }
    }
}