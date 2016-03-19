using System;
using System.Text;
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
		public string Name;
		public int X;
		public int Y;
		public Direction Dir;

		public Point(int x, int y, Direction dir = Direction.Blocked, string name = "")
		{
			X = x;
			Y = y;
			Dir = dir;
			Name = name;
		}

		public Point Move(Direction dir)
		{
			switch (dir) {
			case Direction.Top:
				return new Point(X, Y -1, Convert(dir), Name);
			case Direction.Down:
				return new Point (X, Y + 1, Convert(dir), Name);
			case Direction.Left:
				return new Point (X - 1, Y, Convert(dir), Name);
			case Direction.Right:
				return new Point (X + 1, Y, Convert(dir), Name);
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
			return string.Format("{3} @{0}:{1}", X, Y, Dir, Name);
		}

		public string ToStringShort()
		{
			return string.Format("{0}:{1}", X, Y);
		}

		public override int GetHashCode ()
		{
			return X + 1000 * Y;
		}
		public bool IsSamePosThan(Point b)
		{
			return this.X == b.X && this.Y == b.Y;
		}

		public static bool operator==(Point a, Point b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator!=(Point a, Point b)
		{
			return !(a==b);
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

	private static void Main(string[] args)
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
			Point player = new Point();
			player.ParseString (line);
			player.Name = "Indy";
			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			var room = array[player];
			Action action;
			Point next;
			int dist = 0;
			var rocks = int.Parse(Console.ReadLine());
			var rockList = new List<Point> (rocks);
			int i=0;
			for (i = 0; i < rocks; i++) {
				var rock = Console.ReadLine ();
				Point rockPoint = new Point ();
				rockPoint.ParseString (rock);
				rockPoint.Name = string.Format ("Rock#{0}", i);
				rockList.Add (rockPoint);
			}

			ComputePath(array, player, rockList, true, out action, out next, ref dist, 0);

			switch (action)
			{
			case Action.Left:
			case Action.DoubleLeft:
				array[next] = RotateLeft(array[next]);
				Console.WriteLine("{0} {1} LEFT", next.X, next.Y);
				break;
			case Action.Right:
				array[next] = RotateRight(array[next]);
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

	private static Direction ComputePath(Dictionary<Point, int> map, Point current, 
		List<Point> rocks, bool direct, out Action action, out Point actionPoint, ref int dist, int depth)
	{
		var prefix = new string (' ', depth);
		action = Action.Wait;
		actionPoint = new Point (0, 0, Direction.Top);
		// we move to the next pos
		var scanPoint = current.Move(Traverse(map[current], current.Dir));
		if (scanPoint.Y >= Height)
		{
			if (scanPoint.X != ExitX)
			{
				return Direction.Blocked;
			}
			Console.Error.WriteLine(prefix+"Found Exit");
			return Direction.Down;
		}
		var rockMessage = new StringBuilder (current.ToString());
		// do we reach the border?
		if (scanPoint.X < 0 || scanPoint.X >= Width) {
			if (depth <3)
				Console.Error.WriteLine("Leaving map.");
			return Direction.Blocked;
		}
		// try the next one
		var room = map[scanPoint];
		var possible = IdentifyNextWithRotations(room, scanPoint.Dir);
		int checks = possible.Count;
		if (checks == 0) {
			if (depth <3)
				Console.Error.Write("Dead end.");
			return Direction.Blocked;
		}
		if (checks > 1) {
			// no longer a direct path, possible alternatives
			direct = false;
		}
		// we store actions for this turn
		var possibleActions = new Dictionary<Point, Dictionary<Action, Direction>> ();
		possibleActions.Add (scanPoint, possible);
		foreach (var rock in rocks) {
			int roomType;
			if (rock.X < 0 || rock.X >= Width || rock.Y>= Height) {
				// rock out of map
				continue;
			}
			var rockPoint = rock.Move(Traverse(map[rock], rock.Dir));
			if (rockPoint == scanPoint)
			{
			    // collision
			    return Direction.Blocked;
			}
			if (map.TryGetValue (rockPoint, out roomType)) {
				rockMessage.AppendFormat("{0}, ", rockPoint);
				possible = BlockNextWithRotations (roomType, rockPoint.Dir);
				checks += possible.Count;
				if (!possibleActions.ContainsKey (rockPoint))
					possibleActions.Add (rockPoint, possible);
				else {
					// rocks collide
					possibleActions.Remove(rockPoint);
				}
			}
		}
	
		Console.Error.Write(rockMessage.ToString());
//		Console.Error.WriteLine(" {0} possible moves", checks);
		Console.Error.Write (prefix);
		foreach(var plays in possibleActions)
		{
			var tryRoom = map [plays.Key];
			bool collision = false;
			bool roomOccupied = false;
			if (current.IsSamePosThan(plays.Key)) {
				roomOccupied = true;
			}
			foreach (var rock in rocks) {
				if (rock.IsSamePosThan (plays.Key)) {
					roomOccupied = true;
					break;
				}
			}
//			Console.Error.Write("{1}{0}:", plays.Key, prefix);
		
			foreach (var move in plays.Value) {
				if (roomOccupied && move.Key != Action.Wait) {
					Console.Error.WriteLine ("object in room {0}, can't rotate", plays.Key.ToStringShort());
					continue;
				}
				switch (move.Key) {
				case Action.Left:
					Console.Error.WriteLine("{0} {1}:RL ", prefix, plays.Key);
					map[plays.Key] = RotateLeft(tryRoom);
					break;
				case Action.DoubleLeft:
					Console.Error.WriteLine("{0} {1}:RLL ", prefix, plays.Key);
					map[plays.Key] = RotateLeft(RotateLeft(tryRoom));
					break;
				case Action.Right:
					Console.Error.WriteLine("{0} {1}:RR ", prefix, plays.Key);
					map[plays.Key] = RotateRight(tryRoom);
						break;
				default:
//					Console.Error.Write("wait ");
					map [plays.Key] = tryRoom;
					break;
				}
				// now see what happens
				Direction nextDir = Traverse(map[current], current.Dir);
				Point next = current.Move (nextDir);
				if (nextDir == Direction.Blocked) {
					// can't move
//					Console.Error.WriteLine("Indy Blocked");
//					Console.Error.Write(prefix);
					map [plays.Key] = tryRoom;
					continue;
				}
				var newRocks = new List<Point> ();
				foreach (var curRock in rocks) {
					nextDir = Traverse (map [curRock], curRock.Dir);
					if (nextDir == Direction.Blocked) {
						map[plays.Key] = tryRoom;
						// block destroyed
						Console.Error.WriteLine("{0} crashes", curRock);
						continue;
					}
					Point nextRock = curRock.Move(nextDir);
					// do we reach the border?
					if (nextRock.X < 0 || nextRock.X >= Width) {
						// block destroyed
						Console.Error.WriteLine("{0} leaves area", nextRock);
						continue;
					}
					if (nextRock.IsSamePosThan(next)) {
						// collision
						Console.Error.WriteLine("collision with {0}", nextRock);
						collision = true;
						break;
					}
					if (!newRocks.Contains (nextRock))
						newRocks.Add (nextRock);
					else {
						Console.Error.WriteLine ("Collision between two rocks");
						newRocks.Remove (nextRock);
					}
				}
				if (collision) {
					map [plays.Key] = tryRoom;
					continue;
				}
				// check if everything is ok
//				Console.Error.WriteLine();
				var ret = ComputePath(map, next, newRocks, direct, out action, out actionPoint, ref dist, depth+1);
				map [plays.Key] = tryRoom;
				/*
				if (direct && move.Key != Action.Wait) {
					actionPoint = plays.Key;
					action = move.Key;
					dist = depth;
					Console.Error.WriteLine ("Force action as {0} for {1}", action, actionPoint.ToStringShort());
					return Direction.Down;
				}*/
				if (ret != Direction.Blocked)
				{
					if (move.Key != Action.Wait) {
						action = move.Key;
						actionPoint = plays.Key;
						dist = depth;
						Console.Error.WriteLine ("Force action as {0} for {1}", action, actionPoint.ToStringShort());
					}
					return Direction.Down;
				}
				map [plays.Key] = tryRoom;
			}
		}
		Console.Error.WriteLine(prefix+"No cigar!");
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

	private static Direction Traverse(int roomType, Direction incoming)
	{
		switch (Math.Abs(roomType))
		{
		case 0:
			break;
		case 1:
			return Direction.Down;
			break;
		case 2:
			if (incoming == Direction.Left)
				return Direction.Right;
			if (incoming == Direction.Right)
				return Direction.Left;
			break;
		case 3:
			if (incoming == Direction.Top)
				return Direction.Down;
			break;
		case 4:
			if (incoming == Direction.Top)
				return Direction.Left;
			if (incoming == Direction.Right)
				return Direction.Down;
			break;
		case 5:
			if (incoming == Direction.Top)
				return Direction.Right;
			if (incoming == Direction.Left)
				return Direction.Down;
			break;
		case 6:
			if (incoming == Direction.Right)
				return Direction.Left;
			if (incoming == Direction.Left)
				return Direction.Right;
			break;
		case 7:
			if (incoming == Direction.Top)
				return Direction.Down;
			if (incoming == Direction.Right)
				return Direction.Down;
			break;
		case 8:
			if (incoming == Direction.Right || incoming == Direction.Left)
				return Direction.Down;
			break;
		case 9:
			if (incoming == Direction.Top)
				return Direction.Down;
			if (incoming == Direction.Left)
				return Direction.Down;
			break;
		case 10:
			if (incoming == Direction.Top)
				return Direction.Left;
			break;
		case 11:
			if (incoming == Direction.Top)
				return Direction.Right;
			break;
		case 12:
			if (incoming == Direction .Right)
				return Direction.Down;
			break;
		case 13:
			if (incoming == Direction.Left)
				return Direction.Down;
			break;
		}
		return Direction.Blocked;
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
				ret [Action.Wait] = Direction.Right;
			}
			if (incoming == Direction.Right) {
				ret [Action.Left] = Direction.Blocked;
				ret [Action.Wait] = Direction.Left;
			}
			if (incoming == Direction.Top) {
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Down;
			}
			break;
		case 3:
			if (incoming == Direction.Top) {
				ret [Action.Left] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			if (incoming == Direction.Right){
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Left;
			}
			if (incoming == Direction.Left) {
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Right;
			}
			break;
		case 4:
			if (incoming == Direction.Top) {
				ret [Action.Wait] = Direction.Left;
				ret [Action.Left] = Direction.Right;
			}
			if (incoming == Direction.Right) {
				ret [Action.Right] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			if (incoming == Direction.Left) {
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Down;
			}
			break;
		case 5:
			if (incoming == Direction.Top) {
				ret [Action.Wait] = Direction.Right;
				ret [Action.Left] = Direction.Left;
			}
			if (incoming == Direction.Left) {
				ret [Action.Right] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			if (incoming == Direction.Right) {
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Down;
			}
			break;
		case 6:
			if (incoming == Direction.Top)
			{
				ret[Action.Wait] = Direction.Blocked;
				ret[Action.Right] = Direction.Down;
			}
			if (incoming == Direction.Right)
			{
				ret[Action.Left] = Direction.Blocked;
				ret[Action.Wait] = Direction.Left;
				ret[Action.Right] = Direction.Down;
			}
			if (incoming == Direction.Left)
			{
				ret[Action.Right] = Direction.Blocked;
				ret[Action.Wait] = Direction.Right;
				ret[Action.Left] = Direction.Down;
			}
			break;
		case 7:
			if (incoming == Direction.Top) {
				ret [Action.Left] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			if (incoming == Direction.Right)
			{
				ret[Action.DoubleLeft] = Direction.Blocked;
				ret[Action.Left] = Direction.Left;
				ret[Action.Wait] = Direction.Down;
			}
			if (incoming == Direction.Left)
			{
				ret [Action.Wait] = Direction.Blocked;
				ret[Action.DoubleLeft] = Direction.Down;
				ret [Action.Right] = Direction.Right;
			}
			break;
		case 8:
			if (incoming == Direction.Top) {
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Down;
			}
			if (incoming == Direction.Right) {
				ret [Action.Left] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			if (incoming == Direction.Left) {
				ret [Action.Right] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			break;
		case 9:
			if (incoming == Direction.Top) {
				ret [Action.Left] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			if (incoming == Direction.Left)
			{
				ret[Action.DoubleLeft] = Direction.Blocked;
				ret[Action.Wait] = Direction.Down;
				ret[Action.Right] = Direction.Right;
			}
			if (incoming == Direction.Right)
			{
				ret[Action.Wait] = Direction.Blocked;
				ret[Action.Right] = Direction.Left;
				ret[Action.DoubleLeft] = Direction.Down;
			}
			break;
		case 10:
			if (incoming == Direction.Top){
				ret [Action.Left] = Direction.Blocked;
				ret [Action.Right] = Direction.Right;
				ret [Action.Wait] = Direction.Left;
			}
			if (incoming == Direction.Left){
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Down;
			}
			if (incoming == Direction.Right){
				ret [Action.Wait] = Direction.Blocked;
				ret[Action.DoubleLeft] = Direction.Down;
			}
			break;
		case 11:
			if (incoming == Direction.Top) {
				ret [Action.Right] = Direction.Blocked;
				ret [Action.Wait] = Direction.Right;
			}
			if (incoming == Direction.Left){
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.DoubleLeft] = Direction.Down;
			}
			if (incoming == Direction.Right){
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Right] = Direction.Down;
			}
			break;
		case 12:
			if (incoming == Direction.Top) {
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Right;
			}
			if (incoming == Direction.Left){
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Right] = Direction.Down;
			}
			if (incoming == Direction.Right){
				ret [Action.Left] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			break;
		case 13:
			if (incoming == Direction.Top) {
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Right] = Direction.Left;
			}
			if (incoming == Direction.Left){
				ret [Action.Right] = Direction.Blocked;
				ret [Action.Wait] = Direction.Down;
			}
			if (incoming == Direction.Right){
				ret [Action.Wait] = Direction.Blocked;
				ret [Action.Left] = Direction.Down;
			}
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