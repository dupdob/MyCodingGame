using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * https://www.codingame.com/ide/puzzle/shadows-of-the-knight-episode-2
 **/
namespace CodingGame
{
    class ShardsOfTheKnight
    {
        static void Main(string[] args)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int W = int.Parse(inputs[0]); // width of the building.
            int H = int.Parse(inputs[1]); // height of the building.
            int N = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
            inputs = Console.ReadLine().Split(' ');
            int X0 = int.Parse(inputs[0]);
            int Y0 = int.Parse(inputs[1]);

//            Console.Error.WriteLine($"{W}x{H} in {N} from {X0}:{Y0} ({W*H})");

            string bombDir = Console.ReadLine(); // Current distance to the bomb compared to previous distance (COLDER, WARMER, SAME or UNKNOWN)
            var topX = 0;
            var topY = 0;
            var endX = W - 1;
            var endY = H - 1;
            var onX = (topX>topY);
            var optimized = false;
            // game loop
            while (true)
            {
                Console.Error.WriteLine($"Possible zone is: {topX}:{topY}-{endX}:{endY} ({(endX-topX+1)*(endY-topY+1)}).");
                var previousX = X0;
                var previousY = Y0;
                var movedOnX = onX;
  
                // optimization
                if (topY == endY && Y0 != topY)
                {
                    Y0 = topY;
                    movedOnX = false;
                }
                else if (topX == endX && X0 != topX)
                {
                    X0 = topX;
                    movedOnX = true;
                }
                else if (endX-topX>endY-topY)
                {
                    movedOnX = true;
                    X0 =  endX + topX - X0;
                    if (X0 == previousX)
                    {
                        if (X0 < endX)
                        {
                            X0++;
                        }
                        else
                        {
                            X0--;
                        }
                    }
                }
                else
                {
                    movedOnX = false;
                    Y0 = endY + topY - Y0;
                    if (Y0 == previousY)
                    {
                        if (Y0 < endY)
                        {
                            Y0++;
                        }
                        else
                        {
                            Y0--;
                        }
                    }
                }
                Console.Error.WriteLine($"scanning {(movedOnX ? "X" : "Y")}");

                X0 = Math.Min(W-1, Math.Max(0, X0));
                Y0 = Math.Min(H-1, Math.Max(0, Y0));

                Console.WriteLine($"{X0} {Y0}");
                bombDir = Console.ReadLine();
                switch (bombDir)
                {
                    case "COLDER":
                        if (movedOnX)
                        {
                            Adjust(X0, previousX, ref topX, ref endX, false);
                        }
                        else
                        {
                            Adjust(Y0, previousY, ref topY, ref endY, false);
                        }
                        break;                    
                    case "WARMER":          
                        if (movedOnX)
                        {
                            Adjust(X0, previousX, ref topX, ref endX, true);
                        }
                        else
                        {
                            Adjust(Y0, previousY, ref topY, ref endY, true);
                        }
                        break;                    
                    case "SAME":
                        if (movedOnX)
                        {
                            endX = (previousX + X0) / 2;
                            topX = endX;
                        }
                        else
                        {
                            endY = (previousY + Y0 ) / 2;
                            topY = endY;
                        }
                        break;
                }
                
            }
        }

        private static void Adjust(int newCoord, int prevCoord, ref int zoneStart, ref int zoneEnd, bool warmer)
        {
            var mid = (newCoord + prevCoord) / 2;
            var altMid = (newCoord + prevCoord) / 2+1;
            if ((mid < newCoord && warmer) || (mid>newCoord && !warmer))
            {
                zoneStart = Math.Max(zoneStart, altMid);
            }
            else
            {
                zoneEnd = Math.Min(zoneEnd, mid);
            }

        }
    }
}