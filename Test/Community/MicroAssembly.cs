using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
The task is to write an interpreter for a very simplistic assembly language and print the four register values after the instructions have been processed.

Explanations:
a, b, c, d: registers containing integer values
DEST: write the operation result to this register
SRC: read operand value from this register
IMM: immediate/integer value
SRC|IMM means that the operand can be either a register or an immediate value.

Command and operands are always separated by a blank.
The program starts with the first instruction, iterates through all instructions and ends when the last instruction was processed.
Only valid input is given and there are no endless loops or over-/underflows to be taken care of.

List of defined assembly instructions:

MOV DEST SRC|IMM
copies register or immediate value to destination register
Example: MOV a 3 => a = 3

ADD DEST SRC|IMM SRC|IMM
add two register or immediate values and store the sum in destination register
Example: ADD b c d => b = c + d

SUB DEST SRC|IMM SRC|IMM
subtracts the third value from the second and stores the result in destination register
Example: SUB d d 2 => d = d - 2

JNE IMM SRC SRC|IMM
jumps to instruction number IMM (zero-based) if the other two values are not equal
Example: JNE 0 a 0 => continue execution at line 0 if a is not zero

Full example:
(line 0) MOV a 3      # a = 3
(line 1) ADD a a -1   # a = a + (-1)
(line 2) JNE 1 a 1    # jump to line 1 if a != 1
Program execution:
0: a = 3
1: a = a + (-1) = 3 + (-1) = 2
2: a != 1 -> jump to line 1
1: a = a + (-1) = 2 + (-1) = 1
2: a == 1 -> don't jump

Program finished, register a now contains value 1
Entrée
Line 1 contains the blank-separated values for the registers a, b, c, d
Line 2 contains the number n of the following instruction lines
n lines containing assembly instructions
Sortie
Line with the four blank-separated register values of a, b, c, d
Contraintes
0 < n < 16
-2^15 ≤ a, b, c, d < 2^15
Overflow and underflow behavior is unspecified (and not tested). **/

class MicroAssembly
{
    private static Dictionary<string, int> _registers;

    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        _registers = new Dictionary<string, int>();
        _registers["a"] = int.Parse(inputs[0]);
        _registers["b"] = int.Parse(inputs[1]);
        _registers["c"] = int.Parse(inputs[2]);
        _registers["d"] = int.Parse(inputs[3]);
        int n = int.Parse(Console.ReadLine());
        string[] program = new string[n];
        for (int i = 0; i < n; i++)
        {
            program[i] = Console.ReadLine();
        }
        // now run the program
        for (int pc = 0; pc < program.Length; pc++)
        {
            var parts = program[pc].Split(' ');
            switch (parts[0])
            {
                case "MOV":
                    _registers[parts[1]] = IMMREG(parts[2]);
                    break;
                case "ADD":
                    _registers[parts[1]] = IMMREG(parts[2])+IMMREG(parts[3]);
                    break;
                case "SUB":
                    _registers[parts[1]] = IMMREG(parts[2])-IMMREG(parts[3]);
                    break;
                case "JNE":
                    if (IMMREG(parts[2]) != IMMREG(parts[3]))
                    {
                        // need to decrement pc because it will be incremented by for instruction
                        pc = int.Parse(parts[1]) - 1;
                    }
                    break;
            }
        }
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine("{0} {1} {2} {3}",_registers["a"], _registers["b"], _registers["c"], _registers["d"]);
    }

    // get register or immediate
    private static int IMMREG(string parts)
    {
        var val = _registers.ContainsKey(parts) ? _registers[parts] : int.Parse(parts);
        return val;
    }
}