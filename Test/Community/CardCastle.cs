using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class CardCastle
{
    static void MainCastle(string[] args)
    {
        int H = int.Parse(Console.ReadLine());
        var prevLine = new string('.', H*2);
        var ok = true;
        for (int i = 0; i < H && ok; i++)
        {
            string S = Console.ReadLine();
            for (var c = 0; c < H*2 && ok; c++)
            {
                var previous = prevLine[c];
                switch (S[c])
                {
                    case '.':
                        ok = previous == '.' && (c == (H * 2 - 1) || S[c + 1] != '\\');
                        break;
                    case '/':
                        ok = c < (H*2 - 1) && S[c + 1] == '\\' && previous !='/';
                        break;
                    case '\\':
                        ok = (c == (H * 2 - 1) || S[c + 1] != '\\') && previous!='\\' ;
                        break;
                }
            }
            prevLine = S;
        }
        Console.WriteLine(ok ? "STABLE" : "UNSTABLE");
    }
}