using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

/**
You have to implement a version of the timer in Clash of Code. The timer changes based on players joining.
The timer starts at 5:00 and runs towards 0:00. 

Here are the rules:
* Every time a player joins the clash, the game is set to start at 

t - 256 / ( 2^(p - 1) )

where t is current time in seconds and p is the number of players in the room.
* If the result is under 0:00, set it to 0:00.
* If a player joins at the same time the game is supposed to start, set the new time 
instead of starting the game.

For example:

5:00
When the timer starts there is one player (the one who started the clash).

at 4:47 the second player joins -> set to start 256 seconds from now, at 0:31
at 3:56 the third player joins -> set to start 128 seconds from now, at 1:48
at 3:13 the fourth player joins -> set to start 64 seconds from now, at 2:09
2:09, the game starts

Output: 2:09
    The game starts if:
* The timer reaches the time it is set to stop at, or
* The timer reaches zero and there is more than one player, or
* The clash room is filled (8 players in total, 7 have joined)
 **/

class TimerClash
{
    static void MainTimer(string[] args)
    {
        var n = int.Parse(Console.ReadLine());
        var minstart = 300;
        for (var i = 0; i < n; i++)
        {
            var timeStamp = 300 - (int)TimeSpan.ParseExact(Console.ReadLine(), "m\\:ss", CultureInfo.InvariantCulture).TotalSeconds;
            var maxTime = (int)(256 / (Math.Pow(2, i)));

            minstart = maxTime + timeStamp;
            Console.Error.WriteLine("Adding player{0} at {1}: timeout at {2}", i, timeStamp, minstart);
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(TimeSpan.FromSeconds(300 - minstart).ToString("m\\:ss"));
    }
}