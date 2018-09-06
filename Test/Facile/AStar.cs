using System;
using System.Collections.Generic;


/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 *
 * https://www.codingame.com/ide/puzzle/a-star-exercise
 **/
namespace CodingGame.Facile
{
    static class AStar
    {
        static void MainAStar(string[] args)
        {
            var inputs = Console.ReadLine().Split(' ');
            var N = int.Parse(inputs[0]);
            var E = int.Parse(inputs[1]);
            var S = int.Parse(inputs[2]);
            var G = int.Parse(inputs[3]);
            inputs = Console.ReadLine().Split(' ');
            var nodes = new Dictionary<int, Node>(N);
            for (var i = 0; i < N; i++)
            {
                nodes[i] = new Node(i, int.Parse(inputs[i]));
            }
            for (var i = 0; i < E; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                var x = int.Parse(inputs[0]);
                var y = int.Parse(inputs[1]);
                var c = int.Parse(inputs[2]);
                nodes[x].AddEdge(nodes[y], c);
                nodes[y].AddEdge(nodes[x], c);
            }

            var done = new List<Node>();
            var pending = new List<Node>();

            pending.Add(nodes[S]);
            nodes[S].DistanceFromStart = 0;
            while (true)
            {
                var shortestDist = int.MaxValue;
                Node shortestNode = null;
                foreach (var node in pending)
                {
                    if (node.DistancePassingBy >= shortestDist &&
                        (node.DistancePassingBy != shortestDist || node.Id >= shortestNode.Id)) continue;
                    shortestDist = node.DistancePassingBy;
                    shortestNode = node;
                }

                Console.WriteLine($"{shortestNode.Id} {shortestNode.DistancePassingBy}");
                if (shortestNode == nodes[G])
                {
                    break;
                }
                done.Add(shortestNode);
                pending.Remove(shortestNode);

                foreach (var node in shortestNode.Neighbours)
                {
                    if (done.Contains(node))
                        continue;
                    var dist = shortestNode.DistanceFromStart + shortestNode.DistanceTo(node);
                    if (!pending.Contains(node))
                        pending.Add(node);
                    else if (dist>=node.DistanceFromStart)
                        continue;
                    node.DistanceFromStart = dist;
                }
            }

        }

        private class Node
        {
            private readonly int id;
            private readonly IDictionary<Node, int> neighboursDistance = new Dictionary<Node, int>();

            public Node(int id, int heuristic)
            {
                this.id = id;
                Heuristic = heuristic;
            }

            public int Id => id;

            private int Heuristic { get; }
            public IEnumerable<Node> Neighbours => neighboursDistance.Keys;
            public int DistanceFromStart { get; set; } = int.MaxValue;
            public int DistancePassingBy => DistanceFromStart == int.MaxValue ? int.MaxValue : DistanceFromStart + Heuristic;

            public void AddEdge(Node neighbour, int distance)
            {
                neighboursDistance[neighbour] = distance;
            }

            private bool Equals(Node other)
            {
                return id == other.id;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == GetType() && Equals((Node) obj);
            }

            public override int GetHashCode()
            {
                return id;
            }

            public int DistanceTo(Node node)
            {
                return neighboursDistance[node];
            }
        }
    }
    
}