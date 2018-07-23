using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class River
{
    static void MainRiver(string[] args)
    {
        long r1 = long.Parse(Console.ReadLine());
        long r2 = long.Parse(Console.ReadLine());

        while (r1 != r2)
        {
            if (r1 < r2)
            {
                r1 = Next(r1);
            }
            else
            {
                r2 = Next(r2);
            }
        }
        Console.WriteLine(r1);
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