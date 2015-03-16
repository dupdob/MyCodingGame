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
			
		public IDictionary<int, int> ComputeMinimalPathLenghts(int idFrom)
		{
			var nodesCount = nodes.Count;
			var result = new Dictionary<int, int> (nodesCount);

			var queue = new List<int> (nodesCount);

			foreach (var node in this.nodes.Values) {
				if (node.IsGateWay)
					continue;
				result [node.ID] = int.MaxValue;
				queue.Add (node.ID);
			}
			result [idFrom] = 0;

			while (queue.Count > 0) {
				var minlength = int.MaxValue;
				var minID = -1;
				foreach (var id in queue) {
					if (result [id] < minlength) {
						minlength = result [id];
						minID = id;
					}
				}
				queue.Remove (minID);

				var node = this.nodes [minID];
				var curdist = result [minID];
				foreach (var next in node.Nodes) {
					if (next.IsGateWay)
						continue;
					var nextLength = curdist + (next.AccessibleGatewaysCount > 0 ? 0 : 1); 
					if (nextLength < result [next.ID])
						result [next.ID] = nextLength;
				}
			}

			return result;
		}

		public Node GetMostRiskyNode(int idFrom)
		{
			var lengths = ComputeMinimalPathLenghts (idFrom);
			Node dangerousNode = null;
			int minLength = int.MaxValue;
			int minNodes = 0;
			if (this.nodes [idFrom].AccessibleGatewaysCount > 0)
				return this.nodes [idFrom];
			foreach (var node in nodes.Values) {
				if (node.IsGateWay)
					continue;
				Console.Error.Write ("Scanning {0} ", node.ID);

				var thisLength = lengths [node.ID];
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