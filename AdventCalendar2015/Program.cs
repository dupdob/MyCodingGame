using System;

namespace AdventCalendar2015
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day4();

            Console.WriteLine("Answer 1: {0}", day.Md5Suffix("bgvyzdsv", "00000"));
            Console.WriteLine("Answer 2: {0}", day.Md5Suffix("bgvyzdsv", "000000"));
        }
    }
}