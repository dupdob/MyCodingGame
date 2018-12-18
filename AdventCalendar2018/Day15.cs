using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar2018
{
    public static class Day15
    {
        private static void Main()
        {
            var units = new List<Unit>();
            var map = Input.Split(Environment.NewLine);
            // identify units
            for (var yIndex = 0; yIndex < map.Length; yIndex++)
            {
                for (var xIndex = 0; xIndex < map[yIndex].Length; xIndex++)
                {
                    if (map[yIndex][xIndex] == 'E' || map[yIndex][xIndex] == 'G') units.Add(new Unit(xIndex, yIndex, map[yIndex][xIndex]));
                }
            }

            while (true)
            {
                units.Sort();
                foreach (var unit in units)
                {
                    unit.MoveToClosest(map);
                }
            }
        }
        
        private class Unit : IComparable<Unit>
        {
            public int CompareTo(Unit other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                var yComparison = y.CompareTo(other.y);
                return yComparison != 0 ? yComparison : x.CompareTo(other.x);
            }

            private int Id;
            private static int autoId;
            private int power = 3;
            private int health = 200;
            private int x;
            private int y;
            private char type;

            public Unit(int x, int y, char type)
            {
                this.x = x;
                this.y = y;
                this.type = type;
                Id = autoId++;
            }

            public void MoveToClosest(string[] map)
            {
                // build list of accessible cells
                var cells = new List<Cell>();
                var inRangeCells = new List<Cell>();
                var altMap = new Cell[map.Length, map[0].Length];
                for (var i = 0; i < map.Length; i++)
                {
                    var line = map[i];
                    for (var j = 0; j < line.Length; j++)
                    {
                        if (line[j] == '.')
                        {
                            var nextCell =  new Cell(j, i, int.MaxValue);
                            altMap[i, j] = nextCell;
                            cells.Add(nextCell);
                        }
                    }
                }

                var start = new Cell(x ,y , 0);
                altMap[y, x] = start;
                cells.Add(start);
                var enemy = type == 'G' ? 'E' : 'G';
                while (cells.Count > 0)
                {
                    var min = cells.Aggregate((current, next) => next.dist < current.dist ? next : current);
                    if (min.dist == int.MaxValue)
                    {
                        // cell is unreachable!
                        break;
                    }
                    // there is a border around the playing field, no need to have boundary check, 
                    Check(altMap, min, min.x, min.y - 1);
                    Check(altMap, min, min.x - 1, min.y);
                    Check(altMap, min, min.x + 1, min.y);
                    Check(altMap, min, min.x, min.y + 1);

                    cells.Remove(min);
                    altMap[min.y, min.x] = null;
                    // keep only cells in range
                    if (map[min.y - 1][min.x] == enemy ||
                        map[min.y][min.x - 1] == enemy ||
                        map[min.y][min.x + 1] == enemy ||
                        map[min.y + 1][min.x] == enemy)
                    {
                        inRangeCells.Add(min);
                    }
                }
                var closests = new List<Cell>();
                var closest = inRangeCells.Aggregate((current, next) =>
                {
                    if (closests.Count == 0)
                    {
                        closests.Add(current);
                    }
                    if (next.dist > current.dist)
                    {
                        return current;
                    }

                    if (next.dist < current.dist)
                    {
                        closests.Clear();
                    }
                    closests.Add(next);
                    return next;
                });
                closest = closests.Min();
                DumpPath(map, start, closest);

            }

            private static void DumpPath(string[] map, Cell start, Cell destination)
            {
                if (destination.dist == int.MaxValue)
                {
                    Console.WriteLine("No accessible range");
                    return;
                }
                var mapCopy = (string[])map.Clone();
                Console.WriteLine($"Path from {start.x}:{start.y} to {destination.x}:{destination.y}.");
                // trace path for debug purpose
                while (destination!=null && destination!=start)
                {
                    var line=mapCopy[destination.y];
                    mapCopy[destination.y] = line.Substring(0, destination.x) + '*' + line.Substring(destination.x + 1);
                    destination = destination.predecessor;
                }

                foreach (var line in mapCopy)
                {
                    Console.WriteLine(line);
                }
            }
            
            private static void Check(Cell[,] map, Cell current, int x, int y)
            {
                var neighbour = map[y, x];
                if (neighbour == null || neighbour.dist < current.dist + 1)
                {
                    // no neighbour
                    return;
                }

                if (neighbour.dist == current.dist + 1 && neighbour.predecessor != null &&
                    neighbour.predecessor.CompareTo(current) < 0)
                {
                    return;
                }
                neighbour.dist = current.dist + 1;
                neighbour.predecessor = current;
            }    
        }

        private class Cell : IComparable<Cell>
        {
            public int CompareTo(Cell other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                var yComparison = y.CompareTo(other.y);
                if (yComparison != 0) return yComparison;
                return x.CompareTo(other.x);
            }

            public readonly int x;
            public readonly int y;
            public int dist;
            public Cell predecessor;

            public Cell(int x, int y, int dist)
            {
                this.x = x;
                this.y = y;
                this.dist = dist;
            }
        }

        private const string Demo =
@"#######
#.E...#
#.....#
#...G.#
#######";

        private const string Input = 
@"################################
##########G###.#################
##########..G#G.G###############
##########G......#########...###
##########...##.##########...###
##########...##.#########G..####
###########G....######....######
#############...............####
#############...G..G.......#####
#############.............######
############.............E######
######....G..G.........E....####
####..G..G....#####.E.G.....####
#####...G...G#######........####
#####.......#########........###
####G.......#########.......####
####...#....#########.#.....####
####.#..#...#########E#..E#..###
####........#########..E.#######
###......#..G#######....########
###.......G...#####.....########
##........#............#########
#...##.....G......E....#########
#.#.###..#.....E.......###.#####
#######................###.#####
##########.......E.....###.#####
###########...##........#...####
###########..#####.............#
############..#####.....#......#
##########...######...........##
#########....######..E#....#####
################################";
    }
}