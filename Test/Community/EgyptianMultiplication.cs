using System;

/**
 You have to multiply two integers (a&b) by means of a method used in Ancien Egypt, described in Rhind’s hieratic papyrus written circa −1650 by Ahmes. This method is still used in Russia.

First, sort the two numbers.
Then follow the steps below, the algorithm uses base-2 decomposition of the biggest number.
We multiply 12 by 5, here is what you have to print, excepted the comments after hashes.

12 * 5
= 12 * 4 + 12 # Divide 5 by 2, the remain is 1 and 5=2×2+1, thus 12*5=12*(2*2+1)=12*2*2+12=24*2+12
= 24 * 2 + 12 # Divide 2 by 2, 2=1*2+0 and 12*5=24*(1*2+0)+12=48*1+12
= 48 * 1 + 12 # Divide 1 by 2, 1=0*2+1 and 12*5=48*(0*2+1)+12=12+48
= 48 * 0 + 12 + 48 # End of the algorithm
= 60
**/
class EgyptianMultiplication
{
    static void MainMultiplication(string[] args)
    {
        var inputs = Console.ReadLine().Split(' ');
        var a = int.Parse(inputs[0]);
        var b = int.Parse(inputs[1]);
        if (b > a)
        {
            var c = b;
            b = a;
            a = c;
        }
        Console.WriteLine("{0} * {1}", a, b);
        var result = a * b;
        var line = string.Empty;
        while (b > 0)
        {
            if (b % 2 == 1)
            {
                line += " + " + a;
                b -= 1;
            }
            else
            {
                a *= 2;
                b /= 2;
            }
            Console.WriteLine("= {0} * {1}{2}", a, b, line);
        }
        Console.WriteLine("= {0}", result);
    }
}
