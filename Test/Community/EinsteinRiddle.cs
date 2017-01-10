using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingGame.Community
{
    class EinsteinRiddle
    {
        static void Main(string[] args)
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
            for (int i = 0; i < nbLinks; i++)
            {
                string link = Console.ReadLine();
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine("Amelia Bob Charles Daniel\nCar Autobus Bicycle Roller\nTree Flower Bush Herb\nTurtle Elephant Rhinoceros Ant");
        }
    }
}
