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
        Right
    };

    private static int ExitX;

    private static void MainIndy(string[] args)
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

            var rocks = int.Parse(Console.ReadLine());
            for (var i = 0; i < rocks; i++)
            {
                var rock = Console.ReadLine();
            }
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            var room = array[YI][XI];
            Console.Error.WriteLine("Room type:{0}, from {1}", room, direction);

            Action action;
            int X, Y;
            ComputePath(array, XI, YI, direction, out action, out X, out Y);
            switch (action)
            {
                case Action.Left:
                    Console.WriteLine("{0} {1} LEFT", X, Y);
                    Console.Error.WriteLine("Room {0} at {1}:{2} must rotate", array[Y][X], X, Y);
                    array[Y][X] = RotateLeft(array[Y][X]);
                    Console.Error.WriteLine("Room {0} at {1}:{2} rotated", array[Y][X], X, Y);
                    break;
                case Action.Right:
                    Console.WriteLine("{0} {1} RIGHT", X, Y);
                    Console.Error.WriteLine("Room {0} at {1}:{2} must rotate", array[Y][X], X, Y);
                    array[Y][X] = RotateRight(array[Y][X]);
                    Console.Error.WriteLine("Room {0} at {1}:{2} rotated", array[Y][X], X, Y);
                    break;
                case Action.Wait:
                    Console.WriteLine("WAIT");
                    break;
            }
            direction = IdentifyNext(array[YI][XI], direction);
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

    private static Direction ComputePath(List<List<int>> map, int x, int y, Direction dir, out Action action, out int X, out int Y)
    {
        action = Action.Wait;
        X = Y = 0;
        if (y >= map.Count)
        {
            if (x != ExitX)
            {
                Console.Error.WriteLine("Wrong Exit");
                return Direction.Blocked;
            }
            Console.Error.WriteLine("Found Exit");
            return Direction.Down;
        }
        // try the next one
        var room = map[y][x];
        Console.Error.Write("({2}){0}:{1} ==>", x, y, room);
        var next = IdentifyNext(room, dir);
//        Console.Error.WriteLine("Moving from {0} through {1} to {2}", dir, room, next);
        next = Convert(next);
        var possible = IdentifyNextWithRotations(room, next);
        var ret = TryPath(map, x, y, next, out action, out X, out Y);
        if (ret != Direction.Blocked || room < 2)
            return ret;
//        Console.Error.Write("Room {0} at {1}:{2} must be rotated ", room, x, y);
        // rotate left
        var newRoom = RotateLeft(room);
        next = IdentifyNext(newRoom, dir);
//        Console.Error.WriteLine("Moving from {0} through {1} to {2}", dir, newRoom, next);
        next = Convert(next);
        ret = TryPath(map, x, y, next, out action, out X, out Y);
        if (ret!= Direction.Blocked)
        {
            X = x;
            Y = y;
            action = Action.Left;
            return ret;
        }
        // rotate right
        newRoom = RotateRight(room);
        next = IdentifyNext(newRoom, dir);
//        Console.Error.WriteLine("Moving from {0} through {1} to {2}", dir, newRoom, next);
        next = Convert(next);
        ret = TryPath(map, x, y, next, out action, out X, out Y);
        if (ret !=Direction.Blocked)
        {
            X = x;
            Y = y;
            action = Action.Right;
            return ret;
        }
        return Direction.Blocked;
    }

    private static Direction TryPath(List<List<int>> map, int x, int y, Direction next, out Action act, out int X, out int Y)
    {
        var nx = x;
        var ny = y;
        X = x;
        Y = y;
        act = Action.Wait;
        switch (next)
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
            case Direction.Blocked:
                return next;
        }
        // check if everything is ok
        var ret = ComputePath(map, nx, ny, next, out act, out X, out Y);
        return ret != Direction.Blocked ? next : ret;

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

    private static Direction IdentifyNext(int roomType, Direction incoming)
    {
        if (roomType < 0)
            roomType = -roomType;
        switch (roomType)
        {
            case 0:
                return Direction.Blocked;
            case 1:
                return Direction.Down;
            case 2:
                if (incoming == Direction.Left)
                    return Direction.Right;
                if (incoming == Direction.Right)
                    return Direction.Left;
                return Direction.Blocked;
            case 3:
                if (incoming == Direction.Left || incoming == Direction.Right)
                    return Direction.Blocked;
                return Direction.Down;
            case 4:
                if (incoming == Direction.Top)
                    return Direction.Left;
                if (incoming == Direction.Right)
                    return Direction.Down;
                return Direction.Blocked;
            case 5:
                if (incoming == Direction.Top)
                    return Direction.Right;
                if (incoming == Direction.Left)
                    return Direction.Down;
                return Direction.Blocked;
            case 6:
                if (incoming == Direction.Left)
                    return Direction.Right;
                if (incoming == Direction.Right)
                    return Direction.Left;
                return Direction.Blocked;
            case 7:
                if (incoming == Direction.Top || incoming == Direction.Right)
                    return Direction.Down;
                return Direction.Blocked;
            case 8:
                if (incoming == Direction.Left || incoming == Direction.Right)
                    return Direction.Down;
                return Direction.Blocked;
            case 9:
                if (incoming == Direction.Top || incoming == Direction.Left)
                    return Direction.Down;
                return Direction.Blocked;
            case 10:
                if (incoming == Direction.Top)
                    return Direction.Left;
                return Direction.Blocked;
            case 11:
                if (incoming == Direction.Top)
                    return Direction.Right;
                return Direction.Blocked;
            case 12:
                if (incoming == Direction.Right)
                    return Direction.Down;
                return Direction.Blocked;
            case 13:
                if (incoming == Direction.Left)
                    return Direction.Down;
                return Direction.Blocked;
        }
        return Direction.Blocked;
    }
    private static Dictionary<Action, Direction> IdentifyNextWithRotations(int roomType, Direction incoming)
    {
        var ret = new Dictionary<Action, Direction>();
        if (roomType < 0)
            roomType = -roomType;
        ret[Action.Wait] = Direction.Blocked;
        switch (roomType)
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
                    ret[Action.Left] = Direction.Down;
                if (incoming == Direction.Right)
                    ret[Action.Wait] = Direction.Down;
                if (incoming == Direction.Left)
                    ret[Action.Right] = Direction.Down;
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
                if (incoming == Direction.Top)
                    ret[Action.Wait] = Direction.Left;
                if (incoming == Direction.Left)
                    ret[Action.Left] = Direction.Down;
                break;
            case 11:
                if (incoming == Direction.Top)
                    ret[Action.Wait] = Direction.Right;
                if (incoming == Direction.Right)
                    ret[Action.Right] = Direction.Down;
                break;
            case 12:
                if (incoming == Direction.Top)
                    ret[Action.Left] = Direction.Right;
                if (incoming == Direction.Right)
                    ret[Action.Wait] = Direction.Down;
                break;
            case 13:
                if (incoming == Direction.Top)
                    ret[Action.Right] = Direction.Left;
                if (incoming == Direction.Left)
                    ret[Action.Wait] = Direction.Down;
                break;
        }
        return ret;
    }
}