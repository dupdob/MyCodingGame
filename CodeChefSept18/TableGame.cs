using System;
using System.Linq;
using System.Text;

namespace CodeChefSept18
{
    public class TableGame
    {
        static bool[,] topMatrix;
        static bool[,] leftMatrix;
        
        static void MainTableGame(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var top = Console.ReadLine();
                var left = Console.ReadLine();

                topMatrix = new bool[3, top.Length + 1];
                leftMatrix = new bool[left.Length + 1, 3];
                // fill first line and col
                for (var j = 0; j < top.Length; j++)
                {
                    topMatrix[0, j + 1] = top[j] == '1';
                    if (j < 2)
                    {
                        leftMatrix[0, j + 1] = topMatrix[0, j + 1];
                    }
                }
                for (var j = 0; j < left.Length; j++)
                {
                    leftMatrix[j + 1, 0] = left[j] == '1';
                    if (j < 2)
                    {
                        topMatrix[j + 1, 0] = leftMatrix[j + 1, 0];
                    }
                }
              
                ComputeBorders();
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
                for (var x = 1; x < topMatrix.GetLength(1); x++)
                {
                    topMatrix[y, x] = !topMatrix[y, x - 1] || !topMatrix[y-1, x];
                }
            }
            
            for (var y = 1; y < leftMatrix.GetLength(0); y++)
            {
                for (var x = 1; x < 3; x++)
                {
                    leftMatrix[y, x] = !leftMatrix[y, x - 1] || !leftMatrix[y-1, x];
                }
            }
        }

        private static bool IsAWin(int Y, int X)
        {
            if (Y < 3)
            {
                return topMatrix[Y, X];
            }

            if (X < 3)
            {
                return leftMatrix[Y, X];
            }
            if (X > Y)
            {
                return IsAWin(2, X - Y + 2);
            }
            else
            {
                return IsAWin(Y - X + 2, 2);
            }
        }
    }
}    