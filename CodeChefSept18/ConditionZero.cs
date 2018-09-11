//#define TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CodeChefSept18
{
    public class ConditionZero
    {
        static void MainZero(string[] args)
        {
            var totalPop = 0L;
#if TEST
            var map = BuildSample();
            var N = map.Length;
            var M = map[0].Length;
            var K = new Random().Next(500, 1000);

            Console.WriteLine($"{N}x{M} in {K}");
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    totalPop += map[i][j];
                }
            }
#else
            var parameters = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
            var N = parameters[0];
            var M = parameters[1];
            var K = parameters[2];
            var map = new int[N][];
            for (var i = 0; i < N; i++)
            {
                map[i] = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                foreach (var xPop in map[i])
                {
                    totalPop += xPop;
                }
            }
#endif
            // try down, then left to right strategy
            IScanner cursor = new DownLToR(M, N);
            var answer = new int[N, M];
            var score = FillMap(map, answer, cursor, K, totalPop);
            // try left to right, then down
            cursor = new LToRDown(M, N);
            
            var answer2 = new int[N, M];
            var score2 = FillMap(map, answer2, cursor, K, totalPop);
            if (score > score2)
            {
                Log($"LToR is better ({score2} instead of {score})");
                answer = answer2;
                score = score2;
            }
            
            // sanity check
#if TEST           
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    var cell = answer[i, j];
                    if (cell == 0)
                    {
                        Console.WriteLine($"{j}:{i}: not filled");
                    }

                    if (i > 0 && cell == answer[i - 1, j])
                    {
                        continue;
                    }
                    if (j > 0 && cell == answer[i , j -1])
                    {
                        continue;
                    }
                    if (i < N-1 && cell == answer[i+1 , j])
                    {
                        continue;
                    }
                    if (j < M-1 && cell == answer[i , j + 1])
                    {
                        continue;
                    }
                    Console.WriteLine($"{j}:{i}: isolated");
                }
            }
            Console.WriteLine($"score: {score}, out of {totalPop} ({N}x{M}/{K} ~{totalPop/K}).");
#else
            for (var i = 0; i < N; i++)
            {
                var builder = new StringBuilder(M * 4);
                for (var j = 0; j < M; j++)
                {
                    builder.Append(answer[i, j].ToString());
                    builder.Append(' ');
                }
                Console.WriteLine(builder.ToString());
            }
#endif
        }

        private static void Log(string text)
        {
#if TEST
            Console.WriteLine(text);
#endif
            
        }

        private static int ComputeScore(long[] zonePops)
        {
            var minPop = long.MaxValue;
            var maxPop = 0L;

            foreach (var pop in zonePops)
            {
                if (pop < minPop)
                {
                    minPop = pop;
                }

                if (pop > maxPop)
                {
                    maxPop = pop;
                }
            }

            var score = maxPop - minPop;
            return (int)score;
        }

        private static int FillMap(IReadOnlyList<int[]> map, int[,] answer, IScanner cursor, int k, long totalPop)
        {
            var zonePops = new long[k];
 //           var totalError = 0L;
            var zoneId = 0;
            var targetPop = totalPop / k;
            var cumulatedPopulation = 0L;
            do
            {
                var thisZone = map[cursor.Y][cursor.X];
                var total = cumulatedPopulation - targetPop;
                if (zonePops[zoneId] > 0 && total + thisZone > 0 && Math.Abs(total + thisZone) > Math.Abs(total))
                {
                    if (zoneId < k - 1)
                        zoneId++;
                    targetPop = totalPop * (zoneId + 1) / k;
                }

                answer[cursor.Y, cursor.X] = zoneId + 1;
                cumulatedPopulation += thisZone;
                zonePops[zoneId] += thisZone;
            } while (cursor.Next());

            if (zoneId < k - 1)
            {
                Log($"Failed: {zoneId+1} instead of {k}");
 
                return int.MaxValue;
            }

            return ComputeScore(zonePops);
        }


        private static int[][] BuildSample()
        {
            var rdn = new Random();
            var N = rdn.Next(500, 1000);
            var M = rdn.Next(500, 1000);
            var result = new int[N][];
            for (var i = 0; i < N; i++)
            {
                result[i] = new int[M];
                for (int j = 0; j < M; j++)
                {
                    result[i][j] = rdn.Next(10000000);
                }
            }

            return result;
        }
        
        private interface IScanner
        {
            int X { get; }
            int Y { get; }
            bool Next();
        }

        
        class DownLToR : IScanner
        {
            public int X { get; private set; }
            public int Y{ get; private set; }

            private bool down = true;
            private readonly int width;
            private readonly int height;

            public DownLToR(int width, int height)
            {
                this.width = width;
                this.height = height;
            }

            public bool Next()
            {
                if (down)
                {
                    if (Y == height -1)
                    {
                        X++;
                        down = false;
                        return X < width;
                    }

                    Y++;
                    return true;
                }

                if (Y == 0)
                {
                    X++;
                    down = true;
                    return X < width;
                }

                Y--;
                return true;
            }
        }

        class LToRDown : IScanner
        {
            public int X { get; private set; }
            public int Y{ get; private set; }

            private bool right = true;
            private readonly int width;
            private readonly int height;

            public LToRDown(int width, int height)
            {
                this.width = width;
                this.height = height;
            }

            public bool Next()
            {
                if (right)
                {
                    if (X == width -1)
                    {
                        Y++;
                        right = false;
                        return Y < height;
                    }

                    X++;
                    return true;
                }

                if (X == 0)
                {
                    Y++;
                    right = true;
                    return Y < height;
                }

                X--;
                return true;
            }
        }
    }
}