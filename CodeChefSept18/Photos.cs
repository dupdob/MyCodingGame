using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// explore x1 and y1
namespace CodeChefSept18
{
    public static class Photos
    {
        static void MainPhotos()
        {
            var testCount = int.Parse(Console.ReadLine());
            for (var testId = 0; testId < testCount; testId++)
            {
                var args = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                var x1 = args[1];
                var y1 = args[0];
                var pic1 = Console.ReadLine();
                args = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                var x2 = args[1];
                var y2 = args[0];
                var pic2 = Console.ReadLine();
                var common = ComputeCommon(y1, x1, pic1, y2, x2, pic2);
                Console.WriteLine(common);
            }
        }

        private static void Transpose(ref int sizeY, ref int sizeX, ref string image)
        {
            var result = new StringBuilder(image.Length);
            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    result.Append(image[y * sizeX + x]);
                }
            }

            image = result.ToString();
            var t = sizeX;
            sizeX = sizeY;
            sizeY = t;
        }
        
        private static long ComputeCommon(int y1, int x1, string pic1, int y2, int x2, string pic2)
        {
            if ((x1 > y1 && x1 > y2 && x1 > x2) || (x2 > y1 && x2 > y2 && x2 > x1))
            {
                Transpose(ref y1, ref x1, ref pic1);
                Transpose(ref y2, ref x2, ref pic2);
            }
            if (y1 < y2)
            {
                return ComputeCommon(y2, x2, pic2, y1, x1, pic1);
            }

            var targetX = CommonMultiplier(x1, x2);
            int factorX1 = (int) (targetX / x1);
            int factorX2 = (int) (targetX / x2);
            var targetY = CommonMultiplier(y1, y2);
            int factorY1 = (int) (targetY / y1);
            int factorY2 = (int) (targetY / y2);

            var common = 0L;

            var lineOffset1 = 0;
            var lineOffset2 = 0;
            var errorY = 0L;
            
            // optimization opportunity?
            var optimLength = 50;
            if (x1 < optimLength )
            {
                var cache = new Dictionary<string, long>();
                for (var j = 0; j < y1; j++)
                {
                    var marker = pic1.Substring(lineOffset1, x1);
                    if (!cache.ContainsKey(marker))
                    {
                        cache[marker] = ScanLine(x1, x2, factorX1, factorX2, pic1, lineOffset1, pic2, lineOffset2);
                    }
                    errorY += factorY1;
                    if (errorY >= factorY2)
                    {
                        errorY -= factorY2;
                        common += cache[marker]
                                  * (factorY1 - errorY);
                        cache.Clear();
                        lineOffset2 += x2;
                    }

                    if (errorY > 0)
                    {
                        if (!cache.ContainsKey(marker))
                        {
                            cache[marker] = ScanLine(x1, x2, factorX1, factorX2, pic1, lineOffset1, pic2, lineOffset2);
                        }
                        common += cache[marker]* Math.Min(errorY, factorY1);
                    }
    
                    lineOffset1 += x1;
                }

                return common;
            }
            
            for (var j = 0; j < y1; j++)
            {
                errorY += factorY1;
                if (errorY >= factorY2)
                {
                    errorY -= factorY2;
                    common += ScanLine(x1, x2, factorX1, factorX2, pic1, lineOffset1, pic2, lineOffset2)
                              * (factorY1 - errorY);
                    lineOffset2 += x2;
                }

                if (errorY > 0)
                {
                    common += ScanLine(x1, x2, factorX1, factorX2, pic1, lineOffset1, pic2, lineOffset2)
                              * Math.Min(errorY, factorY1);
                }

                lineOffset1 += x1;
            }

            return common;
        }

        private static long ScanLine(int x1, int x2, int factorX1, int factorX2, string pic1, int offset1, string pic2,
            int offset2)
        {
            var error = 0;
            var common = 0L;
            if (x1 > x2)
            {
                var refPixel = pic2[offset2];
                for (var i = 0; i < x1; i++)
                {
                    error += factorX1;
                    if (error >= factorX2)
                    {
                        error -= factorX2;
                        if (pic1[offset1] == refPixel)
                        {
                            common += factorX1 - error;
                        }

                        offset2++;
                        if (offset2 < pic2.Length)
                        {
                            refPixel = pic2[offset2];
                        }
                        if (error > 0)
                        {
                            if ( error > 0 && pic1[offset1] == pic2[offset2])
                            {
                                common += error;
                            }
                        }
                    }
                    else
                    {
                        if (pic1[offset1] == refPixel)
                        {
                            common += factorX1;
                        }
                    }

                    offset1++;
                }
            }
            else if (x2 > x1)
            {
                return ScanLine(x2, x1, factorX2, factorX1, pic2, offset2, pic1, offset1);
            }
            else
            {
                for (var i = 0; i < x2; i++)
                {
                    if (pic1[offset1++] == pic2[offset2++])
                    {
                        common ++;
                    }
                }
            }

            return common;
        }

        private static long CommonMultiplier(int a, int b)
        {           
            long max = a;
            var min = b;
            if (a < b)
            {
                max = b;
                min = a;
            }

            while (max % min != 0)
            {
                max += a + b - min;
            }

            return max;
        }
    }
}