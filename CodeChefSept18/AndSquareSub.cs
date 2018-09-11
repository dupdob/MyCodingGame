using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChefSept18
{
    class AndSquareSub
    {
        struct Entry
        {
            public int mask;
            public int counter;
        }
        static void MainSquare(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            var squares = new HashSet<int>();
            for (int i = 0; i < 32768; i++)
            {
                squares.Add(i * i);
            }
            for (var i = 0; i < testCases; i++)
            {
                var parameters = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                var integerCount = parameters[0];
                var queries = parameters[1];

                var cache = new Entry[integerCount][];
                var cacheDepths = new int[integerCount];
                var integers = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                for (var j = 0; j < queries; j++)
                {
                    var subMarkers = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                    var left = subMarkers[0];
                    var right = subMarkers[1];
                    var counter = 0;
                    for (var l = left - 1; l < right; l++)
                    {
                        var start = l;
                        var mask = int.MaxValue;
                        var localCache = cache[l];
                        var localCounter = 0;
                        if (localCache != null)
                        {
                            // how far do we have cached values
                            if (cacheDepths[l] >= right)
                            {
                                // everything is in cache
                                counter += localCache[right-1].counter;
                                continue;
                            }

                            start = cacheDepths[l];
                            mask = localCache[start].mask;
                            localCounter = localCache[start].counter;
                            start++;
                        }
                        else
                        {
                            cache[l] = new Entry[integerCount];
                            localCache = cache[l];
                        }
                        for (var r = start; r < right; r++)
                        {
                            mask &= integers[r];
                            localCache[r].mask = mask;
                            if (mask < 2)
                            {
                                // if it is 1 or 0, we can only have perfect squares
                                localCache[r].counter = localCounter+1;
                                localCounter += right - r ;
                                cacheDepths[l] = r;
                                break;
                            }
                            if (squares.Contains(mask))
                            {
                                localCounter++;
                            }

                            localCache[r].counter = localCounter;
                            cacheDepths[l] = r;
                        }

                        counter += localCounter;

                    }
                    Console.WriteLine(counter);
                }
            }
        }
    }
}