using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class AneoSpeed
{
    class StreetLigh
    {
        readonly int dist;
        readonly int period;

        public StreetLigh(int dist, int period)
        {
            this.dist = dist;
            this.period = period;
        }

        public bool CanBeCrossedAtSpeed(int speed)
        {
            var timeToreach = dist*3600/speed/1000;

            return (timeToreach / period % 2 == 0);
        }
    }
    static void MainSpeed(string[] args)
    {
        var speed = int.Parse(Console.ReadLine());
        var lightCount = int.Parse(Console.ReadLine());
        var lights = new StreetLigh[lightCount];
        for (var i = 0; i < lightCount; i++)
        {
            var inputs = Console.ReadLine().Split(' ');
            var distance = int.Parse(inputs[0]);
            var duration = int.Parse(inputs[1]);
            lights[i] = new StreetLigh(distance, duration);
        }

        for (var testSpeed = speed; testSpeed > 0; testSpeed--)
        {
            var isOK = true;
            foreach (var light in lights)
            {
                if (!light.CanBeCrossedAtSpeed(testSpeed))
                {
                    isOK = false;
                    break;
                }
            }

            if (isOK)
            {
                Console.WriteLine(testSpeed);
                break;
            }
        }
    }
}