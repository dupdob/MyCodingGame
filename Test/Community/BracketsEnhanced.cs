using System;
using System.Collections.Generic;

/**
You must determine whether a given expression's bracketing can be made valid by flipping them in-place. An expression has a valid bracketing when all the parentheses (), square brackets [], curly braces {} and angle brackets <> are correctly paired and nested.

You can flip a bracketing element in-place by replacing it with its counterpart, e.g. replace a ( with a ), or a > with a <. For example, converting the second parenthesis in the expression below would make it valid:
<{[(abc(]}> → <{[(abc)]}>

(This is a harder version of community puzzle “Brackets, Extended Edition”. You may want to complete that one first.) **/

    /*

     */
class BracketsEnhanced
{
    private static TimeSpan start;
    class Bracket
    {
        public char Marker;
        public int Count;
    }
    static void Main(string[] args)
    {
        BracketsEnhanced.start = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMilliseconds(200));
        var N = int.Parse(Console.ReadLine());
        var expressions = new string[N];
        for (var i = 0; i < N; i++)
        {
            expressions[i] = Console.ReadLine();
        }
        for (var i = 0; i < N; i++)
        {
            var simplified = new List<Bracket>();
            var nbParenth = 0;
            var nbBrack = 0;
            var nbCurly = 0;
            var nbAngle = 0;
            var next = expressions[i];
            var counts = new Dictionary<char, int>
            {
                ['<'] = 0,
                ['('] = 0,
                ['{'] = 0,
                ['['] = 0
            };
            Bracket lastBracket = null;
            foreach (var car in next)
            {
                char nextChar = (char)0;
                switch (car)
                {
                    case '<':
                    case '>':
                        nextChar = '<';
                        nbAngle++;
                        break;
                    case '(':
                    case ')':
                        nextChar = '(';
                        nbParenth++;
                        break;
                    case '{':
                    case '}':
                        nextChar = '{';
                        nbCurly++;
                        break;
                    case '[':
                    case ']':
                        nextChar = '[';
                        nbBrack++;
                        break;
                    default:
                        continue;
                }
                counts[nextChar]++;
                if (lastBracket!=null && lastBracket.Marker==nextChar)
                {
                    lastBracket.Count++;
                }
                else
                {
                    lastBracket = new Bracket
                    {
                        Marker = nextChar,
                        Count = 1
                    };
                    simplified.Add(lastBracket);
                }
            }

            // fast check. no need to parse if trivially not possible
            if (nbAngle % 2 == 1 || nbParenth % 2 == 1 || nbCurly % 2 == 1 || nbBrack % 2 == 1)
            {
                Console.WriteLine("false");
            }
            else
            {
                // text
                var brackets = new Stack<Bracket>();
                var current = new Dictionary<char, int>(4)
                {
                    ['<'] = 0,
                    ['('] = 0,
                    ['{'] = 0,
                    ['['] = 0
                };
                var j = 0;

                Console.WriteLine(BracketsEnhanced.ScanBrackets(j, simplified, brackets, current, counts) ? "true" : "false");
            }
        }
    }

    private static bool ScanBrackets(int j, IReadOnlyList<Bracket> next, Stack<Bracket> brackets, IDictionary<char, int> current, IDictionary<char, int> counts)
    {
        if (j >= next.Count)
        {
            return brackets.Count == 0;
        }
        if (DateTime.Now.TimeOfDay > BracketsEnhanced.start)
            return false;
        var nextBrackets = next[j];
        var curBracket = 0;
        j++;
        var marker = nextBrackets.Marker;
        counts[marker] -= nextBrackets.Count;
        var sameasCurrent = brackets.Count > 0 && (brackets.Peek().Marker == marker);
        if (sameasCurrent)
        {
            curBracket = brackets.Peek().Count;
        }
        var currentDepth = current[marker];
        bool popped = false, pushed = false;
        var init = Math.Max((curBracket+nextBrackets.Count) % 2, curBracket-nextBrackets.Count);

        for (var i = init; i <= curBracket + nextBrackets.Count; i += 2)
        {
            current[marker] = currentDepth - curBracket + i;
            if (current[marker] > counts[marker])
            {
                // not enought marker left

                break;
            }
            if (i == 0)
            {
                if (sameasCurrent)
                {
                    // remove all brackes
                    brackets.Pop();
                    popped = true;
                }
            }
            else
            {
                if (brackets.Count == 0 || (brackets.Peek().Marker != marker))
                {
                    var temp = new Bracket {Marker = marker};
                    brackets.Push(temp);
                    popped = false;
                    pushed = true;
                }
                brackets.Peek().Count = i;
            }
            if (BracketsEnhanced.ScanBrackets(j, next, brackets, current, counts))
            {
                return true;
            }
        }
        if (pushed && !sameasCurrent)
        {
            brackets.Pop();
        }
        else if (popped)
        {
            var restore = new Bracket {Marker = marker, Count = curBracket};
            brackets.Push(restore);
        }
        else if (sameasCurrent)
        {
            brackets.Peek().Count = curBracket;
        }
        counts[marker] += nextBrackets.Count;
        current[marker] = currentDepth;
        return false;
    }
}