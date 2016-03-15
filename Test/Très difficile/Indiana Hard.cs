using System;
using System.Linq;
using System.Collections.Generic;

/**
Support very hard
 **/
class PlayerIndy
{
	private enum Direction { Top, Left, Right, Down, Blocked };

	private enum Action
	{
		Wait,
		Left,
		Right,
		DoubleLeft
	};


	private struct Point{
		public int X;
		public int Y;
		public Direction Dir;

		public Point(int x, int y, Direction dir = Direction.Blocked)
		{
			X = x;
			Y = y;
			Dir = dir;
		}

		public Point Move(Direction dir)
		{
			switch (dir) {
			case Direction.Top:
				return new Point(X, Y -1, Convert(dir));
			case Direction.Down:
				return new Point (X, Y + 1, Convert(dir));
			case Direction.Left:
				return new Point (X - 1, Y, Convert(dir));
			case Direction.Right:
				return new Point (X + 1, Y, Convert(dir));
			default:
				return this;
			}
		}

		public void ParseString(string line)
		{
			var inputs = line.Split(' ');
			X = int.Parse(inputs[0]);
			Y = int.Parse(inputs[1]);
			var POS = inputs[2];
			switch (POS)
			{
			case "TOP":
			default:
				Dir = Direction.Top;
				break;
			case "DOWN":
				Dir = Direction.Down;
				break;
			case "LEFT":
				Dir = Direction.Left;
				break;
			case "RIGHT":
				Dir = Direction.Right;
				break;
			}
		}

		public override string ToString()
		{
			return string.Format("{0}:{1} ({2})", X, Y, Dir);
		}

		public override int GetHashCode ()
		{
			return X + 1000 * Y;
		}

		public static bool operator==(Point a, Point b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator!=(Point a, Point b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		public override bool Equals (object obj)
		{
			return obj is Point && (Point)obj == this;
		}
	}

	class GamePlan
	{
		private Dictionary<Point, int> floors = new Dictionary<Point, int>();
		private Point exit;

		public GamePlan(Dictionary<Point, int> fl, Point exit)
		{
			floors = fl;
			this.exit = exit;
		}

		private int GetRoom(Point point, Dictionary<Point, int> hypothesis)
		{
			if (hypothesis.ContainsKey (point))
				return hypothesis [point];
			int room;
			if (!floors.TryGetValue (point, out room))
				return 0;
			else
				return room;
		}

		// scan hypothesis
		public bool Scan (Point player, List<Point> rocks, Dictionary<Point, int> hypothesis)
		{
			// exit found?
			if (player == exit)
				return true;
			// player-rock collision?
			if (rocks.Contains (player))
				return false;
			// scan for rock - rock collision
			var newRocks = new List<Point>(rocks.Count);
			var rockChoices = new Dictionary<Point, Dictionary<Action, Direction>> ();
			foreach (var rock in rocks) {
				if (newRocks.Contains (rock))
					// rock-rock collision
					newRocks.Remove (rock);
				else {
					newRocks.Add (rock);
					rockChoices [rock] = BlockNextWithRotations (GetRoom (rock, hypothesis), rock.Dir);
				}
			}

			var playerChoices = IdentifyNextWithRotations (GetRoom(player, hypothesis), player.Dir);
			return true;
		}
	}

	private static int ExitX;
	private static int Width;
	private static int Height;

	private static void MainIndy(string[] args)
	{
		var array = new Dictionary<Point, int>();
		var inputs = Console.ReadLine().Split(' ');
		Width = int.Parse(inputs[0]); // number of columns.
		Height = int.Parse(inputs[1]); // number of rows.
		for (var i = 0; i < Height; i++)
		{
			var line = Console.ReadLine(); // represents a line in the grid and contains W integers. Each integer represents one room of a given type.
			var valuesAsStrings = line.Split(' ');
			var values = valuesAsStrings.Select(int.Parse).ToList();
			for (var j = 0; j < Width; j++)
				array [new Point (j, i, Direction.Blocked)] = values [j];
		}

		ExitX = int.Parse(Console.ReadLine()); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).

		// game loop
		for (;;)
		{
			var line = Console.ReadLine();
			Console.Error.WriteLine(line);
			Point player = new Point();
			player.ParseString (line);

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			var room = array[player];
			Action action;
			Point next;
			int dist = 0;

			ComputePath(array, player, true, out action, out next, ref dist, 0);

			var rocks = int.Parse(Console.ReadLine());
			for (var i = 0; i < rocks; i++)
			{
				var rock = Console.ReadLine();
				Console.Error.WriteLine(rock);
				Point rockPoint = new Point ();;
				rockPoint.ParseString (rock);
				Console.Error.WriteLine ();
				Console.Error.WriteLine ("Evaluate rock {0} at {1}", i, rockPoint);
				int rockDist = 0;
				Point rockActionPoint;
				Action rockAction;
				ComputeRockPath (array, rockPoint, player, out rockAction, out rockActionPoint, ref rockDist, 0);

				if (action == Action.Wait || (rockDist < dist && rockAction != Action.Wait)) {
					dist = rockDist;
					next = rockActionPoint;
					action = rockAction;
					Console.Error.WriteLine ("Act on Rock {0}.", i);
				} else {
					Console.Error.WriteLine ("Skip Rock {0} ({1} {2}).", i, rockActionPoint, rockAction);
				}	
			}


			switch (action)
			{
			case Action.Left:
			case Action.DoubleLeft:
				array[next] = RotateLeft(array[next]);
				Console.Error.WriteLine("Room {0} at {1} rotated (dist: {2})", array[next], next, dist);
				Console.WriteLine("{0} {1} LEFT", next.X, next.Y);
				break;
			case Action.Right:
				array[next] = RotateRight(array[next]);
				Console.Error.WriteLine("Room {0} at {1} rotated (dist: {2})", array[next], next, dist);
				Console.WriteLine("{0} {1} RIGHT", next.X, next.Y);
				break;
			case Action.Wait:
				Console.WriteLine("WAIT");
				break;
			}
		}
	}

	private static Direction Convert(Direction dir)
	{
		switch (dir)
		{
		case Direction.Blocked:
			return Direction.Blocked;
		case Direction.Down:
			return Direction.Top;
		case Direction.Top:
			return Direction.Down;
		case Direction.Left:
			return Direction.Right;
		case Direction.Right:
			return Direction.Left;
		}
		return Direction.Blocked;
	}

	private static Direction ComputePath(Dictionary<Point, int> map, Point current
		, bool direct, out Action action, out Point actionPoint, ref int dist, int depth)
	{
		action = Action.Wait;
		actionPoint = new Point (0, 0, Direction.Top);
		if (current.Y >= Height)
		{
			if (current.X != ExitX)
			{
				return Direction.Blocked;
			}
			Console.Error.WriteLine("Found Exit");
			return Direction.Down;
		}

		if (current.X < 0 || current.X >= Width) {
			return Direction.Blocked;
		}
		// try the next one
		var room = map[current];
		var possible = IdentifyNextWithRotations(room, current.Dir);
		if (possible.Count > 1) {
			Console.Error.Write ("Fork possible");
			// no longer a direct path, possible alternatives
			direct = false;
		}
		foreach (var move in possible) {
			Console.Error.Write("({1}){0}  ==>", current, room);
			if (move.Value == Direction.Blocked)
				continue;
			Console.Error.WriteLine ("evaluating {0}, {1}", move.Value, move.Value);
			if (move.Key == Action.Left) {
				Console.Error.Write("Room  can be rotated left");
			} else if (move.Key == Action.Right) {
				Console.Error.Write("Room can be rotated right");
			}else if (move.Key == Action.DoubleLeft) {
				Console.Error.Write("Room can be rotated left twice");
			}
			Point next = current.Move(move.Value);
			// check if everything is ok

			var ret = ComputePath(map, next, direct, out action, out actionPoint, ref dist, depth+1);

			if (direct && move.Key != Action.Wait) {
				actionPoint = current;
				action = move.Key;
				dist = depth;
				Console.Error.Write ("Force action as {0} for {1}", action, actionPoint);
				return Direction.Down;
			}
			if (ret != Direction.Blocked)
			{
				if (move.Key != Action.Wait) {
					action = move.Key;
					actionPoint = current;
					dist = depth;
					Console.Error.Write ("Force action as {0} for {1}", action, actionPoint);
				}
				return Direction.Down;
			}
		}
		Console.Error.Write ("!");
		return Direction.Blocked;
	}

	private static Direction ComputeRockPath(Dictionary<Point, int> map, Point current, Point player
		, out Action action, out Point next, ref int dist, int depth)
	{
		action = Action.Wait;
		next = new Point ();

		if (current.Y >= Height)
		{
			if (current.X != ExitX)
			{
				// Console.Error.WriteLine("Wrong Exit");
				return Direction.Blocked;
			}
			Console.Error.WriteLine("Found Exit");
			return Direction.Down;
		}
		if (current == player) {
			Console.Error.WriteLine ("Rock is behind player, no need to change.");
			action = Action.Wait;
			return Direction.Blocked;
		}
		if (current.X < 0 || current.X >= Width) {
			// Console.Error.WriteLine("Dead end ");
			return Direction.Blocked;
		}
		// try the next one
		var room = map[current];
		var possible = BlockNextWithRotations(depth == 0 ? -Math.Abs(room) : room, current.Dir);

		foreach (var move in possible) {
			Console.Error.Write("({1}){0} ==>", current, room);
			if (move.Value == Direction.Blocked) {
				dist = depth;
				if (move.Key != Action.Wait) {
					action = move.Key;
					next = current;
				}
				return Direction.Blocked;
			}
			Console.Error.WriteLine ("evaluating {0}, {1}", move.Key, move.Value);
			var moved = current.Move (move.Value);
			// check if everything is ok
			var ret = ComputeRockPath(map, moved, player, out action, out next, ref dist, depth+1);

			if (ret == Direction.Blocked)
			{
				if (move.Key != Action.Wait) {
					next = current;
					action = move.Key;
					dist = depth;
					Console.Error.Write ("Force action as {0} for {1}", action, next);
				}
				return Direction.Blocked;
			}
		}
		return Direction.Blocked;
	}

	private static int RotateLeft(int room)
	{
		switch (room)
		{
		case 0:
		case 1:
			return room;
		case 2:
			return 3;
		case 3:
			return 2;
		case 4:
			return 5;
		case 5:
			return 4;
		case 6:
			return 9;
		case 7:
			return 6;
		case 8:
			return 7;
		case 9:
			return 8;
		case 10:
			return 13;
		case 11:
			return 10;
		case 12:
			return 11;
		case 13:
			return 12;
		}
		return 0;
	}
	private static int RotateRight(int room)
	{
		switch (room)
		{
		case 0:
		case 1:
			return room;
		case 2:
			return 3;
		case 3:
			return 2;
		case 4:
			return 5;
		case 5:
			return 4;
		case 6:
			return 7;
		case 7:
			return 8;
		case 8:
			return 9;
		case 9:
			return 6;
		case 10:
			return 11;
		case 11:
			return 12;
		case 12:
			return 13;
		case 13:
			return 10;
		}
		return 0;
	}

	private static Dictionary<Action, Direction> IdentifyNextWithRotations(int roomType, Direction incoming)
	{
		var ret = new Dictionary<Action, Direction>();
		switch (Math.Abs(roomType))
		{
		case 0:
			break;
		case 1:
			ret[Action.Wait] = Direction.Down;
			break;
		case 2:
			if (incoming == Direction.Left)
				ret[Action.Wait] = Direction.Right;
			if (incoming == Direction.Right)
				ret[Action.Wait]=Direction.Left;
			if (incoming == Direction.Top)
				ret[Action.Left] = Direction.Down;
			break;
		case 3:
			if (incoming == Direction.Top)
				ret[Action.Wait] = Direction.Down;
			if (incoming == Direction.Right)
				ret[Action.Left] = Direction.Left;
			if (incoming == Direction.Left)
				ret[Action.Left] = Direction.Right;
			break;
		case 4:
			if (incoming == Direction.Top)
			{
				ret[Action.Wait] = Direction.Left;
				ret[Action.Left] = Direction.Right;
			}
			if (incoming == Direction.Right)
				ret[Action.Wait] = Direction.Down;
			if (incoming == Direction.Left)
				ret[Action.Left] = Direction.Down;
			break;
		case 5:
			if (incoming == Direction.Top)
			{
				ret[Action.Wait] = Direction.Right;
				ret[Action.Left] = Direction.Left;
			}
			if (incoming == Direction.Right)
				ret[Action.Left] = Direction.Down;
			if (incoming == Direction.Left)
				ret[Action.Wait] = Direction.Down;
			break;
		case 6:
			if (incoming == Direction.Top)
				ret[Action.Left] = Direction.Down;
			if (incoming == Direction.Right)
			{
				ret[Action.Wait] = Direction.Left;
				ret[Action.Right] = Direction.Down;
			}
			if (incoming == Direction.Left)
			{
				ret[Action.Wait] = Direction.Right;
				ret[Action.Left] = Direction.Down;
			}
			break;
		case 7:
			if (incoming == Direction.Top)
				ret[Action.Wait] = Direction.Down;
			if (incoming == Direction.Right)
			{
				ret[Action.Wait] = Direction.Down;
				ret[Action.Left] = Direction.Left;
			}
			if (incoming == Direction.Left)
			{
				ret[Action.Right] = Direction.Down;
				ret[Action.Left] = Direction.Right;
			}
			break;
		case 8:
			if (incoming == Direction.Top)
				ret [Action.Left] = Direction.Down;
			if (incoming == Direction.Right) {
				ret [Action.Wait] = Direction.Down;
				ret [Action.DoubleLeft] = Direction.Left;
			}
			if (incoming == Direction.Left)
			{
				ret[Action.Right] = Direction.Down;
				ret [Action.DoubleLeft] = Direction.Right;
			}
			break;
		case 9:
			if (incoming == Direction.Top)
				ret[Action.Wait] = Direction.Down;
			if (incoming == Direction.Left)
			{
				ret[Action.Wait] = Direction.Down;
				ret[Action.Right] = Direction.Right;
			}
			if (incoming == Direction.Right)
			{
				ret[Action.Left] = Direction.Down;
				ret[Action.Right] = Direction.Left;
			}
			break;
		case 10:
			if (incoming == Direction.Top){
				ret [Action.Wait] = Direction.Left;
				ret [Action.Right] = Direction.Right;
			}
			if (incoming == Direction.Left)
				ret[Action.Left] = Direction.Down;
			if (incoming == Direction.Right)
				ret[Action.DoubleLeft] = Direction.Down;
			break;
		case 11:
			if (incoming == Direction.Top) {
				ret [Action.Wait] = Direction.Right;
				ret [Action.Left] = Direction.Left;
			}
			if (incoming == Direction.Right)
				ret[Action.Right] = Direction.Down;
			if (incoming == Direction.Left)
				ret[Action.DoubleLeft] = Direction.Down;
			break;
		case 12:
			if (incoming == Direction.Top) {
				ret [Action.Left] = Direction.Right;
				ret [Action.DoubleLeft] = Direction.Left;
			}
			if (incoming == Direction .Right)
				ret [Action.Wait] = Direction.Down;
			if (incoming == Direction.Left)
				ret [Action.Right] = Direction.Down;
			break;
		case 13:
			if (incoming == Direction.Top) {
				ret [Action.Right] = Direction.Left;
				ret [Action.DoubleLeft] = Direction.Right;
			}
			if (incoming == Direction.Left)
				ret[Action.Wait] = Direction.Down;
			if (incoming == Direction.Right)
				ret [Action.Left] = Direction.Down;
			break;
		}
		if (roomType < 0) {
			// if neg can't rotate
			ret.Remove (Action.Right);
			ret.Remove (Action.Left);
			ret.Remove (Action.DoubleLeft);
		}
		return ret;
	}
	private static Dictionary<Action, Direction> BlockNextWithRotations(int roomType, Direction incoming)
	{
		var ret = new Dictionary<Action, Direction>();
		switch (Math.Abs(roomType))
		{
		case 0:
			break;
		case 1:
			ret[Action.Wait] = Direction.Down;
			break;
		case 2:
			if (incoming == Direction.Left) {
				ret [Action.Left] = Direction.Blocked;
				if (roomType < 0)
					ret [Action.Wait] = Direction.Right;
			}
			if (incoming == Direction.Right) {
				ret [Action.Left] = Direction.Blocked;
				if (roomType < 0)
					ret [Action.Wait] = Direction.Left;
			}
			break;
		case 3:
			if (incoming == Direction.Top) {
				ret [Action.Left] = Direction.Blocked;
				if (roomType < 0)
					ret [Action.Wait] = Direction.Down;
			}
			break;
		case 4:
			if (incoming == Direction.Top)
			{
				ret[Action.Wait] = Direction.Left;
				ret[Action.Left] = Direction.Right;
			}
			if (incoming == Direction.Right)
				ret[Action.Right] = Direction.Blocked;
			break;
		case 5:
			if (incoming == Direction.Top)
			{
				ret[Action.Wait] = Direction.Right;
				ret[Action.Left] = Direction.Left;
			}
			if (incoming == Direction.Left)
				ret[Action.Right] = Direction.Blocked;
			break;
		case 6:
			if (incoming == Direction.Right)
			{
				ret[Action.Left] = Direction.Blocked;
			}
			if (incoming == Direction.Left)
			{
				ret[Action.Right] = Direction.Down;
			}
			break;
		case 7:
			if (incoming == Direction.Top)
				ret[Action.Left] = Direction.Blocked;
			if (incoming == Direction.Right)
			{
				ret[Action.DoubleLeft] = Direction.Blocked;
			}
			break;
		case 8:
			if (incoming == Direction.Right) {
				ret [Action.Left] = Direction.Blocked;
			}
			if (incoming == Direction.Left)
			{
				ret[Action.Right] = Direction.Blocked;
			}
			break;
		case 9:
			if (incoming == Direction.Top)
				ret[Action.Left] = Direction.Blocked;
			if (incoming == Direction.Left)
			{
				ret[Action.DoubleLeft] = Direction.Blocked;
			}
			break;
		case 10:
			if (incoming == Direction.Top){
				ret [Action.Right] = Direction.Blocked;
				if (roomType < 0) {
					ret [Action.Wait] = Direction.Left;
				}
			}
			break;
		case 11:
			if (incoming == Direction.Top) {
				ret [Action.Right] = Direction.Blocked;
				if (roomType < 0) {
					ret [Action.Wait] = Direction.Right;
				}
			}
			break;
		case 12:
			if (incoming == Direction.Right)
				ret [Action.Left] = Direction.Blocked;
			break;
		case 13:
			if (incoming == Direction.Left)
				ret[Action.Right] = Direction.Blocked;
			break;
		}
		if (roomType < 0) {
			// if neg can't rotate
			ret.Remove (Action.Right);
			ret.Remove (Action.Left);
			ret.Remove (Action.DoubleLeft);
		}

		return ret;
	}
}