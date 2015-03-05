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
class SolutionScrabble
{
	static string onePoint = "eaionrtlsu";
	static string twoPoints = "dg";
	static string threePoints = "bcmp";
	static string fourPoints = "fhvwy";
	static string fivePoints = "k";
	static string eightPoints = "jx";
	static string tenPoints = "qz";

	static int Score(char car)
	{
		if (onePoint.IndexOf (car) >= 0)
			return 1;
		if (twoPoints.IndexOf (car) >= 0)
			return 2;
		if (threePoints.IndexOf (car) >= 0)
			return 3;
		if (fourPoints.IndexOf (car) >= 0)
			return 4;
		if (fivePoints.IndexOf (car) >= 0)
			return 5;
		if (eightPoints.IndexOf (car) >= 0)
			return 8;
		if (tenPoints.IndexOf (car) >= 0)
			return 10;
		return 0;
	}

	static int ScoreWord(string word)
	{
		var score = 0;
		foreach (var car in word) {
			score += Score (car);
		}
		return score;
	}

	static bool CanMakeWord(string letters, string word)
	{
		if (word.Count() > letters.Count())
			return false;
		foreach (var car in word) {
			var index = letters.IndexOf (car);
			if (index < 0)
				return false;

			letters = letters.Substring(0, index) + (letters.Substring(index+1));
		}
		return true;
	}

	static void Main2(string[] args)
	{
		int N = int.Parse(Console.ReadLine());
		var words = new Dictionary<string, int> ();

		for (int i = 0; i < N; i++)
		{
			string W = Console.ReadLine();
			words [W] = ScoreWord (W);
			Console.Error.WriteLine ("{0} is worth {1}.", W, ScoreWord (W));
		}
		string LETTERS = Console.ReadLine();
		Console.Error.WriteLine ("Letters are {0}.", LETTERS);

		var maxScore = 0;
		string bestWord = null;
		foreach (var word in words.Keys) {
			if (CanMakeWord(LETTERS, word))
			{
				var score = words[word];
				if (score > maxScore)
				{
					maxScore = score;
					bestWord = word;
				}
			}
		}
		Console.WriteLine(bestWord);
	}
}