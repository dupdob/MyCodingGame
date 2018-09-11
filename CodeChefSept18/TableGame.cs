using System;
using System.Linq;
using System.Text;

namespace CodeChefSept18
{
    public class TableGame
    {
        static bool[,] matrix;
        private static int lastX;
        private static int lastY;
        
        static void Main(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var top = Console.ReadLine();
                var left = Console.ReadLine();
                lastX = 0;
                lastY = 0;

                matrix = new bool[left.Length + 1, top.Length + 1];
                // fill first line and col
                for (var j = 0; j < top.Length; j++)
                {
                    matrix[0, j + 1] = top[j] == '1';
                }
                for (var j = 0; j < left.Length; j++)
                {
                    matrix[j + 1, 0] = left[j] == '1';
                }
              
//                ComputeBorders();
                var q = int.Parse(Console.ReadLine());
                var text = new StringBuilder();
                for (var j = 0; j < q; j++)
                {
                    var pars = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                    
                    text.Append(IsAWin(pars[0], pars[1]) ? '1':'0');
                }
                Console.WriteLine(text);
            }
            
        }

        private static void ComputeBorders()
        {
            for (var y = 1; y < 3; y++)
            {
                for (var x = 1; x < matrix.GetLength(1); x++)
                {
                    matrix[y, x] = !matrix[y, x - 1] || !matrix[y-1, x];
                }
            }
            for (var y = 3; y < matrix.GetLength(0); y++)
            {
                for (var x = 1; x < 3; x++)
                {
                    matrix[y, x] = !matrix[y, x - 1] || !matrix[y-1, x];
                }
            }
        }

        private static bool IsAWin(int Y, int X)
        {
            if (X > lastX || Y > lastY)
            {
                for (var j = lastX+1; j <= X; j++)
                {
                    for (var k = 1; k <= lastY; k++)
                    {
                        matrix[k, j] = !matrix[k, j - 1] || !matrix[k-1, j];
                    }
                }
                lastX = Math.Max(lastX, X);
                for (var k = lastY+1; k <= Y; k++)
                {
                    for (var j = 1; j <= lastX; j++)
                    {
                        matrix[k, j] = !matrix[k, j - 1] || !matrix[k-1, j];
                    }
                }
                lastY = Math.Max(lastY, Y);
            }
            return matrix[Y, X];
        }
    }
}    