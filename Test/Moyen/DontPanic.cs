using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class DontPanic
{
    static void MainPanic(string[] args)
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

        var elevatorPos = new Dictionary<int, int>(nbFloors);
        for (int i = 0; i < nbElevators; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
            int elevatorPosX = int.Parse(inputs[1]); // position of the elevator on its floor
            Console.Error.WriteLine("Elevator at {0} {1}", elevatorFloor, elevatorPosX);
            elevatorPos[elevatorFloor] = elevatorPosX;
        }
        elevatorPos[exitFloor] = exitPos;
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
            if ((clonePos == 0 && direction == "LEFT") || (clonePos == width - 1 && direction == "RIGHT"))
                Block();
            else
            {
                // if nothing ahead, block
                if (elevatorPos.ContainsKey(cloneFloor) &&
                    ((direction == "LEFT" && elevatorPos[cloneFloor] > clonePos) || (direction == "RIGHT" && elevatorPos[cloneFloor] < clonePos)))
                    Block();
                else
                    Console.WriteLine("WAIT"); // action: WAIT or BLOCK
            }
        }
    }

    static void Block()
    {
        Console.WriteLine("BLOCK"); // action: WAIT or BLOCK
    }

}