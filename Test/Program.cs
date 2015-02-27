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
class Solution2
{
	class Node
	{
		public List<Node> links = new List<Node> ();
		public Dictionary<int, int> PathsDepth = new Dictionary<int, int> ();
		public int? Time;
		public int ID;

		public Node(int id)
		{
			this.ID = id;
		}

		public int Scan()
		{
			return Scan (0, -1);
		}

		private int Scan(int depth, int from)
		{
			depth++;
			var maxTime = 0;
			foreach (var node in links) {
				if (node.ID == from)
					// we are coming from this node, nothing to do
					continue;
				if (!PathsDepth.ContainsKey (node.ID))
					PathsDepth [node.ID] = node.Scan (0, this.ID) +1;
				maxTime = Math.Max(PathsDepth [node.ID], maxTime);
			}
			Console.Error.WriteLine ("Scanning {0} at time {1}", ID, maxTime+1);
			return maxTime ;
		}

		public void Reset()
		{
		}
	}

	static void Main(string[] args)
	{
		var nodesDic = new Dictionary<int, Node> ();
		int n = int.Parse(Console.ReadLine()); // the number of adjacency relations
		for (int i = 0; i < n; i++)
		{
			string[] inputs = Console.ReadLine().Split(' ');
			int xi = int.Parse(inputs[0]); // the ID of a person which is adjacent to yi
			int yi = int.Parse(inputs[1]); // the ID of a person which is adjacent to xi
			if (!nodesDic.ContainsKey (xi)) {
				nodesDic [xi] = new Node (xi);
			}

			if (!nodesDic.ContainsKey (yi)) {
				nodesDic [yi] = new Node (yi);
			}

			nodesDic [xi].links.Add (nodesDic [yi]);
			nodesDic [yi].links.Add (nodesDic [xi]);
		}

		int minTime = int.MaxValue;
		foreach (var node in nodesDic.Values) {
			var time = node.Scan ();

			Console.Error.WriteLine ("Time from {0} : {1}", node.ID, time);
			minTime = Math.Min (time, minTime);
			node.Reset ();
		}
		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");

		Console.WriteLine("{0}", minTime); // The minimal amount of steps required to completely propagate the advertisement
	}
}