using System;
using System.Linq;

namespace CodeChefSept18
{
    public class ChefAdventure
    {
        static void MainAdventure(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var parameters = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                var N = parameters[0]-1;
                var M = parameters[1]-1;
                var X = parameters[2];
                var Y = parameters[3];

                var success = Success(N, M, X, Y) || Success(N-1, M-1, X, Y);
                Console.WriteLine(success ? "Chefirnemo" : "Pofik");
            }
        }

        private static bool Success(int N, int M, int X, int Y)
        {
            var diffN = N % X;
            var diffM = M % Y;

            var success = diffM == diffN && diffM == 0 && N>= 0 && M>=0;
            return success;
        }
    }
}