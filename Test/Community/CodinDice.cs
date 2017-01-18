using System;
using System.Collections.Generic;

/**
 Rossi is a cute chibi devil who rolls giant dice, arranged on a grid, by walking across the top of them.

Your goal is to help Rossi solve the puzzle with the minimum number of die rolls.

The puzzle is solved when the following two conditions hold true:
- The value of each die is 6, i.e. they all have 6 pips showing on the TOP as such: ⚅
- All dice form a single connected group.


==============================
Moving around
==============================

- Rossi's position is on top of the last die he moved. If he hasn't moved yet, he's on top of the first die of the list.
- Dice form a group when they are standing adjacent to one another. Diagonals do not count.
- Rossi can walk freely between dice in the same group. Walking is not counted against the solution's length.
- The map is always a 4 x 4 square map; the coordinates range from 0 to 3. Rossi is not allowed to move outside of the boundaries.


==============================
Rolling dice
==============================

- If there is a free grid space from Rossi's standing position on a die, he can roll the die there. Since he can walk freely among the group, he can thus roll any "free" die within the group boundary.
- When a die is rolled, one of its bottom edges (the one between the old and new position) stays still, causing it to tumble and 'roll' in the relevant direction.
- Therefore rolling a die can change where the number 6 face is.

For example, if the die is rolled to the RIGHT and the 6 face was TOP, it will then be EAST. If the 6 face was BOTTOM, it will become WEST. If it was NORTH or SOUTH, it would twist but remain in the same relative position. (Pick a die and roll it in one direction, you'll see that two faces stay)

- Rolling a die adds one to the solution's length.
- We're only interested in the 6 faces' ending position, so you only need to track that face on each die.
- Each time Rossi rolls a die, output the position of that die and the direction in which it was rolled to to STDOUT.


==============================
IRON dice
==============================

Some dice are made of IRON. They are very heavy.

- Rossi cannot roll IRON dice. He cannot "jump" from an IRON die to an empty spot either.
- Their 6 face is always on TOP.


==============================
Output to STDOUT
==============================

You must find the shortest solution to the puzzle. The puzzles are designed so that there is always only one "shortest" solution.

- The shortest solution is based on the number of times a die was rolled.

Try checking the test cases with real dice if you are confused! 
  **/
class CodinDice
{
    static void MainDice(string[] args)
    {
        var n = int.Parse(Console.ReadLine());
        var dices = new List<Dice>(n);
        for (var i = 0; i < n; i++)
        {
            var inputs = Console.ReadLine().Split(' ');
            var dice = new Dice();
            dice.ID = i;
            dice.X = int.Parse(inputs[0]);
            dice.Y = int.Parse(inputs[1]);
            dice.Face = inputs[2];
            dices.Add(dice);
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine("answer");
    }

    class Dice
    {
        public int ID;
        public int X;
        public int Y;
        public string Face;

    }
}