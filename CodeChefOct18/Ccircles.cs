using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeChefOct18
{
    public class Ccircles
    {
        private class Circle
        {
            private int x;
            private int y;
            private int radius;
            private readonly int _id;

            public int X => x;

            public int Y => y;

            public int Radius => radius;

            public int Id => _id;

            public Circle(int x, int y, int radius, int id)
            {
                this.x = x;
                this.y = y;
                this.radius = radius;
                _id = id;
            }

        }

        private class CirclePair: IComparable
        {
            private readonly long minDist;
            private readonly long maxDist;
            private readonly Circle a;
            private readonly Circle b;

            public long MaxDist => maxDist;

            public CirclePair(Circle a, Circle b)
            {
                var dist = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
                this.a = a;
                this.b = b;
                var roundedUpDist = (long)Math.Ceiling(dist);
                var roundedDownDist = (long)Math.Floor(dist);
                maxDist = roundedDownDist + a.Radius + b.Radius;

                if (a.Radius + dist < b.Radius)
                {
                    minDist = b.Radius - roundedUpDist - a.Radius;
                }
                else if (b.Radius + dist < a.Radius)
                {
                    minDist = a.Radius - roundedUpDist - b.Radius;
                    
                }
                else
                {
                    minDist = Math.Max(0, roundedUpDist - a.Radius - b.Radius);
                }
            }

            public bool IsPossible(int dist)
            {
                return dist >= minDist && dist <= maxDist;
            }

            public int CompareTo(object obj)
            {
                var other = obj as CirclePair;
                if (other == null)
                    return 1;
                if (maxDist > other.maxDist)
                {
                    return 1;
                }

                if (maxDist < other.maxDist)
                {
                    return -1;
                }

                if (minDist > other.minDist)
                {
                    return 1;
                }

                if (minDist < other.minDist)
                {
                    return -1;
                }

                if (a.Id > other.a.Id)
                {
                    return 1;
                }
                
                if (a.Id < other.a.Id)
                {
                    return -1;
                }

                if (b.Id > other.b.Id)
                {
                    return 1;
                }

                if (b.Id < other.b.Id)
                {
                    return -1;
                }

                return b.Id > other.b.Id ? 1 : 0;
            }
        }
        
        static void Main(string[] args)
        {
            var scope = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var N = scope[0];
            var Q = scope[1];
            var circles = new List<Circle>(N);
            for (var i = 0; i < N; i++)
            {
                var pars = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                circles.Add(new Circle(pars[0], pars[1], pars[2], i));
            }
            
            var pairs = new List<CirclePair>(N*(N-1)/2);
            for (var i = 0; i < circles.Count-1; i++)
            {
                for (var j = i+1; j < circles.Count; j++)
                {
                    var pair = new CirclePair(circles[i], circles[j]);
                    pairs.Add(pair);
                }
            }

            pairs.Sort();
            
            var result = new StringBuilder(Q * 10);
            for (var i = 0; i < Q; i++)
            {
                var len = int.Parse(Console.ReadLine());

                var index = FindScan(len, pairs);
                var res = 0;
                if (index >= 0)
                {
                    for (var j = index; j < pairs.Count; j++)
                    {
                        if (pairs[j].IsPossible(len))
                        {
                            res++;
                        }
                    }
                }

                result.AppendLine(res.ToString());   
            }
            Console.Write(result);
        }

        private static int FindScan(int len, IReadOnlyList<CirclePair> pairs)
        {
            var end = pairs.Count - 1;

            if (pairs[end].MaxDist < len)
            {
                return -1;
            }

            return subscan(len, pairs, 0, end);
        }

        private static int subscan(int len, IReadOnlyList<CirclePair> pairs, int begin, int end)
        {
            var mid = (begin + end) / 2;

            if (end - begin < 2)
            {
                return begin;
            }
            
            if (pairs[mid].MaxDist < len)
            {
                begin = mid + 1;
            }
            else
            {
                end = mid;
            }

            return subscan(len, pairs, begin, end);
        }
    }
}