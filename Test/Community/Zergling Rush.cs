using System;

/**
 In the popular real-time strategy game called Starcraft, there are three races: Terran, Protoss, and Zerg. Many Zerg players use a "cheese" strategy called the "zergling rush", where very early in the game, they produce a couple of their basic combat unit called the "zergling", and immediately attack the enemy base.

The enemy base is represented as a grid, with buildings occupying different shapes. Additionally, there can be some impassable terrain blocking zerglings. Zerglings will surround all enemy buildings they can reach, taking up horizontally, vertically, and diagonally adjacent cells. However they cannot access locations completely blocked off by buildings or terrain. Note that they will only be able to enter horizontal or vertical gaps: if buildings are diagonally adjacent they will block the zerglings.

Your task is to visualize what the base will look like after the zerglings arrive.

Note 1: zerglings enter from all sides (top, left, right, bottom).
Note 2: if no building can be reached or if there are no buildings at all, the zerglings will not stay (no zerglings should be included in the output).

For example, a base like:

...#####
...#...#
..B..B.#
...#...#
...#####

will become

...#####
.zz#...#
.zB..B.#
.zz#...#
...#####
 **/
class ZerglingRush
{
    static void Main2(string[] args)
    {
        var inputs = Console.ReadLine().Split(' ');
        var W = int.Parse(inputs[0]);
        var H = int.Parse(inputs[1]);

        var map = new char[W,H];
        for (var i = 0; i < H; i++)
        {
            var ROW = Console.ReadLine();
            for (var j = 0; j < W; j++)
            {
                map[j, i] = ROW[j];
            }
        }

        // now all cells that are reachable from the edges. We use a 'fillpaint' like approach, iterative until no further progress observed
        var progress = true;
        var paintedMap = new bool[W,H];
        while(progress)
        {
            progress = false;
            for(var y=0; y<H; y++)
            {
                for (var x = 0; x < W; x++)
                {
                    if (map[x, y] != '.' || paintedMap[x, y])
                    {
                        // visited or not free
                        continue;
                    }
                    if (x != 0 && y != 0 && x != W - 1 && y != H - 1 && !paintedMap[x - 1, y] && !paintedMap[x + 1, y] &&
                        !paintedMap[x, y - 1] && !paintedMap[x, y + 1])
                    {
                        // not an edge or next to a painted cell
                        continue;
                    }
                    progress = true;
                    paintedMap[x, y] = true;
                }
            }
        }
        // now we change cells around a Base
        for (var y = 0; y < H; y++)
        {
            for (var x = 0; x < W; x++)
            {
                if (map[x, y] != 'B')
                {
                    // not a base, don't care
                    continue;
                }
                // paint visited neighbors
                for (var i = Math.Max(0, x - 1); i < Math.Min(W, x + 2); i++)
                {
                    for (var j = Math.Max(0, y - 1); j < Math.Min(H, y + 2); j++)
                    {
                        if (paintedMap[i, j] && map[i,j] == '.')
                        {
                            map[i, j] = 'z';
                        }
                    }
                }
            }
        }
        // now we can print the output
        for (var y = 0; y < H; y++)
        {
            for (var x = 0; x < W; x++)
            {
                Console.Write(map[x,y]);
            }
            Console.WriteLine();
        }
    }
}