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
class PlayerKirk
{

	enum Direction {Right, Down, Left, Up, None};

	class Coordinates
	{
		public int R;
		public int C;

		public Coordinates(int r, int c)
		{
			this.R = r;
			this.C = c;
		}

		public Coordinates Move(Direction dir)
		{
			switch (dir) {
			case Direction.Right:
				return new Coordinates (R, C + 1);
			case Direction.Down:
				return new Coordinates (R + 1, C);
			case Direction.Left:
				return new Coordinates (R, C - 1);
			case Direction.Up:
				return new Coordinates (R - 1, C);
			}
			return null;
		}

		public override bool Equals(object obj)
		{
			var other =  obj as Coordinates;
			return (other != null && other.R == this.R && other.C == this.C);
		}

		public override int GetHashCode()
		{
			return R * 10000 + C;
		}
	}

	class Plan
	{
		private char[,] room;

		public Coordinates Teleport;

		public Coordinates Control;

		public Plan(int R, int C)
		{
			room = new char[R, C];
			for(int i=0; i<room.GetLength(0); i++) {
				for (int j = 0; j < room.GetLength (1); j++) {
					room [i, j] = '?';
				}
			}
		}

		public bool IsAWall(Coordinates coord)
		{
			return room [coord.R, coord.C] == '#';
		}

		public bool IsUnknonw(Coordinates coord)
		{
			return room [coord.R, coord.C] == '?';
		}

		private Direction PathFromTo(Coordinates start, Coordinates to, List<Coordinates> path, ref int minLength)
		{
			if (path.Where((x) => x.Equals(start)).Any()|| path.Count == minLength) {
				return Direction.None;
			}

			if (start.Equals(to)) {
				Console.Error.WriteLine ("Found path to dest");
				minLength = path.Count;
				return Direction.Up;
			}
			Console.Error.Write("{0}.{1}, ", start.R, start.C);
			var updatedPath = new List<Coordinates> (path);
			updatedPath.Add (start);

			Direction result = Direction.None;

			foreach (Direction value in typeof(Direction).GetEnumValues()) {
				int length = minLength;
				var next = start.Move (value);
				if (next == null || IsAWall (next) || IsUnknonw(next)) {
					continue;
				}
				var tempresult = PathFromTo (next, to, updatedPath, ref length);
				if (tempresult != Direction.None && length<minLength) {
					Console.Error.WriteLine ("Found path going {0} (l:{1})", value, length);
					minLength = length;
					result = value;
				}
			}
			return result;
		}

		public Direction PathFromTo(Coordinates from, Coordinates to)
		{
			Console.Error.WriteLine ("Start Scan path to {0}, {1}", to.R, to.C);
			var path = new List<Coordinates> ();
			int length = int.MaxValue;
			var result = PathFromTo (from, to, path, ref length);
			Console.Error.WriteLine("Path length is {0}.", length);
			return result;
		}

		public Direction NextScan(Coordinates start)
		{
			foreach (Direction value in typeof(Direction).GetEnumValues()) {
				if (!IsAWall( start.Move (value)))
					return value;
			}
			return Direction.None;
		}

		public void Capture()
		{
			for(int i=0; i<room.GetLength(0); i++) {
				string ROW = Console.ReadLine(); // C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
				for (int j = 0; j < room.GetLength (1); j++) {
					var cell = ROW [j];
					Console.Error.Write (cell);
					room [i, j] = cell;
					switch (cell) {
					case 'T':
						this.Teleport = new Coordinates (i, j);
						break;
					case 'C':
						this.Control = new Coordinates (i, j);
						break;
					}
				}
				Console.Error.WriteLine ();
			}
				
		}
	}

	static void Main2(string[] args)
	{
		string[] inputs;
		inputs = Console.ReadLine().Split(' ');
		int R = int.Parse(inputs[0]); // number of rows.
		int C = int.Parse(inputs[1]); // number of columns.
		int A = int.Parse(inputs[2]); // number of rounds between the time the alarm countdown is activated and the time the alarm goes off.

		bool goToControl = true;
		var plan = new Plan (R, C);
		// game loop
		while (true)
		{
			inputs = Console.ReadLine().Split(' ');
			int KR = int.Parse(inputs[0]); // row where Kirk is located.
			int KC = int.Parse(inputs[1]);// column where Kirk is located.
			plan.Capture ();

			Direction next;
			Coordinates kirk = new Coordinates (KR, KC);
			if (plan.Control != null && kirk .Equals(plan.Control)) {
				Console.Error.WriteLine ("In control");
				goToControl = false;
			}
			if (goToControl) {
				if (plan.Control == null) {
					// keep scanning
					Console.Error.WriteLine ("Looking for control");
					next = plan.NextScan (kirk);
				} else {
					Console.Error.WriteLine ("Move to control");
					next = plan.PathFromTo (kirk, plan.Control);
				}
			} else {
				Console.Error.WriteLine ("Moving back to teleport");
				next = plan.PathFromTo (kirk, plan.Teleport);
			}
			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");

			Console.WriteLine("{0}", next.ToString().ToUpper()); // Kirk's next move (UP DOWN LEFT or RIGHT).
		}
	}
}