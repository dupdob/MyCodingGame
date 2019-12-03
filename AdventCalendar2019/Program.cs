using System;

namespace AdventCalendar2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Day3();

            puzzle.Parse();
            
            Console.Write("Answer 1: ");
            Console.WriteLine(puzzle.FindCloserIntersection());
            Console.Write("Answer 2: ");
            Console.WriteLine(puzzle.FindShortestPathToIntersection());
        }
    }
}