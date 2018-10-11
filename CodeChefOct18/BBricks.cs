using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;

namespace CodeChefOct18
{
    public class BBricks
    {
        private const long modulo = 1000000007;
        static void Main(string[] args)
        {
            var test = Combination(2, 6);
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                var N = inputs[0];
                var K = inputs[1];

                var result = P(N, K);
                Console.WriteLine(result);
            }
        }

        private static long P(int N, int K)
        {

            if (N <= 0 || N <K)
            {
                return 0;
            }

            if (K == 0)
            {
                return 1;
            }

            if (N == K)
            {
                return 2;
            }

            var blankRow = N - K;
            var nbPossibility = 0L;
            var baseCombo = 2L;
            for (var j = 1; j <= K; j++)
            {
                var pattern = (Combination(j + 1, blankRow - j+1) * Combination(j , K - j)) % modulo;
                nbPossibility += pattern*baseCombo;
                baseCombo = (baseCombo*2)% modulo;
                nbPossibility %= modulo;
            }

            return nbPossibility;
        }

        private static long Combination(int slots, int blanks)
        {
            if (blanks == 0)
            {
                return 1;
            }
            if (blanks < 0)
            {
                return 0;
            }
            var start = 1L;
            var sub = 1;
            for (var j = blanks+1; j < slots + blanks; j++)
            {
                start *= j;
                if (sub < slots)
                {
                    start /= sub++;
                }

                start %= modulo;
            }

            while (sub < slots)
            {
                start /= sub++;
            }

            return start;
        }
        
    }
}