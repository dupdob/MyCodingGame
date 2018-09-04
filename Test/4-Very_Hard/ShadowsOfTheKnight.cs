using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * https://www.codingame.com/ide/puzzle/shadows-of-the-knight-episode-2
 **/
namespace CodingGame
{
    static class ShadowsOfTheKnight
    {
        static void MainShadows(string[] args)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int W = int.Parse(inputs[0]); // width of the building.
            int H = int.Parse(inputs[1]); // height of the building.
            int N = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
            inputs = Console.ReadLine().Split(' ');
            int X0 = int.Parse(inputs[0]);
            int Y0 = int.Parse(inputs[1]);


            string bombDir = Console.ReadLine(); // Current distance to the bomb compared to previous distance (COLDER, WARMER, SAME or UNKNOWN)
            var topX = 0;
            var topY = 0;
            var endX = W - 1;
            var endY = H - 1;
            // game loop
            while (true)
            {
                var ignoreResult = false;
                Console.Error.WriteLine($"Possible zone is: {topX}:{topY}-{endX}:{endY} ({(endX-topX+1)*(endY-topY+1)}).");
                var previousX = X0;
                var previousY = Y0;
                var movedOnX = false;
  
                // optimization
                if (topY == endY && Y0 != topY)
                {
                    Y0 = topY;
                    X0 = topX;
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

                }
                else
                {
                    Y0 = endY + topY - Y0;
                }


                
                X0 = Math.Min(W-1, Math.Max(0, X0));
                Y0 = Math.Min(H-1, Math.Max(0, Y0));
                Console.Error.WriteLine($"scanning {(movedOnX ? "X" : "Y")}");

                if (movedOnX && endX-topX>2)
                {
                    var refX = (previousX + X0) / 2;
                    if (refX <= topX || refX>=endX)
                    {
                        // we will not learn anything
                        ignoreResult = true;
                    }
                }
                else if (!movedOnX && endY - topY>2)
                {
                    var refY = (previousY + Y0) / 2;
                    if (refY <= topY || refY>=endY)
                    {
                        // we will not learn anything
                        ignoreResult = true;
                    }
                    
                }

                if (ignoreResult)
                {
                    X0 = topX+(endX-topX)*2/3;
                    Y0 = topY+(endY - topY)*2/3;
                }
                X0 = Math.Min(W-1, Math.Max(0, X0));
                Y0 = Math.Min(H-1, Math.Max(0, Y0));
                if (X0 == previousX && Y0 == previousY)
                {
                    X0 = endX;
                    Y0 = endY;
                    ignoreResult = true;
                }
                Console.WriteLine($"{X0} {Y0}");
                bombDir = Console.ReadLine();
                if (ignoreResult)
                {
                    continue;
                }
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
                            if (X0 != previousX)
                            {
                                endX = (previousX + X0) / 2;
                                topX = endX;
                            }
                        }
                        else if (Y0 != previousY)
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
            if (zoneStart == zoneEnd)
            {
                return;
            }
            var mid = (newCoord + prevCoord) / 2;
            var altMid = (newCoord + prevCoord) / 2+1;
            if ((newCoord + prevCoord) % 2 == 0)
            {
                mid--;
                altMid = mid + 2;
            }
            if ((mid <= newCoord && warmer) || (mid>=newCoord && !warmer))
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