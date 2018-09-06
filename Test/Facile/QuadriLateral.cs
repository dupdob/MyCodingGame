using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * https://www.codingame.com/ide/puzzle/nature-of-quadrilaterals
 **/
namespace CodingGame.Facile
{
    static class Quadrilateral
    {
        static void MainQuadri(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            for (var i = 0; i < n; i++)
            {
                var inputs = Console.ReadLine().Split(' ');
                var A = inputs[0];
                var xA = int.Parse(inputs[1]);
                var yA = int.Parse(inputs[2]);
                var B = inputs[3];
                var xB = int.Parse(inputs[4]);
                var yB = int.Parse(inputs[5]);
                var C = inputs[6];
                var xC = int.Parse(inputs[7]);
                var yC = int.Parse(inputs[8]);
                var D = inputs[9];
                var xD = int.Parse(inputs[10]);
                var yD = int.Parse(inputs[11]);
                var lengthAB = SquareDist(xA, yA, xB, yB);
                var lengthCD = SquareDist(xC,yC, xD, yD);
                var lengthBC = SquareDist(xB, yB, xC, yC);
                var lengthDA = SquareDist(xD, yD, xA, yA);
                var slopeAB = Slope(xA, yA, xB, yB);
                var slopeBC = Slope(xB, yB, xC, yC);
                var slopeDC = Slope(xD, yD, xC, yC);
                var slopeAD = Slope(xA, yA, xD, yD);
                var isParallelogram = (slopeAB == slopeDC) && (slopeAD == slopeBC);
                var isRhombus = (lengthAB==lengthCD) 
                                && (lengthBC == lengthDA) && (lengthAB == lengthBC) ;
                var isRectangle = isParallelogram && (SquareDist(xA, yA, xC, yC) == SquareDist(xB, yB, xD, yD));

                var isSquare = isRhombus && isRectangle;
                var form = "quadrilateral";
                if (isSquare)
                {
                    form = "square";
                }
                else if (isRectangle)
                {
                    form = "rectangle";
                }
                else if (isRhombus)
                {
                    form = "rhombus";
                }
                else if (isParallelogram)
                {
                    form = "parallelogram";
                }
                Console.WriteLine($"{A}{B}{C}{D} is a {form}.");
            }
    
        }

        private static int SquareDist(int x1, int y1, int x2, int y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        private static double Slope(int x1, int y1, int x2, int y2)
        {
            return (y2 - y1) / (double)(x2 - x1);
        }
    }
}