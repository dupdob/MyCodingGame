//#define TEST
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

            public long X => x;

            public long Y => y;

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

        private class CirclePair
        {
            private readonly long minDist;
            private readonly long maxDist;
            private readonly Circle a;
            private readonly Circle b;

            public int MaxDist => (int) maxDist;

            public int MinDist => (int) minDist;

            public CirclePair(Circle a, Circle b)
            {
                var dist = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
                this.a = a;
                this.b = b;
                var roundedUpDist = (long)Math.Ceiling(dist);
                var roundedDownDist = (long)Math.Floor(dist);
                maxDist = roundedDownDist + a.Radius + b.Radius;

                if (a.Radius + dist <= b.Radius)
                {
                    minDist = b.Radius - roundedDownDist - a.Radius;
                }
                else if (b.Radius + dist <= a.Radius)
                {
                    minDist = a.Radius - roundedDownDist - b.Radius;
                    
                }
                else
                {
                    minDist = Math.Max(0, roundedUpDist - a.Radius - b.Radius);
                }
            }
        }
        
        static void MainCircles(string[] args)
        {
#if TEST
            var N = 1000;
            var rnd = new Random();
            var coord = 400000;
            var circles = new List<Circle>(N);
            for (var i = 0; i < N; i++)
            {
                circles.Add(new Circle(rnd.Next(coord) - coord/2, rnd.Next(coord) - coord/2, rnd.Next(coord*2), i));
            }

            var Q = 100;
#else
            var scope = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var N = scope[0];
            var Q = scope[1];
            var circles = new List<Circle>(N);
            for (var i = 0; i < N; i++)
            {
                var pars = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                circles.Add(new Circle(pars[0], pars[1], pars[2], i));
            }
#endif    
            var mileStones = new SortedDictionary<int,int>();
            for (var i = 0; i < circles.Count-1; i++)
            {
                for (var j = i+1; j < circles.Count; j++)
                {
                    var pair = new CirclePair(circles[i], circles[j]);
                    var beginOfZone = pair.MinDist;
                    if (!mileStones.ContainsKey(beginOfZone))
                    {
                        mileStones[beginOfZone] = 1;
                    }
                    else
                    {
                        mileStones[beginOfZone] ++;
                    }

                    var endOfZone = pair.MaxDist+1;
                    if (!mileStones.ContainsKey(endOfZone))
                    {
                        mileStones[endOfZone] = -1;
                    }
                    else
                    {
                        mileStones[endOfZone] --;
                    }                
                }
            }
            // build
            var accumulator = 0;
            var indices = new List<int>(mileStones.Count);
            var values = new List<int>(mileStones.Count);
            foreach (var pair in mileStones)
            {
                accumulator += pair.Value;
                indices.Add(pair.Key);
                values.Add(accumulator);
            }
 
            var result = new StringBuilder(Q * 10);
            for (var i = 0; i < Q; i++)
            {
                var len = int.Parse(Console.ReadLine());
                var index = FindScan(len, indices);
                var res = 0;
                if (index >= 0)
                {
                    res = values[index];
                }

                result.AppendLine(res.ToString());   
            }
            Console.Write(result);
        }

        private static int FindScan(int len, IReadOnlyList<int> counts)
        {
            var end = counts.Count - 1;

            if (len > counts[end] || len <counts[0])
            {
                return -1;
            }

            return subscan(len, counts, 0, end);
        }

        private static int subscan(int len, IReadOnlyList<int> pairs, int begin, int end)
        {
            while (true)
            {
                var mid = (begin + end) / 2;

                if (end - begin < 2)
                {
                    return pairs[end] > len ? begin : end;
                }

                if (pairs[mid] < len)
                {
                    begin = mid;
                }
                else
                {
                    end = mid;
                }
            }
        }
    }
}