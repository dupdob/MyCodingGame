using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 https://www.codingame.com/training/easy/counting-squares-on-pegs
 **/
namespace CodingGame.Facile
{
    internal static class CountingSquares
    {
        private static void MainSquares()
        {    
            var n = int.Parse(Console.ReadLine());
            var vertex = new Vertex[n];
            for (var i = 0; i < n; i++)
            {
                var inputs = Console.ReadLine().Split(' ');
                var x = int.Parse(inputs[0]);    
                var y = int.Parse(inputs[1]);
                vertex[i] = new Vertex(x, y);
            }

            var edges = new Dictionary<int, ICollection<Edge>>();
            var vertices = new Dictionary<int, ICollection<Vertex>>();
            var count = 0;
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    var vertice = new Edge(vertex[i], vertex[j]);
                    
                    if (vertice.Dx < 0 || vertice.Dy <= 0)
                    {
                        continue;
                    }
                    
                    if (!edges.ContainsKey(vertice.Distance))
                    {
                        edges[vertice.Distance] = new List<Edge >();
                        vertices[vertice.Distance] = new HashSet<Vertex>();
                    }
                    edges[vertice.Distance].Add(vertice);
                    vertices[vertice.Distance].Add(vertex[i]);
                    vertices[vertice.Distance].Add(vertex[j]);
                    count++;
                }
            }
            Console.Error.WriteLine($"Found {count} vertices.");
            // now scan length
            var answer = 0;
            foreach (var entry in edges)
            {
                if (entry.Value.Count < 2 || !edges.ContainsKey(entry.Key * 2))
                {
                    continue;
                }

                var findSquare = FindSquare(entry.Value, vertices[entry.Key]);

                answer += findSquare;
            }
            Console.Write(answer);
        }

        private static int FindSquare(ICollection<Edge> edges, ICollection<Vertex> diagVertex)
        {
            var count = 0;
            // identify edges per vertex

            var excluded = new HashSet<Edge>();
            foreach(var entry in edges)
            {
                var vertex = entry.Start;
                var secondVertex = entry.End;

                if (excluded.Contains(entry))
                {
                    continue;
                }

                excluded.Add(entry);
                var yDiff = secondVertex.Y - vertex.Y;
                var xDiff = secondVertex.X - vertex.X;
                var potentialThirdVertex = new Vertex(vertex.X - yDiff, vertex.Y + xDiff);
                var potentialFourthVertex = new Vertex(secondVertex.X - yDiff, secondVertex.Y + xDiff);
                if (diagVertex.Contains(potentialThirdVertex) && diagVertex.Contains(potentialFourthVertex))
                {
                    count++;
                }
                /*
                // let's see if we can find the fourth vertex
                potentialThirdVertex = new Vertex(vertex.X + YDiff, vertex.Y - xDiff);
                potentialFourthVertex = new Vertex(secondVertex.X + YDiff, secondVertex.Y - xDiff);
                if (diagVertex.Contains(potentialThirdVertex) && diagVertex.Contains(potentialFourthVertex))
                {
                    count++;
                }
*/
              
            }
            return count;
        }

        private class Vertex
        {
            private bool Equals(Vertex other)
            {
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == GetType() && Equals((Vertex) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (X * 397) ^ Y;
                }
            }

            public Vertex(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }

            public override string ToString()
            {
                return $"({X}:{Y})";
            }

            public int DistanceTo(Vertex other)
            {
                return (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y);
            }
        }

        private class Edge
        {
            private bool Equals(Edge other)
            {
                return Equals(Start, other.Start) && Equals(End, other.End) ||
                       Equals(Start, other.End) && Equals(End, other.Start);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Edge) obj);
            }

            public override int GetHashCode()
            {
                return (Start != null ? Start.GetHashCode() : 0 ) ^ (End != null ? End.GetHashCode() : 0);
            }

            public Edge(Vertex start, Vertex end)
            {
                Start = start;
                End = end;
                Distance = start.DistanceTo(end);
            }

            public Vertex Start { get; }
            
            public Vertex End { get; }

            public int Distance { get; }

            public int Dx => End.X - Start.X;
            
            public int Dy => End.Y - Start.Y;

            public override string ToString()
            {
                return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
            }
        }
    }
}
