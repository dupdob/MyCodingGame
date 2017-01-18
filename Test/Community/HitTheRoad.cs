using System;
using System.Linq;
using System.Collections.Generic;

/**
A driver is on a road network with n junctions and m roads with two specific positions s and t. 
Each junction of the network can be associated with a time window [b,e] : the driver can go through that intersection only between times b and e. Every other junction is free : the driver can cross it at any time. Each road is associated with a duration: the time a car needs to drive through that road. The roads are directed : the car cannot go through a road backward.

Your goal is to decide if a driver can go from s to t such that, for each junction associated with a time window [b,e], the moment r when the driver reaches that junction belongs to its window : b <= r <= e. If the junction has no time window, the driver can reach it at any time.

The driver starts in s at time 0.
The driver cannot wait. Once he reaches a junction, he must go though an outgoing road of that junction.
The time windows are always closed intervals.

Here is an example where <, >, ^ and v represent roads (and the directions).

s > u > w > t

v   ^   v   ^

x > y > z > r


The time window on x, u and y is [1,2].
The time window on w, z and r is [3,5].
The time window on t is [1,7].
The duration on each arc is 1.


It is possible to go from s to t with that path.

s           t

v           ^

x > y > z > r


The driver would be in x at time 1, y at time 2, z at time 3, r at time 4 and t at time 5. Each time fits within the specified time window.

In this example with the same time windows and same durations :

s > u > w > t

v   ^   v   ^

x > y   z > r


You cannot go from s to t, because you must go through w in order to reach t and it is not possible to be in w between time 3 and 5 and in u between time 1 and 2:
- either the driver goes from s to u and arrives at w at time 2;
- or it uses the path s x y u w and arrives at u at time 3.
Entrée
Line 1 : Three integers n, m and ntw the number of junctions and roads in the network and the number of junctions with a time window.
    The junctions are numbered from 0 to n-1.
Line 2 : Two integers s and t, the origin and the destination of the driver
ntw next lines : three integers v, b and e, a junction v, the beginning and the end of the time window of that junction.
m next lines : three integers u, v and d, the origin, the destination and the duration of each arc.
Sortie
Line 1 : print true if there is a path from s to t satisfying the time windows constraints. Otherwise, print false.
    **/
class Solution
{
    class Junction
    {
        public int b;
        public int e;

        public Junction()
        {
            b = 0;
            e = int.MaxValue;
            links = new List<Road>();
        }

        public bool Scan(Junction terminal, int time, Stack<Road> path)
        {
            if (time < this.b || time > this.e)
                return false;
            if (terminal == this)
                return true;
            foreach (var link in this.links)
            {
                if (path.Where(x => x==link).Count()>1)
                    // reuse the road only once (arbitrary)
                    continue;
                path.Push(link);
                if (link.v.Scan(terminal, time + link.d, path))
                    return true;
                path.Pop();
            }
            return false;
        }

        public IList<Road> links;
    }

    class Road
    {
        public Junction v;
        public int d;
    }

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int n = int.Parse(inputs[0]);

        var junctions = new Junction[n];
        int m = int.Parse(inputs[1]);
        int ntw = int.Parse(inputs[2]);
        inputs = Console.ReadLine().Split(' ');
        int s = int.Parse(inputs[0]);
        int t = int.Parse(inputs[1]);
        for (int index = 0; index < junctions.Length; index++)
        {
            junctions[index] = new Junction();
        }
        for (int i = 0; i < ntw; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int v = int.Parse(inputs[0]);
            junctions[v].b = int.Parse(inputs[1]);
            junctions[v].e = int.Parse(inputs[2]);
        }
        for (int i = 0; i < m; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int u = int.Parse(inputs[0]);
            var j = new Road();
            j.v = junctions[int.Parse(inputs[1])];
            j.d = int.Parse(inputs[2]);
            junctions[u].links.Add(j);
        }
        var start = junctions[s];
        var terminal = junctions[t];
        var found = start.Scan(terminal, 0, new Stack<Road>());
        
        Console.WriteLine(found ? "true":"false");
    }
}