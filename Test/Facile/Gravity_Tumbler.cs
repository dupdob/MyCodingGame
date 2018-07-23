using System;
using System.Text;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace CodingGame.Facile
{
    class Gravity_Tumbler
    {
        static void MainGravity_Tumbler(string[] args)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int width = int.Parse(inputs[0]);
            int height = int.Parse(inputs[1]);
            int count = int.Parse(Console.ReadLine());
            
            var landscape = new string[height];
            for (int i = 0; i < height; i++)
            {
                landscape[i] = Console.ReadLine();
            }

            Console.Error.WriteLine("Input");
            Dump(landscape);
            for (int i = 0; i < count; i++)
            {
                var newLandscape = Rotate(landscape);
                Console.Error.WriteLine("Rotated");
                Dump(newLandscape);
                landscape = Gravity(newLandscape);
                Console.Error.WriteLine("Gravited");
                Dump(landscape);
            }

            Console.Error.WriteLine("Answer");
            foreach (var s in landscape)
            {
                Console.WriteLine(s);
            }

        }

        private static string[] Rotate(string[] result)
        {
            var newLandscape = new string[result[0].Length];
            for (int j = 0; j < result[0].Length; j++)
            {
                var builder = new StringBuilder();
                for (int k = 0; k < result.Length; k++)
                {
                    builder.Append(result[k][j]);
                }

                newLandscape[j] = builder.ToString();
            }

            return newLandscape;
        }

        private static void Dump(string[] landscape)
        {
            Console.Error.WriteLine("Landscape");
            foreach (var s in landscape)
            {
                Console.Error.WriteLine(s);
            }
        }
        
        private static string[] Gravity(string[] landscape)
        {
            var depth = new int[landscape[0].Length];
            for (var i = 0; i < landscape.Length; i++)
            {
                var line = landscape[i];
                for (var index = 0; index < depth.Length; index++)
                {
                    var car = line[index];
                    if (car == '#')
                        depth[index]++;
                }
            }

            foreach (var i in depth)
            {
                Console.Error.Write("{0}-", i);
            }

            Console.Error.WriteLine();
            var result = new string[landscape.Length];
            for (int i = 0; i < result.Length; i++)
            {
                StringBuilder builder = new StringBuilder();
                for (var index = 0; index < depth.Length; index++)
                {
                    builder.Append((i>=result.Length-depth[index]) ? '#' : '.');
                }

                result[i] = builder.ToString();
            }

            return result;
        }
    }
}