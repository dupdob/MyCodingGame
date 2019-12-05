using System;

namespace AdventCalendar2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Day5();
            puzzle.ParseInput();
            //Console.WriteLine($"Answer 1: {puzzle.ComputeAnswer(1)}");
            Console.WriteLine($"Answer 1: {puzzle.ComputeAnswer(5)}");
        }
    }
}