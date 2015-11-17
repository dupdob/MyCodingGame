using System;
using System.Linq;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
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

	private static int ExitX;

	private static void Main(string[] args)
	{
		var array = new List<List<int>>();
		var inputs = Console.ReadLine().Split(' ');
		var W = int.Parse(inputs[0]); // number of columns.
		var H = int.Parse(inputs[1]); // number of rows.
		for (var i = 0; i < H; i++)
		{
			var line = Console.ReadLine(); // represents a line in the grid and contains W integers. Each integer represents one room of a given type.
			var valuesAsStrings = line.Split(' ');
			var values = valuesAsStrings.Select(int.Parse).ToList();
			array.Add(values);
		}

		ExitX = int.Parse(Console.ReadLine()); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).

		var direction = Direction.Top;
		// game loop
		while (true)
		{
			var line = Console.ReadLine();
			Console.Error.WriteLine(line);
			inputs = line.Split(' ');
			int XI = int.Parse(inputs[0]);
			int YI = int.Parse(inputs[1]);
			string POS = inputs[2];
			switch (POS)
			{
			case "TOP":
				direction = Direction.Top;
				break;
			case "LEFT":
				direction = Direction.Left;
				break;
			case "RIGHT":
				direction = Direction.Right;
				break;
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			var room = array[YI][XI];
			Action action;
			int X, Y, dist = 0;

			ComputePath(array, XI, YI, direction, true, out action, out X, out Y, ref dist, 0);

			var rocks = int.Parse(Console.ReadLine());
			for (var i = 0; i < rocks; i++)
			{
				var rock = Console.ReadLine();
				Console.Error.WriteLine(rock);
				inputs = rock.Split(' ');
				int rockX = int.Parse(inputs[0]);
				int rockY = int.Parse(inputs[1]);
				Direction rockDir;
				POS = inputs[2];
				rockDir = Direction.Top;
				switch (POS)
				{
				case "TOP":
					rockDir = Direction.Top;
					break;
				case "LEFT":
					rockDir = Direction.Left;
					break;
				case "RIGHT":
					rockDir = Direction.Right;
					break;
				}
				Console.Error.WriteLine ();
				Console.Error.WriteLine ("Evaluate rock {0} at {1}:{2}, dir {3}", i, rockX, rockY, rockDir);
				int rockXAction, rockYAction, rockDist = 0;
				Action rockAction;
				ComputeRockPath (array, rockX, rockY, rockDir, XI, YI, out rockAction, out rockXAction, out rockYAction, ref rockDist, 0);

				if (action == Action.Wait || (rockDist < dist && rockAction != Action.Wait)) {
					dist = rockDist;
					X = rockXAction;
					Y = rockYAction;
					action = rockAction;
					Console.Error.WriteLine ("Act on Rock {0}.", i);
				} else {
					Console.Error.WriteLine ("Skip Rock {0} ({1}:{2} {3}).", i, rockXAction, rockYAction, rockAction);
				}
			}


			switch (action)
			{
			case Action.Left:
			case Action.DoubleLeft:
				array[Y][X] = RotateLeft(array[Y][X]);
				Console.Error.WriteLine("Room {0} at {1}:{2} rotated (dist: {3})", array[Y][X], X, Y, dist);
				Console.WriteLine("{0} {1} LEFT", X, Y);
				break;
			case Action.Right:
				array[Y][X] = RotateRight(array[Y][X]);
				Console.Error.WriteLine("Room {0} at {1}:{2} rotated (dist: {3})", array[Y][X], X, Y, dist);
				Console.WriteLine("{0} {1} RIGHT", X, Y);
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

	private static Direction ComputePath(List<List<int>> map, int x, int y, Direction dir
		, bool direct, out Action action, out int X, out int Y, ref int dist, int depth)
	{
		action = Action.Wait;
		X = Y = 0;
		if (y >= map.Count)
		{
			if (x != ExitX)
			{
				//                Console.Error.WriteLine("Wrong Exit");
				return Direction.Blocked;
			}
			Console.Error.WriteLine("Found Exit");
			return Direction.Down;
		}
		if (x < 0 || x >= map [0].Count) {
			//			Console.Error.WriteLine("Dead end ");
			return Direction.Blocked;
		}
		// try the next one
		var room = map[y][x];
		var possible = IdentifyNextWithRotations(room, dir);
		if (possible.Count > 1) {
			Console.Error.Write ("Fork possible");
			// no longer a direct path, possible alternatives
			direct = false;
		}
		foreach (var move in possible) {
			Console.Error.Write("({2}){0}:{1} from {3} ==>", x, y, room, dir);
			if (move.Value == Direction.Blocked)
				continue;
			Console.Error.WriteLine ("evaluating {2}, {3}", room, dir, move.Key, move.Value);
			if (move.Key == Action.Left) {
				Console.Error.Write("Room  can be rotated left");
			} else if (move.Key == Action.Right) {
				Console.Error.Write("Room can be rotated right");
			}else if (move.Key == Action.DoubleLeft) {
				Console.Error.Write("Room can be rotated left twice");
			}
			var nextDirection = Convert(move.Value);
			var nx = x;
			var ny = y;
			X = x;
			Y = y;
			switch (nextDirection)
			{
			case Direction.Top:
				ny = y + 1;
				break;
			case Direction.Down:
				ny = y - 1;
				break;
			case Direction.Right:
				nx = x - 1;
				break;
			case Direction.Left:
				nx = x + 1;
				break;
			}
			// check if everything is ok

			var ret = ComputePath(map, nx, ny, nextDirection, direct, out action, out X, out Y, ref dist, depth+1);

			if (direct && move.Key != Action.Wait) {
				X = x;
				Y = y;
				action = move.Key;
				dist = depth;
				Console.Error.Write ("Force action as {0} for {1}:{2}", action, X, Y);
				return Direction.Down;
			}
			if (ret != Direction.Blocked)
			{
				if (move.Key != Action.Wait) {
					X = x;
					Y = y;
					action = move.Key;
					dist = depth;
					Console.Error.Write ("Force action as {0} for {1}:{2}", action, X, Y);
				}
				return Direction.Down;
			}
		}
		Console.Error.Write ("!");
		return Direction.Blocked;
	}

	private static Direction ComputeRockPath(List<List<int>> map, int x, int y, Direction dir, int playerX, int playerY
		, out Action action, out int X, out int Y, ref int dist, int depth)
	{
		action = Action.Wait;
		X = Y = 0;
		if (y >= map.Count)
		{
			if (x != ExitX)
			{
				// Console.Error.WriteLine("Wrong Exit");
				return Direction.Blocked;
			}
			Console.Error.WriteLine("Found Exit");
			return Direction.Down;
		}
		if (x == playerX && y == playerY) {
			Console.Error.WriteLine ("Rock is behind player, no need to change.");
			action = Action.Wait;
			X = Y = 0;
			return Direction.Blocked;
		}
		if (x < 0 || x >= map [0].Count) {
			// Console.Error.WriteLine("Dead end ");
			return Direction.Blocked;
		}
		// try the next one
		var room = map[y][x];
		var possible = BlockNextWithRotations(depth == 0 ? -Math.Abs(room) : room, dir);

		foreach (var move in possible) {
			Console.Error.Write("({2}){0}:{1} from {3} ==>", x, y, room, dir);
			if (move.Value == Direction.Blocked) {
				dist = depth;
				if (move.Key != Action.Wait) {
					action = move.Key;
					X = x;
					Y = y;
				}
				return Direction.Blocked;
			}
			Console.Error.WriteLine ("evaluating {2}, {3}", room, dir, move.Key, move.Value);
			var nextDirection = Convert(move.Value);
			var nx = x;
			var ny = y;
			X = x;
			Y = y;
			switch (nextDirection)
			{
			case Direction.Top:
				ny = y + 1;
				break;
			case Direction.Down:
				ny = y - 1;
				break;
			case Direction.Right:
				nx = x - 1;
				break;
			case Direction.Left:
				nx = x + 1;
				break;
			}
			// check if everything is ok
			var ret = ComputeRockPath(map, nx, ny, nextDirection, playerX, playerY, out action, out X, out Y, ref dist, depth+1);

			if (ret == Direction.Blocked)
			{
				if (move.Key != Action.Wait) {
					X = x;
					Y = y;
					action = move.Key;
					dist = depth;
					Console.Error.Write ("Force action as {0} for {1}:{2}", action, X, Y);
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
			if (incoming == Direction.Right)
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
			if (incoming == Direction.Top)
				ret[Action.Left] = Direction.Blocked;
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
				ret [Action.Left] = Direction.Blocked;
			}
			break;
		case 11:
			if (incoming == Direction.Top) {
				ret [Action.Right] = Direction.Blocked;
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