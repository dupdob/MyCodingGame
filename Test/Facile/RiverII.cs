using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class RiverII
{
    static void Main(string[] args)
    {
        long r1 = long.Parse(Console.ReadLine());
        int foundPrecedent = 1;

        for (var i = r1 - 1;i > 0; i--)
        {
            if (Next(i) == r1)
            {
                Console.WriteLine("YES");
                return;
            }
        }
        Console.WriteLine("NO");
    }

    static long Next(long prev)
    {
        var ret = prev;
        while (prev!=0)
        {
            ret += prev % 10;
            prev /= 10;
        }
        return ret;
    }
}