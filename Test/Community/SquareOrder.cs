using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
    class Square : IEnumerable<Tuple<int, int>>
    {
        public List<int> Overlays = new List<int>();
        public List<Tuple<int, int>> points = new List<Tuple<int, int>>();
        public int MinX = int.MaxValue;
        public int MaxX = int.MinValue;
        public int MinY = int.MaxValue;
        public int MaxY = int.MinValue;

        public void UpdateCorners(int x, int y)
        {
            this.points.Add(new Tuple<int, int>(x, y));
            if (x < this.MinX)
            {
                this.MinX = x;
            }
            if (x > this.MaxX)
            {
                this.MaxX = x;
            }
            if (y < this.MinY)
            {
                this.MinY = y;
            }
            if (y > this.MaxY)
            {
                this.MaxY = y;
            }
        }

        public List<Square> IdentifyPotentialSquares(int widht, int height)
        {
            // confirm edges
            var confirmLeft = false;
            var confirmRight = false;
            var confirmTop = false;
            var confirmBottom = false;
            var countTop = 0;
            var countBottom = 0;
            var countLeft = 0;
            var countRight = 0;
            // confirm edges
            foreach (var point in this.points)
            {
                if (point.Item1 == this.MinX)
                {
                    countLeft++;
                }
                else if (point.Item1 == this.MaxX)
                {
                    countRight++;
                }
                if (point.Item2 == this.MinY)
                {
                    countTop++;
                }
                else if (point.Item2 == this.MaxY)
                {
                    countBottom++;
                }
            }
            if (countLeft > 2)
            {
                confirmLeft = true;
            }
            if (countRight > 2)
            {
                confirmRight = true;
            }
            if (countTop > 2)
            {
                confirmTop = true;
            }
            if (countBottom > 2)
            {
                confirmBottom = true;
            }
            var tempHeight = this.MaxY - this.MinY + 1;
            var tempWidth = this.MaxX - this.MinX + 1;
            if (countLeft >= 2 && countRight >= 2 && countTop >= 2 && countBottom > 2)
            {
                if (tempWidth == tempHeight)
                {
                    confirmRight = confirmTop = confirmBottom = confirmLeft = true;
                }
                if (tempWidth > tempHeight)
                {
                    confirmRight = confirmLeft = true;
                }
                else
                {
                    confirmTop = confirmBottom = true;
                }
            }
            int minDim = Math.Max(tempWidth, tempHeight);
            var result = new List<Square>();
            // if all edges are sure, no questions
            if (confirmBottom && confirmLeft && confirmTop && confirmRight)
            {
                result.Add(this);
            }
            else
            {
                var minX = confirmLeft ? this.MinX : 0;
                var maxX = confirmRight ? this.MaxX : widht - 1;
                var minY = confirmTop ? this.MinY : 0;
                var maxY = confirmBottom ? this.MaxY : height - 1;

                var maxDim = Math.Min(maxX - minX + 1, maxY - minY + 1);
                // try all possible dimensions
                for (var size = minDim; size <= maxDim; size++)
                {
                    // try all possible top lines
                    for (var ty = minY; ty <= this.MinY; ty++)
                    {
                        var by = ty + size - 1;
                        // check if bottom line is possible
                        if (by < this.MaxY || by > maxY)
                        {
                            continue;
                        }
                        // check all left column
                        for (var lx = minX; lx <= this.MinX; lx++)
                        {
                            var rx = lx + size - 1;
                            // check if right column is possible
                            if (rx < this.MaxX || rx > maxX)
                            {
                                continue;
                            }
                            var rect = new Square
                            {
                                MinX = lx,
                                MaxX = rx,
                                MinY = ty,
                                MaxY = @by
                            };
                            result.Add(rect);
                        }
                    }
                }
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

        private class BorderEnumerator : IEnumerator<Tuple<int, int>>
        {
            private readonly Square pathSquare;
            private Tuple<int, int> current = new Tuple<int, int>(0, 0);

            public BorderEnumerator(Square square)
            {
                this.pathSquare = square;
                Reset();
            }

            public Tuple<int, int> Current => this.current;

            object IEnumerator.Current => this.current;

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
                        if (Y == this.pathSquare.MinY)
                            return false;
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
        var inputs = Console.ReadLine().Split(' ');
        var h = int.Parse(inputs[0]);
        var w = int.Parse(inputs[1]);
        var nb = int.Parse(Console.ReadLine());
        var map = new List<string>();
        var squares = new Dictionary<int, Square>(nb);
        var chrono = new Stopwatch();
        chrono.Start();
        // we capture inputs and identify squares coordinates
        for (var i = 0; i < h; i++)
        {
            var line = Console.ReadLine();
            map.Add(line);
//            Console.Error.WriteLine(line);
            for (var l = 0; l < line.Length; l++)
            {
                if (line[l] == '.') continue;
                var id = line[l] - '0';
                if (!squares.ContainsKey(id))
                {
                    squares[id] = new Square();
                }
                squares[id].UpdateCorners(l, i);
            }
        }
        // now we identify overlapping.
        foreach (var nextSquare in squares)
        {
            Console.Error.WriteLine("Scaning square {0}({1}).", nextSquare.Key, chrono.ElapsedMilliseconds);
            var possibleSquares = nextSquare.Value.IdentifyPotentialSquares(w, h);
            Square validSquare = null;
            foreach (var square in possibleSquares)
            {
                var errorFound = false;
//                Console.Error.WriteLine("scanning {0} = {1}:{2} - {3} {4}", nextSquare.Key, square.MinX, square.MinY,
//                    square.MaxX, square.MaxY);
                foreach (var coords in square)
                {
                    var car = map[coords.Item2][coords.Item1];
                    Console.Error.Write(car);
                    if (car == '.')
                    {
 //                       Console.Error.WriteLine("Unexpected dot at {0}:{1}", coords.Item1, coords.Item2);
                        errorFound = true;
                        // rectangles was not properly identified
                        break;
                    }
                    var id = car - '0';
                    if (id != nextSquare.Key)
                    {
                        if (!square.Overlays.Contains(id))
                        {
                            square.Overlays.Add(id);
//                            Console.Error.WriteLine("Square {0} overlaps square {1}", id, nextSquare.Key);
                        }
                    }
                }
                if (!errorFound)
                {
                    validSquare = square;
                    break;
                }
            }
            nextSquare.Value.MinX = validSquare.MinX;
            nextSquare.Value.MaxX = validSquare.MaxX;
            nextSquare.Value.MinY = validSquare.MinY;
            nextSquare.Value.MaxY = validSquare.MaxY;
            nextSquare.Value.Overlays = validSquare.Overlays;
        }
        Console.Error.WriteLine("Ordering ({0})", chrono.ElapsedMilliseconds);
        var squareList = new List<int>(squares.Keys);
        var orderedSquares = new Stack<int>();
        // we still have to identify the order
        while (squareList.Count != 0)
        {
            for (var i = 0; i < squareList.Count; i++)
            {
                var overlaysFound = squares[squareList[i]].Overlays.All(overlay => orderedSquares.Contains(overlay));
                // we can add a square to the list if all the overlays are in the list
                if (!overlaysFound) continue;
                orderedSquares.Push(squareList[i]);
                squareList.RemoveAt(i--);
            }
        }
        // now we dump

        foreach (var orderedSquare in orderedSquares)
        {
            var square = squares[orderedSquare];
            Console.WriteLine("{0} {1}", orderedSquare, square.MaxX - square.MinX + 1);
        }
    }
}