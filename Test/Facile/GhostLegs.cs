using System;
using System.Collections.Generic;
using System.Linq;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * https://www.codingame.com/training/easy/ghost-legs
 **/
namespace CodingGame.Facile
{
    internal static class GhostLegs
    {
        private static void MainLegs(string[] args)
        {
            var inputs = Console.ReadLine().Split(' ');
            var W = int.Parse(inputs[0]);
            var H = int.Parse(inputs[1]);
            var lines = Console.ReadLine().Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
            var order = lines.ToArray();
            for (var i = 1; i < H-1; i++)
            {
                var line = Console.ReadLine();
                var links = line.Split('|');
                for (var j = 1; j < links.Length; j++)
                {
                    if (links[j] == "--")
                    {
                        var firstCol = lines[j];
                        lines[j] = lines[j - 1];
                        lines[j - 1] = firstCol;
                    }
                }
            }

            var ends = Console.ReadLine().Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
            var dico = new Dictionary<string, string>();
            for (var i = 0; i < ends.Length; i++)
            {
                dico[lines[i]] = ends[i];
            }

            foreach (var col in order)
            {
                Console.WriteLine($"{col}{dico[col]}");
            }
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

        }
    }
}