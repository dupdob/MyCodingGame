using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;

/**
 * Don't let the machines win. You are humanity's last hope...
 * https://www.codingame.com/ide/puzzle/there-is-no-spoon-episode-2
 **/
namespace CodingGame
{
    internal static class NoSpoon2
    {
        private static void Main()
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
            foreach (var link in links)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}", link.From.X, link.From.Y, link.To.X, link.To.Y, link.Count);
            }
            var allSegments = BuildLinkOptions(nodes, links);
            var allIndices = new Dictionary<Node, int>(allSegments.Count);
            foreach (var key in allSegments.Keys)
            {
                allIndices[key] = 0;
            }
            // now try all combination (brute force)
            var listOfNodes = allSegments.Keys.ToArray();
            var stackOfLinks = new Stack<ICollection<Link>>(listOfNodes.Length);
            var builtLinks = new List<Link>();
            // try one combination for each node
            for(var i = 0; i< listOfNodes.Length; i++)
            {
                var key = listOfNodes[i];
                var localLinks = new List<Link>();
                var localFailed = false;
                do
                {
                    localFailed = false;
                    var subList = allSegments[key][allIndices[key]];
                    foreach (var tryLink in subList)
                    {
                        if (!key.LinkCanBeEstablished(tryLink) || tryLink.CrossesAny(builtLinks))
                        {
                            localFailed = true;
                            break;
                        }
                    }

                    if (!localFailed)
                    {
                        foreach (var tryLink in subList)
                        {
                            var result = key.EstablishLink(tryLink);
                            if (result != null)
                            {
                                localLinks.Add(result);
                            }
                        }
                        break;
                    }

                    allIndices[key]++;
                    if (allIndices[key] != allSegments[key].Count) continue;
                    // we tried all remaining combos, none of them is possible
                    do
                    {
                        allIndices[key] = 0;
                        // we need to go back to previous stage
                        i--;
                        var toRemove = stackOfLinks.Pop();
                        key = listOfNodes[i];
                        builtLinks.RemoveRange(builtLinks.Count - toRemove.Count, toRemove.Count);
                        foreach (var link in toRemove)
                        {
                            key.DestroyLink(link.To);
                        }

                        allIndices[key]++;
                    } while (allIndices[key] == allSegments[key].Count);
                    break;
                } while (true);

                if (!localFailed)
                {
                    stackOfLinks.Push(localLinks);
                    builtLinks.AddRange(localLinks);
                }
            }

            foreach (var link in builtLinks)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}", link.From.X, link.From.Y, link.To.X, link.To.Y, link.Count);
            }
            Console.Error.WriteLine("Instructions Finished");
        }
 
        private static Dictionary<Node, List<List<Link>>> BuildLinkOptions(IEnumerable<Node> nodes, IList<Link> links)
        {
            var result = new Dictionary<Node, List<List<Link>>>();
            foreach (var node in nodes)
            {
                if (node.MissingLinks == 0)
                {
                    continue;
                }
                var potentialLinks = node.PossibleLinks(links);
                if (potentialLinks.Count == 0)
                {
                    continue;
                }
                var combi = BuildOptions(potentialLinks, node.MissingLinks);
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
                    if (!node.IsTrivial(links)) continue;
                    worthATry = true;
                    foreach (var nextNode in node.Neighbours)
                    {
                        if (new Link(node, nextNode, 1).CrossesAny(links))
                        {
                            continue;
                        }
                        var link = node.MaximizeLink(nextNode);
                        if (link != null)
                        {
                            links.Add(link);
                        }
                    }
                }
            }

            foreach (var node in nodes)
            {
                if (node.MissingLinks == 0)
                {
                    Console.Error.WriteLine($"Node @ {node.X},{node.Y} had only one solution.");                
                }
            }

            Console.Error.WriteLine("Optimization done.");
        }

    
        private static List<List<Link>> BuildOptions(List<Link> items, int total)
        {
            var result = new List<List<Link>>();
            for (var i = 0; i < items.Count; i++)
            {
                if (total == 1)
                {
                    result.Add(new List<Link> {items[i].ExtractSingle()});
                }
                else if (items[i].Count==2)
                {
                    var temp = items[i];
                    items[i] = items[i].ExtractSingle();
                    foreach (var combo in BuildOptions(items.GetRange(i, items.Count-i), total-1))
                    {
                        var toAdd = true;
                        for (var j = 0; j < combo.Count; j++)
                        {
                            if (combo[j]!=items[i]) continue;
                            combo[j] = combo[j].ExtractDouble();
                            toAdd = false;
                            break;
                        }

                        if (toAdd)
                        {
                            combo.Add(items[i]);
                        }
                        result.Add(combo);
                    }

                    items[i] = temp;
                }
                else
                {
                    foreach (var combo in BuildOptions(items.GetRange(i+1, items.Count-i - 1), total-1))
                    {
                        combo.Add(items[i]);
                        result.Add(combo);
                    }
                }
            }

            return result;
        }

        private class Link
        {
            public Node From { get;}
            public Node To { get;}
            public int Count { get; private set; }

            public Link(Node from, Node to, int count)
            {
                From = from;
                To = to;
                Count = count;
            }

            public Link ExtractSingle()
            {
                return Count == 1 ? this : new Link(From, To, 1);
            }

            public Link ExtractDouble()
            {
                return new Link(From, To, 2);
            }
        
            private bool Crosses(Link other)
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
                return links.Any(Crosses);
            }
    
            public override string ToString()
            {
                return $"{From.X}:{From.Y}<=>{To.X}:{To.Y} ({Count})";
            }

            private bool Equals(Link other)
            {
                return (Equals(From, other.From) && Equals(To, other.To)) || (Equals(From, other.To) && Equals(To, other.From));
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == GetType() && Equals((Link) obj);
            }

            public override int GetHashCode()
            {
                return ((From != null ? From.GetHashCode() : 0)) ^ (To != null ? To.GetHashCode() : 0);
            }

        }

        private class Node 
        {
            public int X { get; }
            public int Y { get; }
            private int NeededLinks { get; }
            public readonly List<Node> Neighbours = new List<Node>();
            private readonly Dictionary<Node, int> _linkedTo = new Dictionary<Node, int>();
            private int _linkEstablished;

            // number of links that must be set to this node
            public int MissingLinks => NeededLinks - _linkEstablished;

            public Node(int x, int y, int neededLinks)
            {
                Y = y;
                X = x;
                NeededLinks = neededLinks;
            }

            private int PossibleLinksCount(ICollection<Link> links)
            {
                var res = 0;
                foreach (var neighbour in Neighbours)
                {
                    if (neighbour.MissingLinks == 0)
                    {
                        continue;
                    }

                    if (new Link(this, neighbour, 1).CrossesAny(links))
                    {
                        continue;
                    }

                    res += Math.Min(Math.Min(2 - _linkedTo[neighbour], neighbour.MissingLinks), MissingLinks);
                }

                return res;
            }

            public bool IsTrivial(ICollection<Link> links)
            {
                return MissingLinks>0 && ((MissingLinks == 1 && Neighbours.Count == 1) || (PossibleLinksCount(links) == MissingLinks));
            }
        
            public  List<Link> PossibleLinks(IList<Link> links)
            {
// extract links
                var possibleLinks = new List<Link>(Neighbours.Count);
                foreach (var neighBour in Neighbours)
                {
                    if (neighBour.MissingLinks == 0)
                    {
                        continue;
                    }
                    var link = new Link(this, neighBour, Math.Min(Math.Min(MissingLinks, 2-_linkedTo[neighBour]), neighBour.MissingLinks));
                    if (link.CrossesAny(links)) continue;
                    possibleLinks.Add(link);
                }

                return possibleLinks;
            }

            public void AddNeighbour(Node node)
            {
                Neighbours.Add(node);
                _linkedTo[node] = 0;
            }

            public bool LinkCanBeEstablished(Link link)
            {
                Debug.Assert(link.From == this);
                if (_linkedTo[link.To] == link.Count)
                {
                    // already exists
                    return true;
                }
                return _linkedTo[link.To] + link.Count <= 2 && MissingLinks >= link.Count && link.To.MissingLinks >= link.Count;
            }

            public Link EstablishLink(Link link)
            {
                Debug.Assert(LinkCanBeEstablished(link));
                if (_linkedTo[link.To] == link.Count)
                {
                    // already exists
                    return null;
                }

                _linkEstablished += link.Count;
                link.To._linkEstablished += link.Count;
                _linkedTo[link.To] = link.Count;
                link.To._linkedTo[link.From] = link.Count;

                return link;
            }

            public Link MaximizeLink(Node other)
            {
                Link result;
                if (_linkedTo[other] > 1 || MissingLinks == 0 || other.MissingLinks == 0)
                {
                    return null;
                }

                if (_linkedTo[other] == 0 && other.MissingLinks > 1 && MissingLinks > 1)
                {
                    _linkEstablished += 2;
                    _linkedTo[other] += 2;
                    other._linkEstablished += 2;
                    other._linkedTo[this] += 2;
                    result = new Link(this, other, 2);
                }
                else
                {
                    _linkEstablished ++;
                    _linkedTo[other] ++;
                    other._linkEstablished ++;
                    other._linkedTo[this] ++;
                    result = new Link(this, other, 1);
                }

                return result;
            }

            public void DestroyLink(Node other)
            {
                if (!RemoveLink(other)) return;
                other.RemoveLink(this);
            }

            private bool RemoveLink(Node other)
            {
                if (!Neighbours.Contains(other))
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
}