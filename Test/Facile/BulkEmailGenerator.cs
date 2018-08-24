using System;
using System.Text;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * https://www.codingame.com/ide/puzzle/bulk-email-generator
 **/
namespace CodingGame.Facile
{
    static class BulkEmailGenerator
    {
        static void MainEmail(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var choice = 0;
            var line = string.Empty;
            for (var i = 0; i < n; i++)
            {
                line += Console.ReadLine()+Environment.NewLine;
            }

                var builder = new StringBuilder();
                var endParenthesis = -1;
                while (true)
                {
                    var openingParenthesis = line.IndexOf('(', endParenthesis + 1);
                    if (openingParenthesis >= 0)
                    {
                        // static part
                        builder.Append(line.Substring(endParenthesis + 1, openingParenthesis - endParenthesis-1));
                        endParenthesis = line.IndexOf(')', openingParenthesis + 1);
                        if (endParenthesis == -1)
                        {
                            builder.Append(line.Substring(openingParenthesis));
                            break;
                        }
                        else
                        {
                            var options = line.Substring(openingParenthesis + 1,
                                    endParenthesis - openingParenthesis - 1)
                                .Split('|');
                            builder.Append(options[choice++ % options.Length]);
                        }
                    }
                    else
                    {
                        // no more options
                        builder.Append(line.Substring(endParenthesis + 1));
                        break;
                    }
                }
                Console.Write(builder.ToString());
        }
    }
}