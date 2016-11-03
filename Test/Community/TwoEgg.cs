using System;
using System.Collections.Generic;

/**
A building has N floors. One of the floors is the highest floor an egg can be dropped from without breaking.
If an egg is dropped from above that floor, it will break. If it is dropped from that floor or below, it will be completely undamaged and you can drop the egg again.

You are given two identical eggs, find the minimal number of drops it will take in the worst case to find out the highest floor from which an egg will not break.
 **/
class TwoEggs
{
    static Dictionary<int, int> cache = new Dictionary<int, int>(1000);
    static void MainEggs(string[] args)
    {
        int N = int.Parse(Console.ReadLine());

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        Console.WriteLine(NeededTrialsWithTwoEggs(N));
    }

    static int NeededTrialsWithTwoEggs(int nbFloors)
    {
        int trials = 0;
        for (int i = 0; i < nbFloors; i++)
        {
            trials += i;
            if (trials >= nbFloors)
                return i;
        }
        return 0;

    }

}