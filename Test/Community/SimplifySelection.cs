using System;
using System.Linq;
using System.Text;

namespace CodingGame.Community
{
    class SimplifySelection
    {
        static void MainSimplify()
        {
            string N = Console.ReadLine().Replace("[","").Replace("]","");
            var values = N.Split(',').Select( x => int.Parse(x)).OrderBy(x => x).ToList();
            var output = new StringBuilder();
            for (var index = 0; index < values.Count; index++)
            {
                if (index > 0)
                    output.Append(',');
                if (index < values.Count - 2 && values[index+2] == values[index+1]+1 && values[index+1] == values[index]+1)
                {
                    var start = values[index];
                    while (index < values.Count - 1 && values[index + 1] == values[index] + 1 )
                    {
                        index++;
                    }
                    output.AppendFormat("{0}-{1}", start, values[index]);
                }
                else
                {
                    output.Append(values[index].ToString());
                }
            }
            Console.WriteLine(output);
        }
    }
}
