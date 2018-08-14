using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodingGame
{
    internal static class HauntedManor
    {
/*
     * Given a grid representing a haunted manor filled with mirrors, empty cells, and cells containing different types of monsters, you must determine the type and position of each monster.

The different types of monsters are:
- V: Vampire, who can be seen directly, but not inside a mirror.
- Z: Zombie, who can be seen both directly and in a mirror.
- G: Ghost, who cannot be seen directly but can be seen in a mirror.

Each cell on the border of the grid has a window through which you can peer into the manor. For each border, you are given the number of monsters visible through that window. Line of sight will bounce off the mirrors, making it possible or impossible to see certain monsters.

There are two types of mirror:
-\: Diagonal down. 
-/: Diagonal up.

The manor is always a square grid. None of the cells are empty.

Example:
This 3 by 3 manor has 0 vampires, 4 zombies and 2 ghosts. 3 mirrors are present.

   0   0   0
 ┌───┬───┬───┐
0│ / │ \ │ / │2
 ├───┼───┼───┤
1│   │   │   │1
 ├───┼───┼───┤
3│   │   │   │3
 └───┴───┴───┘
   3   3   2

No monster can be seen from the top because of the mirrors.
The middle row seems to contain the two ghosts since only 1 monster is visible from either side.
Using all the reported sightings of monsters through the windows, we can easily come to the configuration below.

   0   0   0
 ┌───┬───┬───┐
0│ / │ \ │ / │2
 ├───┼───┼───┤
1│ G │ G │ Z │1
 ├───┼───┼───┤
3│ Z │ Z │ Z │3
 └───┴───┴───┘
   3   3   2

The input for this case would be:
0 4 2
3
0 0 0
3 3 2
0 1 3
2 1 3
/\/
...
...

And the required output would be:
/\/
GGZ
ZZZ
https://www.codingame.com/ide/puzzle/haunted-manor
     */
        static void MainHaunted()
        {
            // monster distribution
            var readLine = Console.ReadLine();
            Console.Error.WriteLine(readLine);
            var monsters = readLine.Split(' ').Select(x => int.Parse(x)).ToArray();
            // siez of manor
            readLine = Console.ReadLine();
            Console.Error.WriteLine(readLine);
            var size = int.Parse(readLine);
            // number of monsters seen from windows
            var views = new List<int>();
            readLine = Console.ReadLine();
            Console.Error.WriteLine(readLine);
            views.AddRange(readLine.Split(' ').Select(int.Parse));
            readLine = Console.ReadLine();
            Console.Error.WriteLine(readLine);
            views.AddRange(readLine.Split(' ').Select(int.Parse));
            readLine = Console.ReadLine();
            Console.Error.WriteLine(readLine);
            views.AddRange(readLine.Split(' ').Select(int.Parse));
            readLine = Console.ReadLine();
            Console.Error.WriteLine(readLine);
            views.AddRange(readLine.Split(' ').Select(int.Parse));
            // mirror locations
            var manorFloor = new char[size, size];
            var roomWithMonsters = new List<Room>(size * size);
            for (var row = 0; row < size; row++)
            {
                readLine = Console.ReadLine();
                var line = readLine.ToCharArray();
                for (var column = 0; column < size; column++)
                {
                    manorFloor[row, column] = line[column];
                    if (line[column] == '.')
                    {
                        roomWithMonsters.Add(new Room(row, column, manorFloor, false));
                    }
                    Console.Error.Write(line[column]);
                }
                Console.Error.WriteLine();
            }
            // build room seen from windows
            var windows = new List<View>(4*size);
            for (var col = 0; col < size; col++)
            {
                var viewFrom = ViewFrom(Direction.Down, 0, col, manorFloor, views);
                if (viewFrom != null && !windows.Any(_ => _.IsReversedView(viewFrom)))
                {
                    windows.Add(viewFrom);
                }
                viewFrom = ViewFrom(Direction.Up, size - 1, col, manorFloor, views);
                if (viewFrom != null && !windows.Any(_ => _.IsReversedView(viewFrom)))
                {
                    windows.Add(viewFrom);
                }
            }
            for (var row = 0; row < size; row++)
            {
                var viewFrom = ViewFrom(Direction.Right, row, 0, manorFloor, views);
                if (viewFrom != null && !windows.Any(_ => _.IsReversedView(viewFrom)))
                {
                    windows.Add(viewFrom);
                }
                viewFrom = ViewFrom(Direction.Left, row, size-1, manorFloor, views);
                if (viewFrom != null && !windows.Any(_ => _.IsReversedView(viewFrom)))
                {
                    windows.Add(viewFrom);
                }
            }

            var goOn = true;
            var optimizationPass = 1;
            // Todo: solve
            while (goOn)
            {
                goOn = false;
                Console.Error.WriteLine($"Optimization pass #{optimizationPass}.");
                foreach (var view in windows)
                {
                    view.UpdateStatus();
                   goOn|=view.Analyze();
                }

                optimizationPass++;
            }

            Console.Error.WriteLine("Post optimization");
            for (var row = 0; row < size; row++)
            {
                for (var column = 0; column < size; column++)
                {
                    Console.Error.Write(manorFloor[row, column]);
                }
                Console.Error.WriteLine();
            }
            
            var roomList = new List<Room>();
            // identify monsters already identified
            foreach (var room in roomWithMonsters)
            {
                switch (room.Get())
                {
                    case 'Z':
                        monsters[1]--;
                        break;
                    case 'V':
                        monsters[0]--;
                        break;
                    case 'G':
                        monsters[2]--;
                        break;
                    case '.':
                        roomList.Add(room);
                        break;
                }
            }

            Console.Error.WriteLine("Brute Force");

            Solve(monsters, roomList, windows);
            for (var row = 0; row < size; row++)
            {
                for (var column = 0; column < size; column++)
                {
                    Console.Write(manorFloor[row, column]);
                }
                Console.WriteLine();
            }
        }

        private static void Solve(IReadOnlyList<int> monsters, IReadOnlyList<Room> roomWithMonsters,
            IReadOnlyList<View> windows)    
        {
            var monstersList = new StringBuilder();
            monstersList.Append('V', monsters[0]);
            monstersList.Append('Z', monsters[1]);
            monstersList.Append('G', monsters[2]);

            var monsterCombos = GenerateCombination(monstersList.ToString());
            Console.Error.WriteLine("Found {0} combos.", monsterCombos.Count());
            var good = false;
            foreach (var monsterCombo in monsterCombos)
            {
                // fill the manor with monsters
                for (var index = 0; index < roomWithMonsters.Count; index++)
                {
                    roomWithMonsters[index].Set(monsterCombo[index]);
                }

                foreach (var window in windows)
                {
                    window.UpdateStatus();
                }
                good = windows.All(window => window.IsSolved());
                // we check the windows

                if (good)
                    // we found it
                {
                    return;
                }
            }

            if (!good)
            {
                Console.Error.WriteLine("Failed to solve.");
            }
        }

        private static IEnumerable<string> GenerateCombination(string monstersList)
        {
            var result = new List<string>();
            var sample = new string(' ', monstersList.Length).ToCharArray();

            return GenerateSubCombination(monstersList, sample, result, 0);
        }

        private static IEnumerable<string> GenerateSubCombination(string monstersList, char[] sample, 
             ICollection<string> result, int i)
        {
            var monster = monstersList[0];
            monstersList = monstersList.Substring(1);
            for (; i < sample.Length; i++)
            {
                if (sample[i] != ' ')
                    continue;
                sample[i] = monster;
                if (!string.IsNullOrEmpty(monstersList))
                {
                    GenerateSubCombination(monstersList, sample, result, monster == monstersList[0] ? i + 1 : 0);
                }
                else
                {
                    result.Add(new string(sample));
                }
                sample[i] = ' ';
            }
            return result;
        }

        private static View ViewFrom(Direction dir, int row, int col, char[,] map, List<int> views)
        {
            var result = new List<Room>();
            var count = ViewCount(views, row, col, dir);
            var initDir = dir;
            while (row < map.GetLength(0) && col < map.GetLength(1) && row>=0 && col >=0)
            {
                var room = map[row, col];
                if (room == '.')
                {
                    result.Add(new Room(row, col, map, false));
                }
                else
                {
                    // mirror
                    switch (dir)
                    {
                        case Direction.Down:
                            dir = room == '/' ? Direction.Left : Direction.Right;
                            break;
                        case Direction.Left:
                            dir = room == '/' ? Direction.Down : Direction.Up;
                            break;
                        case Direction.Up:
                            dir = room == '/' ? Direction.Right : Direction.Left;
                            break;
                        case Direction.Right:
                            dir = room == '/' ? Direction.Up : Direction.Down;
                            break;
                    }
                    result.Add(new Room(row, col, map, true));
                }
                // we move
                switch (dir)
                {
                    case Direction.Down:
                        row++;
                        break;
                    case Direction.Left:
                        col--;
                        break;
                    case Direction.Up:
                        row--;
                        break;
                    case Direction.Right:
                        col++;
                        break;
                }
            }

            if (count > 0)
            {
                var reverseCount = ViewCount(views, row, col, View.Reverse(dir));
                var view = new View(result, count, reverseCount, initDir, dir);
                Console.Error.WriteLine(view.ToString());
                return view;
            }

            return null;
        }

        private static int Dimension(ICollection views)
        {
            return views.Count / 4;
        }
        
        private static int ViewCount(List<int> views, int row, int col, Direction dir)
        {
            var dim = Dimension(views);
            if (row <= 0)
            {
                switch (dir)
                {
                    case Direction.Down:
                    case Direction.Up:
                        return views[col];
                    case Direction.Left:
                        return views[dim * 3];
                    case Direction.Right:
                        return views[dim * 2];
                }
            }

            if (row >= dim-1)
            {
                switch (dir)
                {
                    case Direction.Down:
                    case Direction.Up:
                        return views[col+dim];
                    case Direction.Left:
                        return views[dim * 4 -1];
                    case Direction.Right:
                        return views[dim * 3 - 1];
                }
            }

            if (col <= 0)
            {
                switch (dir)
                {
                    case Direction.Left:
                    case Direction.Right:
                        return views[dim * 2 + row];
                    case Direction.Up:
                        return views[0];
                    case Direction.Down:
                        return views[dim];
                }
            }
            
            if (col >= dim-1)
            {
                switch (dir)
                {
                    case Direction.Left:
                    case Direction.Right:
                        return views[dim * 3 + row];
                    case Direction.Up:
                        return views[dim - 1];
                    case Direction.Down:
                        return views[dim*2 - 1];
                }
            }

            return -1;
        }
        
        private class Room
        {
            public readonly int Row;
            public readonly int Column;
            private readonly char[,] map;
            public readonly bool IsMirror;

            public Room(int row, int column, char[,] map, bool isMirror)
            {
                Row = row;
                Column = column;
                this.map = map;
                IsMirror = isMirror;
            }

            public override string ToString()
            {
                return $"{Column}:{Row}";
            }

            public void Set(char c)
            {
                map[Row, Column] = c;
            }

            public char Get()
            {
                return map[Row, Column];
            }
        }

        private class View
        {
            private readonly List<Room> roomsInSight;
            private readonly int count;
            private readonly int reverseCount;
            private readonly Direction initDir;
            private readonly Direction finalDir;
            private int _directDirect;
            private int _reverseDirect;
            private int remainingCount;
            private int remainingReverse;
            private int remainingMonsters;

            public View(List<Room> roomsInSight, int count, int reverseCount, Direction initDir, Direction finalDir)
            {
                this.roomsInSight = roomsInSight;
                this.count = count;
                this.reverseCount = reverseCount;
                this.initDir = initDir;
                this.finalDir = finalDir;
                remainingCount = count;
                remainingReverse = reverseCount;
                UpdateStatus();
            }

            public bool IsSolved()
            {
                return remainingMonsters == 0 && remainingCount == 0 && remainingReverse == 0;
            }

            public bool IsReversedView(View other)
            {
                var lastOther = other.roomsInSight[other.roomsInSight.Count - 1];
                var thisFirst = roomsInSight[0];
                return lastOther.Column == thisFirst.Column && lastOther.Row == thisFirst.Row &&
                       DirectionsAreReversed(initDir, other.finalDir);
            }

            private static bool DirectionsAreReversed(Direction first, Direction second)
            {
                return second == Reverse(first);
            }

            public static Direction Reverse(Direction init)
            {
                switch (init)
                {
                    case Direction.Down:
                        return Direction.Up;
                    case Direction.Left:
                        return Direction.Right;
                    case Direction.Up:
                        return Direction.Down;
                    case Direction.Right:
                        return Direction.Left;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(init), init, null);
                }
            }

            public void UpdateStatus()
            {
                var res = 0;
                var mirror = false;
                _directDirect = 0;
                remainingMonsters = 0;
                foreach (var room in roomsInSight)
                {
                    if (room.IsMirror)
                    {
                        mirror = true;
                    }
                    else
                    {
                        switch (room.Get())
                        {
                            case 'Z':
                                res++;
                                break;
                            case 'V':
                                res += mirror ? 0 : 1;
                                break;
                            case 'G':
                                res += mirror ? 1 : 0;
                                break;
                            case '.':
                                remainingMonsters++;
                                if (!mirror)
                                {
                                    _directDirect++;
                                }
                                break;
                        }
                    }

                }

                remainingCount = count - res;
                res = 0;
                mirror = false;
                _reverseDirect = 0;
                for(var i= roomsInSight.Count-1; i>=0; i--)
                {
                    var room = roomsInSight[i];
                    if (room.IsMirror)
                    {
                        mirror = true;
                    }
                    else
                    {
                        switch (room.Get())
                        {
                            case 'Z':
                                res++;
                                break;
                            case 'V':
                                res += mirror ? 0 : 1;
                                break;
                            case 'G':
                                res += mirror ? 1 : 0;
                                break;
                            case '.':
                                if (!mirror)
                                {
                                    _reverseDirect++;
                                }
                                break;
                        }
                    }
                }

                remainingReverse = reverseCount - res;
            }
   
            public bool Analyze()
            {
                if (remainingMonsters == 0 || IsSolved())
                {
                    // no info
                    return false;
                }
                var mid = Math.Max(remainingMonsters - _directDirect - _reverseDirect, 0);
                Console.Error.WriteLine($"{this} split:{_directDirect}:{mid}:{_reverseDirect}.");
                // do we have only invisible monsters?
                if (remainingCount == 0 && remainingReverse == 0)
                {
                    if (_directDirect == _reverseDirect && _reverseDirect == remainingMonsters)
                    {
                        Console.Error.WriteLine($"View contains only Ghosts {remainingMonsters}.");
                        foreach (var room in roomsInSight)
                        {
                            if (room.Get() == '.')
                            {
                                room.Set('G');
                            }
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine($"View contains only Vampires {remainingMonsters}.");
                        foreach (var room in roomsInSight)
                        {
                            if (room.Get() == '.')
                            {
                                room.Set('V');
                            }
                        }

                        return true;
                    }
                }
                // do we have only invisible monsters in Direct?
                else if (remainingCount == 0)
                {
                    Console.Error.WriteLine($"View contains {_directDirect} Ghosts and {remainingMonsters-_directDirect} Vampires.");
                    foreach (var room in roomsInSight)
                    {
                        if (room.IsMirror)
                        {
                            break;
                        }

                        if (room.Get() == '.')
                        {
                            room.Set('G');
                        }
                    }

                    for (var i = roomsInSight.Count - 1; i >= 0; i--)
                    {
                        var room = roomsInSight[i];
                        if (room.IsMirror)
                        {
                            break;
                        }

                        if (room.Get() == '.')
                        {
                            room.Set('V');
                        }
                    }

                    return true;
                }
                // do we have only invisible monsters in Reverse?
                else if (remainingReverse == 0)
                {
                    Console.Error.WriteLine($"View contains {_reverseDirect} Ghosts and {remainingMonsters-_reverseDirect} Vampires.");
                    foreach (var room in roomsInSight)
                    {
                        if (room.IsMirror)
                        {
                            break;
                        }

                        if (room.Get() == '.')
                        {
                            room.Set('V');
                        }
                    }

                    for (var i = roomsInSight.Count - 1; i >= 0; i--)
                    {
                        var room = roomsInSight[i];
                        if (room.IsMirror)
                        {
                            break;
                        }

                        if (room.Get() == '.')
                        {
                            room.Set('G');
                        }
                    }

                    return true;
                }
                else if (mid == 0 && remainingCount == remainingReverse && remainingCount == (_directDirect+_reverseDirect))
                {
                    Console.Error.WriteLine($"View contains {remainingCount} Zombies.");
                    foreach (var room in roomsInSight)
                    {
                        if (room.Get()=='.')
                        {
                            room.Set('Z');
                        }
                    }

                    return true;
                }

                return false;
            }
           
            public override string ToString()
            {
                var builder = new StringBuilder();
                builder.Append($"View from {roomsInSight[0]}({initDir}) to {roomsInSight[roomsInSight.Count-1]}({finalDir})");
                builder.Append($" [direct:{remainingCount}, reverse:{remainingReverse}]");
                builder.Append($" {remainingMonsters} rooms seen.");
                return builder.ToString();
            }
        }

        private enum Direction
        {
            Down,
            Left,
            Up,
            Right
        }
        
    }
}