using System;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class NoSpoon2
{
    static void Main(string[] args)
    {
        var width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        Console.Error.WriteLine(width);
        var height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
        Console.Error.WriteLine(height);
        var map = new Node[width, height];
        for (var i = 0; i < height; i++)
        {
            string line = Console.ReadLine(); // width characters, each either 0 or .
            Console.Error.WriteLine(line);
            for (var j = 0; j < line.Length; j++)
            {
                var car = line[j];
                if (car == '.')
                {
                    map[j, i] = null;
                }
                else
                {
                    var node = new Node(j, i, car-'0');

                    map[j, i] = node;
                }
            }
        }
        var nodes = new List<Node>();
        // build a graph
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var node = map[i, j];
                if (node == null)
                {
                    continue;
                }
                // look for node going up
                for (var x = i - 1; x >= 0; x--)
                {
                    if (map[x, j] == null) continue;
                    node.AddNeighbour(map[x, j]);
                    break;
                }
                for (var x = i + 1; x < width; x++)
                {
                    if (map[x, j] == null) continue;
                    node.AddNeighbour(map[x, j]);
                    break;
                }
                for (var y = j - 1; y >= 0; y--)
                {
                    if (map[i, y] == null) continue;
                    node.AddNeighbour(map[i, y]);
                    break;
                }
                for (var y = j + 1; y < height; y++)
                {
                    if (map[i, y] == null) continue;
                    node.AddNeighbour(map[i, y]);
                    break;
                }
                Console.Error.WriteLine("Added node {0}", node);
                nodes.Add(map[i, j]);
            }
        }
        var links = new List<Link>();
//        CreateObviousLinks(nodes, links);
        StartScan(nodes, links);
  
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        foreach (var link in links)
        {
            Console.WriteLine("{0} {1} {2} {3} {4}", link.From.X, link.From.Y, link.To.X, link.To.Y, 1);
        }
        Console.Error.WriteLine("Instructions Finished");
    }

    private static bool CreateObviousLinks(ICollection<Node> nodes, ICollection<Link> links)
    {
        // handle trivial nodes (1 node and 1 neighbour or twice as much as neighbors)
        foreach (var node in nodes)
        {
            if (node.MissingLinks == 1 && node.NeighBours.Count == 1)
            {
                Console.Error.WriteLine($"Node @ {node.X},{node.Y} has only one neighbour.");
                var link = node.EstablishLink(node.NeighBours[0]);
                if (link != null)
                {
                    links.Add(link);
                }
            }
            else if (node.NeedeedLinks == node.NeighBours.Count*2)
            {
                Console.Error.WriteLine($"Node @ {node.X},{node.Y} is trivial.");
                foreach (var nextNode in node.NeighBours)
                {
                    var link = node.EstablishLink(nextNode);
                    if (link != null)
                    {
                        links.Add(link);
                    }
                    link = node.EstablishLink(nextNode);
                    if (link != null)
                    {
                        links.Add(link);
                    }
                }
            }
        }

        Console.Error.WriteLine("Optimization done.");
        return true;
    }

    private static void StartScan(List<Node> nodes, List<Link> links)
    {
        // find a simple node
        var minNeedeedLinks = 9;
        var minFreeNodes = minNeedeedLinks;
        Node start = null;
        foreach (var node in nodes)
        {
            if (node.NeedeedLinks > minNeedeedLinks)
            {
                continue;
            }
            minNeedeedLinks = node.NeedeedLinks;
            if (node.NeighBours.Count * 2 - node.NeedeedLinks> minFreeNodes)
            {
                continue;
            }
            minFreeNodes = node.NeighBours.Count*2 - node.NeedeedLinks;
            start = node;
            if (minFreeNodes == 0)
            {
                break;
            }
        }
        
        BuildNetwork(links, start);
    }

    private static bool BuildNetwork(IList<Link> links, Node start)
    {
        // we are done here
        if (start.MissingLinks == 0)
        {
            return true;
        }
// extract links
        var possibleLinks = new List<Node>(8);
        for (var i = 0; i < Math.Min(2, start.MissingLinks); i++)
        {
            foreach (var neighBour in start.NeighBours)
            {
                if (neighBour.MissingLinks > i)
                {
                    var newLink = new Link(start, neighBour);
                    foreach (var link in links)
                    {
                        if (newLink.Crosses(link))
                        {
                            newLink = null;
                            break;
                        }
                    }

                    if (newLink != null)
                    {
                        possibleLinks.Add(neighBour);
                    }
                }
            }
        }

        if (possibleLinks.Count < start.MissingLinks)
        {
            // not enough available links: dead end.
            return false;
        }

        // build all potential links
        var combinations = BuildCombinations(possibleLinks, start.MissingLinks);

        var tempLinks = new List<Link>(links.Count);
        tempLinks.AddRange(links);
        // try all combinations
        foreach (var combination in combinations)
        {
            foreach (var node in combination)
            {
                var link = start.EstablishLink(node);
                links.Add(link);
            }

            var success = true;
            foreach (var nextNode in start.NeighBours)
            {
                if (!start.IsLinkedTo(nextNode))
                {
                    // we dont try to extend to non linked node
                    continue;
                }
                if (!BuildNetwork(links, nextNode))
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                break;
            }
            // remove all added links
            while( tempLinks.Count < links.Count)
            {
                var link = links[tempLinks.Count];
                link.From.DestroyLink(link.To);
                links.RemoveAt(tempLinks.Count);
            }     
        }

        return true;
    }

    private static List<List<T>> BuildCombinations<T>(List<T> items, int len)
    {
        var result = new List<List<T>>();
        for (var i = 0; i < items.Count; i++)
        {
            if (len == 1)
            {
                var sub = new List<T> {items[i]};
                result.Add(new List<T>(sub));
            }
            else
            {
                foreach (var combination in BuildCombinations(items.GetRange(i+1, items.Count-(i+1)), len - 1))
                {
                    combination.Add(items[i]);
                    result.Add(combination);
                }
            }
        }

        return result;
    }
    
    private class Node 
    {
        public int X { get; }
        public int Y { get; }
        public int NeedeedLinks { get; }
        public readonly List<Node> NeighBours = new List<Node>();
        private readonly Dictionary<Node, int> linkedTo = new Dictionary<Node, int>();
        private int linkEstablished;

        // number of links that must be set to this node
        public int MissingLinks => this.NeedeedLinks - this.linkEstablished;

        public int FreeNeighboursLinks
        {
            get
            {
                var res = 0;
                foreach (var neighBour in this.NeighBours)
                {
                    if (neighBour.MissingLinks > 0)
                    {
                        res += Math.Min(neighBour.MissingLinks, 2 - this.linkedTo[neighBour]);
                    }
                }
                return res;
            }
        }

        public Node(int x, int y, int needeedLinks)
        {
            Y = y;
            X = x;
            NeedeedLinks = needeedLinks;
        }

        public bool IsLinkedTo(Node other)
        {
            return this.linkedTo[other] > 0;
        }

        public void AddNeighbour(Node node)
        {
            this.NeighBours.Add(node);
            this.linkedTo[node] = 0;
        }

        public Link EstablishLink(Node other)
        {
            if (!this.LinkTo(other))
            {
                return null;
            }
            other.LinkTo(this);
            var result = new Link(this, other);

            return result;
        }

        private bool LinkTo(Node other)
        {
            if (this.linkedTo[other] > 1)
            // link saturated
            {
                return false;
            }
            this.linkEstablished++;
            this.linkedTo[other]++;
            return true;
        }

        public bool DestroyLink(Node other)
        {
            if (!this.RemoveLink(other))
                return false;
            other.RemoveLink(this);
            return true;
        }

        private bool RemoveLink(Node other)
        {
            if (!this.NeighBours.Contains(other))
                return false;
            this.linkedTo[other]--;
            this.linkEstablished--;
            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1} ({2})", this.X, this.Y, this.MissingLinks);
        }
    }

    private class Link
    {
        public Node From { get;}
        public Node To { get;}

        public Link(Node from, Node to)
        {
            From = from;
            To = to;
        }

        public bool Crosses(Link other)
        {
            if (this.From.X == this.To.X && other.From.X != other.To.X)
            {
                return (this.From.Y - other.From.Y) * (other.To.Y - this.To.Y) > 0 &&
                       (other.To.X - this.To.X) * (this.From.X - other.From.X) > 0;
            }
            if (this.From.Y == this.To.Y && other.From.Y != other.To.Y)
            {
                return (this.From.Y - other.From.Y) * (other.To.Y - this.To.Y) > 0 &&
                       (other.To.X - this.To.X) * (this.From.X - other.From.X) > 0;
            }
            return false;
        }
    
        public override string ToString()
        {
            return $"{From.X}:{From.Y}<=>{this.To.X}:{this.To.Y}";
        }
    }
}