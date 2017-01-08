using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Gravity
{
    static void MainGravity(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int width = int.Parse(inputs[0]);
        int height = int.Parse(inputs[1]);
        var depth = new int[width];
        for (int i = 0; i < height; i++)
        {
            string line = Console.ReadLine();
            Console.Error.WriteLine(line);
            for (var index = 0; index < width; index++)
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
        for (int i = height; i > 0; i--)
        {
            for (var index = 0; index < width; index++)
            {
                Console.Write((depth[index] >= i) ? '#' : '.');
            }
            Console.WriteLine();
        }

    }
}