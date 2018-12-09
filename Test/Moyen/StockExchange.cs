using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class StockExchange
{
    static void MainStock(string[] args)
    {
        int n = int.Parse(Console.ReadLine());
        string[] inputs = Console.ReadLine().Split(' ');
        int maxloss = 0;
        int currentMax = 0;
        for (int i = 0; i < n; i++)
        {
            int v = int.Parse(inputs[i]);
            if (v > currentMax)
            {
                currentMax = v;
            }
            else if (v - currentMax < maxloss)
            {
                maxloss = v - currentMax;
            }
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(maxloss);
    }
}