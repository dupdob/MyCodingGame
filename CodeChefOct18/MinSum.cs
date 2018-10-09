//#define TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CodeChefOct18
{
    public class MinSum
    {
        
        static void MainSum(string[] args)
        {
#if TEST
            for (int i = 1; i < 100; i++)
            {
                for (int j = 1; j < 100; j++)
                {
                    string algo;
                    long N = i; 
                    var findNumberOfAdd = FindResut(j, ref N);
                    if (!BruteForce(i, j, (int) N, findNumberOfAdd, out algo))
                    {
                        Console.WriteLine($"Failure with {i} {j}: {algo} vs {findNumberOfAdd}");
                    }
                }
            }
 #else
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var inputs = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
                var N = inputs[0];
                var D = inputs[1];

//                var findNumberOfAdd = FindResut(D, ref N);
                var findNumberOfAdd = BruteForce(N, D, out N);

                Console.WriteLine($"{N} {findNumberOfAdd}");
            }
    #endif
        }

        private static int FindResut(long D, ref long N)
        {
            var findNumberOfAdd = FindNumberOfAdd(N, D);
            N += findNumberOfAdd * D;
            var nbDigits = 0;
            while (N > 9)
            {
                nbDigits++;
                N = AddDigits(N);
            }
            // number of add is a constant, but we can try to reduce number of digitsum
            
            return findNumberOfAdd + nbDigits;
        }

#if TEST
        private static bool BruteForce(long N, long D, int expectedN, int expectedDepth, out string algo)
        {
            var results = new Dictionary<int, int>(10);
            var algos = new Dictionary<int, string>(10);
            RecurseForceBrute(N, D, results, 0, string.Empty, algos);
            for (int i = 1; i < 10; i++)
            {
                if (results.ContainsKey(i))
                {
                    if (i != expectedN || results[i] != expectedDepth)
                    {
                        algo = algos[i];
                        return false;
                    }

                    algo = string.Empty;
                    return true;
                }
            }

            algo = "not solved";   
            return false;
        }
    #endif

        private static int BruteForce(long N, long D, out long min)
        {
            var results = new Dictionary<int, int>(10);
            var algos = new Dictionary<int, string>(10);
            RecurseForceBrute(N, D, results, 0, string.Empty, algos);
            for (int i = 1; i < 10; i++)
            {
                if (results.ContainsKey(i))
                {
                    min = i;
                    return results[i];

                }
            }

            min = 0;
            return 0;
        }

        private static void RecurseForceBrute(long N, long D, Dictionary<int, int> results, int depth, string path, IDictionary<int, string> algos)
        {
            if (depth > 15)
            {
                return;
            }

            if (N < 10)
            {
                var digit = (int) N;
                if (!results.ContainsKey(digit) || results[digit] > depth)
                {
                    results[digit] = depth;
                    algos[digit] = path;
                }
            }
            else
            {
                RecurseForceBrute(AddDigits(N), D, results, depth + 1, path+"S", algos);
            }
            RecurseForceBrute(N + D, D, results, depth + 1, path+"A", algos);
        }

        private static int FindNumberOfAdd(long N, long D)
        {
            var res = 0;
            var hit = new SortedDictionary<int, int>();
            D = D % 9;
            for (;;)
            {
                N = (N - 1) % 9 +1;
                if (hit.ContainsKey((int)N))
                {
                    // we have a loop
                    break;
                }

                hit[(int)N] = res;

                res++;
                N += D;
            }

            return hit.First().Value;
        }

        static long AddDigits(long val)
        {
            var res = 0L;
            while (val > 0)
            {
                res += val % 10;
                val /= 10;
            }
            return res;
        }
    }
}