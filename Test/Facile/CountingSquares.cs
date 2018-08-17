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
        private static void Main(string[] args)
        {
            var N = int.Parse(Console.ReadLine());
            var vertex = new Vertex[N];
            for (var i = 0; i < N; i++)
            {
                var inputs = Console.ReadLine().Split(' ');
                var X = int.Parse(inputs[0]);
                var Y = int.Parse(inputs[1]);
                vertex[i] = new Vertex(X, Y);
            }

            var edges = new Dictionary<int, ICollection<Edge>>();
            for (var i = 0; i < N; i++)
            {
                for (var j = i+1; j < N; j++)
                {
                    var vertice = new Edge(vertex[i], vertex[j]);
                    if (!edges.ContainsKey(vertice.Distance))
                    {
                        edges[vertice.Distance] = new List<Edge >();
                    }
                    edges[vertice.Distance].Add(vertice);
                }
            }
            // now scan length
            var answer = 0;
            foreach (var entry in edges)
            {
                if (entry.Value.Count < 4)
                {
                    continue;
                }

                //Console.Error.WriteLine($"Found {entry.Value.Count} vertices with a squared length of ${entry.Key}.");
                var findSquare = FindSquare(entry.Value);
                if (findSquare > 0)
                {
                    Console.Error.WriteLine($"Found {findSquare} squares of dim {Math.Sqrt(entry.Key)}.");
                }
                answer += findSquare;
            }
            Console.Write(answer);
        }

        private static int FindSquare(IEnumerable<Edge> edges)
        {
            var count = 0;
            var edgePerVertex = new Dictionary<Vertex, HashSet<Vertex>>();
            var allVertices = new HashSet<Vertex>();
            // identify edges per vertex
            foreach (var edge in edges)
            {
                if (!edgePerVertex.ContainsKey(edge.Start))
                {
                    edgePerVertex.Add(edge.Start, new HashSet<Vertex>());
                }

                if (!edgePerVertex.ContainsKey(edge.End))
                {
                    edgePerVertex.Add(edge.End, new HashSet<Vertex>());
                }

                allVertices.Add(edge.Start);
                allVertices.Add(edge.End);
                edgePerVertex[edge.Start].Add(edge.End);
                edgePerVertex[edge.End].Add(edge.Start);
            }

            var excluded = new HashSet<Edge>();
            foreach(var entry in edgePerVertex)
            {
                var vertexList = entry.Value;
                var vertex = entry.Key;
//                Console.Error.WriteLine($"Found {vertexList.Count} edges starting at {vertex}");
                if (vertexList.Count < 2)
                {
                    // no interest
                    continue;
                }

                foreach (var secondVertex in vertexList)
                {
                    var log = false;
                    var edge = new Edge(vertex, secondVertex);
                    if (log)
                    {
                        Console.Error.Write($"examine {secondVertex}.");
                    }
                    if (excluded.Contains(edge))
                    {
                        continue;
                    }

                    excluded.Add(edge);
                    var YDiff = secondVertex.Y - vertex.Y;
                    var xDiff = secondVertex.X - vertex.X;
                    var potentialThirdVertex = new Vertex(vertex.X - YDiff, vertex.Y + xDiff);
                    var potentialFourthVertex = new Vertex(secondVertex.X - YDiff, secondVertex.Y + xDiff);
                    if (log)
                    {
                        Console.Error.Write($"with {potentialThirdVertex} and {potentialFourthVertex}, ");
                    }
                    if (vertexList.Contains(potentialThirdVertex) && allVertices.Contains(potentialFourthVertex))
                    {
                        if (!excluded.Contains(new Edge(secondVertex, potentialFourthVertex))
                            && !excluded.Contains(new Edge(potentialThirdVertex, potentialFourthVertex))
                            && !excluded.Contains(new Edge(potentialThirdVertex, vertex)))
                        {
                            Console.Error.WriteLine($"Found {vertex}/{secondVertex}/{potentialFourthVertex}/{potentialThirdVertex}");
                            count++;
                        }
                    }
                    // let's see if we can find the fourth vertex
                    potentialThirdVertex = new Vertex(vertex.X + YDiff, vertex.Y - xDiff);
                    potentialFourthVertex = new Vertex(secondVertex.X + YDiff, secondVertex.Y - xDiff);
                    if (log)
                    {
                        Console.Error.WriteLine($"with {potentialThirdVertex} and {potentialFourthVertex}.");
                    }
                    if (vertexList.Contains(potentialThirdVertex) && allVertices.Contains(potentialFourthVertex))
                    {
                        if (!excluded.Contains(new Edge(secondVertex, potentialFourthVertex))
                            && !excluded.Contains(new Edge(potentialThirdVertex, potentialFourthVertex))
                            && !excluded.Contains(new Edge(potentialThirdVertex, vertex)))
                        {
                            Console.Error.WriteLine($"Found {vertex}/{secondVertex}/{potentialFourthVertex}/{potentialThirdVertex}");
                            count++;
                        }
                    }

                }
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
            protected bool Equals(Edge other)
            {
                return Equals(Start, other.Start) && Equals(End, other.End) ||
                       Equals(Start, other.End) && Equals(End, other.Start);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Edge) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Start != null ? Start.GetHashCode() : 0 ) ^ (End != null ? End.GetHashCode() : 0));
                }
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

            public override string ToString()
            {
                return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
            }
        }
    }
}
