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

	static void Log(string fmt, params object[] obs)
	{
		Console.Error.Write (fmt, obs);
	}

	static void LogLine(string fmt, params object[] obs)
	{
		Console.Error.WriteLine (fmt, obs);
	}


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

		public bool Equals(Coordinates other)
		{
			return (other != null && other.R == this.R && other.C == this.C);
		}


		public override int GetHashCode()
		{
			return R * 10000 + C;
		}

		public override string ToString ()
		{
			return string.Format ("{0}:{1}", C, R);
		}
	}

	class Plan
	{
		private char[,] room;

		public Coordinates Teleport;

		public Coordinates Control;

		public int Countdown;

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

		public bool CanGoThrough(Coordinates coord)
		{
			return !IsAWall (coord) && !IsUnknonw (coord);
		}
			
		public Direction PathFromTo(Coordinates from, Coordinates to, out int len)
		{
			LogLine ("Start Scan path from {0} to {1}.", from, to);

			var distances = new Dictionary<Coordinates, int> (room.Length);
			var nodesToScan = new List<Coordinates> (room.Length);
			var previous = new Dictionary<Coordinates, Coordinates> (room.Length);

			for(int i=0; i<room.GetLength(0); i++) {
				for (int j = 0; j < room.GetLength (1); j++) {
					var cell = new Coordinates (i, j);
					if (CanGoThrough(cell))
						nodesToScan.Add (cell);
				}
			}
			distances [from] = 0;

			len = int.MaxValue;

			while (nodesToScan.Count > 0) {
				// find closest node to source
				Coordinates closest = null;
				var minLength = int.MaxValue;
				foreach (var node in nodesToScan) {
					int dist;
					if (distances.TryGetValue (node, out dist)) {
						if (dist < minLength) {
							minLength = dist;
							closest = node;
						}
					}
				}
				if (closest == null) {
					PlayerKirk.LogLine ("No path to target");
					return Direction.None;
				}

				PlayerKirk.LogLine("Closest Node : {0}", closest);
				nodesToScan.Remove (closest);
				if (closest.Equals (to)) {
					PlayerKirk.LogLine ("Found path to dest");
					break;
				}
				// scan neighbours
				foreach (Direction value in typeof(Direction).GetEnumValues()) {
					var neighbor = closest.Move (value);
					if (neighbor != null && CanGoThrough (neighbor)) {
						var newdist = minLength +1;
						int oldDist;
						if (!distances.TryGetValue(neighbor, out oldDist) || oldDist > newdist){
							distances[neighbor] = newdist;
							previous [neighbor] = closest;
						}
					}
				}
			}
			// find the path;
			var scan = to;
			Log ("Unwinding path from {0}, ", to);
			var result = Direction.None;
			while(true) {
				if (previous.ContainsKey (scan)) {
					var prev = previous [scan];
					if (prev.Equals(from)) {
						break;
					}
					scan = prev;
				} else {
					break;
				}
			}

			len = distances[to];

			if (scan != null) {
				LogLine ("Moving to {0}", scan);
				if (from.C == scan.C) {
					result = from.R > scan.R ? Direction.Up : Direction.Down;
				} else {
					result = from.C > scan.C ? Direction.Left : Direction.Right;
				}
			}
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

		private Direction MaximizeDiscovery(Coordinates start, List<Coordinates> path, ref int minLength)
		{
			if (path.Where((x) => x.Equals(start)).Any()|| path.Count == minLength) {
				return Direction.None;
			}

			if (IsUnknonw(start)) {
				Console.Error.WriteLine ("Found unknown cell at {0}", start.R, start.C);
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
				if (next == null || IsAWall (next)) {
					continue;
				}

				var tempresult = MaximizeDiscovery (next, updatedPath, ref length);
				if (tempresult != Direction.None && length<minLength) {
					Console.Error.WriteLine ("Found path going {0} (l:{1})", value, length);
					minLength = length;
					result = value;
				}
			}
			return result;
		}

		public Direction MaximizeDiscovery(Coordinates start)
		{
			// start looking for a unknown room
			Console.Error.WriteLine ("Start Scan for unknown.");
			var path = new List<Coordinates> ();
			int length = int.MaxValue;
			var result = MaximizeDiscovery (start, path, ref length);
			Console.Error.WriteLine("Path length is {0}.", length);
			return result;

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

	static void Main(string[] args)
	{
		string[] inputs;
		inputs = Console.ReadLine().Split(' ');
		int R = int.Parse(inputs[0]); // number of rows.
		int C = int.Parse(inputs[1]); // number of columns.
		int A = int.Parse(inputs[2]); // number of rounds between the time the alarm countdown is activated and the time the alarm goes off.

		bool goToControl = true;
		var plan = new Plan (R, C);
		plan.Countdown = A;
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
				next = Direction.None;
				if (plan.Control != null) {
					int len;
					next = plan.PathFromTo (kirk, plan.Control, out len);
					plan.PathFromTo (plan.Control, plan.Teleport, out len);
					if (len > plan.Countdown) {
						LogLine ("Looking for shorter path.");
						next = Direction.None;
					}
				}
				if (next == Direction.None) {
					LogLine ("Looking for control");
					next = plan.MaximizeDiscovery (kirk);
				} else {
					LogLine ("Move to control");
				}
			} else {
				int len;
				LogLine ("Moving back to teleport");
				next = plan.PathFromTo (kirk, plan.Teleport, out len);
			}
			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");

			Console.WriteLine("{0}", next.ToString().ToUpper()); // Kirk's next move (UP DOWN LEFT or RIGHT).
		}
	}
}