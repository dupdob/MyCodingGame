using System;
using System.Linq;

namespace CodeChefSept18
{
    public class ConditionZero
    {
        static void MainZero(string[] args)
        {
            var parameters = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
            var N = parameters[0];
            var M = parameters[1];
            var K = parameters[2];
            var map = new int[N][];
            var answer = new int[N, M];
            var totalPop = 0L;
            for (var i = 0; i < N; i++)
            {
                map[i] = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                foreach (var xPop in map[i])
                {
                    totalPop += xPop;
                }
            }
            // ideal size of each
            var targetPop = totalPop / K;

            var cursor = new DownLToR(M, N);
            var totalError = 0L;
            var zoneId = 0;
            var zonePops = new long[K];
            do
            {
                var thisZone = map[cursor.Y][cursor.X];
                var total = zonePops[zoneId] - targetPop + totalError;
                if (total + thisZone > 0 && Math.Abs(total + thisZone) > Math.Abs(total))
                {
                    if (zoneId<K-1)
                        zoneId++;
                    totalError = total;
                }
                answer[cursor.Y, cursor.X] = zoneId+1;

                zonePops[zoneId] += thisZone;
            } while (cursor.Next());


            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Console.Write($"{answer[i,j]} ");
                }
                Console.WriteLine();
            }
        }

        class DownLToR
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

    }
}