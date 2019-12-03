using System;

namespace AdventCalendar2015
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day7();
            Console.WriteLine("Answer 1: {0}", day.FindSignal_a());
            Console.WriteLine("Answer 2: {0}", day.FindSignal_aWhenForcing());
        }
    }
}