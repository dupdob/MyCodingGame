using System;
using System.Linq;

namespace CodeChefOct18
{
    static class ChefAndServes
    {
        static void MainServes(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

                var totalPoints = inputs[0] + inputs[1];

                if ((totalPoints / inputs[2] % 2) == 0)
                {
                    Console.WriteLine("CHEF");
                }
                else
                {
                    Console.WriteLine("COOK");
                }
            }
        }
    }
}