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
			if (val >= x) {
				break;
			}
			result = x;
		}
		return result;
	}

	static int FirstElevatorOnRight(int floor, int x)
	{
		int result = -1;
		if (!elevatorPos.ContainsKey (floor))
			return result;

		var list = elevatorPos [floor];
		foreach (var val in elevatorPos[floor]) {
			result = x;
			if (val >= x) {
				break;
			}
		}
		return result;
	}

	static void MoveToClosestLift(int floor, int x, string direction)
	{
		int left = FirstElevatorOnLeft (floor, x);
		int right = FirstElevatorOnRight (floor, x);

		if (left == -1 && right == -1) {
			Elevator ();
			return;
		}
		if (left == -1) {
			if (direction == "LEFT")
				Block ();
			else
				Wait ();
			return;
		}
		if (right == -1) {
			if (direction == "RIGHT")
				Block ();
			else
				Wait ();
			return;
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

			Console.Error.WriteLine("Clone at {0} {1}", cloneFloor, clonePos);
			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			// stop at edge of screen
			if (cloneFloor < 0 || clonePos < 0)
				Wait ();
			else if ((clonePos == 0 && direction == "LEFT") || (clonePos == width - 1 && direction == "RIGHT")) {
				Block ();
			} else if (elevatorPos.ContainsKey (cloneFloor)) {
				if ((direction == "LEFT" && elevatorPos [cloneFloor] > clonePos) || (direction == "RIGHT" && elevatorPos [cloneFloor] < clonePos))
					Block ();
				else
					// waiting to reach the elevator
					Wait (); // action: WAIT or BLOCK
			} else {
				Elevator (cloneFloor, clonePos);
			}
		}
	}

	static void Block()
	{
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