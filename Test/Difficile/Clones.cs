using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class ClonesHard
{
 
    static void Log(string msg, params object[] pars)
    {
        Console.Error.Write(msg, pars);
    }

    static void LogLine(string msg, params object[] pars)
    {
        Console.Error.WriteLine(msg, pars);
    }

    static void LogLine()
    {
        Console.Error.WriteLine();
    }


    class Context
    {
        public enum Actions
        {
            Wait,
            Block,
            Lift
        };
        private readonly Dictionary<int, List<int>> elevatorPos;
        private readonly Dictionary<int, List<int>> blockerPos;

        public int NbTotalClones; // number of generated clones
        public int NbRounds;
        public int NbLifts;
        public int ExitFloor;
        public int ExitPos;
        private int freeLifts;
        private readonly int nbFloors;
        private readonly int CloneFrequency = 3;

        public Context(int nbFloors)
        {
            this.nbFloors = nbFloors;
            this.blockerPos = new Dictionary<int, List<int>>(nbFloors);
            this.elevatorPos = new Dictionary<int, List<int>>(nbFloors);
        }

        public Context(Context context)
        {
            this.elevatorPos = new Dictionary<int, List<int>>(context.elevatorPos);
            this.blockerPos = new Dictionary<int, List<int>>(context.blockerPos);
            this.NbTotalClones = context.NbTotalClones;
            this.NbRounds = context.NbRounds;
            this.NbLifts = context.NbLifts;
            this.freeLifts = context.freeLifts;
            this.nbFloors = context.nbFloors;
        }

        public void Init()
        {
            this.freeLifts = this.NbLifts;
            // compute nb of 'free lifts' to shorten pah
            for (var i = 0; i < this.nbFloors; i++)
            {
                if (!this.elevatorPos.ContainsKey(i) || this.elevatorPos[i].Count == 0)
                {
                    this.freeLifts--;
                }
            }
            LogLine("Free Lifts: {0} ", this.freeLifts);
        }

        public void Wait()
        {
            Console.WriteLine("WAIT"); // action: WAIT or BLOCK
        }

        public void AddElevator(int elevatorFloor, int elevatorPosX)
        {
            LogLine("Elevator at {0} {1}", elevatorFloor, elevatorPosX);
            if (!this.elevatorPos.ContainsKey(elevatorFloor))
            {
                this.elevatorPos[elevatorFloor] = new List<int>(2);
            }
            this.elevatorPos[elevatorFloor].Add(elevatorPosX);
            this.elevatorPos[elevatorFloor].Sort();
        }

        private int FirstElevatorOnLeft(int floor, int x)
        {
            var result = -1;
            if (floor == this.ExitFloor)
            {
                if (this.ExitPos <= x)
                    return this.ExitPos;
            }
            if (!this.elevatorPos.ContainsKey(floor))
                return result;

            foreach (var val in this.elevatorPos[floor])
            {
                if (val > x)
                {
                    break;
                }
                result = val;
            }
            return result;
        }

        private int FirstElevatorOnRight(int floor, int x)
        {
            var result = -1;
            if (floor == this.ExitFloor)
            {
                if (this.ExitPos >= x)
                    return this.ExitPos;
            }
            if (!this.elevatorPos.ContainsKey(floor))
                return result;

            foreach (var val in this.elevatorPos[floor])
            {
                if (val < x) continue;
                result = val;
                break;
            }
            return result;
        }

        public void MoveToClosestLift(int floor, int x, string direction)
        {
            var left = this.FirstElevatorOnLeft(floor, x);
            var right = this.FirstElevatorOnRight(floor, x);

            if (left == -1 && right == -1)
            {
                // no lift at this floor
                this.Elevator(floor, x);
            }
            else if (direction == "RIGHT")
            {
                if (right == -1 || (left >= 0 && (x - left+ this.CloneFrequency) < right - x))
                    this.Block(floor, x);
                else
                    this.Wait();
            }
            else
            {
                if (left == -1 || (right >= 0 && x - left > (right - x + this.CloneFrequency)))
                    this.Block(floor, x);
                else
                    this.Wait();
            }
        }

        public int ShortestPath(int floor, int x, string direction, out Actions action)
        {
            int shortestPath = iShortestPath(floor, floor, x, direction, out action);

            return shortestPath;
        }

        private int iShortestPath(int floor, int refFloor, int x, string direction, out Actions action)
        {
            var left = this.FirstElevatorOnLeft(floor, x);
            var right = this.FirstElevatorOnRight(floor, x);
            var leftDist = (int)(Math.Pow(10, this.nbFloors-floor)*100);
            var rightDist = leftDist;
            Actions leftAction = Actions.Wait, rightAction = Actions.Wait;
            Actions dump;
            if (left != -1)
            {
                leftDist = x - left;
                if (floor == refFloor)
                {
                    LogLine("Elevator on left at: {0}", leftDist);
                }
                if (direction == "RIGHT")
                {
                    leftAction = Actions.Block;
                    leftDist += 3;
                }
                // we can try the left lift
                if (floor<this.ExitFloor)
                    leftDist += this.iShortestPath(floor + 1, refFloor, left, "LEFT", out dump);
            }
            if (right != -1)
            {
                // we can try the right lift
                rightDist = right-x;
                if (floor == refFloor)
                {
                    LogLine("Elevator on right at: {0}", rightDist);
                }
                if (direction == "LEFT")
                {
                    rightAction = Actions.Block;
                    rightDist += 3;
                }
                if (floor < this.ExitFloor)
                    rightDist += this.iShortestPath(floor + 1, refFloor, right, "RIGHT", out dump);
            }
            else if (left == -1)
            {
                action = Actions.Lift;
                return rightDist;
            } 
            if ((leftDist < rightDist) || (leftDist == rightDist && leftAction == Actions.Wait))
            {
                if (floor == refFloor)
                {
                    LogLine("Shortest path is left: {0}", leftDist);
                }
                action = leftAction;
                return leftDist;
            }
            else
            {
                if (floor == refFloor)
                {
                    LogLine("Shortest path is right: {0}", rightDist);
                }
                action = rightAction;
                return rightDist;
            }
        }

        public void Block(int blockerFloor, int blockerPosX)
        {
            if (!this.blockerPos.ContainsKey(blockerFloor))
            {
                this.blockerPos[blockerFloor] = new List<int>(2);
            }
            else if (this.blockerPos[blockerFloor].Contains(blockerPosX))
            {
                this.Wait();
                return;
            }
            LogLine("Blocker at {0} {1}", blockerFloor, blockerPosX);
            this.blockerPos[blockerFloor].Add(blockerPosX);
            this.blockerPos[blockerFloor].Sort();

            this.NbTotalClones--;

            Console.WriteLine("BLOCK"); // action: WAIT or BLOCK
        }

        public void Elevator(int level, int x)
        {
            this.AddElevator(level, x);
            this.NbLifts--;
            Console.WriteLine("ELEVATOR"); // action: WAIT or BLOCK
        }
    }

    static void Main(string[] args)
    {
        var inputs = Console.ReadLine().Split(' ');
        var nbFloors = int.Parse(inputs[0]); // number of floors
        var width = int.Parse(inputs[1]); // width of the area
        var context = new Context(nbFloors);
        context.NbRounds = int.Parse(inputs[2]); // maximum number of rounds
        var exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
        var exitPos = int.Parse(inputs[4]); // position of the exit on its floor
        context.NbTotalClones = int.Parse(inputs[5]); // number of generated clones
        context.NbLifts = int.Parse(inputs[6]); // ignore (always zero)
        context.ExitFloor = exitFloor;
        context.ExitPos = exitPos;
        var nbElevators = int.Parse(inputs[7]); // number of elevators

        LogLine("Nb Rounds: {0}", context.NbRounds);
        LogLine("Nb Clones: {0}", context.NbTotalClones);
        LogLine("Nb Extra Lifts :{0}", context.NbLifts);

        for (int i = 0; i < nbElevators; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
            int elevatorPosX = int.Parse(inputs[1]); // position of the elevator on its floor
            context.AddElevator(elevatorFloor, elevatorPosX);
        }
        context.AddElevator(exitFloor, exitPos);

        context.Init();
        var shortestPathComputed = false;
        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            var cloneFloor = int.Parse(inputs[0]); // floor of the leading clone
            var clonePos = int.Parse(inputs[1]); // position of the leading clone on its floor
            var direction = inputs[2]; // direction of the leading clone: LEFT or RIGHT

            if (cloneFloor < 0)
            {
                Console.Error.WriteLine("No Clone");
                context.Wait();
                continue;
            }
            Console.Error.WriteLine("Clone at {0} {1} toward {2}", cloneFloor, clonePos, direction);

            if (!shortestPathComputed)
            {
                Context.Actions act;
                context.ShortestPath(cloneFloor, clonePos, direction, out act);
                switch (act)
                {
                    case Context.Actions.Wait:
                        context.Wait();
                        break;
                    case Context.Actions.Block:
                        context.Block(cloneFloor, clonePos);
                        break;
                    case Context.Actions.Lift:
                        context.Elevator(cloneFloor, clonePos);
                        break;
                }
            }
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            // stop at edge of screen
            else if (cloneFloor < 0 || clonePos < 0)
            {
                context.Wait();
            }
            else
            {
                context.MoveToClosestLift(cloneFloor, clonePos, direction);
            }
            context.NbRounds--;
        }
    }
}