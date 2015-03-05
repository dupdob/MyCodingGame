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
class PlayerSkynet
{
	class Node
	{
		static public int MaxDepth = 5;
		public int ID;
		public List<Node> Nodes = new List<Node> ();
		public bool IsGateWay;

		public bool IsNextToGateway()
		{
			foreach (var node in Nodes) {
				if (node.IsGateWay) {
					return true;
				}
			}
			return false;
		}
		public bool IsDeadEnd()
		{
			var path = new List<Node> ();
			return IsDeadEnd (path);
		}

		private bool IsDeadEnd(List<Node> path)
		{
			if (Nodes.Count > 2) {
				return false;
			}
			if (Nodes.Count == 1)
				return true;

			path.Add (this);
			if (!path.Contains (Nodes [0]))
				return Nodes [0].IsDeadEnd (path);
			if (!path.Contains (Nodes [1]))
				return Nodes [1].IsDeadEnd (path);
			return false;
		}

		public int NonGatewayPath()
		{
			int path = 0;
			foreach (var node in Nodes) {
				if (!node.IsGateWay) {
					path++;
				}
			}
			return path;
		}

		public int DistanceToGateWay()
		{
			return DistanceToGateWay (new List<Node> ());
		}

		private int DistanceToGateWay(List<Node> path)
		{
			if (this.IsGateWay) {
				Console.Error.WriteLine("Found GW at {1}, dist is {0}", path.Count, this.ID);
				return path.Count;
			}
			if (path.Count >= MaxDepth) {
				// we stop searching
				return int.MaxValue;
			}
			var minDist = int.MaxValue;
			foreach (var next in this.Nodes) {
				if (path.Contains (next))
					continue;
				var path2 = new List<Node> (path);
				path2.Add (next);
				var dist = next.DistanceToGateWay (path2);
				if (dist < minDist) {
					minDist = dist;
				}
			}
			return minDist;
		}
	}

	static void Main2(string[] args)
	{
		string[] inputs;
		inputs = Console.ReadLine().Split(' ');
		int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
		var nodes = new Dictionary<int, Node> ();
		for (int i = 0; i < N; i++) {
			nodes [i] = new Node (){ ID = i };
		}
		int L = int.Parse(inputs[1]); // the number of links
		int E = int.Parse(inputs[2]); // the number of exit gateways
		for (int i = 0; i < L; i++)
		{
			inputs = Console.ReadLine().Split(' ');
			int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
			int N2 = int.Parse(inputs[1]);
			nodes [N1].Nodes.Add (nodes [N2]);
			nodes [N2].Nodes.Add (nodes [N1]);
		}
		for (int i = 0; i < E; i++)
		{
			int EI = int.Parse(Console.ReadLine()); // the index of a gateway node
			nodes [EI].IsGateWay = true;
			Console.Error.WriteLine("{0} is a gateway", EI);
		}

		// game loop
		while (true)
		{
			int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn

			var curNode = nodes [SI];
			int? tocut = null;


			if (!tocut.HasValue) {
				foreach (var nextNode in curNode.Nodes) {
					if (nextNode.IsGateWay) {
						// we need to cut this path
						tocut = nextNode.ID;
						break;
					}
				}
			}

			if (!tocut.HasValue) {
				if (curNode.Nodes.Count < 3) {
					if (curNode.Nodes.Count == 1) {
						tocut = curNode.Nodes [0].ID;
					} else {
						if (curNode.Nodes [0].IsDeadEnd ()) {
							tocut = curNode.Nodes [1].ID;
						} else {
							tocut = curNode.Nodes [0].ID;
						}
					}
				}
			}
			if (!tocut.HasValue) {
				// let's look for something to cut
				foreach (var node in nodes.Values) {
					if (node.NonGatewayPath () == 2) {
						foreach (var endNode in node.Nodes) {
							if (endNode.NonGatewayPath () == 2) {
								SI = node.ID;
								tocut = endNode.ID;
								break;
							}
						}
					}
					if (tocut.HasValue)
						break;
				}
				if (!tocut.HasValue) {
					var minDist = int.MaxValue;
					int minID = SI;
					foreach (var nextNodes in curNode.Nodes) {
						var dist = nextNodes.DistanceToGateWay ();
						if (dist <= minDist) {
							minDist = dist;
							minID = nextNodes.ID;
							Console.Error.WriteLine("Short path at {0} (dist is {1})", minID, dist);
						}
					}
					tocut = minID;
				}
			}
			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			nodes[SI].Nodes.Remove(nodes[tocut.Value]);
			nodes[tocut.Value].Nodes.Remove(nodes[SI]);
			Console.WriteLine("{0} {1}", SI, tocut); // Example: 0 1 are the indices of the nodes you wish to sever the link between
		}
	}
}