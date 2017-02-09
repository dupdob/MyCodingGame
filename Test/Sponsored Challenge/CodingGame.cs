using System;
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
static class CodinGame
{
    private enum Direction { L, U, R, D, N };
    private static char[,] gamearea;

    /// <summary>
    /// 
    /// </summary>
    private class Coord
    {
        public int X;
        public int Y;

        public int MaxX;
        public int MaxY;

        public Coord Move(Direction dir)
        {
            Coord ret = new Coord();
            ret.MaxX = this.MaxX;
            ret.MaxY = this.MaxY;

            switch (dir)
            {
                case Direction.D:
                    ret.X = this.X;
                    ret.Y = (this.Y + 1) % this.MaxY;
                    break;
                case Direction.U:
                    ret.X = this.X;
                    ret.Y = (this.Y - 1 + this.MaxY) % this.MaxY;
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

    static void Main(string[] args)
    {
        var firstInitInput = int.Parse(Console.ReadLine());
        var secondInitInput = int.Parse(Console.ReadLine());
        var thirdInitInput = int.Parse(Console.ReadLine());
        Console.Error.WriteLine(firstInitInput);
        Console.Error.WriteLine(secondInitInput);
        Console.Error.WriteLine(thirdInitInput);

        var player = new Coord
        {
            MaxX = firstInitInput,
            MaxY = secondInitInput
        };

        gamearea = new char[firstInitInput, secondInitInput];
        var visited = new int[firstInitInput, secondInitInput];
        for (var x = 0; x < gamearea.GetLength(0); x++)
            for (var y = 0; y < gamearea.GetLength(1); y++)
            {
                gamearea[x, y] = ' ';
                visited[x, y] = 0;
            }
        // game loop
        while (true)
        {
            var firstInput = Console.ReadLine();
            var secondInput = Console.ReadLine();
            var thirdInput = Console.ReadLine();
            var fourthInput = Console.ReadLine();
            //			Console.Error.WriteLine(firstInput);
            //			Console.Error.WriteLine(secondInput);
            //			Console.Error.WriteLine(thirdInput);
            //			Console.Error.WriteLine(fourthInput);
            var ghosts = new List<Coord>(thirdInitInput);
            for (var i = 0; i < thirdInitInput; i++)
            {
                var inputs = Console.ReadLine().Split(' ');
                var fifthInput = int.Parse(inputs[0]);
                var sixthInput = int.Parse(inputs[1]);
                ///				Console.Error.Write("#{0}=X{1}.Y{2},", i, fifthInput, sixthInput );
                if (i == thirdInitInput - 1)
                {
                    player.X = sixthInput % firstInitInput;
                    player.Y = fifthInput % secondInitInput;
                    player.MaxX = firstInitInput;
                    player.MaxY = secondInitInput;
                    //Console.Error.WriteLine("Player @ {0}:{1}", fifthInput, sixthInput);
                }
                else
                {
                    var ghost = new Coord();
                    ghost.X = sixthInput;
                    ghost.Y = fifthInput;
                    ghost.MaxX = firstInitInput;
                    ghost.MaxY = secondInitInput;
                    ghosts.Add(ghost);
                    //Console.Error.WriteLine("Ghost @ {0}:{1}", fifthInput, sixthInput);
                }
            }
            // we discover some room
            visited[player.X, player.Y]++;
            SetRoom(player, '*');
            SetRoom(player.Move(Direction.L), firstInput[0]);
            SetRoom(player.Move(Direction.D), secondInput[0]);
            SetRoom(player.Move(Direction.R), thirdInput[0]);
            SetRoom(player.Move(Direction.U), fourthInput[0]);

            foreach (var ghost in ghosts)
                SetRoom(ghost, '@');

            DumpArea();

            var dir = PathDiscovery(visited, player, false);
            if (NextToGhost(player) && dir == Direction.N)
            {
                dir = PathDiscovery(visited, player, true);
            }
            // erase actors
            SetRoom(player, '_');
            foreach (var ghost in ghosts)
                SetRoom(ghost, '_');
            string command;
            switch (dir)
            {
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
            Console.WriteLine(command);
        }
    }
    // basic pathdiscovery	
    private static Direction PathDiscovery(int[,] visited, Coord c, bool ignoreGhosts)
    {
        var score = int.MaxValue;
        var dir = Direction.N;
        var next = c.Move(Direction.R);
        if (GetRoom(next).IsEmpty() && ( ignoreGhosts || !NextToGhost(next)))
        {
            var upScore = visited[next.X, next.Y];
            if (score > upScore)
            {
                score = upScore;
                dir = Direction.R;
            }
        }
        next = c.Move(Direction.U);
        if (GetRoom(next).IsEmpty() && (ignoreGhosts || !NextToGhost(next)))
        {
            var upScore = visited[next.X, next.Y];
            if (score > upScore)
            {
                score = upScore;
                dir = Direction.U;
            }
        }
        next = c.Move(Direction.L);
        if (GetRoom(next).IsEmpty() && (ignoreGhosts || !NextToGhost(next)))
        {
            var upScore = visited[next.X, next.Y];
            if (score > upScore)
            {
                score = upScore;
                dir = Direction.L;
            }
        }
        next = c.Move(Direction.D);
        if (GetRoom(next).IsEmpty() && (ignoreGhosts || !NextToGhost(next)))
        {
            var upScore = visited[next.X, next.Y];
            if (score > upScore)
            {
                score = upScore;
                dir = Direction.D;
            }
        }
        return dir;
    }

    private static void SetRoom(Coord coord, char car)
    {
        gamearea[coord.X, coord.Y] = car;
    }

    private static char GetRoom(Coord coord)
    {
        return gamearea[coord.X, coord.Y];
    }

    private static bool IsEmpty(this char car)
    {
        return car == '_';
    }

    private static bool NextToGhost(Coord pos)
    {
        if (GetRoom(pos.Move(Direction.U)) == '@' ||
            GetRoom(pos.Move(Direction.D)) == '@' ||
            GetRoom(pos.Move(Direction.L)) == '@' ||
            GetRoom(pos.Move(Direction.R)) == '@')
            return true;
        return false;
    }

    private static void DumpArea()
    {
        Console.Error.WriteLine(new string('_', gamearea.GetLength(0)+2));
        for (var y = 0; y < gamearea.GetLength(1); y++)
        {
            Console.Error.Write('|');
            for (var x = 0; x < gamearea.GetLength(0); x++)
            {
                Console.Error.Write(gamearea[x, y]);
            }
            Console.Error.WriteLine('|');
        }
        Console.Error.WriteLine(new string('_', gamearea.GetLength(0)+2));
    }
}