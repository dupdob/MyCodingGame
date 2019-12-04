using System;

namespace AdventCalendar2015
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day8();
            Console.WriteLine("Answer 1: {0}", day.SumOverhead());
            Console.WriteLine("Answer 2: {0}", day.SumEncodedOverhead());
        }
    }
}