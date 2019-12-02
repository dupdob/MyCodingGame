using System;

namespace AdventCalendar2015
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day3();

            day.ParseInput();
            Console.WriteLine("Answer 1: {0}", day.VisitedHouses());
            Console.WriteLine("Answer 2: {0}", day.VisitedHousesWithRobot());
        }
    }
}