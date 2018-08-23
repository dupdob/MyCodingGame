using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace CodingGame
{
    class Queen
    {
        static void MainQueen(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            var matrix = new bool[n, n];

            var count = Solve(matrix, 0);
            Console.WriteLine(count);
        }

        private static int Solve(bool[,] matrix, int row)
        {
            if (row == matrix.GetLength(1))
            {
                return 1;
            }
            var count = 0;
            
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                // see if it is possible
                var isPossible = true;
                for (var j = 0; j < row; j++)
                {
                    var dist = row - j;
                    if (!matrix[j, i] && (i < dist || !matrix[j, i - dist]) &&
                        (i+dist > matrix.GetLength(0)-1 || !matrix[j, i + dist])) continue;
                    // col already occupied
                    isPossible = false;
                    break;
                }

                if (!isPossible)
                {
                    continue;
                }

                matrix[row, i] = true;
                count += Solve(matrix, row + 1);
                matrix[row, i] = false;
            }

            return count;
        }

    }
}