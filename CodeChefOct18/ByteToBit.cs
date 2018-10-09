using System;

namespace CodeChefOct18
{
    public class ByteToBit
    {
        static void MainBit(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var time = int.Parse(Console.ReadLine()) -1;
                var cycles = time / 26;
                var inCycle = time % 26;
                var count = 1L << cycles;
                if (inCycle >= 10)
                {
                    Console.WriteLine("0 0 {0}", count);
                }
                else if (inCycle >= 2)
                {
                    Console.WriteLine("0 {0} 0", count);
                }
                else
                {
                    Console.WriteLine("{0} 0 0", count);
                }
            }
        }
    }
}