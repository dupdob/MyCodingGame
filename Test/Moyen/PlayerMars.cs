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
class PlayerMarsLander
{

	static double g = 3.7111;

	class Surface
	{
		class Point
		{
			public int x;
			public int y;

			public Point(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

		private int leftLanding;
		private int rightLanding;
		private int landingHeight;

		private List<Point> segments = new List<Point> ();


		public int StartLanding {
			get { return this.leftLanding;}
		}

		public int EndLanding {
			get { return this.rightLanding;}
		}

		public int LandingHeight {
			get { return this.landingHeight;}
		}

		public void AddPoint(int x, int y)
		{
			if (segments.Count () > 0) {
				if (segments.Last ().y == y) {
					leftLanding = segments.Last ().x;
					rightLanding = x;
					landingHeight = y;
				}
			}
			segments.Add (new Point (x, y));
		}

		public int MaxAltitude(int x)
		{
			// above landing?
			if (this.leftLanding <= x && this.rightLanding >= x) {
				return this.landingHeight;
			}

			if (this.leftLanding > x) {
				var i = this.segments.Count - 1;
				var maxHeight = int.MinValue;
				for (; i >= 0; i--) {
					var nextX = this.segments [i].x;
					if (nextX > leftLanding)
						continue;
					if (nextX <= x) {
						break;
					}
					maxHeight = Math.Max (maxHeight, this.segments [i].y);
				}
				Console.Error.Write ("Last point {0} (alt {1}) ", i, maxHeight);
				// need to integrate the last height
				var heigthOfXSeg = (x - this.segments [i].x) * (this.segments [i + 1].y - this.segments [i].y) / (this.segments [i + 1].x - this.segments [i].x)+this.segments[i].y;
				maxHeight = Math.Max (maxHeight, heigthOfXSeg);
				return maxHeight;
			} else {
				var i = 0;
				var maxHeight = int.MinValue;
				for (; i < this.segments.Count; i++) {
					var nextX = this.segments [i].x;
					if (nextX < rightLanding)
						continue;
					if (nextX >= x) {
						break;
					}
					maxHeight = Math.Max (maxHeight, this.segments [i].y);
				}
				// need to integrate the last height
				var heigthOfXSeg = (x - this.segments [i].x) * (this.segments [i - 1].y - this.segments [i].y) / (this.segments [i - 1].x - this.segments [i].x) + this.segments[i].y;
				maxHeight = Math.Max (maxHeight, heigthOfXSeg);
				return maxHeight;
			}
		}
	}

	static void Main2(string[] args)
	{
		string[] inputs;
		int N = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.

		var surface = new Surface ();

		for (int i = 0; i < N; i++)
		{
			inputs = Console.ReadLine().Split(' ');
			int LAND_X = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
			int LAND_Y = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
			surface.AddPoint (LAND_X, LAND_Y);

			Console.Error.WriteLine ("Point ({0}, {1})", LAND_X, LAND_Y);
		}
		// locate flat 
		Console.Error.WriteLine ("Landing ({0}, {1})", surface.StartLanding, surface.EndLanding);

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

			int power;
			int angle = 0;

			int maxAltToLanding = surface.MaxAltitude (X);
			Console.Error.WriteLine("Min Altitude to maintain : {0}", maxAltToLanding);

			if (X<surface.StartLanding)
			{
				power = 4;
				if (HS <0)
					angle = -45;
				else if (HS > 55)
					angle = 75;
				else if (HS > 20)
					angle = 25;
				else
				{
					if (HS < 18)
						angle = -35;
					else
						angle = 0;
					if (VS >-25)
						power = 3;
				}

				if (VS< -42)
					angle = angle*1/3;

				if (angle*R<0 && Math.Abs(R)> 15)
				{
					power = 0;
				}
				if (Y - maxAltToLanding < -VS * 25 && HS > 0) {
					Console.Error.WriteLine ("Obstacle ahead");
					power = 4;
					if (HS > 15)
						angle = 10;
					else
						angle = 0;
				}
			}
			else if (X >surface.EndLanding)
			{
				power = 4;
				if (HS >0)
					angle = 45;
				else if (HS < -55)
					angle = -75;
				if (HS <-20)
					angle = -25;
				else
				{
					angle = 0;
					if (VS >-25)
						power = 3;
				}

				if (VS< -42)
					angle = angle*1/3;

				if (angle*R<0 && Math.Abs(R)> 15)
				{
					power = 0;
				}

				if ((Y - maxAltToLanding < Math.Max(-VS * 20, 0)) && HS <0) {
					Console.Error.WriteLine ("Obstacle ahead");
					power = 4;
					if (HS < -25)
						angle = -10;
					else
						angle = 0;
				}
			}
			else
			{
				if (HS > 15) {
					angle = 60;
				} else if (HS < -15) {
					angle = -60;
				} else if (HS > 10) {
					angle = 20;
				} else if (HS < -10) {
					angle = -20;
				}
				else if (HS > 0) {
					angle = 10;
				} else if (HS < 0) {
					angle = -10;
				}
				if (VS< -35)
					angle = angle*1/3;

				if (VS< -45)
					angle = angle*1/3;
				if (VS<-35 || Math.Abs(HS) > 20)
					power = 4;
				else if (VS<-10)
					power = 3;
				else
					power = 0;

				if ((surface.LandingHeight - Y > VS*2)|| Y-surface.LandingHeight < 50 )
				{
					// make sure we land vertically
					angle = 0;
				}
			}

			Console.WriteLine("{0} {1}", angle, power);
		}
	}
}