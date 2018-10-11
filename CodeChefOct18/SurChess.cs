//#define TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeChefOct18
{
    public class SurChess
    {
        static void MainChess(string[] args)
        {
#if TEST
            var rnd = new Random();
            var N = 200;
            var M = 200;
            var board = new string[N];
            for (int i = 0; i < N; i++)
            {
                var line = new StringBuilder(M);
                for (int j = 0; j < M; j++)
                {
                    line.Append(rnd.NextDouble() >= 0.5 ? '1' : '0');
                }
                board[i] = line.ToString();
            }

            var start = Environment.TickCount;
#else
            var inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var N = inputs[0];
            var M = inputs[1];
            var board = new string[N];
            for (int i = 0; i < N; i++)
            {
                board[i] = Console.ReadLine();
            }
#endif
            var syntheticBoard = new int[N, M];
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    var isOn = board[i][j] == '1';
                    syntheticBoard[i, j] = (isOn == ((i + j) %2 == 1)) ? 0 : 1;
                }
            }

            var minErrors = new Dictionary<int, int>(Math.Min(M, N));
            for (var y = 0; y < N; y++)
            {
                for (var x = 0; x < M; x++)
                {
                    var cellInError = 0;
                    var maxSize = Math.Min(N - y, M - x);
                    for(var size = 1; size <= maxSize; size++)
                    {
                        for (var j = 0; j < size; j++)
                        {

                                cellInError+=syntheticBoard[y+j, x+size-1];
                        }

                        for (var i = 0; i < size -1; i++)
                        {
                                cellInError+=syntheticBoard[y + size - 1, x + i];
                        }

                        var error = cellInError;
                        if (error > size * size / 2)
                        {
                            error = size * size - error;
                        }

                        if (!minErrors.ContainsKey(size))
                        {
                            minErrors[size] = error;
                        }
                        else
                        {
                            minErrors[size] = Math.Min(minErrors[size], error);
                        }
                     }                
                }
            }

#if TEST
            Console.WriteLine($"Elapsed: {Environment.TickCount - start} ms.");
#endif
            minErrors[1] = 0;
            var Q = int.Parse(Console.ReadLine());
            var queries = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            foreach (var correction in queries)
            {
                for (var size = Math.Min(M, N); size > 0; size--)
                {
                    if (correction >= minErrors[size])
                    {
                        Console.WriteLine(size);
                        break;
                    }
                }
            }
        }
    }
}