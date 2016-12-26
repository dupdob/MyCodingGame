using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class HauntedManor
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
     */
    static void MainHaunted()
    {
        // monster distribution
        var monsters = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
        // siez of manor
        var size = int.Parse(Console.ReadLine());
        // number of monsters seen from windows
        var views = new List<int>();
        views.AddRange(Console.ReadLine().Split(' ').Select(x => int.Parse(x)));
        views.AddRange(Console.ReadLine().Split(' ').Select(x => int.Parse(x)));
        views.AddRange(Console.ReadLine().Split(' ').Select(x => int.Parse(x)));
        views.AddRange(Console.ReadLine().Split(' ').Select(x => int.Parse(x)));
        // mirror locations
        var manorFloor = new char[size, size];
        var roomWithMonsters = new List<Room>(size * size);
        for (var row = 0; row < size; row++)
        {
            var line = Console.ReadLine().ToCharArray();
            for (int column = 0; column < size; column++)
            {
                manorFloor[row, column] = line[column];
                if (line[column] == '.')
                {
                    roomWithMonsters.Add(new Room(row, column, manorFloor));
                }
                Console.Error.Write(line[column]);
            }
            Console.Error.WriteLine();
        }
        // build room seen from windows
        var windows = new List<List<Room>>(4*size);
        for (var col = 0; col < size; col++)
        {
            windows.Add(ViewFrom(0, 0, col, manorFloor));
        }
        for (var col = 0; col < size; col++)
        {
            windows.Add(ViewFrom(2, size - 1, col, manorFloor));
        }
        for (var row = 0; row < size; row++)
        {
            windows.Add(ViewFrom(3, row, 0, manorFloor));
        }
        for (var row = 0; row < size; row++)
        {
            windows.Add(ViewFrom(1, row, size-1, manorFloor));
        }
        // dump room list
        foreach (var rooms in windows)
        {
//            Console.Error.Write("View: ");
            foreach (var room in rooms)
            {
//                Console.Error.Write("{0}, ", room);
            }
//            Console.Error.WriteLine("end");
        }

        // Todo: solve
        Solve(monsters, roomWithMonsters, windows, views);
        for (var row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                Console.Write(manorFloor[row, column]);
            }
            Console.WriteLine();
        }
    }

    private static void Solve(IReadOnlyList<int> monsters, List<Room> roomWithMonsters, List<List<Room>> windows, List<int> views)
    {
        var monstersList = new StringBuilder();
        monstersList.Append('V', monsters[0]);
        monstersList.Append('Z', monsters[1]);
        monstersList.Append('G', monsters[2]);
        var monsterCombos = GenerateCombination(monstersList.ToString());
        Console.WriteLine("Found {0} combos.", monsterCombos.Count());
        foreach (var monsterCombo in monsterCombos)
        {
//            Console.Error.WriteLine(monsterCombo);
            // fill the manor with monsters
            for (var index = 0; index < roomWithMonsters.Count; index++)
            {
                roomWithMonsters[index].Set(monsterCombo[index]);
            }
            var good = true;
            // we check the windows
            for (var index = 0; index < views.Count; index++)
            {
                var view = windows[index];
                var seen = 0;
                var mirror = false;
                foreach (var room in view)
                {
                    if (room == null)
                        mirror = true;
                    else
                    {
                        switch (room.Get())
                        {
                            case 'V':
                                if (!mirror)
                                    seen++;
                                break;
                            case 'G':
                                if (mirror)
                                    seen++;
                                break;
                            case 'Z':
                                seen++;
                                break;
                        }
                    }
                    if (seen > views[index])
                        break;
                }
                if (seen != views[index])
                {
                    good = false;
                    break;
                }
            }
            if (good)
                // we found it
                return;
        }
    }

    private static IEnumerable<string> GenerateCombination(string monstersList)
    {
        var result = new List<string>();
        var sample = new string(' ', monstersList.Length).ToCharArray();

        return GenerateSubCombination(monstersList, sample, result, 0);
    }

    private static IEnumerable<string> GenerateSubCombination(string monstersList, char[] sample, List<string> result, int i)
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

    private static List<Room> ViewFrom(int dir, int row, int col, char[,] map)
    {
        var result = new List<Room>();
        var mirror = false;
        while (row < map.GetLength(0) && col < map.GetLength(1) && row>=0 && col >=0)
        {
            var room = map[row, col];
            if (room == '.')
            {
                result.Add(new Room(row, col, map));
            }
            else
            {
                // mirror
                switch (dir)
                {
                    case 0:
                        dir = room == '/' ? 1 : 3;
                        break;
                    case 1:
                        dir = room == '/' ? 0 : 2;
                        break;
                    case 2:
                        dir = room == '/' ? 3 : 1;
                        break;
                    case 3:
                        dir = room == '/' ? 2 : 0;
                        break;
                }
                if (!mirror)
                {
                    mirror = true;
                    result.Add(null);
                }
            }
            // we move
            switch (dir)
            {
                case 0:
                    row++;
                    break;
                case 1:
                    col--;
                    break;
                case 2:
                    row--;
                    break;
                case 3:
                    col++;
                    break;
            }
        }

        return result;
    }

    class Room
    {
        public int Row;
        public int Column;
        private char[,] map;

        public Room(int row, int column, char[,] map)
        {
            Row = row;
            Column = column;
            this.map = map;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Row, Column);
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
}