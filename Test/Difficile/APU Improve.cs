
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class PlayerAPUImproved
{
	static void Main2(string[] args)
	{
		int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
		int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
		var map = new List<string>(height);
		for (int i = 0; i < height; i++)
		{
			string line = Console.ReadLine(); // width characters, each either 0 or .
			map.Add(line);
		}
		for (int i = 0; i < height; i++)
		{
			Console.Error.Write("Parsing line {0} ", i);
			for (int j = 0; j < width;j++)
			{
				Console.Error.Write("col {0} ", j);
				if (map[i][j] == '.')
				{
					continue;
				}
				Console.Write("{0} {1} ", j, i);

				int rightNeghbour = (j == width-1) ? -1 : map[i].IndexOf("0", j+1);
				if (rightNeghbour > -1)
				{
					Console.Write("{0} {1} ", rightNeghbour, i);                    
				}
				else
				{
					Console.Write("{0} {1} ", -1, -1);

				}
				// look vertically
				int bottomNeighbour = -1;
				for (int k = i + 1; k < height; k++)
				{
					if (map[k][j] == '0')
					{
						bottomNeighbour = k;
						break;
					}
				}
				if (bottomNeighbour == -1)
				{
					Console.WriteLine("{0} {1}", -1, -1);
				}
				else
				{
					Console.WriteLine("{0} {1}", j, bottomNeighbour);
				}
			}
		}
		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");
		Console.WriteLine();
	}
}