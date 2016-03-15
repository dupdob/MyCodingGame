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
 B : bottom
 C : move top (second coord -1)
 D : move down (second coord +1)
 E : move left (first coord -1)
 
 */
static class  CodinGame
{
	private enum Direction{L, U, R, D, N};
	private static char[,] gamearea;

	private class Coord
	{
		public int X;
		public int Y;

		public int MaxX;
		public int MaxY;

		public Coord Move(Direction dir)
		{
			Coord ret = new Coord ();
			ret.MaxX = this.MaxX;
			ret.MaxY = this.MaxY;

			switch (dir) {
			case Direction.D:
				ret.X = this.X;
				ret.Y = (this.Y + 1) % this.MaxY;
				break;
			case Direction.U:
				ret.X = this.X;
				ret.Y = (this.Y  -1 + this.MaxY) % this.MaxY;
				break;
			case Direction.L:
				ret.X = (this.X + this.MaxX - 1) % this.MaxX;
				ret.Y = this.Y;
				break;
			case Direction.R:
				ret.X = (this.X + 1) % this.MaxX;
				ret.Y = this.Y;
				break;
			case Direction.N:
			default:
				ret.X = this.X;
				ret.Y = this.Y;
				break;
			}
			return ret;
		}

	}

	static void MainCodinGame(string[] args)
	{
		int firstInitInput = int.Parse(Console.ReadLine());
		int secondInitInput = int.Parse(Console.ReadLine());
		int thirdInitInput = int.Parse(Console.ReadLine());
		Console.Error.WriteLine(firstInitInput);
		Console.Error.WriteLine(secondInitInput);
		Console.Error.WriteLine(thirdInitInput);

		Coord player = new Coord ();
		player.MaxX = firstInitInput;
		player.MaxY = secondInitInput;

		gamearea = new char[firstInitInput, secondInitInput];
		int [,] visited = new int[firstInitInput, secondInitInput];
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
			var ghosts = new List<Coord>(thirdInitInput);
			for (int i = 0; i < thirdInitInput; i++)
			{
				string[] inputs = Console.ReadLine().Split(' ');
				int fifthInput = int.Parse(inputs[0]);
				int sixthInput = int.Parse(inputs[1]);
///				Console.Error.Write("#{0}=X{1}.Y{2},", i, fifthInput, sixthInput );
				if (i == thirdInitInput - 1) {
					player.X = sixthInput % firstInitInput;
					player.Y = fifthInput % secondInitInput;
					player.MaxX = firstInitInput;
					player.MaxY = secondInitInput;
        			Console.Error.WriteLine ("Player @ {0}:{1}", fifthInput, sixthInput);
				} else {
					var ghost = new Coord ();
					ghost.X = sixthInput;
					ghost.Y = fifthInput;
					ghost.MaxX = firstInitInput;
					ghost.MaxY = secondInitInput;
					ghosts.Add (ghost);
       			    Console.Error.WriteLine ("Ghost @ {0}:{1}", fifthInput, sixthInput);
				}
			}
			// we discover some room
			visited [player.X, player.Y]++;
			SetRoom(player, '*');
			foreach (var ghost in ghosts)
				SetRoom (ghost, '@');
			SetRoom(player.Move(Direction.L), firstInput[0]);
			SetRoom(player.Move(Direction.D), secondInput[0]);
			SetRoom(player.Move(Direction.R), thirdInput[0]);
			SetRoom(player.Move(Direction.U), fourthInput[0]);

			DumpArea ();

			Direction dir = PathDiscovery (visited, player);
			SetRoom(player, '_');
			foreach (var ghost in ghosts)
				SetRoom (ghost, '_');
			string command;
			switch (dir) {
			case Direction.D:
				command = "A";
				break;
			case Direction.U:
				command = "E";
				break;
			case Direction.L:
				command = "C";
				break;
			case Direction.R:
				command = "D";
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
	static Direction PathDiscovery(int[,] visited, Coord c)
	{
		int score = int.MaxValue;
		Direction dir = Direction.N;
		Coord next = c.Move (Direction.R);
		if (GetRoom(next).IsEmpty() && !NextToGhost(next)) {
			var upScore = visited [next.X, next.Y];
			if (score > upScore) {
				score = upScore;
				dir = Direction.R;
			}
		}
		next = c.Move (Direction.U);
		if (GetRoom(next).IsEmpty() && !NextToGhost(next)) {
			var upScore = visited [next.X, next.Y];
			if (score > upScore) {
				score = upScore;
				dir = Direction.U;
			}
		}
		next = c.Move (Direction.L);
		if (GetRoom(next).IsEmpty() && !NextToGhost(next)) {
			var upScore = visited [next.X, next.Y];
			if (score > upScore) {
				score = upScore;
				dir = Direction.L;
			}
		}
		next = c.Move (Direction.D);
		if (GetRoom(next).IsEmpty() && !NextToGhost(next)) {
			var upScore = visited [next.X, next.Y];
			if (score > upScore) {
				score = upScore;
				dir = Direction.D;
			}
		}
		return dir;
	}

	static private void SetRoom(Coord coord, char car)
	{
		gamearea [coord.X, coord.Y] = car;
	}

	static private char GetRoom(Coord coord)
	{
		return gamearea [coord.X, coord.Y];
	}

	static private bool IsEmpty(this char car)
	{
		return car == '_';
	}

	static private bool NextToGhost(Coord pos)
	{
		if (GetRoom(pos.Move(Direction.U)) =='@' ||
			GetRoom(pos.Move(Direction.D)) =='@' ||
			GetRoom(pos.Move(Direction.L)) =='@' ||
			GetRoom(pos.Move(Direction.R)) =='@')
					return true;
		return false;
	}

	static void DumpArea()
	{
		for (int y = 0; y < gamearea.GetLength (1); y++) {
			for (int x = 0; x < gamearea.GetLength (0); x++) {
				Console.Error.Write (gamearea [x, y]);
			}
			Console.Error.WriteLine ();
		}
	}
}