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
 /*
 Note:
 Init vars (3 lines):
 Width ?
 Height ?
 NB mobile items (NB)
 Run Vars:
 4 chars: - means empty? # means blocking
 - Top room?
 - right room
 - Bottom room
 - Left room?
 NB coordonates pair
 - Last coordonates are player coord
 
 Actions:
 A : move right (first coord +1)
 B : move left (first coord -1)
 C : move top (second coord -1)
 D : move down (second coord +1)
 E : move left (first coord -1)
 
 */
static class  CodinGame
{
	private enum Direction{L, U, R, D, N};
	private class Coord
	{
		public int X;
		public int Y;
	}

	static void Main(string[] args)
	{
		int firstInitInput = int.Parse(Console.ReadLine());
		int secondInitInput = int.Parse(Console.ReadLine());
		int thirdInitInput = int.Parse(Console.ReadLine());
		Console.Error.WriteLine(firstInitInput);
		Console.Error.WriteLine(secondInitInput);
		Console.Error.WriteLine(thirdInitInput);

		Coord player = new Coord ();
		char[,] gamearea = new char[secondInitInput, firstInitInput];
		int [,] visited = new int[secondInitInput, firstInitInput];
		for (int x = 0; x < gamearea.GetLength (0); x++)
			for (int y = 0; y < gamearea.GetLength (1); y++) {
				gamearea [x, y] = ' ';
				visited [x, y] = 0;
			}
		// game loop
		while (true)
		{
			string firstInput = Console.ReadLine();
			string secondInput = Console.ReadLine();
			string thirdInput = Console.ReadLine();
			string fourthInput = Console.ReadLine();
//			Console.Error.WriteLine(firstInput);
//			Console.Error.WriteLine(secondInput);
//			Console.Error.WriteLine(thirdInput);
//			Console.Error.WriteLine(fourthInput);
			for (int i = 0; i < thirdInitInput; i++)
			{
				string[] inputs = Console.ReadLine().Split(' ');
				int fifthInput = int.Parse(inputs[0]);
				int sixthInput = int.Parse(inputs[1]);
///				Console.Error.Write("#{0}=X{1}.Y{2},", i, fifthInput, sixthInput );
				switch (i) {
				case 4:
					player.X = fifthInput;
					player.Y = sixthInput;
					break;
				}
			}
			Console.Error.WriteLine ("Player @ {0}:{1}", player.X, player.Y);
			// we discover some room
			gamearea[player.X, player.Y] = '*';
			visited [player.X, player.Y]++;
			gamearea[player.X, player.Y-1] = firstInput[0];
			gamearea[player.X+1, player.Y] = secondInput[0];
			gamearea[player.X, player.Y+1] = thirdInput[0];
			gamearea[player.X-1, player.Y] = fourthInput[0];

			DumpArea (gamearea);
			gamearea[player.X, player.Y] = '_';

			Direction dir = PathDiscovery (gamearea, visited, player);
			string command;
			switch (dir) {
			case Direction.D:
				command = "D";
				break;
			case Direction.U:
				command = "C";
				break;
			case Direction.L:
				command = "E";
				break;
			case Direction.R:
				command = "A";
				break;
			case Direction.N:
			default:
				command = "B";
				break;
			}
			Console.WriteLine (command);
		}
	}
	// basic pathdiscovery	
	static Direction PathDiscovery(char[,] area, int[,] visited, Coord c)
	{
		int score = int.MaxValue;
		Direction dir = Direction.N;
		if (area [c.X + 1, c.Y].IsEmpty()) {
			var upScore = visited [c.X + 1, c.Y];
			if (score > upScore) {
				score = upScore;
				dir = Direction.R;
			}
		}
		if (area [c.X , c.Y - 1].IsEmpty()) {
			var upScore = visited [c.X , c.Y-1];
			if (score > upScore) {
				score = upScore;
				dir = Direction.U;
			}
		}
		if (area [c.X - 1, c.Y].IsEmpty()) {
			var upScore = visited [c.X-1 , c.Y];
			if (score > upScore) {
				score = upScore;
				dir = Direction.L;
			}
		}
		if (area [c.X , c.Y + 1].IsEmpty()) {
			var upScore = visited [c.X , c.Y+1];
			if (score > upScore) {
				score = upScore;
				dir = Direction.D;
			}
		}
		return dir;
	}

	static private bool IsEmpty(this char car)
	{
		return car == '_';
	}

	static void DumpArea(char[,] area)
	{
		for (int x = 0; x < area.GetLength (1); x++) {
			for (int y = 0; y < area.GetLength (0); y++)
				Console.Error.Write (area [y, x]);
			Console.Error.WriteLine ();
		}
	}
}