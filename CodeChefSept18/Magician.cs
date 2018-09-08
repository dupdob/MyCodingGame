using System;
using System.Linq;

namespace CodeChefSept18
{
    public class Magician
    {
        static void MainMagician(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var parameters = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                var N = parameters[0];
                var X = parameters[1];
                var S = parameters[2];
                for (int j = 0; j < S; j++)
                {
                    var swaps = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                    if (X == swaps[0])
                    {
                        X = swaps[1];
                    }
                    else if (X == swaps[1])
                    {
                        X = swaps[0];
                    }
                }
                Console.WriteLine(X);
            }
        }
    }
}