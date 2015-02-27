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
class PlayerMars
{

	static double g = 3.7111;

	class Point
	{
		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	class Surface
	{
		private List<Point> points = new List<Point> ();

		public void Add(int x, int y)
		{
			points.Add (new Point (x, y));
		}

		public Point Intersect(Point refPoint, int vSpeed, int hSpeed)
		{
			Point previous = null;
			double slope = ((double)vSpeed) / hSpeed;

			foreach(var point in points)
			{
				if (previous != null)
				{
					double floorSlope = (point.Y - previous.Y) / (point.X - previous.X);
					if (Double.IsInfinity (slope)) {
						// trajectory is pure vertical
						if (previous.X < refPoint.X && refPoint.X < point.X) {
							// we intersect
							int intersectY = previous.Y + (int)(floorSlope * (refPoint.X - previous.X));
							return new Point (refPoint.X, intersectY);
						}
					} else {
						int startY = refPoint.Y + (int)(slope * (refPoint.X - previous.X));

						int intersectX = (int)((startY - previous.Y) / (slope - floorSlope));
					}
				}

				previous = point;
			}
			return null;
		}
	}

	static void Main2(string[] args)
	{
		string[] inputs;
		int N = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.

		var startLanding = 0;
		var endLanding = 0;
		var lastY = int.MinValue;
		var lastX = int.MinValue;
		var landingHeight = 0;
		for (int i = 0; i < N; i++)
		{
			inputs = Console.ReadLine().Split(' ');
			int LAND_X = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
			int LAND_Y = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
			if (LAND_Y == lastY)
			{
				startLanding = lastX;
				endLanding = LAND_X;
				landingHeight = lastY;
			}
			lastX = LAND_X;
			lastY = LAND_Y;
			Console.Error.WriteLine ("Point ({0}, {1})", LAND_X, LAND_Y);
		}
		// locate flat 
		Console.Error.WriteLine ("Landing ({0}, {1})", startLanding, endLanding);

		// game loop
		while (true)
		{
			inputs = Console.ReadLine().Split(' ');
			int X = int.Parse(inputs[0]);
			int Y = int.Parse(inputs[1]);
			int HS = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
			int VS = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
			int F = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
			int R = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
			int P = int.Parse(inputs[6]); // the thrust power (0 to 4).
			if (X<startLanding)
			{
				if (HS < 20)
					Console.WriteLine("-30 4");
				else if (HS > -20)
					Console.WriteLine("-30 4");
				else
					Console.WriteLine("0 3");
				continue;
			}
			else if (X >endLanding)
			{
				if (HS >-20)
					Console.WriteLine("30 4"); // R P. R is the desired rotation angle. P is the desired thrust power.
				else if (HS < -25)
					Console.WriteLine("-30 4");
				else
					Console.WriteLine("0 3");
				continue;
			}
			int power;
			int angle = 0;
			if (HS > 5) {
				angle = 30;
			} else if (HS < -5) {
				angle = -30;
			}
			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			if (VS<-35)
				power = 4;
			else if (VS<-10)
				power = 3;
			else
				power = 0;
			Console.WriteLine("{0} {1}", angle, power);
		}
	}
}