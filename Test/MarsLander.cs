using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{

	static void MainLander(string[] args)
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

			int power;
			int angle = 0;
			int remHeight = Y - landingHeight; 
			if (X<startLanding)
			{
				power = 4;
				angle = 45;
				if (HS <0)
					angle = -45;
				else if (HS > 55)
					angle = 75;
				else if (HS > 20)
					angle = 25;
				else
				{
					angle = 0;
					if (VS >-25)
						power = 3;
				}

				if (VS< -42)
					angle = angle*1/3;

				if (angle*R<0 && R<-15)
				{
					power = 0;
				}

				Console.WriteLine("{0} {1}", angle, power);
				continue;
			}
			else if (X >endLanding)
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

				if (angle*R<0 && R>15)
				{
					power = 0;
				}
				Console.WriteLine("{0} {1}", angle, power);
				continue;
			}
			else
			{
				if (HS > 15) {
					angle = 50;
				} else if (HS < -15) {
					angle = -50;
				}
				if (VS< -35)
					angle = angle*1/3;

				if (VS< -45)
					angle = angle*1/3;
				if (VS<-35 || Math.Abs(HS) > 20)
					power = 4;
				else if (VS<-10 && Math.Abs(HS) < 15)
					power = 3;
				else
					power = 0;
			}
			if (landingHeight - Y > VS*2)
			{
				angle = 0;
			}
			Console.WriteLine("{0} {1}", angle, power);
		}
	}
}