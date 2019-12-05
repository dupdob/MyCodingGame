using System;

namespace AdventCalendar2015
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var day = new Day9();
            day.Parse();
            Console.WriteLine("Answer 1: {0}", day.FindShortestPath());
            Console.WriteLine("Answer 2: {0}", day.FindLongestPath());
        }
    }
}