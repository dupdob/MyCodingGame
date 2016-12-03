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

    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int nbFloors = int.Parse(inputs[0]);
        int up = int.Parse(inputs[1]);
        int down = int.Parse(inputs[2]);
        int start = int.Parse(inputs[3]);
        int end = int.Parse(inputs[4]);
        int gap = end - start;

        int factor = PGCD(up, down);
        Console.Error.WriteLine("PGCD {0}", factor);
        up /= factor;
        down /= factor;
        if (gap%factor != 0)
            Console.WriteLine("IMPOSSIBLE");
        else
        {
            gap /= factor;
            Console.Error.WriteLine("gap {0}", gap);
            var click = 0;
            while (gap != 0)
            {
                var actualFloor = (end - gap) * factor;
                if ((actualFloor+up*factor)>nbFloors)
                {
                    if ((actualFloor - down*factor) < 0)
                    {
                        Console.WriteLine("IMPOSSIBLE");
                        return;
                    }
                    gap += down;
                    click++;
                }
                else if ((actualFloor - down*factor) < 0)
                {
                    gap -= up;
                    click++;
                }
                else if (gap > 0 )
                {
                    gap -= up;
                    click++;
                }
                else
                {
                    gap += down;
                    click++;
                }
                if (actualFloor > nbFloors || (actualFloor<0))
                {
                    Console.WriteLine("IMPOSSIBLE");
                    return;
                }

            }
            Console.WriteLine(click);
        }
    }
}