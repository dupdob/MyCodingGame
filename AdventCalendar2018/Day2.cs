using System;
using System.Collections.Generic;

namespace AdventCalendar2018
{
    public static class Day2
    {
        private static int nbSingleRepeat;
        private static int nbDoubleRepeat;
        private static void MainDay2One()
        {
            for(;;)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                ScanLine(line);
            }
            Console.WriteLine(nbSingleRepeat*nbDoubleRepeat);
        }
        
        private static List<string> seen = new List<string>();
        private static void MainDay2()
        {
            var answer = string.Empty;
            while(string.IsNullOrEmpty(answer))
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }
                foreach (var other in seen)
                {
                    var diffPos = -1;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (other[i] != line[i])
                        {
                            if (diffPos != -1)
                            {
                                diffPos = -1;
                                break;
                            }

                            diffPos = i;
                        }
                    }

                    if (diffPos>=0)
                    {
                        Console.WriteLine("Found");
                        answer = line.Substring(0, diffPos) + line.Substring(diffPos + 1);
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(answer))
                {
                    break;
                }
                seen.Add(line);
             }
            Console.WriteLine(answer);
        }

        private static void ScanLine(string line)
        {
            var letterCounts = new Dictionary<char, int>();
            foreach (var car in line)
            {
                if (!letterCounts.ContainsKey(car))
                {
                    letterCounts[car] = 1;
                }
                else
                {
                    letterCounts[car]++;
                }
            }

            if (letterCounts.ContainsValue(2))
            {
                nbSingleRepeat++;
            }

            if (letterCounts.ContainsValue(3))
            {
                nbDoubleRepeat++;
            }
        }
    }
}