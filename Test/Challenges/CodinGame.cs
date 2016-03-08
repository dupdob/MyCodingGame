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
class CodinGame
{
	static void Main(string[] args)
	{
		int firstInitInput = int.Parse(Console.ReadLine());
		int secondInitInput = int.Parse(Console.ReadLine());
		int thirdInitInput = int.Parse(Console.ReadLine());
		Console.Error.WriteLine(firstInitInput);
		Console.Error.WriteLine(secondInitInput);
		Console.Error.WriteLine(thirdInitInput);

		// game loop
		while (true)
		{
			string firstInput = Console.ReadLine();
			string secondInput = Console.ReadLine();
			string thirdInput = Console.ReadLine();
			string fourthInput = Console.ReadLine();
			Console.Error.WriteLine(firstInput);
			Console.Error.WriteLine(secondInput);
			Console.Error.WriteLine(thirdInput);
			Console.Error.WriteLine(fourthInput);
			for (int i = 0; i < thirdInitInput; i++)
			{
				string[] inputs = Console.ReadLine().Split(' ');
				int fifthInput = int.Parse(inputs[0]);
				int sixthInput = int.Parse(inputs[1]);
				Console.Error.Write("#{0}={1}.{2},", i, fifthInput, sixthInput );
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");

			Console.WriteLine("A");
		}
	}
}