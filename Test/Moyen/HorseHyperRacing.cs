using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * https://www.codingame.com/ide/puzzle/horse-hyperracing-hyperduals
 **/

class HorseHyperRacing
{
    static void MainHorse(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int N = int.Parse(inputs[0]);
        int M = int.Parse(inputs[1]);
        int X = int.Parse(inputs[2]);
        for (int i = 0; i < N; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int V = int.Parse(inputs[0]);
            int E = int.Parse(inputs[1]);
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine("42");
    }
}