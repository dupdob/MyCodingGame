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

    static void LogLine()
    {
        Console.Error.WriteLine();
    }

    static void LogLine(string msg, params object[] pars)
    {
        Console.Error.WriteLine(msg, pars);
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
		public int Width;
        private int freeLifts;
        private readonly int nbFloors;

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
			this.Width = context.Width;
        }

        public void Init()
        {
            this.freeLifts = this.NbLifts;
            // compute nb of 'free lifts' to shorten pah
			for (var i = 0; i < Math.Min(this.nbFloors, this.ExitFloor); i++)
            {
                if (i == this.ExitFloor)
                    continue;
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
            // no lift at this level
			if (this.elevatorPos.ContainsKey (floor)) {
				foreach (var val in this.elevatorPos[floor])
				{
					if (val > x)
						break;
					result = val;
				}
			}

            return result;
        }

        private int FirstElevatorOnRight(int floor, int x)
        {
            var result = -1;
			if (this.elevatorPos.ContainsKey (floor)) {
				var lifts = this.elevatorPos[floor];
				for(var i = lifts.Count-1; i >= 0; i--)
				{
					var val = lifts[i];
					if (val < x) break;
					result = val;	
				}
			}

            return result;
        }

        public void ShortestPath(int floor, int x, string direction, out Actions action)
        {
            iShortestPath(floor, floor, x, direction, out action);
        }

        private int iShortestPath(int floor, int refFloor, int x, string direction, out Actions action)
        {
            var left = this.FirstElevatorOnLeft(floor, x);
            var right = this.FirstElevatorOnRight(floor, x);
			var leftDist = int.MaxValue;
            var rightDist = leftDist;
            Actions leftAction = Actions.Wait, rightAction = Actions.Wait;
            Actions dump;
			bool noLift = (right == -1) && (left == -1);
			bool onALift = false;
			if (this.elevatorPos.ContainsKey (floor)) {
				onALift = this.elevatorPos [floor].Contains (x);
			}

			if (floor == this.ExitFloor) {
				if (x < this.ExitPos) {
					// exit is on right
					if (right < this.ExitPos) {
						action = Actions.Wait;
						return leftDist;
					}
					rightDist = this.ExitPos - x;
					if (direction == "LEFT") {
						action = Actions.Block;
						rightDist += 3;
					} else
						action = Actions.Wait;
					return rightDist+1;
				} else if (x>this.ExitPos) {
					// exit is on left
					if (left > this.ExitPos) {
						// there is a lift on front, we can't reach exit
						action = Actions.Wait;
						return leftDist;
					}
					leftDist = x-this.ExitPos;
					if (direction == "RIGHT") {
						action = Actions.Block;
						leftDist += 3;
					} else
						action = Actions.Wait;
					return leftDist+1;
				}
				action = Actions.Wait;
				return 0;
			}

            if (left != -1)
            {
				// lift on left
                string nextDir;
				var nbClones = this.NbTotalClones;
                leftDist = x - left;
                if (direction == "RIGHT")
                {
					if (NbTotalClones > 1) {
						nextDir = "LEFT";
						leftAction = Actions.Block;
						leftDist += 3;
						this.NbTotalClones--;
					} else {
						leftDist = int.MaxValue;
						nextDir = direction;
					}
                }
                else
                {
                    nextDir = direction;
                }	
                // we can try the left lift
				var next = this.iShortestPath (floor + 1, refFloor, left, nextDir, out dump);
				this.NbTotalClones = nbClones;
				leftDist = (next==int.MaxValue) ? next : leftDist + next;
            }

			if (right != -1)
			{
				string nextDir;
				var nbClones = this.NbTotalClones;
				// we can try the right lift
				rightDist = right - x;
				if (direction == "LEFT" && rightDist > 0)
				{
					if (this.NbTotalClones > 1) {
						nextDir = "RIGHT";
						rightAction = Actions.Block;
						this.NbTotalClones--;
						rightDist += 3;
					} else {
						nextDir = direction;
						rightDist = int.MaxValue;
					}
				}
				else
				{
					nextDir = direction;
				}
				var next = this.iShortestPath (floor + 1, refFloor, right, nextDir, out dump);
				rightDist = (next==int.MaxValue) ? next: rightDist + next;
				this.NbTotalClones = nbClones;
			}
			if (NbTotalClones>1 && (this.freeLifts > 0 || noLift) && !onALift)
			{
				// try to create some lift
				List<int> pos;

				if (!this.elevatorPos.TryGetValue (floor + 1, out pos)) {
					pos = new List<int> ();
				} else {
					pos = new List<int> (pos);
				}
				if (floor + 1 == this.ExitFloor) {
					pos.Add (this.ExitPos);
				}
				if (!pos.Contains(x))
					pos.Add (x);
				string nextDir;
				foreach(var scan in pos) {
					for (var i = 0; i < 2; i++) {
						var y = i + scan;
						if (i != 0 && pos.Contains (y))
							continue;
						var mustBlock = false;
						if (direction == "RIGHT" && y < x) {
							mustBlock = true;
							nextDir = "LEFT";
						} else if (direction == "LEFT" && y > x) {
							mustBlock = true;
							nextDir = "RIGHT";
						} else {
							nextDir = direction;
						}
						if ((left != -1 && y <= left) || (right != -1 && y >= right) || (mustBlock && this.NbTotalClones < 3))
							continue;
						var tmpFreeLifts = this.freeLifts;
						var nbClones = this.NbTotalClones;
						if (!noLift)
							this.freeLifts--;
						this.NbTotalClones--;
						var next = this.iShortestPath (floor + 1, refFloor, y, nextDir, out dump);
						if (next < int.MaxValue) {
							next += (mustBlock ? 6 : 3) + Math.Abs (x - y);
							if (y <= x && next < leftDist) {
								leftDist = next;
								if (y == x)
									leftAction = onALift ? Actions.Wait : Actions.Lift;
								else
									leftAction = mustBlock ? Actions.Block : Actions.Wait;
								left = y;
							}
							if (y >= x && next < rightDist) {
								rightDist = next;
								if (y == x)
									rightAction = Actions.Lift;
								else
									rightAction = mustBlock ? Actions.Block : Actions.Wait;
								right = y;
							}
						}
						this.freeLifts = tmpFreeLifts;
						this.NbTotalClones = nbClones;
					}
				}
			}
			if (floor == refFloor) {
				LogLine ("Exit on left {0}, on right {1}", leftDist, rightDist);
			}
			var retDist = int.MaxValue;
            if ((leftDist < rightDist) || (leftDist == rightDist && leftAction == Actions.Wait))
            {
                action = leftAction;
                retDist= leftDist;
				if (floor == refFloor) {
					LogLine ("Going left {0}, for a lift at {1}", leftDist, left);
				}
            }
            else
            {
                action = rightAction;
                retDist = rightDist;
				if (floor == refFloor) {
					LogLine ("Going right {0}, for a lift at {1}", rightDist, right);
				}
            }
			if (onALift)
				action = Actions.Wait;
			return retDist+= retDist== int.MaxValue ? 0 :  1;
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
			if (this.elevatorPos.ContainsKey (level)) {
				this.freeLifts--;
				LogLine("Free Lift at {0} {1}", level, x);
			} else {
				LogLine("Lift at {0} {1}", level, x);
			}
            this.AddElevator(level, x);
            this.NbLifts--;
			this.NbTotalClones--;
            Console.WriteLine("ELEVATOR"); // action: WAIT or BLOCK
        }
    }

    static void MainClone(string[] args)
    {
        var inputs = Console.ReadLine().Split(' ');
        var nbFloors = int.Parse(inputs[0]); // number of floors
        var width = int.Parse(inputs[1]); // width of the area
        var context = new Context(nbFloors);
        context.NbRounds = int.Parse(inputs[2]); // maximum number of rounds
        var exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
        var exitPos = int.Parse(inputs[4]); // position of the exit on its floor
        context.NbTotalClones = int.Parse(inputs[5])+1; // number of generated clones
        context.NbLifts = int.Parse(inputs[6]); // ignore (always zero)
        context.ExitFloor = exitFloor;
        context.ExitPos = exitPos;
		context.Width = width;
        var nbElevators = int.Parse(inputs[7]); // number of elevators

        LogLine("Nb Rounds: {0}", context.NbRounds);
        LogLine("Nb Clones: {0}", context.NbTotalClones);
        LogLine("Nb Lifts allowed :{0}", context.NbLifts);

        for (int i = 0; i < nbElevators; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
            int elevatorPosX = int.Parse(inputs[1]); // position of the elevator on its floor
            context.AddElevator(elevatorFloor, elevatorPosX);
        }

        context.Init();
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
            context.NbRounds--;
        }
    }
}