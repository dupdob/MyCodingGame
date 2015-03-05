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
class SolutionCalculMaya
{
	class Digit
	{
		private string[] lines = new string[4];

		public Digit(string line1, string line2, string line3, string line4)
		{
			lines[0] = line1;
			lines[1] = line2;
			lines[2] = line3;
			lines[3] = line4;
		}

		public override bool Equals(object other)
		{
			var otherDigit = other as Digit;
			if (otherDigit == null)
				return false;
			return this.lines[0] == otherDigit.lines[0] && this.lines[1] == otherDigit.lines[1]
				&& this.lines[2] == otherDigit.lines[2] && this.lines[3] == otherDigit.lines[3];
		}

		public override int GetHashCode()
		{
			int hash = lines[0].GetHashCode() + lines[1].GetHashCode()+lines[2].GetHashCode() + lines[3].GetHashCode();
			return hash;
		}
	}

	static void Main(string[] args)
	{
		string[] inputs = Console.ReadLine().Split(' ');
		int L = int.Parse(inputs[0]);
		int H = int.Parse(inputs[1]);
		string[] lines = new string[4];
		for (int i = 0; i < H; i++)
		{
			string numeral = Console.ReadLine();
			lines [i] = numeral;
		}
		Digit[] digits = new Digit[20];
		for (int i = 0; i < 20; i++) {
			digits [i] = new Digit (lines [0].Substring (i * 4, 4), lines [1].Substring (i * 4, 4), lines [2].Substring (i * 4, 4), lines [3].Substring (i * 4, 4));
		}
		int S1 = int.Parse(Console.ReadLine());
		for (int i = 0; i < S1; i++)
		{
			string num1Line = Console.ReadLine();
		}
		int S2 = int.Parse(Console.ReadLine());
		for (int i = 0; i < S2; i++)
		{
			string num2Line = Console.ReadLine();
		}
		string operation = Console.ReadLine();

		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");

		Console.WriteLine("result");
	}
}