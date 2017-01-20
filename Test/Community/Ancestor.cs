using System;
using System.Linq;
using System.Collections.Generic;

/**
Print the names of a family's descendants.

An individual in the family is represented by a line of input. The parent/child relationship of that individual is determined by the number of dots preceding his or her name.

Each dot represents a previously mentioned ancestor in the family tree. So if a name is preceded by two dots, then the first dot represents the most recently mentioned name with zero dots before it, and the second dot represents the most recently mentioned name with a single dot before it.

An example set of input lines to represent a family would be:

Jade
.Andrew
..Rose
.Mark
Heidi

The explanation for this input is:

Jade is a grandfather.
Andrew is Jade's son.
Rose is Andrew's daughter.
Mark is Jade's son.
Heidi has no children.

The correct output lines to represent this family's descendants would be:

Jade > Andrew > Rose
Jade > Mark
Heidi **/

class Ancestor
{
    static void MainAncestor(string[] args)
    {
        int count = int.Parse(Console.ReadLine());
        var parents = new Stack<string>();
        for (int i = 0; i < count; i++)
        {
            string line = Console.ReadLine();
            var depth = 0;
            while (line[0] == '.')
            {
                line = line.Substring(1);
                depth++;
            }
//            Console.Error.WriteLine("{0} {1}", depth, line);
            if (depth >= parents.Count)
            {
                parents.Push(line);
            }
            else
            {
                var output = string.Empty;

                var reversed = parents.ToArray().Reverse();
                foreach (var parent in reversed)
                {
                    if (output.Length > 0)
                    {
                        output += " > ";
                    }
                    output += parent;
                }
                Console.WriteLine(output);
                while (depth < parents.Count)
                {
                    parents.Pop();
                } 
                parents.Push(line);
            }
        }
        var end = string.Empty;
        var reversedEnd = parents.ToArray().Reverse();
        foreach (var parent in reversedEnd)
        {
            if (end.Length > 0)
            {
                end += " > ";
            }
            end += parent;
        }
        Console.WriteLine(end);
    }
}