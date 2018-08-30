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

            Console.Error.WriteLine($"{W}x{H} in {N} from {X0}:{Y0}");

            string bombDir = Console.ReadLine(); // Current distance to the bomb compared to previous distance (COLDER, WARMER, SAME or UNKNOWN)
            var topX = 0;
            var topY = 0;
            var endX = W - 1;
            var endY = H - 1;
            var onX = (topX>topY);
            // game loop
            while (true)
            {
                Console.Error.WriteLine($"Possible zone is: {topX}:{topY}-{endX}:{endY}, scanning {(onX ? "X" : "Y")}");
                var previousX = X0;
                var previousY = Y0;
                if (onX)
                {
                    X0 =  endX + topX - X0;
                }
                else
                {
                    Y0 = endY + topY - Y0;
                }

                if (X0 == previousX && Y0 == previousY)
                {
                    X0++;
                    Y0++;
                }
                X0 = Math.Min(endX, Math.Max(topX, X0));
                Y0 = Math.Min(endY, Math.Max(topY, Y0));
                Console.WriteLine($"{X0} {Y0}");
                bombDir = Console.ReadLine();
                switch (bombDir)
                {
                    case "COLDER":
                        if (onX)
                        {
                            if (previousX > X0)
                            {
                                topX = (endX + topX) / 2;
                            }
                            else
                            {
                                endX = (endX + topX ) / 2;
                            }
                        }
                        else
                        {
                            if (previousY > Y0)
                            {
                                topY = (endY + topY) / 2;
                            }
                            else
                            {
                                endY = (endY + topY) / 2;
                            }
                        }
                        break;
                    case "WARMER":
                        
                        if (onX)
                        {
                            if (previousX < topX || previousX > endX)
                            {
                                break;
                            }

                            if (topX + 1 == endX)
                            {
                                topX = endX = X0;
                            }
                            else if (previousX > X0)
                            {
                                endX = Math.Min(endX, (endX + topX) / 2);
                            }
                            else
                            {
                                topX = Math.Max(topX, (endX + topX) / 2);
                            }
                        }
                        else
                        {
                            if (previousY < topY || previousY > endY)
                            {
                                break;
                            }

                            if (topY + 1 == endY)
                            {
                                topY = endY = Y0;
                            }
                            if (previousY > Y0)
                            {
                                endY = Math.Min(endY, (endY + topY) / 2);
                            }
                            else
                            {
                                topY = Math.Max(topY, (endY + topY ) / 2);
                            }
                        }
                        break;
                    case "SAME":
                        if (onX)
                        {
                            endX = (previousX + X0) / 2;
                            topX = endX;
                        }
                        else
                        {
                            endY = (previousY + endY ) / 2;
                            topY = endY;
                        }
                        break;
                }

                if (topX == endX && topY != endY)
                {
                    if (Y0 == endY)
                    {
                        endY--;
                    }
                    else if (Y0 == topY)
                    {
                        topY++;
                    }
                    
                    if (X0 == endX)
                    {
                        onX = false;
                    }
                }
                
                if (topY == endY && topX !=endX)
                {
                    if (X0 == endX)
                    {
                        endX--;
                    }
                    else if (X0 == topX)
                    {
                        topX++;
                    }
                    if (Y0 == endY)
                    {
                        onX = true;
                    }
                    
                }
                
                // optimization
                if ((X0 <topX || X0 > endX) && (topY != endY))
                {
                    onX = false;
                }
                
                // optimization
                if ((Y0 <topY || Y0 > endY) && (topX != endX))
                {
                    onX = true;
                }
            }
        }
    }
}