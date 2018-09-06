using System;
using System.Collections.Generic;
using System.Text;

/**
You must determine whether a given expression's bracketing can be made valid by flipping them in-place. An expression has a valid bracketing when all the parentheses (), square brackets [], curly braces {} and angle brackets <> are correctly paired and nested.

You can flip a bracketing element in-place by replacing it with its counterpart, e.g. replace a ( with a ), or a > with a <. For example, converting the second parenthesis in the expression below would make it valid:
<{[(abc(]}> → <{[(abc)]}>

(This is a harder version of community puzzle “Brackets, Extended Edition”. You may want to complete that one first.) **/

    /*

     */
namespace CodingGame.Facile
{
    internal class BracketsEnhancedEasy
    {
        private static TimeSpan start;
        static void MainBrackets(string[] args)
        {
            BracketsEnhancedEasy.start = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMilliseconds(200));
            var N = int.Parse(Console.ReadLine());
            var expressions = new string[N];
            for (var i = 0; i < N; i++)
            {
                expressions[i] = Console.ReadLine();
            }
            for (var i = 0; i < N; i++)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var car in expressions[i])
                {
                    char nextChar = (char) 0;
                    switch (car)
                    {
                        case '<':
                        case '>':
                            nextChar = '<';
                            break;
                        case '(':
                        case ')':
                            nextChar = '(';
                            break;
                        case '{':
                        case '}':
                            nextChar = '{';
                            break;
                        case '[':
                        case ']':
                            nextChar = '[';
                            break;
                        default:
                            continue;
                    }
                    builder.Append(nextChar);
                }

                // text
                Console.WriteLine(BracketsEnhancedEasy.SimpleScan(builder.ToString()) ? "true" : "false");
            }
        }

        private static bool SimpleScan(string list)
        {
            var stack = new Stack<char>();
            foreach (var bracket in list)
            {
                if (stack.Count == 0 || stack.Peek() != bracket)
                    stack.Push(bracket);
                else
                    stack.Pop();
            }
            return stack.Count == 0;
        }
    }
}