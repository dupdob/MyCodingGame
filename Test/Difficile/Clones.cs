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
        private Dictionary<int, List<int>> elevatorPos;
        private Dictionary<int, List<int>> blockerPos;

        public int nbTotalClones; // number of generated clones
        public int nbRounds;
        public int nbLifts;
        private int freeLifts;
        private int nbFloors;

        public Context(int nbFloors)
        {
            this.nbFloors = nbFloors;
            blockerPos = new Dictionary<int, List<int>>(nbFloors);
            elevatorPos = new Dictionary<int, List<int>>(nbFloors);
        }

        public void Init()
        {
            this.freeLifts = this.nbLifts;
            // compute nb of 'free lifts' to shorten pah
            for (int i = 0; i < this.nbFloors; i++)
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
            if (!elevatorPos.ContainsKey(elevatorFloor))
            {
                elevatorPos[elevatorFloor] = new List<int>(2);
            }
            elevatorPos[elevatorFloor].Add(elevatorPosX);
            elevatorPos[elevatorFloor].Sort();
        }

        private int FirstElevatorOnLeft(int floor, int x)
        {
            int result = -1;
            if (!elevatorPos.ContainsKey(floor))
                return result;

            foreach (var val in elevatorPos[floor])
            {
                if (val > x)
                {
                    break;
                }
                result = val;
            }
            LogLine("Elevator on left at {0}", result);
            return result;
        }

        private int FirstElevatorOnRight(int floor, int x)
        {
            int result = -1;
            if (!elevatorPos.ContainsKey(floor))
                return result;

            foreach (var val in elevatorPos[floor])
            {
                if (val >= x)
                {
                    result = val;
                    break;
                }
            }
            LogLine("Elevator on right at {0}", result);
            return result;
        }

        public void MoveToClosestLift(int floor, int x, string direction)
        {
            int left = FirstElevatorOnLeft(floor, x);
            int right = FirstElevatorOnRight(floor, x);

            if (left == -1 && right == -1)
            {
                Elevator(floor, x);
                return;
            }

            if (this.freeLifts > 0)
            {
                LogLine("Let's build a free lift.");
                this.freeLifts--;
                Elevator(floor, x);
                return;
            }
            if (direction == "RIGHT")
            {
                if (right == -1 || (left >= 0 && x - left < right - x))
                    Block(floor, x);
                else
                    Wait();
            }
            else
            {
                if (left == -1 || (right >= 0 && x - left > right - x))
                    Block(floor, x);
                else
                    Wait();
            }
        }

        private void Block(int blockerFloor, int blockerPosX)
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

            this.nbTotalClones--;

            Console.WriteLine("BLOCK"); // action: WAIT or BLOCK
        }

        private void Elevator(int level, int x)
        {
            AddElevator(level, x);
            this.nbLifts--;
            Console.WriteLine("ELEVATOR"); // action: WAIT or BLOCK
        }
    }

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int nbFloors = int.Parse(inputs[0]); // number of floors
        int width = int.Parse(inputs[1]); // width of the area
        var context = new Context(nbFloors);
        context.nbRounds = int.Parse(inputs[2]); // maximum number of rounds
        int exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
        int exitPos = int.Parse(inputs[4]); // position of the exit on its floor
        context.nbTotalClones = int.Parse(inputs[5]); // number of generated clones
        context.nbLifts = int.Parse(inputs[6]); // ignore (always zero)
        int nbElevators = int.Parse(inputs[7]); // number of elevators

        LogLine("Nb Rounds: {0}", context.nbRounds);
        LogLine("Nb Clones: {0}", context.nbTotalClones);
        LogLine("Nb Extra Lifts :{0}", context.nbLifts);

        for (int i = 0; i < nbElevators; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
            int elevatorPosX = int.Parse(inputs[1]); // position of the elevator on its floor
            context.AddElevator(elevatorFloor, elevatorPosX);
        }
        context.AddElevator(exitFloor, exitPos);

        context.Init();
        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int cloneFloor = int.Parse(inputs[0]); // floor of the leading clone
            int clonePos = int.Parse(inputs[1]); // position of the leading clone on its floor
            string direction = inputs[2]; // direction of the leading clone: LEFT or RIGHT

            Console.Error.WriteLine("Clone at {0} {1} toward {2}", cloneFloor, clonePos, direction);
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            // stop at edge of screen
            if (cloneFloor < 0 || clonePos < 0)
                context.Wait();
            else
            {
                context.MoveToClosestLift(cloneFloor, clonePos, direction);
            }
            context.nbRounds--;
        }
    }
}