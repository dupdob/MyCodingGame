﻿using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class NoSpoon2
{
    static void Main(string[] args)
    {
        var width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        var height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
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
                    Node node = new Node();
                    node.X = j;
                    node.Y = i;
                    node.NeedeedLinks = car - '0';
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
                    if (map[x, j] != null)
                    {
                        node.AddNeighbour(map[x, j]);
                        break;
                    }
                }
                for (var x = i + 1; x < width; x++)
                {
                    if (map[x, j] != null)
                    {
                        node.AddNeighbour(map[x, j]);
                        break;
                    }
                }
                for (var y = j - 1; y >= 0; y--)
                {
                    if (map[i, y] != null)
                    {
                        node.AddNeighbour(map[i, y]);
                        break;
                    }
                }
                for (var y = j + 1; y < height; y++)
                {
                    if (map[i, y] != null)
                    {
                        node.AddNeighbour(map[i, y]);
                        break;
                    }
                }
                nodes.Add(map[i, j]);
            }
        }
        var links = new List<Links>();
        while (nodes.Count > 0)
        {
            if (!NoSpoon2.BuildGraph(nodes, links))
            {
                Console.Error.WriteLine("Algo is in a dead end.");
                break;
            }
        }
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        foreach (var link in links)
        {
            Console.WriteLine("{0} {1} {2} {3} {4}", link.From.X, link.From.Y, link.To.X, link.To.Y, 1);
        }
        Console.Error.WriteLine("Instructions Finished");
    }

    private static bool BuildGraph(List<Node> nodes, List<Links> links)
    {
        var addedLink = false;
        // we need to add a link
        Node node = null;
        var minPriority = int.MaxValue;
        foreach (var scan in nodes)
        {
            if (scan.Priority < minPriority)
            {
                minPriority = scan.Priority;
                node = scan;
            }
        }
        if (node.NeedeedLinks > 0)
        {
            var copy = new List<Node>(node.NeighBours);
            copy.Sort();
            foreach (var nodeNeighBour in copy)
            {
                if (!node.CanLinkToo(nodeNeighBour))
                {
                    continue;
                }
                // try to add a link between those two
                var link = new Links
                {
                    From = node,
                    To = nodeNeighBour
                };
                var crossing = false;
                foreach (var existing in links)
                {
                    if (link.Crosses(existing))
                    {
                        crossing = true;
                        break;
                    }
                }
                if (crossing)
                {
                    continue;
                }
                node.LinkTo(nodeNeighBour);
                nodeNeighBour.LinkTo(node);
                links.Add(link);
                //               Console.Error.WriteLine("Add link {0}", link);
                addedLink = true;
                if (!BuildGraph(nodes, links))
                {
                    links.Remove(link);
                    node.RemoveLink(nodeNeighBour);
                    nodeNeighBour.RemoveLink(node);
                    addedLink = false;
                }
                if (node.MissingLinks == 0)
                {
                    break;
                }
            }
        }
        if (node.MissingLinks == 0)
        {
            addedLink = true;
            nodes.Remove(node);
        }
        return addedLink;
    }


    class Node : IComparable
    {
        public int X;
        public int Y;
        public int NeedeedLinks;
        public readonly List<Node> NeighBours = new List<Node>();
        private readonly Dictionary<Node, int> linkedTo = new Dictionary<Node, int>();
        private int linkUsed = 0;

        // number of links that must be set to this node
        public int MissingLinks => this.NeedeedLinks - this.linkUsed;

        public int Priority => this.FlexibleLinks * 10000 + this.Y * 100 + this.X;
        // number of links freely allocable(i.e for which multiple options are possible

        private int FlexibleLinks => this.MissingLinks > 0 ? this.FreeNeighboursLinks - this.MissingLinks : 100;

        private int FreeNeighboursLinks
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

        public bool CanLinkToo(Node other)
        {
            return other.MissingLinks > 0 && this.linkedTo[other] < 2;
        }

        public void AddNeighbour(Node node)
        {
            this.NeighBours.Add(node);
            this.linkedTo[node] = 0;
        }

        public bool LinkTo(Node other)
        {
            if (this.linkedTo[other] > 1)
            // link saturated
            {
                return false;
            }
            this.linkUsed++;
            this.linkedTo[other]++;
            return true;
        }

        public bool RemoveLink(Node other)
        {
            if (!this.NeighBours.Contains(other))
                return false;
            this.linkedTo[other]--;
            this.linkUsed--;
            return true;
        }

        public int CompareTo(object obj)
        {
            var other = obj as Node;
            if (other == null)
            {
                return 1;
            }
            if (this.Priority < other.Priority)
            {
                return -1;
            }
            if (this.Priority > other.Priority)
            {
                return 1;
            }
            return 0;
        }

        public void SortNeighbours()
        {
            this.NeighBours.Sort();
        }
    }

    class Links
    {
        public Node From;
        public Node To;

        public bool Crosses(Links other)
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
            return string.Format("{0}:{1}<=>{2}:{3}", this.From.X, this.From.Y, this.To.X, this.To.Y);
        }
    }
}