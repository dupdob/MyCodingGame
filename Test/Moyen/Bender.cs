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
internal enum Directions {North, West, South, East, None}; 


class SolutionBender
{

	class Bender
	{
		class Step
		{
			public int Column;
			public int Row;
			public Directions Direction;

			public Step Move(Directions dir)
			{
				var ret = new Step ();
				ret.Direction = dir;
				ret.Row = this.Row;
				ret.Column = this.Column;

				switch (dir) {
				case Directions.East:
					ret.Column++;
					break;
				case Directions.North:
					ret.Row--;
					break;
				case Directions.West:
					ret.Column--;
					break;
				case Directions.South:
					ret.Row++;
					break;
				}
				return ret;
			}
		}

		private int referencePoint = 0;
		private List<string> Area = new List<string> ();

		private List<Step> previousSteps = new List<Step> ();

		private Directions[] priorities = { Directions.South, Directions.East, Directions.North, Directions.West };

		private bool BreakMode;
		private bool Inverted;
		private Directions LastDir;

		private bool HasLoop;

		public Bender(IEnumerable<string> area)
		{
			int row = 0;

			foreach(var line in area)
			{
				if (line.IndexOf("@")>=0)
				{
					var start = new Step();
					LastDir = Directions.South;
					start.Direction = Directions.None;
					start.Row = row;
					start.Column = line.IndexOf("@");
					previousSteps.Add(start);
				}
				Area.Add(line);
				row++;
			}
		}

		public bool OneStep()
		{
			var current = this.previousSteps.Last ();
			var dirs = new List<Directions> (priorities);
			bool ret = true;
			Step next = null;

			if (Inverted) {
				dirs.Reverse ();
			}
			dirs.Remove (this.LastDir);
			dirs.Insert (0, this.LastDir);

			foreach (var dir  in dirs) {
				this.LastDir = dir;
				Console.Error.Write ("Try {0},", dir);
				next = current.Move (dir);
				switch (Area [next.Row] [next.Column]) {
				case '#':
					continue;
				case ' ':
					break;
				case 'X':
					if (BreakMode) {
						Console.Error.WriteLine ("Break wall");
						Area [next.Row] = Area[next.Row].Substring(0, next.Column)+' '+Area[next.Row].Substring(next.Column+1);
						this.referencePoint = this.previousSteps.Count;
						break;
					}
					continue;
				case 'I':
					this.Inverted = !this.Inverted;
					break;
				case 'B':
					this.BreakMode = !this.BreakMode;
					this.referencePoint = this.previousSteps.Count;
					break;
				case 'N':
					this.LastDir = Directions.North;
					break;
				case 'E':
					this.LastDir = Directions.East;
					break;
				case 'S':
					this.LastDir = Directions.South;
					break;
				case 'W':
					this.LastDir = Directions.West;
					break;
				case '$':
					ret = false;
					break;
				case 'T':
					{
						Console.Error.Write ("Teleport from {0}:{1}", next.Column, next.Row);
						bool found = false;
						for (var i = 0; i < this.Area.Count; i++) {
							int teleportCol = this.Area[i].IndexOf ("T");
							while (teleportCol != -1) {
								if (teleportCol != next.Column || i != next.Row) {
									next.Column = teleportCol;
									next.Row = i;
									found = true;
									break;
								}
								teleportCol = this.Area[i].IndexOf ("T", teleportCol + 1);
							}
							if (found)
								break;
						}
						Console.Error.Write (" to {0}:{1} ", next.Column, next.Row);
						break;
					}
				}
				break;
			}
			ret = StoreStep (next) && ret;
			Console.Error.WriteLine ("Moved to: {0}:{1} dir {2}", next.Column, next.Row, next.Direction);
			return ret;
		}

		private bool StoreStep(Step step)
		{
			for (int i = referencePoint; i < this.previousSteps.Count; i++) {
				var prev = this.previousSteps [i];
				if (prev.Column == step.Column && prev.Row == step.Row && prev.Direction == step.Direction) {
					this.HasLoop = true;
					return false;
				}
			}
			this.previousSteps.Add (step);
			return true;
		}

		public void Dump()
		{
			if (HasLoop) {
				Console.WriteLine ("LOOP");
				return;
			}
			foreach (var step in this.previousSteps) {
				if (step.Direction == Directions.None)
					continue;
				Console.WriteLine (step.Direction.ToString ().ToUpper ());
			}
		}
	}


	static void Main2(string[] args)
	{
		string[] inputs = Console.ReadLine().Split(' ');
		var tempArea = new List<string> ();
		int L = int.Parse(inputs[0]);
		int C = int.Parse(inputs[1]);
		for (int i = 0; i < L; i++)
		{
			string row = Console.ReadLine();
			tempArea.Add (row);
			Console.Error.WriteLine (row);
		}
		var bender = new Bender (tempArea);

		while (bender.OneStep ())
			;
		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");

		bender.Dump ();
	}
}