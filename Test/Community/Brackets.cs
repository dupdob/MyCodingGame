using System;
using System.Collections.Generic;

/**
Dire si l'expression donnée en entrée est bien parenthésée. Une expression est bien parenthésée si les parenthèses (), les crochets [] et les accolades {} sont correctement appairés.

L'expression ne contient pas d'espaces.
/*

 */
class Brackets
{
    static void MainBrackets(string[] args)
    {
        var expressions = Console.ReadLine();
        var stack = new Stack<char>();
        var good = true;
        foreach (var car in expressions)
        {
            switch (car)
            {
                case '(':
                case '{':
                case '[':
                    stack.Push(car);
                    break;
                case ')':
                    if (stack.Count == 0 || stack.Pop()!='(')
                    {
                        good = false;
                    }
                    break;
                case '}':
                    if (stack.Count == 0 || stack.Pop()!='{')
                    {
                        good = false;
                    }
                    break;
                case ']':
                    if (stack.Count == 0 || stack.Pop()!='[')
                    {
                        good = false;
                    }
                    break;
            }
        }
        if (stack.Count > 0)
            good = false;
        // text
        Console.WriteLine(good ? "true" : "false");
    }

}