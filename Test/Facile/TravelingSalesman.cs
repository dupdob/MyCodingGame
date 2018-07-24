using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace CodingGame.Facile
{
    class TravelingSalesman
    {
        static void Main(string[] args)
        {
            var N = int.Parse(Console.ReadLine());
            var cities = new List<Point>(N);
            for (var i = 0; i < N; i++)
            {
                var inputs = Console.ReadLine().Split(' ');
                var X = int.Parse(inputs[0]);
                var Y = int.Parse(inputs[1]);
                var point = new Point();
                point.X = X;
                point.Y = Y;
                cities.Add(point);
            }

            double dist = 0;
            var current = cities[0];
            var firstpoint = current;
            cities.RemoveAt(0);
            while (cities.Count > 0)
            {

                var nextDist = double.MaxValue;
                var nextId = 0;
                for (var i = 0; i < cities.Count; i++)
                {
                    if (!(current.Dist(cities[i]) < nextDist)) continue;
                    nextDist = current.Dist(cities[i]);
                    nextId = i;
                }

                dist += nextDist;
                current = cities[nextId];
                cities.RemoveAt(nextId);
            }

            dist += current.Dist(firstpoint);
        
        
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine(Math.Round(dist));
        }

        class Point
        {
            public int X;
            public int Y;

            public double Dist(Point other)
            {
                return Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y));
            }
        }
    }
}