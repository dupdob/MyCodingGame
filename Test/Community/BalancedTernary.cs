using System;

/**
Balanced ternary(3 base) is a non-standard positional numeral system. In the standard (unbalanced)
    ternary system, digits have values 0, 1 and 2. The digits in the balanced ternary system have values −1, 0, and 1.
  We use letter T to represent -1, so the digits are (T, 0, 1).

E.g.: 1T0 = 1 * (3**2) + (-1)*(3**1) + 0*(3**0) = 9 - 3 + 0 = 6

You must convert input integer (decimal) number to its balanced ternary representation.
 **/
class BalancedTernary
{
    static void MainTernary(string[] args)
    {
        var DEC = int.Parse(Console.ReadLine());
        var output = "";
        do
        {
            var digit = DEC%3;
            var nextChar = ' ';
            switch (digit)
            {
                case 2:
                case -1:
                    nextChar = 'T';
                    DEC += 1;
                    break;
                case -2:
                    nextChar = '1';
                    DEC -= 3;
                    break;
                default:
                    nextChar = digit.ToString()[0];
                    DEC -= digit;
                    break;
            }
            output = nextChar + output;
            DEC /= 3;
        } while (DEC!=0);

        Console.WriteLine(output);
    }
}