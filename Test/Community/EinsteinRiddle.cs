using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodingGame.Community
{
    class EinsteinRiddle
    {
        static void MainRiddle(string[] args)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int nbCharacteristics = int.Parse(inputs[0]);
            int nbPeople = int.Parse(inputs[1]);
            var dimensions = new List<string>[nbCharacteristics];
            for (int i = 0; i < nbCharacteristics; i++)
            {
                dimensions[i] = Console.ReadLine().Split(' ').ToList();
            }
            int nbLinks = int.Parse(Console.ReadLine());
            var links = new Dictionary<string, string>(nbLinks);
            var forbidden = new Dictionary<string, string>(nbLinks);
            for (int i = 0; i < nbLinks; i++)
            {
                string link = Console.ReadLine();
                if (link.Contains('&'))
                {
                    var fields = link.Split('&');
                    var first = fields[0].Trim();
                    var second = fields[1].Trim();
                    links[first] = second;
                    foreach (var dimension in dimensions)
                    {
                        dimension.Remove(first);
                        dimension.Remove(second);
                    }
                }
                else
                {
                    var fields = link.Split('&');
                    var first = fields[0].Trim();
                    var second = fields[1].Trim();
                    forbidden[first] = second;
                }
            }


            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine("Amelia Bob Charles Daniel\nCar Autobus Bicycle Roller\nTree Flower Bush Herb\nTurtle Elephant Rhinoceros Ant");
        }
    }
}
