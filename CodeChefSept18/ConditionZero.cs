using System;
using System.Linq;

namespace CodeChefSept18
{
    public class ConditionZero
    {
        static void MainAdventure(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            var parameters = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
            var N = parameters[0];
            var M = parameters[1];
            var K = parameters[2];
            var map = new int[N][];
            var totalPop = 0L;
            for (var i = 0; i < N; i++)
            {
                map[i] = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                foreach (var xPop in map[i])
                {
                    totalPop += xPop;
                }
            }
        }

    }
}