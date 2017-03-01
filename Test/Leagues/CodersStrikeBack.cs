using System;
using System.Collections.Generic;
using System.Security.Policy;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class CodersStrikeBacks
{
    private sealed class Coord
    {
        public int X;
        public int Y;

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coord other)
        {
            if (other == null)
                return false;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coord) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public int Distance(Coord other)
        {
           return (int)(Math.Sqrt(Math.Pow(other.X-X, 2.0)+Math.Pow(other.Y-Y, 2.0)));
        }
    }

    private static void Main(string[] args)
    {
        var boosted = false;
        var lastPosX = 0;
        var lastPosY = 0;
        var firstPoint = true;
        var points = new List<Coord>();
        var identifySegments = false;
        Coord furthestPoint = null;
        Coord lastPoint = null;
        // game loop
        while (true)
        {
            var boost = false;
            var inputs = Console.ReadLine().Split(' ');
            var x = int.Parse(inputs[0]);
            var y = int.Parse(inputs[1]);
            var nextCheckpointX = int.Parse(inputs[2]); // x position of the next check point
            var nextCheckpointY = int.Parse(inputs[3]); // y position of the next check point
            var nextCheckpointDist = int.Parse(inputs[4]); // distance to the next checkpoint
            var nextCheckpointAngle = int.Parse(inputs[5]); // angle between your pod orientation and the direction of the next checkpoint
            inputs = Console.ReadLine().Split(' ');
            //var opponentX = int.Parse(inputs[0]);
            //var opponentY = int.Parse(inputs[1]);
            var nextPoint = new Coord(nextCheckpointX, nextCheckpointY);
            if (!nextPoint.Equals(lastPoint))
            {
                lastPoint = nextPoint;
                if (!identifySegments && points.Contains(nextPoint))
                {
                    Console.Error.WriteLine("Computing longest segment out of {0}.", points.Count);
                    // we have to go back to a known point
                    identifySegments = true;
                    var longestSegment = 0;
                    // identify longest segment
                    for (var i = 0; i < points.Count; i++)
                    {
                        var point = points[(i + 1) % points.Count];
                        var distance = points[i].Distance(nextPoint);
                        if (distance > longestSegment)
                        {
                            Console.Error.WriteLine("Longest segment: {0}-{1} ({2})", i, i + 1, distance);
                            longestSegment = distance;
                            furthestPoint = point;
                        }
                    }
                }
                else
                {
                    points.Add(nextPoint);
                }
            }
            if (!boosted && identifySegments && nextPoint.Equals(furthestPoint) && Math.Abs(nextCheckpointAngle)<10)
            {
                boosted = true;
                boost = true;
            }
            //var lastMove = (int) (firstPoint ? 0 : Math.Sqrt(Math.Pow(lastPosX - x, 2) + Math.Pow(lastPosY - y, 2)));
            var thrust = 100;
            if (Math.Abs(nextCheckpointAngle) > 90)
            {
                thrust = 0;
            }

            firstPoint = false;
            lastPosX = x;
            lastPosY = y;
            Console.WriteLine("{0} {1} {2}", nextCheckpointX, nextCheckpointY, boost ? "BOOST" : thrust.ToString());
        }
    }
}