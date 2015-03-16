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
class PlayerSkynetHard
{
	internal class Pair
	{
		public int[] ID = new int[2];

		public Pair(int first, int second)
		{
			ID[0] = first;
			ID[1] = second;
		}

		public override bool Equals (object obj)
		{
			var other = obj as Pair;
			return Equals(other);
		}
		public bool Equals(Pair pair)
		{
			if (pair == null)
				return false;
			else
				return ID [0] == pair.ID [0] && ID [1] == pair.ID [1];
		}

		public override int GetHashCode ()
		{
			return ID [0] + 1000 * ID [1];
		}
	}

	internal class Node
	{
		static public int MaxDepth = 5;
		public int ID;
		public List<Node> Nodes = new List<Node> ();
		public bool IsGateWay;

		public bool IsNextToGateway()
		{
			return AccessibleGatewaysCount > 0;
		}

		public int AccessibleGatewaysCount {
			get {
				var result = 0;
				if (IsGateWay)
					return result;
				foreach (var node in Nodes) {
					if (node.IsGateWay) {
						result++;
					}
				}
				return result;
			}
		}

		public Node GetFirstGateway()
		{
			foreach (var node in this.Nodes)
				if (node.IsGateWay)
					return node;
			return null;
		}
	}

	internal class Graph
	{
		private IDictionary<int, Node> nodes;

		public Graph(int count)
		{
			nodes = new Dictionary<int, Node>(count);
			for(int i=0; i< count; i++)
			{	
				nodes[i] = new Node(){ ID = i};
			}
		}

		public void AddLink(int x, int y)
		{
			Console.Error.WriteLine ("Add link {0} -> {1}", x, y);
			nodes [x].Nodes.Add (nodes [y]);
			nodes [y].Nodes.Add (nodes [x]);
		}

		public void CutLink(int x, int y)
		{
			Console.Error.WriteLine ("Cut link {0} -> {1}", x, y);
			nodes [x].Nodes.Remove (nodes [y]);
			nodes [y].Nodes.Remove (nodes [x]);
		}

		public Node this[int ID]
		{
			get{ return this.nodes [ID];}
		}

		public IDictionary<Pair, int> ComputeMinimalPathLenghts()
		{
			var nodesCount = nodes.Count;
			// build initial graph
			var result = new Dictionary<Pair, int> (nodesCount * nodesCount);
			foreach (var node in nodes.Values) {
				if (node.IsGateWay)
					continue;
				result [new Pair (node.ID, node.ID)] = 0;
				foreach (var subnode in node.Nodes) {
					if (subnode.IsGateWay)
						continue;
					var pair = new Pair (node.ID, subnode.ID);
					result [pair] = 1 - subnode.AccessibleGatewaysCount > 0 ? 1 : 0;
				}
			}

			// initial
			for (int k = 0; k < nodesCount; k++) {
				for (int i = 0; i < nodesCount; i++) {
					int partialPath1;
					if (result.TryGetValue(new Pair(i, k), out partialPath1)) {
						for (int j = 0; j < nodesCount; j++) {
							int partialPath2;
							if (result.TryGetValue(new Pair(k, j), out partialPath2)) {
								int fullpath;
								if (!result.TryGetValue (new Pair (i, j), out fullpath) || fullpath> partialPath1 + partialPath2) {
									result [new Pair (i, j)] = partialPath1 + partialPath2;
								}
							}
						}
					}
				}
			}
			// initial
			Console.Error.WriteLine("Computed");
			DumpDistances (result);
			return result;
		}

		public Node GetMostRiskyNode(int idFrom)
		{
			var lengths = ComputeMinimalPathLenghts ();
			Node dangerousNode = null;
			int minLength = int.MaxValue;
			int minNodes = 0;
			if (this.nodes [idFrom].AccessibleGatewaysCount > 0)
				return this.nodes [idFrom];
			foreach (var node in nodes.Values) {
				var pair = new Pair (idFrom, node.ID);
				Console.Error.Write ("Scanning {0} ", node.ID);
				if (!lengths.ContainsKey (pair)) {
					// no path to this node
					Console.Error.WriteLine ("is inaccessible!", node.ID);
					continue;
				}

				var thisLength = lengths [pair];
				var exits = node.AccessibleGatewaysCount-1;
				Console.Error.WriteLine ("at {0} with {1} gateways!", thisLength, exits);
				if (node.AccessibleGatewaysCount>0 &&  (minNodes < exits || (minNodes == exits && minLength>thisLength))) {
					Console.Error.WriteLine ("Selected");
					// more dangerous
					dangerousNode = node;
					minLength = thisLength;
					minNodes = exits;
				}
			}
			Console.Error.WriteLine ("Found a dangerous node : {0} at {1} with {2} gateways.", dangerousNode.ID, minLength, minNodes);
			return dangerousNode;
		}

		static void DumpDistances(IDictionary<Pair, int> paths)
		{
			foreach (var path in paths) {
				Console.Error.WriteLine ("Path {0} -> {1} : {2}", path.Key.ID [0], path.Key.ID [1], path.Value);
			}
		}
	}

	static void Main(string[] args)
	{
		string[] inputs;
		inputs = Console.ReadLine().Split(' ');
		int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
		var graph = new Graph(N);
		int L = int.Parse(inputs[1]); // the number of links
		int E = int.Parse(inputs[2]); // the number of exit gateways
		for (int i = 0; i < L; i++)
		{
			inputs = Console.ReadLine().Split(' ');
			int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
			int N2 = int.Parse(inputs[1]);
			graph.AddLink (N1, N2);
		}
		for (int i = 0; i < E; i++)
		{
			int EI = int.Parse(Console.ReadLine()); // the index of a gateway node
			graph [EI].IsGateWay = true;
			Console.Error.WriteLine("{0} is a gateway", EI);
		}
		// scan length

		// game loop
		while (true)
		{
			int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
			var node = graph.GetMostRiskyNode(SI);

			var next = node.GetFirstGateway ();

			graph.CutLink (node.ID, next.ID);
			Console.WriteLine("{0} {1}", node.ID, next.ID); // Example: 0 1 are the indices of the nodes you wish to sever the link between
		}
	}
}