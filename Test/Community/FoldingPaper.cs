using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
A sheet of paper is folded several times. The goal is to determine how many layers of paper are visible from one side of the obtained folding.

Folding motions are:
R for Right: take the right side and fold it on the left side.
L for Left: take the left side and fold it on the right side.
U for Up: take the high side and fold it on the low side.
D for Down: take the low side and fold it on the high side.
 **/
class FoldingPaper
{
    static void MainFolding(string[] args)
    {
        string order = Console.ReadLine();
        string side = Console.ReadLine();
        var dicos = new Dictionary<char, int>(4);
        dicos['L'] = 1;
        dicos['R'] = 1;
        dicos['U'] = 1;
        dicos['D'] = 1;
        foreach (var car in order)
        {
            switch (car)
            {
                case 'L':
                    dicos['R'] += dicos[car];
                    dicos['D'] *=2;
                    dicos['U'] *=2;
                    break;
                case 'R':
                    dicos['L'] += dicos[car];
                    dicos['D'] *= 2;
                    dicos['U'] *= 2;
                    break;
                case 'U':
                    dicos['D'] += dicos[car];
                    dicos['L'] *= 2;
                    dicos['R'] *= 2;
                    break;
                case 'D':
                    dicos['U'] += dicos[car];
                    dicos['L'] *= 2;
                    dicos['R'] *= 2;
                    break;
            }
            dicos[car] = 1;
        }
            // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(dicos[side[0]]);
    }
}