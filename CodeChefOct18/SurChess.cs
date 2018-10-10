#define TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeChefOct18
{
    public class SurChess
    {
        static void Main(string[] args)
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
            var syntheticBoard = new bool[N, M];
            for (int i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    var isOn = board[i][j] == '1';
                    syntheticBoard[i, j] = (isOn == ((i + j) %2 == 1));
                }
            }

            var minErrors = new Dictionary<int, int>(Math.Min(M, N));
            for (var size = Math.Min(M, N); size > 1; size--)
            {
                var minSize = int.MaxValue;
                for (var y = 0; y <= N - size; y++)
                {
                    for (var x = 0; x <= M - size; x++)
                    {
                        var cellInError = 0;
                        for (var j = y; j < y + size; j++)
                        {
                            for (var i = x; i < x + size; i++)
                            {
                                if (syntheticBoard[j, i])
                                {
                                    cellInError++;
                                }
                            }
                        }

                        if (cellInError > size * size / 2)
                        {
                            cellInError = size * size - cellInError;
                        }

                        minSize = Math.Min(minSize, cellInError);
                    }
                }

                minErrors[size] = minSize;
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