using System;

namespace AdventCalendar2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Day2();

            var original = puzzle.ParseInput(Day2.Input);
            var input =  (int[]) original.Clone();
            input[1] = 12;
            input[2] = 2;
         
            Console.Write("Answer 1: ");
            Console.WriteLine(puzzle.ComputeAnswer(input));

            for (var i = 0; i < 99; i++)
            {
                for (int j = 0; j < 99; j++)
                {
                    input =  (int[]) original.Clone();
                    input[1] = i;
                    input[2] = j;
                    var result = puzzle.ComputeAnswer(input);
                    if (result == 19690720)
                    {
                        Console.WriteLine("Answer 2: {0}", i*100+j);
                        break;
                    }
                }
            }
        }
    }
}