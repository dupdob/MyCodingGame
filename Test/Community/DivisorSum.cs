using System;

namespace CodingGame.Community
{
    class DivisorSum
    {
        static void MainDivisor()
        {
            var n = int.Parse(Console.ReadLine());
            var sum = 0L;
            for (var i = 1L; i <= n; i++)
                sum += (n / i) * i;
            Console.WriteLine(sum);
        }
    }
}
