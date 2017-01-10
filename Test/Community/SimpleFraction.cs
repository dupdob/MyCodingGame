using System;
using System.Linq;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class SimpleFraction
{
    static void MainFraction(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            var xY = Console.ReadLine().Split('/').Select(_=> int.Parse(_)).ToList();
            if (xY[1] == 0)
            {
                Console.WriteLine("DIVISION BY ZERO");
            }
            else
            {
                if (xY[1] < 0)
                {
                    xY[0] = -xY[0];
                    xY[1] = -xY[1];
                }
                var intPart = xY[0]/xY[1];
                var fracPart = xY[0] - intPart*xY[1];
                if (intPart != 0)
                {
                    Console.Write(intPart);
                    if (fracPart != 0)
                    {
                        Console.Write(' ');
                    }
                    fracPart = Math.Abs(fracPart);
                }
                if (fracPart != 0)
                {
                    // simplify
                    for (var div = 2; div <= Math.Abs(fracPart); div++)
                    {
                        if (fracPart%div == 0 && xY[1]%div == 0)
                        {
                            fracPart /= div;
                            xY[1] /= div;
                            div--;
                        }
                    }
                    Console.Write("{0}/{1}", fracPart, xY[1]);
                }
                else if (intPart==0)
                {
                    Console.Write('0');
                }
                Console.WriteLine();
            }
        }
    }
}