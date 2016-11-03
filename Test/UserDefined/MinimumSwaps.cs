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
class MinimumSwaps
{
	static void MainSwaps(string[] args)
	{
		int n = int.Parse(Console.ReadLine());
		string[] inputs = Console.ReadLine().Split(' ');
		int zeros = 0;
		int ones = 0;
		// lazily count
		for (int i = 0; i < n; i++)
		{   
			int x = int.Parse(inputs[i]);
			if (x == 1)
			{
				ones++;
			}
		}
		Console.Error.WriteLine ("{0} ones", ones);
		if (ones >= 1) {
			
			for (int i = 0; i < n; i++) {
				int x = int.Parse (inputs [i]);
				if (x == 0) {
					zeros++;
				} else {
					ones--;
				}

				if (zeros >= ones) {
					break;
				}
			}
		}
		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");

		Console.WriteLine(zeros);
	}
}