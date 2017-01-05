using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 Find the nth term in the sequence starting with S(0) = start and defined by the rule:

Given a term in the sequence, S(i), the next term, S(i+1) can be found by counting the letters (ignoring whitespace) in the spelled-out binary representation of S(i).

As an example, starting from 5 (S(0) = 5), we convert to the binary representation, 101, then spell it out as an English string "one zero one", and count the letters which yields 10 (S(1) = 10).
 **/
class LettersinBin
{
    static void MainLetters(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        long start = long.Parse(inputs[0]);
        long n = long.Parse(inputs[1]);
        var cache = new Dictionary<long, long>();
        var zero = 4;
        var one = 3;

        for (var i = 0; i < n; i++)
        {
            if (!cache.ContainsKey(start))
            {
                cache[start] = ComputeLetters(start, zero, one);
            }
            Console.Error.WriteLine("{0} =>{1}", start, cache[start]);
            if (start == cache[start])
            {
                break;
            }
            start = cache[start];
        }

        Console.WriteLine(start);
    }

    private static int ComputeLetters(long start, int zero, int one)
    {
        var next = 0;
        if (start == 0)
        {
            next = zero;
        }
        else
        {
            while (start > 0)
            {
                next += (start % 2 == 0) ? zero : one;
                start /= 2;
            }
        }
        return next;
    }
}