//#define TEST
using System;
using System.Linq;
using System.Numerics;

namespace CodeChefOct18
{
    public static class BBricks
    {
        private const long modulo = 1000000007;

        private static void Main(string[] args)
        {
#if !TEST
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                var N = inputs[0];
                var K = inputs[1];

                var result = P(N, K);
                Console.WriteLine(result);
            }
#else

            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                var N = inputs[0];
                var K = inputs[1];

                var result = P(N, K);
                var refP = RefP(N, K);
                if (refP != result)
                {
                    Console.WriteLine($"Error for {N} {K} {result} expected {refP}");
                }
                else
                {
                    Console.WriteLine(result);
                }
            }
#endif
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
            var nbPossibility = new BigInteger(0);
            var baseCombo = 2L;
            var combo1 = ComboFast(2, blankRow);
            var combo2 = ComboFast(1, K - 1);
            var pattern = (long)((combo1 * combo2)%modulo);
            nbPossibility += pattern*baseCombo;
            baseCombo = baseCombo*2 % modulo;
            for (var j = 2; j <= K; j++)
            {
                // next pattern
                combo1 *= (blankRow - j + 2);
                combo1 /= j;
                combo2 *= (K - j + 1);
                combo2 /= j-1;
                pattern = (long)((combo1 * combo2)%modulo);
                nbPossibility += pattern*baseCombo;
                baseCombo = baseCombo*2 % modulo;
            }

            return (int)(nbPossibility % modulo);
        }
 
        private static BigInteger ComboFast(int slots, int tokens)
        {
            var max = slots + tokens - 1;
            var result = new BigInteger(1);

            var div = 1;

            for (var i = tokens + 1; i <= max; i++)
            {
                result *= i;
                if (div <= slots - 1)
                {
                    result /= div;
                    div++;
                }
            }

            return result;
        }
        private static long RefP(int N, int K)
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
            var nbPossibility = new BigInteger(0);
            var baseCombo = new BigInteger(2);
            for (var j = 1; j <= K; j++)
            {
                var pattern = (ComboBigI(j + 1, blankRow - j+1) * ComboBigI(j , K - j));
                nbPossibility += pattern*baseCombo;
                baseCombo = baseCombo*2;
            }

            return (int)(nbPossibility % modulo);
        }

        private static BigInteger ComboBigI(int slots, int tokens)
        {
            var max = slots + tokens - 1;
            var div1 = tokens;
            var div2 = slots - 1;
            var result = new BigInteger(1);

            var div = 1;

            for (var i = div1+1; i <= max; i++)
            {
                result *= i;
                if (div <= div2)
                {
                    result /= div;
                    div++;
                }
            }

            while (div <= div2)
            {
                result /= div;
                div++;
            }
            return result;
        }
 
    }
}