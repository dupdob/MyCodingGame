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
class SolutionHorses
{
	static void Main2(string[] args)
	{
		int N = int.Parse(Console.ReadLine());
		var list = new List<int> ();
		for (int i = 0; i < N; i++)
		{
			int Pi = int.Parse(Console.ReadLine());
			list.Add (Pi);
		}
		list.Sort ();

		var minDist = int.MaxValue;
		for (int i = 0; i < N - 1; i++) {
			var curDist = list.ElementAt (i + 1) - list.ElementAt (i);

			minDist = Math.Min (minDist, curDist);
		}

		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");

		Console.WriteLine(minDist);
	}
}