using System;

namespace AdventCalendar2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Day4();
            var count = 0;
            for (var i = 347312; i <= 805915; i++)
            {
                if (puzzle.IsValidPassword(i, false))
                {
                    count++;
                }
            }
            
            Console.WriteLine($"Answer 1: {count}");
             count = 0;
            for (var i = 347312; i <= 805915; i++)
            {
                if (puzzle.IsValidPassword(i, true))
                {
                    count++;
                }
            }
            Console.WriteLine($"Answer 2: {count}");
        }
    }
}