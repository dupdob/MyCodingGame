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
	static Dictionary<Digit, int> digits = new Dictionary<Digit, int> (20); 
	static Digit[] digitList = new Digit[20];
	static int CharLen;
	static int CharHeight;

	class Digit
	{
		private IList<string> lines;

		public Digit(IList<string> extract)
		{
			lines = extract;
		}

		public override bool Equals(object other)
		{
			var otherDigit = other as Digit;
			if (otherDigit == null)
				return false;
			for (int i = 0; i < lines.Count; i++) {
				if (lines [i] != otherDigit.lines [i])
					return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int hash = 0;
			foreach (var line in lines) {
				hash += line.GetHashCode ();
			}
			return hash;
		}

		public void Print()
		{
			foreach (var line in lines) {
				Console.WriteLine (line);
			}
		}
	}

	static long ReadVal()
	{
		int S1 = int.Parse(Console.ReadLine());
		var val = 0L;
		for (int i = 0; i < S1/CharHeight; i++)
		{
			var lines = new List<string> (CharHeight);
			val *= 20;
			for (int j = 0; j < CharHeight; j++) {
				lines.Add(Console.ReadLine ());
			}
			var thisDigit = new Digit (lines);
			val += digits [thisDigit];
		}
		Console.Error.WriteLine ("Value is {0}", val);
		return val;
	}

	static void Print(long value)
	{
		var toPrint = new List<int> ();

		do {
			toPrint.Add ((int)(value % 20));
			value /= 20;
		} while (value != 0);
		toPrint.Reverse ();
		foreach (var next in toPrint) {
			Console.Error.WriteLine("Digit: {0}", next);
			digitList [next].Print ();
		}
	}

	static void Main2(string[] args)
	{
		string[] inputs = Console.ReadLine().Split(' ');
		CharLen = int.Parse(inputs[0]);
		CharHeight = int.Parse(inputs[1]);
		string[] lines = new string[CharHeight];
		for (int i = 0; i < CharHeight; i++)
		{
			string numeral = Console.ReadLine();
			lines [i] = numeral;
		}
		for (int i = 0; i < 20; i++) {
			var extract = new List<string> (CharHeight);
			foreach (var line in lines) {
				extract.Add(line.Substring(i * CharLen, CharLen));
			}
			var digit = new Digit (extract);
			digits [digit] = i;
			digitList [i] = digit;
		}
		var val = ReadVal ();
		var val2 = ReadVal ();
		string operation = Console.ReadLine();

		long result;
		switch (operation) {
		case "+":
			result = val + val2;
			break;
		case "-":
			result = val - val2;
			break;
		case "*":
			result = val * val2;
			break;
		case "/":
			result = val / val2;
			break;
		default:
			result = 0;
			break;
		}
		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");
		Console.Error.WriteLine ("Result : {0}", result);
		Print (result);
	}
}