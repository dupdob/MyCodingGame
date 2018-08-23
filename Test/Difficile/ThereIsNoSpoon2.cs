using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

/**
 * Don't let the machines win. You are humanity's last hope...
 * https://www.codingame.com/ide/puzzle/there-is-no-spoon-episode-2
 **/
class NoSpoon2
{
    static void MainSpoon2(string[] args)
    {
        var width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        Console.Error.WriteLine(width);
        var height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
        Console.Error.WriteLine(height);
        var map = new Node[width, height];
        for (var i = 0; i < height; i++)
        {
            var line = Console.ReadLine(); // 'width' characters, each either digit or .
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
                // look for a node going up
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
                nodes.Add(map[i, j]);
            }
        }
        var links = new List<Link>();
        // simplify problem
        CreateObviousLinks(nodes, links);
        var allSegments = BuildLinkOptions(nodes, links);
        var allIndices = new Dictionary<Node, int>(allSegments.Count);
        foreach (var key in allSegments.Keys)
        {
            allIndices[key] = 0;
        }
        // now try all combination (brute force)

        while (true)
        {
            var builtLinks = new List<Link>();
            var failed = false;
            for(var i = 0; i< allSegments.Keys.Count; i++)
            {
                var key = allSegments.Keys.ElementAt(i);
                var subList = allSegments[key][allIndices[key]];
                var localLinks = new List<Link>();
                var localFailed = false;
                do
                {
                    foreach (var node in subList)
                    {
                        var link = key.EstablishLink(node);
                        if (link == null)
                        {
                            localFailed = true;
                            break;
                        }
                        localLinks.Add(link);
                        if (!link.CrossesAny(builtLinks)) continue;
                        localFailed = true;
                        break;
                    }

                    if (!localFailed)
                    {
                        break;
                    }
                    foreach (var localLink in localLinks)
                    {
                        key.DestroyLink(localLink.To);
                    }
                    localLinks.Clear();
                    allIndices[key]++;
                    if (allIndices[key] == allSegments[key].Count)
                    {
                        // we tried all remaining combos, none of them is possible
                        allIndices[key] = 0;
                    }
                } while (true);
            }
        }
        
        StartScan(nodes, links);
  
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        foreach (var link in links)
        {
            Console.WriteLine("{0} {1} {2} {3} {4}", link.From.X, link.From.Y, link.To.X, link.To.Y, 1);
        }
        Console.Error.WriteLine("Instructions Finished");
    }

    private static Dictionary<Node, List<List<Node>>> BuildLinkOptions(IEnumerable<Node> nodes, IList<Link> links)
    {
        var result = new Dictionary<Node, List<List<Node>>>();
        foreach (var node in nodes)
        {
            var potentialLins = node.PossibleLinks(links);
            var combi = BuildCombinations(potentialLins, node.MissingLinks);
            result.Add(node, combi);
        }

        return result;
    }

    private static void CreateObviousLinks(IEnumerable<Node> nodes, ICollection<Link> links)
    {
        // handle trivial nodes (1 node and 1 neighbour or twice as much as neighbors)
        var worthATry = true;
        while (worthATry)
        {
            worthATry = false;
            foreach (var node in nodes)
            {
                if (node.PossibleLinksCount(links) != node.MissingLinks || node.MissingLinks <= 0) continue;
                worthATry = true;
                Console.Error.WriteLine($"Node @ {node.X},{node.Y} is now trivial.");
                foreach (var nextNode in node.NeighBours)
                {
                    if (new Link(node, nextNode).CrossesAny(links))
                    {
                        continue;
                    }
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
    }

    private static void StartScan(IReadOnlyCollection<Node> nodes, IList<Link> links)
    {
        // find a simple node
        var maxNeedeedLinks = 0;
        var maxFreeNodes = 0;
        Node start = null;
        foreach (var node in nodes)
        {
            if (node.NeedeedLinks < maxNeedeedLinks)
            {
                continue;
            }
            maxNeedeedLinks = node.NeedeedLinks;
            if (node.NeighBours.Count * 2 - node.NeedeedLinks< maxFreeNodes)
            {
                continue;
            }
            maxFreeNodes = node.NeighBours.Count*2 - node.NeedeedLinks;
            start = node;
            if (maxFreeNodes == 0)
            {
                break;
            }
        }
        var toVisit = new List<Node>(nodes);    
        BuildNetwork(links, start, toVisit);
    }

    private static bool BuildNetwork(IList<Link> links, Node start, ICollection<Node> toVisit)
    {
        // we are done here
        if (start.MissingLinks == 0)
        {
            if (!toVisit.Contains(start))
            {
                // already visited
                return true;
            }

            toVisit.Remove(start);
            foreach (var nextNode in start.NeighBours)
            {
                if (!start.IsLinkedTo(nextNode))
                {
                    // we dont try to extend to non linked node
                    continue;
                }
                if (!BuildNetwork(links, nextNode, toVisit))
                {
                    return false;
                }
            }

            return true;
        }

        if (!toVisit.Contains(start))
        {
            toVisit.Add(start);
        }
        var possibleLinks = start.PossibleLinks(links);

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
                if (!nextNode.IsSolvable(links))
                {
                    success = false;
                    break;
                }
                
                if (!start.IsLinkedTo(nextNode))
                {
                    // we dont try to extend to non linked node
                    continue;
                }

                if (BuildNetwork(links, nextNode, toVisit)) continue;
                success = false;
                break;
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
            if (From.X == To.X && other.From.X != other.To.X)
            {
                return (From.Y - other.From.Y) * (other.To.Y - To.Y) > 0 &&
                       (other.To.X - To.X) * (From.X - other.From.X) > 0;
            }
            if (From.Y == To.Y && other.From.Y != other.To.Y)
            {
                return (From.Y - other.From.Y) * (other.To.Y - To.Y) > 0 &&
                       (other.To.X - To.X) * (From.X - other.From.X) > 0;
            }
            return false;
        }

        public bool CrossesAny(IEnumerable<Link> links)
        {
            foreach (var link in links)
            {
                if (Crosses(link))
                {
                    return true;
                }
            }

            return false;
        }
    
        public override string ToString()
        {
            return $"{From.X}:{From.Y}<=>{To.X}:{To.Y}";
        }
    }

    private class Node 
    {
        public int X { get; }
        public int Y { get; }
        public int NeedeedLinks { get; }
        public readonly List<Node> NeighBours = new List<Node>();
        private readonly Dictionary<Node, int> _linkedTo = new Dictionary<Node, int>();
        private int _linkEstablished;

        // number of links that must be set to this node
        public int MissingLinks => NeedeedLinks - _linkEstablished;

        public Node(int x, int y, int needeedLinks)
        {
            Y = y;
            X = x;
            NeedeedLinks = needeedLinks;
        }

        public bool IsSolvable(ICollection<Link> links)
        {
            return MissingLinks <= PossibleLinksCount(links);
        }

        public int PossibleLinksCount(ICollection<Link> links)
        {
            var res = 0;
            foreach (var neighBour in NeighBours)
            {
                if (neighBour.MissingLinks == 0)
                {
                    continue;
                }

                if (new Link(this, neighBour).CrossesAny(links))
                {
                    continue;
                }
                res += Math.Min(2 - _linkedTo[neighBour], neighBour.MissingLinks);
            }

            return res;
        }
        
        public  List<Node> PossibleLinks(IList<Link> links)
        {
// extract links
            var possibleLinks = new List<Node>(8);
            for (var i = 0; i < Math.Min(2, MissingLinks); i++)
            {
                foreach (var neighBour in NeighBours)
                {
                    if (neighBour.MissingLinks <= i) continue;
                    if (new Link(this, neighBour).CrossesAny(links))
                    {
                        continue;
                    }

                    possibleLinks.Add(neighBour);
                }
            }

            return possibleLinks;
        }

        public bool IsLinkedTo(Node other)
        {
            return _linkedTo[other] > 0;
        }

        public void AddNeighbour(Node node)
        {
            NeighBours.Add(node);
            _linkedTo[node] = 0;
        }

        public Link EstablishLink(Node other)
        {
            if (!LinkTo(other))
            {
                return null;
            }

            if (!other.LinkTo(this))
            {
                RemoveLink(other);
                return null;
            }
            var result = new Link(this, other);

            return result;
        }

        private bool LinkTo(Node other)
        {
            if (_linkedTo[other] > 1 || MissingLinks == 0)
                // link saturated
            {
                return false;
            }
            _linkEstablished++;
            _linkedTo[other]++;
            return true;
        }

        public void DestroyLink(Node other)
        {
            if (!RemoveLink(other)) return;
            other.RemoveLink(this);
        }

        private bool RemoveLink(Node other)
        {
            if (!NeighBours.Contains(other))
                return false;
            _linkedTo[other]--;
            _linkEstablished--;
            return true;
        }

        public override string ToString()
        {
            return $"{X}:{Y} ({MissingLinks})";
        }
    }
}