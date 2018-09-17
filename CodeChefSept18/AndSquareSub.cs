//#define TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeChefSept18
{
    class AndSquareSub
    {
        private static Entry[][] _cache;
        private static int[] _cacheDepths;

        struct Entry
        {
            public int mask;
            public int counter;
        }
        
        static void Main(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            /*
            var squares = new HashSet<int>();
            for (var i = 0; i < 32768; i++)
            {
                squares.Add(i * i);
            }
            */
            var answer = new StringBuilder();
            for (var i = 0; i < testCases; i++)
            {
#if TEST
                var integerCount = 100000;
                var queries = 2;

                var integers = new int[integerCount];
                var rnd = new Random();
                for (var j = 0; j < integers.Length; j++)
                {
                    integers[j] = rnd.Next(1 << 30);
                }
#else
                var parameters = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                var integerCount = parameters[0];
                var queries = parameters[1];

                var integers = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

#endif
                var startTime = Environment.TickCount;
                _cache = new Entry[integerCount][];
                _cacheDepths = new int[integerCount];

                for (var j = 0; j < queries; j++)
                {
#if TEST
                    var left = j+1;
                    var right = integerCount;
        
#else
                    var subMarkers = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                    var left = subMarkers[0];
                    var right = subMarkers[1];
#endif
                    if (integerCount == 100000)
                    {
                        if (i > 0)
                        {
                            throw new Exception();
                        }
                    }
                    var counter = //ScanSquare(left, right, integers);
                    integerCount == 100000 ? AdvancedScan(left, right, integers) : ScanSquare(left, right, integers);
                    answer.AppendLine(counter.ToString());
                }
#if TEST
                Console.WriteLine(Environment.TickCount - startTime);
#endif
            }
            Console.Write(answer);
        }

        private static int AdvancedScan(int left, int right, int[] integers)
        {
            var result = 0;
            var counters = new Dictionary<int, int>();
            for (int i = left-1; i < right; i++)
            {
                var next = integers[i];
                var nextCounters = new Dictionary<int, int>(counters.Count);
                foreach (var pair in counters)
                {
                    var test = next & pair.Key;
                    if (nextCounters.ContainsKey(test))
                    {
                        nextCounters[test] += pair.Value;
                    }
                    else
                    {
                        nextCounters.Add(test, pair.Value);
                    }
                }
                if (nextCounters.ContainsKey(next))
                {
                    nextCounters[next]++;
                }
                else
                {
                    nextCounters.Add(next, 1);
                }
                counters = nextCounters;

                foreach (var pair in counters)
                {
                    if (IsPerfectSquare(pair.Key))
                    {
                        result += pair.Value;
                    }
                }
            }
        
            return result;
        }

        private static int ScanSquare(int left, int right, int[] integers)
        {
            int integerCount = integers.Length;
            var counter = 0;
            for (var l = left - 1; l < right; l++)
            {
                var start = l;
                var mask = int.MaxValue;
                var localCache = _cache[l];
                var localCounter = 0;
                if (localCache != null)
                {
                    // how far do we have cached values
                    if (_cacheDepths[l] >= right)
                    {
                        // everything is in cache
                        counter += localCache[right - 1].counter;
                        continue;
                    }

                    start = _cacheDepths[l];
                    mask = localCache[start].mask;
                    localCounter = localCache[start].counter;
                    start++;
                }
                else
                {
                    localCache = new Entry[integerCount];
                    _cache[l] = localCache;
                }

                for (var r = start; r < right; r++)
                {
                    mask &= integers[r];
                    localCache[r].mask = mask;
                    if (mask < 2)
                    {
                        // if it is 1 or 0, we can only have perfect squares
                        localCache[r].counter = localCounter + 1;
                        localCounter += right - r;
                        _cacheDepths[l] = r;
                        break;
                    }

                    if (IsPerfectSquare(mask))
                    {
                        localCounter++;
                    }

                    localCache[r].counter = localCounter;
                    _cacheDepths[l] = r;
                }

                counter += localCounter;
            }

            return counter;
        }

        static bool IsPerfectSquare(int input)
        {
            if (input < 2)
                return true;
            long closestRoot = (long) Math.Sqrt(input);
            return input == closestRoot * closestRoot;
        }
    }
}