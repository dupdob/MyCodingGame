using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

class Evalator
{
    static int PGCD(int x, int y)
    {
        for (;;)
        {
            if (x == 0)
                return y;
            if (y == 0)
                return x;
            if (x > y)
            {
                x -= y;
            }
            else
            {
                y -= x;
            }
        }
    }

    static void MainElevator(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int nbFloors = int.Parse(inputs[0]);
        int up = int.Parse(inputs[1]);
        int down = int.Parse(inputs[2]);
        int start = int.Parse(inputs[3]);
        int end = int.Parse(inputs[4]);
        int gap = end - start;

        int factor = PGCD(up, down);
        if (gap%factor != 0)
            Console.WriteLine("IMPOSSIBLE");
        else
        {
            Console.Error.WriteLine("gap {0}", gap);
            var click = 0;
            var level = start;
            while (level != end)
            {
                if (level+up>nbFloors)
                {
                    if (level - down< 1)
                    {
                        Console.WriteLine("IMPOSSIBLE");
                        return;
                    }
                    level -= down;
                    click++;
                }
                else if (level-down <1)
                {
                    level += up;
                    click++;
                }
                else if (level < end )
                {
                    level += up;
                    click++;
                }
                else
                {
                    level -= down;
                    click++;
                }
            }
            Console.WriteLine(click);
        }
    }
}