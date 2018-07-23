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
class SolutionClone
{

	class Node
	{
		static public Dictionary<int, Node> Nodes = new Dictionary<int, Node> ();
		private List<Node> subNode = new List<Node>();

		private int? depth = null;

		public void AddLink(int sub)
		{
			if (!Nodes.ContainsKey (sub)) {
				Nodes.Add (sub, new Node ());
			}
			subNode.Add (Nodes [sub]);
		}

		public int Depth()
		{
			if (!depth.HasValue) {
				int newDepth = 0;
				foreach (var node in this.subNode) {
					newDepth = Math.Max (newDepth, node.Depth());
				}
				depth = newDepth + 1;
			}
			return depth.Value;
		}

		static public void AddLink(int root, int sub)
		{
			if (!Nodes.ContainsKey (root)) {
				Nodes.Add (root, new Node ());
			}
			Nodes [root].AddLink (sub);
		}
	}

	static void MainClone(string[] args)
	{
		int n = int.Parse(Console.ReadLine()); // the number of relationships of influence
		for (int i = 0; i < n; i++)
		{
			string[] inputs = Console.ReadLine().Split(' ');
			int x = int.Parse(inputs[0]); // a relationship of influence between two people (x influences y)
			int y = int.Parse(inputs[1]);
			Node.AddLink (x, y);
		}

		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");
		var depth = 0;
		foreach (var node in Node.Nodes.Values) {
			depth = Math.Max (depth, node.Depth ());
		}
		Console.WriteLine("{0}", depth); // The number of people involved in the longest succession of influences
	}
}