using System;
using System.Linq;
using System.Text;

namespace CodeChefSept18
{
    public class TableGame
    {
        static ushort[][] matrix;
        private static int lastX;
        private static int lastY;
        private static string left;
        private static ushort[] transformed;
        private static ushort[] altTransformed;
        
        
        static void Main(string[] args)
        {
            InitTransformed();
            var testCases = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCases; i++)
            {
                var top = Console.ReadLine();
                left = "0"+Console.ReadLine();
                lastX = 0;
                lastY = 0;
                var width = (top.Length + 15) >> 4;
                var padding = 16-(top.Length % 16);
                matrix = new ushort[left.Length][];
 
                matrix[0] = new ushort[width];
                ushort store = 0;
                for (var j = 0; j < top.Length; j++)
                {
                    store <<= 1;
                    if (top[j] == '1')
                    {
                        store++;
                    }

                    if (j % 16 == 15)
                    {
                        matrix[0][j >> 4] = store;
                        store = 0;
                    }
                }
                matrix[0][width - 1] = (ushort) (store<<padding);

              
                // fill first line and col
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

        private static void InitTransformed()
        {
            transformed = new ushort[ushort.MaxValue+1];
            altTransformed = new ushort[ushort.MaxValue+1];
            for (int i = 0; i <= ushort.MaxValue; i++)
            {
                ushort trans = 0;
                ushort altTrans = 0;
                var previous = false;
                var altPrevious = true;
                for (ushort bit = 0x8000; bit > 0; bit >>= 1)
                {
                    if (!previous || (i & bit) == 0)
                    {
                        trans += bit;
                        previous = true;
                    }
                    else
                    {
                        previous = false;
                    }
                    if (!altPrevious || (i & bit) == 0)
                    {
                        altTrans += bit;
                        altPrevious = true;
                    }
                    else
                    {
                        altPrevious = false;
                    }    
                }

                altTransformed[i] = altTrans;
                transformed[i] = trans;
            }
            
        }

        private static bool IsAWin(int Y, int X)
        {
            if (X > lastX || Y > lastY)
            {
                if (Y > lastY)
                {
                    for (var y = lastY+1; y <= Y; y++)
                    {
                        var previous = left[y]=='1';
                        var newLine = new ushort[matrix[0].Length];
                        for (var i = 0; i < newLine.Length; i++)
                        {
                            var word = matrix[y - 1][i];
                            word = previous ? altTransformed[word] : transformed[word];

                            newLine[i] = word;
                            previous = (word & 1) == 1;
                        }

                        matrix[y] = newLine;
                    }
                }
                
                lastX = Math.Max(lastX, X);
                lastY = Math.Max(lastY, Y);
            }

            X--;
            var index = X >> 4;
            var mask = 1 << (15 - (X % 16));
            return (matrix[Y][index] & mask) != 0;

        }
    }
}    