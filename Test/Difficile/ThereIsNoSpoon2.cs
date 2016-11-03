using System;
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
            for (var j=0; j<line.Length; j++)
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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var node = map[i, j];
                if (node !=null)
                {
                    /// look for node going up
                    for (var x = i - 1; x >= 0; x--)
                    {
                        if (map[x, j] != null)
                        {
                            node.NeighBours.Add(map[x,j]);
                            break;
                        }
                    }
                    for (var x = i+1; x <width; x++)
                    {
                        if (map[x, j] != null)
                        {
                            node.NeighBours.Add(map[x,j]);
                            break;
                        }
                    }
                    for (var y = j - 1; y >= 0; y--)
                    {
                        if (map[i, y] != null)
                        {
                            node.NeighBours.Add(map[i,y]);
                            break;
                        }
                    }
                    for (var y = j + 1; y < height; y++)
                    {
                        if (map[i, y] != null)
                        {
                            node.NeighBours.Add(map[i,y]);
                            break;
                        }
                    }
                    nodes.Add(map[i,j]);
                }
            }
        }
        // sort the nodes
        nodes.Sort();
        foreach (var node in nodes)
        {
            Console.Error.Write("{0}:{1}:{2} =>{3} Nextto:", node.X, node.Y, node.NeedeedLinks, node.Priority);
            foreach (var nodeNeighBour in node.NeighBours)
            {
                Console.Error.Write("{0}:{1}  ", nodeNeighBour.X, nodeNeighBour.Y);
            }
            Console.Error.WriteLine();
        }
        var links = new List<Links>();
        NoSpoon2.BuildGraph(nodes, links);
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        foreach (var link in links)
        {
            Console.WriteLine("{0} {1} {2} {3} {4}", link.From.X, link.From.Y, link.To.X, link.To.Y, 1);
        }
        Console.Error.WriteLine("Instructions Finished");
    }

    private static void BuildGraph(List<Node> nodes, List<Links> links)
    {
        foreach (var node in nodes)
        {
            if (node.NeedeedLinks > 0)
            {
                // we need to add a link
                node.SortNeighbours();
                foreach (var nodeNeighBour in node.NeighBours)
                {
                    // try to add a link between those two
                    var link = new Links
                    {
                        From = node,
                        To = nodeNeighBour
                    };
                    if (nodeNeighBour.NeedeedLinks > 0)
                    {
                        node.NeedeedLinks--;
                        nodeNeighBour.NeedeedLinks--;
                        links.Add(link);
                        if (node.NeedeedLinks == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }


    class Node:IComparable
    {
        public int X;
        public int Y;
        public int NeedeedLinks;
        public List<Node> NeighBours = new List<Node>();

        public int FreeLinks { get { return this.NeighBours.Count*2 - this.NeedeedLinks; } }

        public int Priority {  get { return this.FreeLinks * 10000 + this.Y * 100 + this.X; } }

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
    }
}