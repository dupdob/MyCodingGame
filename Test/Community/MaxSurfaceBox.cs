using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class MaxSurfaceBox
{
    static void MainSurface(string[] args)
    {
        var N = int.Parse(Console.ReadLine());

        var minSurf = N * 4 + 2;

        for (var i = 1; i <= Math.Sqrt(N); i++)
        {
            if (N%i != 0) continue;
            var face = N / i;
            for (var j = 1; j <= Math.Sqrt(face); j++)
            {
                if (face%j != 0) continue;
                var k = face / j;
                var surface = i * j * 2 + i * k * 2 + j * k * 2;
                if (surface < minSurf)
                {
                    minSurf = surface;
                }
            }
        }
        Console.WriteLine("{0} {1}", minSurf, N * 4 + 2);
    }
}