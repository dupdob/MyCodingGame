using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChefSept18
{
    public class WarOfXORS
    {
        static void MainXor(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());

            for (var i = 0; i < testCases; i++)
            {
                var N = int.Parse(Console.ReadLine());
                var integers = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                var evenIntegers = integers.Where(x => x % 2 == 0).ToArray();
                var oddIntegers = integers.Where(x => x % 2 == 1).ToArray();
                Console.WriteLine(SearchPairs(evenIntegers) + SearchPairs(oddIntegers));
            }
        }

        static long SearchPairs(int[] integers)
        {
            Dictionary<int, long> reUse = new Dictionary<int, long>();
            foreach (var integer in integers)
            {
                var key = integer >> 2;
                if (!reUse.ContainsKey(key))
                {
                    reUse[key] = 1;
                }
                else
                {
                    reUse[key]+=1;
                }
            }
            var counter = integers.LongLength*(integers.LongLength-1)/2;
            foreach (var reUseValue in reUse.Values)
            {
                if (reUseValue > 1)
                {
                    counter -= reUseValue * (reUseValue-1)/2;
                }
            }
            return counter;
        }
    }
}