using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace CodingGame.Moyen
{
    class FloodFlill
    {
        static void MainFlood(string[] args)
        {
            var W = int.Parse(Console.ReadLine());
            var H = int.Parse(Console.ReadLine());
            var map = new string[H];
            for (int i = 0; i < H; i++)
            {
                map[i] = Console.ReadLine();
            }

            var towers = new Tower[W,H];
            for (int yIndex  = 0; yIndex < H; yIndex++)
            {
                for (int xIndex = 0; xIndex < W; xIndex++)
                {

                   towers[xIndex, yIndex] = new Tower(map[yIndex][xIndex]);
                }
            }
            bool changed;
            do
            {
                changed = false;
                var newTowers = new Tower[W,H];
                for (int yIndex = 0; yIndex < H; yIndex++)
                {
                    var line = string.Empty;
                    for (var xIndex = 0; xIndex < W; xIndex++)
                    {
                        var nextTower = getNextStatus(towers, xIndex, yIndex);
                        line+=nextTower.Name;
                        newTowers[xIndex, yIndex] = nextTower;
                    }

                    if (map[yIndex] != line)
                    {
                        map[yIndex] = line;
                        changed = true;
                    }
                }
                
                towers = newTowers;
                Console.Error.WriteLine("NextFill");
                foreach (var s in map)
                {
                    Console.Error.WriteLine(s);
                }            
                Console.Error.WriteLine("========");
            } while (changed);

            foreach (var s in map)
            {
                Console.WriteLine(s);
            }
        }

        static Tower getNextStatus(Tower[,] map, int X, int Y)
        {
            if (map[X,Y].IsOccupied())
            {
                return map[X,Y];
            }

            Tower result = map[X,Y];
            if (Y > 0)
            {
                var next = map[X,Y-1];
                if (next.IsATower())
                {
                    result = next;
                }
            }

            if (X > 0)
            {
                var next = map[X - 1,Y];
                if (next.IsATower())
                {
                    result = (!result.IsATower()|| result == next) ? next : new Tower('+');
                }
            }

            if (Y < map.GetUpperBound(1))
            {
                var next = map[X, Y+1];
                if (next.IsATower())
                {
                    result = (!result.IsATower()|| result == next) ? next : new Tower('+');
                }
            }

            if (X < map.GetUpperBound(0))
            {
                var next = map[X+1, Y];
                if (next.IsATower())
                {
                    result = (!result.IsATower()|| result == next) ? next : new Tower('+');
                }
            }

            return result;
        }   

        class Tower
        {
            private readonly char name;

            public char Name => name;

            public Tower(char name)
            {
                this.name = name;
            }

            public bool IsOccupied()
            {
                return name!='.';
            }

            public bool IsATower()
            {
                return IsOccupied() && name!='#';
            }
        }
    }
}