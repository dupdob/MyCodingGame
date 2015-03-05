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
class SolutionCadeau
{
	static List<int> budgets = new List<int> ();

	static void Main2(string[] args)
	{
		int N = int.Parse(Console.ReadLine());
		int C = int.Parse(Console.ReadLine());
		int total=0;
		for (int i = 0; i < N; i++)
		{
			int B = int.Parse(Console.ReadLine());
			budgets.Add (B);
			total += B;
		}

		var paiement = new List<int> ();
		int reste = C;
		budgets.Sort ();
		for (int i = 0; i < budgets.Count (); i++) {
			var budget = budgets [i];
			var averageToPay = reste / (budgets.Count - i);
			if (budget < averageToPay) {
				paiement.Add (budget);
				reste -= budget;
			} else {
				paiement.Add (averageToPay);
				reste -= averageToPay;
			}
		}
		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");
		if (total < C) {
			Console.WriteLine ("IMPOSSIBLE");
		} else {
			foreach (var pays in paiement) {
				Console.WriteLine ("{0}", pays);
			}
		}
	}
}