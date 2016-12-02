using System;
using System.Linq;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class GreatestNumber
{
    static void MainNumber(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        string input = Console.ReadLine();
        bool neg = input.Contains('-');
        bool dot = input.Contains('.');
        var digits = input.Replace("-", "").Replace(".", "").Replace(" ","").ToCharArray();
        Array.Sort(digits);
        if (!neg)
        {
            Array.Reverse(digits);
        }
        var result = new string(digits);
        if (neg)
        {
            if (result.EndsWith("0"))
            {
                result = "0";
            }
            else if (dot)
            {
                result = "-" + result.Substring(0, 1) + "." + result.Substring(1);
            }
            else
            {
                result = "-" + result;
            }
        }
        else
        {
            if (dot)
            {
                result = result.Substring(0, result.Length - 1) + "." + result.Substring(result.Length - 1);
                if (result.EndsWith(".0"))
                {
                    result = result.Substring(0, result.Length - 2);
                }
            }
        }
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(result);
    }
}