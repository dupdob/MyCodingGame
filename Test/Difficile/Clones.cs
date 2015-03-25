using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class ClonesHard
{
	static Dictionary<int, List<int> > elevatorPos;
	static Dictionary<int, List<int> > blockerPos;

	static void Log(string msg, params object[] pars)
	{
		Console.Error.Write (msg, pars);
	}

	static void LogLine(string msg, params object[] pars)
	{
		Console.Error.WriteLine (msg, pars);
	}

	static void LogLine()
	{
		Console.Error.WriteLine ();
	}

	static void AddElevator(int elevatorFloor, int elevatorPosX)
	{
		LogLine("Elevator at {0} {1}", elevatorFloor, elevatorPosX);
		if (!elevatorPos.ContainsKey (elevatorFloor)) {
			elevatorPos [elevatorFloor] = new List<int> (2);
		}
		elevatorPos [elevatorFloor].Add(elevatorPosX);
		elevatorPos [elevatorFloor].Sort ();
	}

	static int FirstElevatorOnLeft(int floor, int x)
	{
		int result = -1;
		if (!elevatorPos.ContainsKey (floor))
			return result;

		var list = elevatorPos [floor];
		foreach (var val in elevatorPos[floor]) {
			if (val > x) {
				break;
			}
			result = val;
		}
		LogLine ("Elevator on left at {0}", result);
		return result;
	}

	static int FirstElevatorOnRight(int floor, int x)
	{
		int result = -1;
		if (!elevatorPos.ContainsKey (floor))
			return result;

		var list = elevatorPos [floor];
		foreach (var val in elevatorPos[floor]) {
			if (val >= x) {
				result = val;
				break;
			}
		}
		LogLine ("Elevator on right at {0}", result);
		return result;
	}

	static void MoveToClosestLift(int floor, int x, string direction)
	{
		int left = FirstElevatorOnLeft (floor, x);
		int right = FirstElevatorOnRight (floor, x);

		if (left == -1 && right == -1) {
			Elevator (floor, x);
			return;
		}
		if (direction == "RIGHT") {
			if (right == -1 || (left>=0 && x - left  < right - x))
				Block (floor, x);
			else
				Wait ();
		} else {
			if (left == -1 || (right >=0 && x - left  > right - x ) )
				Block (floor, x);
			else
				Wait ();
		}

	}

	static void Main(string[] args)
	{
		string[] inputs;
		inputs = Console.ReadLine().Split(' ');
		int nbFloors = int.Parse(inputs[0]); // number of floors
		int width = int.Parse(inputs[1]); // width of the area
		int nbRounds = int.Parse(inputs[2]); // maximum number of rounds
		int exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
		int exitPos = int.Parse(inputs[4]); // position of the exit on its floor
		int nbTotalClones = int.Parse(inputs[5]); // number of generated clones
		int nbAdditionalElevators = int.Parse(inputs[6]); // ignore (always zero)
		int nbElevators = int.Parse(inputs[7]); // number of elevators

		LogLine ("Nb Rounds: {0}", nbRounds);
		LogLine ("Nb Clones: {0}", nbTotalClones);
		LogLine ("Nb Extra Lifts :{0}", nbAdditionalElevators);

		blockerPos = new Dictionary<int, List<int>> (nbFloors);
		elevatorPos = new Dictionary<int, List<int>> (nbFloors);
		for (int i = 0; i < nbElevators; i++)
		{
			inputs = Console.ReadLine().Split(' ');
			int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
			int elevatorPosX = int.Parse(inputs[1]); // position of the elevator on its floor
			AddElevator(elevatorFloor, elevatorPosX);
		}
		AddElevator(exitFloor, exitPos);

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
				Wait ();
			else
				MoveToClosestLift (cloneFloor, clonePos, direction);
		}
	}

	static void Block(int blockerFloor, int blockerPosX)
	{
		if (!blockerPos.ContainsKey (blockerFloor)) {
			blockerPos [blockerFloor] = new List<int> (2);
		} else if (blockerPos [blockerFloor].Contains (blockerPosX)) {
			Wait ();
			return;
		}
		LogLine("Blocker at {0} {1}", blockerFloor, blockerPosX);
		blockerPos [blockerFloor].Add(blockerPosX);
		blockerPos [blockerFloor].Sort ();

		Console.WriteLine("BLOCK"); // action: WAIT or BLOCK
	}

	static void Wait()
	{
		Console.WriteLine("WAIT"); // action: WAIT or BLOCK
	}

	static void Elevator(int level, int x)
	{
		AddElevator (level, x);
		Console.WriteLine("ELEVATOR"); // action: WAIT or BLOCK
	}

}