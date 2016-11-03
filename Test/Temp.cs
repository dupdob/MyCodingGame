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
class Solution
{
    static void MainTemp(string[] args)
    {
		Console.WriteLine(Kariakoo.GetPositionAt (0));
		Console.WriteLine(Kariakoo.GetPositionAt (1));
		Console.WriteLine(Kariakoo.GetPositionAt (2));
		Console.WriteLine(Kariakoo.GetPositionAt (100000));
		Console.WriteLine(Kariakoo.GetPositionAt (3));
        string S = Console.ReadLine();

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

	
    }
}


public class Kariakoo
{
	public static int GetPositionAt(int n)
	{
		int count = 0;
		int lastStep = 0;
		int penultimate = 0;

		for (int i = 0; i <= n; i++) {
			int steps;
			if (i == 0)
				steps = 0;
			else if (i == 1)
				steps = 1;
			else if (i == 2)
				steps = -2;
			else
				steps = lastStep - penultimate;

			count += steps;
			penultimate = lastStep;
			lastStep = steps;
		}
		return count;
	}
}