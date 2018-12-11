using System;
using System.Collections.Generic;

namespace AdventCalendar2018
{
    class Day1
    {
        static void MainDay1(string[] args)
        {
            var changes = new List<int>();
            var dico = new HashSet<int>();
            var freq = 0;
            for(;;)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }
                if (dico.Contains(freq))
                {
                    break;
                }

                dico.Add(freq);
                var change = int.Parse(line);
                changes.Add(change);
                freq += change;
            }

            var goOn = true;
            while (goOn)
            {
                foreach (var change in changes)
                {
                    if (dico.Contains(freq))
                    {
                        goOn = false;
                        break;
                    }
                    dico.Add(freq);
                    freq += change;
                }
                
            }

            Console.WriteLine(freq);
        }
    }
}