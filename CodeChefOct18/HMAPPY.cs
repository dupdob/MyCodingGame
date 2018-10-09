using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChefOct18
{
    public static class Hmappy
    {
        class Entry
        {
            private long balloons;
            private readonly long requestedBalloons;
            private readonly long candies;
            private readonly int _id;

            
            public long Cost => balloons * candies;

            public long NbBalloonsToTake(long maxCost)
            {             
                if (candies == 0)
                {
                    // we can give all we have
                    return balloons;
                }

                return Math.Min(maxCost / candies, requestedBalloons);
            }

            public long RoundGap(long maxCost)
            {
                return NbBalloonsToTake(maxCost) * candies;
            }

            public Entry(long balloons, long candies, int id)
            {
                this.balloons = balloons;
                requestedBalloons = balloons;
                this.candies = candies;
                _id = id;
            }
        }
        
        static void MainHappy(string[] args)
        {
            
            var inputs = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
            var N = inputs[0];
            var M = inputs[1];

            var first = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
            var second = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
            var res= Resolve((int)N, first, second, M);    
            Console.WriteLine(res);
        }

        private static long Resolve(int N, IReadOnlyList<long> first, IReadOnlyList<long> second, long M)
        {
            var candies = new List<Entry>(N);
            var neededBalloons = 0L;
            for (var i = 0; i < N; i++)
            {
                if (first[i]!=0 && second[i]!=0)
                {
                    candies.Add(new Entry(first[i], second[i], i));
                    neededBalloons += first[i];
                }
            }

            var res = 0L;
            if (neededBalloons > M)
            {
                res = TakingBalloons(neededBalloons, M, candies);
            }

            return res;
        }

        private static long TakingBalloons(long neededBallons, long M, List<Entry> candies)
        {
            var missingBalloons = neededBallons - M;
            var fullCost = 0.0;
            foreach (var candy in candies)
            {
                fullCost += candy.Cost;
            }

            var minGap = 0L;
            var maxGap = long.MaxValue;
            var d = fullCost/candies.Count * missingBalloons / neededBallons;
            var gap = (long)d;
            // now we remove balloons
            for(;;)
            {
                var ret = Compare(candies, missingBalloons, gap);
                if (ret == 0)
                {
                    break;
                }

                if (maxGap - minGap == 1)
                {
                    // there is no perfect match, we use higher value
                    gap = maxGap;
                    break;
                }

                if (ret < 0)
                {
                    maxGap = gap;
                    gap = (maxGap + minGap) / 2;
                }
                else
                {
                    minGap = gap;
                    if (maxGap == long.MaxValue)
                    {
                        gap = 2 * gap;
                    }
                    else
                    {
                        gap = (maxGap + minGap) / 2;
                    }
                }
            }

            var finalGap = long.MinValue;
            foreach (var candy in candies)
            {
                var next = candy.RoundGap(gap);
                finalGap = Math.Max(finalGap, next);
            }

            return finalGap;
        }

        private static int Compare(List<Entry> candies, long missingBalloons, long maxGap)
        {

            foreach (var candy in candies)
            {
                var nbBalloonsToTake = candy.NbBalloonsToTake(maxGap);
                missingBalloons -= nbBalloonsToTake;

                if (missingBalloons < 0)
                {
                    return -1;
                }
            }

            return missingBalloons == 0 ? 0 : 1;
        }
    }
}