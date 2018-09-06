using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * https://www.codingame.com/ide/puzzle/xml-mdf-2016
 **/
namespace CodingGame.Facile
{
    static class Xmlmdf
    {
        private static void MainXml(string[] args)
        {
            var sequence = Console.ReadLine();
            var depth = 1d;
            var dico = new Dictionary<char, double>();
            var skip = false;
            foreach (var car in sequence)
            {
                if (car == '-')
                {
                    depth--;
                    skip = true;
                    continue;
                }

                if (skip)
                {
                    skip = false;
                    continue;
                }
                
                if (!dico.ContainsKey(car))
                {
                    dico[car] = 0;
                }

                dico[car] += 1 / depth;
                depth++;
            }

            var maxWeight = 0.0;
            var best = ' ';
            foreach (var pair in dico)
            {
                if (pair.Value > maxWeight)
                {
                    maxWeight = pair.Value;
                    best = pair.Key;
                }
            }
            Console.WriteLine(best);
        }
    }
}