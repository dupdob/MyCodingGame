using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/*
A generation starship is heading towards a new home for mankind : Sky's Edge. The exoplanet being distant of several light years,
the journey will last centuries and see multiple generations of crew members being born, reproduce and die at the service of
human extrasolar expansion. Your goal is to determine which life expectancies are the best suited to reach Sky's Edge with at
least 200 settlers, while avoiding overcrowding the ship.

The journey will last Y years, with a variable starting group of people, on a starship having a maximum capacity of C.
Every member of the expedition will have exactly the same life expectancy. 

Every year, these modifications will take place in the given order :
1 - Every crew member will get older by one year.
2 - Every crew member exceeding the life expectancy will die.
3 - For each batch of 10 crew members between the age of 20 and half the life expectancy (rounded down),
    a baby is born, adding one 0-year-old individual to the crew.These limits are inclusive.

If the number of people exceeds the ship capacity C, overpopulation causes a civil war leading to the destruction of the ship.
The expedition is considered successful if at least 200 people reach Sky's Edge after Y years of travel.


Your goal is to give the minimum and maximum life expectancies in order to have a successful expedition. There is always at least
one valid life expectancy.

Example :
The ship has 10 fifteen-year-old, 15 twenty-year-old, 5 fourty-year-old and 2 eighty-year-old.The life expectancy is of 82 years.
- At year 1, there will be 10 sixteen-year-old, 15 twenty-one-year-old, 5 fourty-one-year-old, 2 eighty-one-year-old
    AND 2 zero-year-old since the 15 twenty-one-year-old and 5 fourty-one-year-old are within the "fertility span" of[20, 41].
- At year 2, there will be 2 one-year-old, 10 seventeen-year-old, 15 twenty-two-year-old, 5 fourty-two-year-old,
    2 eighty-two-year-old and only one new zero-year-old.
- At year 3, there will be 1 one-year-old, 2 two-year-old, 10 eighteen-year-old, 15 twenty-three-year-old,
    5 fourty-three-year-old and one zero-year-old.Sadly, the 2 old timers have passed away since they exceeded their life expectancy...
- ... And so on, with a new peak of natality when the 10 teenagers will reach their twenties, but still heading to extinction...
*/


class SkyEdge
{
    static void Main(string[] args)
    {
        int Y = int.Parse(Console.ReadLine());
        int C = int.Parse(Console.ReadLine());
        int N = int.Parse(Console.ReadLine());
        var crew = new Dictionary<int, int>();
        for (int i = 0; i < N; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int AGE = int.Parse(inputs[0]);
            int NUMBER = int.Parse(inputs[1]);
            crew[AGE] = NUMBER;
        }
        var max = 100;
        var min = 20;
        var trial = 0;
        for (;;)
        {
            trial = (max + min)/2;
            Console.Error.WriteLine("Trying {0}", trial);
            var result = Travel(Y, C, trial, crew);
            if (result == -1)
            {
                // expectancy too high
                max = trial;
            }
            else if (result < 200)
            {
                // expectency too low
                min = trial;
            }
            else
            {
                // success
                break;
            }
        }
        // trying to find lowerlimit
        var lower = min;
        var higher = trial;
        while (higher-lower>1)
        {
            var attempt = (higher + lower) / 2;
            Console.Error.WriteLine("Searching lower trying {0} ({1} - {2})", attempt, higher, lower);
            var result = Travel(Y, C, attempt, crew);
            if (result == -1)
            {
                Console.Error.WriteLine("unexpected failure at {0}", attempt);
            }
            else if (result < 200)
            {
                // expectency too low
                lower = attempt;
            }
            else
            {
                // success
                higher = attempt;
            }
        }
        var lowerExpectancy = higher;
        // trying to find higherlimit
        lower = trial;
        higher = max;
        while (higher-lower>1)
        {
            var attempt = (higher + lower) / 2;
            Console.Error.WriteLine("Searching higher trying {0} ({1} - {2})", attempt, higher, lower);
            var result = Travel(Y, C, attempt, crew);
            if (result == -1)
            {
                higher = trial;
            }
            else if (result < 200)
            {
                Console.Error.WriteLine("unexpected failure at {0}", attempt);
            }
            else
            {
                // success
                lower = attempt;
            }
        }
        var higherExpectancy = lower;
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine("{0} {1}", lowerExpectancy, higherExpectancy);
    }

    private static int Travel(int Y, int C, int expectancy, Dictionary<int, int> crew)
    {
        for(;Y > 0; Y--)
        {
            var staff = crew.Values.Sum();
            if (staff > C)
                // overcrowded, revolt, failure
                return -1;
            if (staff == 10)
                // everybody is dead, no need to go on
                return 0;
            var newCrew = new Dictionary<int, int>(crew.Count);
            foreach (var crewKey in crew.Keys)
            {
                if (crewKey>expectancy)
                    // too old
                    continue;
                newCrew[crewKey + 1] = crew[crewKey];
            }
            newCrew[0] = newCrew.Where(x => (x.Key >= 20 && x.Key <= expectancy/2)).Select(x => x.Value).Sum()/10;
            crew = newCrew;
        }

        return crew.Values.Sum();
    }
}