using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Zone
{
	private int owner;
	static public int MyID;

	public List<Zone> Links = new List<Zone>();
	public int ID;
	public bool EnemyBase;

	public Dictionary<int, int> Pods = new Dictionary<int, int>(); 
	public int Platinum;
	public int Owner
	{
		get { return this.owner;}
		set {
			if (this.owner != value) {
				if (value != MyID)
					VisitCount = 0;
			}
		}
	}

	public int VisitCount;

	public int ComputeScore(int podsCount)
	{
		if (this.EnemyBase)
			return 1000;
		if (Owner < 0) { // no owner
			return 10*(1+Platinum);
		}
		if (Owner != MyID) {
			return 50*(1+Platinum);
		}
		return (1+Platinum)-VisitCount;
	}
}

class Pod
{
	public int CurrentZone;
}

class Player
{
	static void Main(string[] args)
	{
		Console.Error.WriteLine ("dupdob: Algo V2");
		string[] inputs = Console.ReadLine().Split(' ');
//		int playerCount = int.Parse(inputs[0]); // the amount of players (2 to 4)
		Zone.MyID = int.Parse(inputs[1]); // my player ID (0, 1, 2 or 3)
		int zoneCount = int.Parse(inputs[2]); // the amount of zones on the map
		int linkCount = int.Parse(inputs[3]); // the amount of links between all zones
		var rnd = new Random (0);
		var zones = new Dictionary<int, Zone>(zoneCount);
		for (var i = 0; i < zoneCount; i++)
		{
			var zone = new Zone();
			inputs = Console.ReadLine().Split(' ');
			zone.ID = int.Parse (inputs [0]);
			zones[zone.ID] = zone;
			zone.Platinum = int.Parse(inputs[1]);
		}
		for (var i = 0; i < linkCount; i++)
		{
			inputs = Console.ReadLine().Split(' ');
			var z1 = int.Parse (inputs [0]);
			var z2 = int.Parse (inputs [1]);
			zones[z1].Links.Add(zones[z2]);
			zones[z2].Links.Add (zones [z1]);
		}

		bool firstRound = true;
		// game loop
		for(;;)
		{
			var myPods = new List<Pod>(10);

			int platinum = int.Parse(Console.ReadLine()); // my available Platinum
			for (int i = 0; i < zoneCount; i++)
			{
				inputs = Console.ReadLine().Split(' ');
				var zone = zones[int.Parse(inputs[0])];
				zone.Owner = int.Parse(inputs[1]);
				for (var j = 0; j < 4; j++)
				{
					var pods = int.Parse(inputs[2 + j]);
					zone.Pods[j] = pods;
					if (j == Zone.MyID) {
						var pod = new Pod ();
						pod.CurrentZone = i;
						for (var k = 0; k < pods; k++)
							myPods.Add (pod);
					} else if (firstRound && pods > 0) {
						zone.EnemyBase = true;
					}
				}
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			for (var i = 0; i < myPods.Count; i++) {
				var pod = myPods [i];
				var zone = zones [pod.CurrentZone];
				var nbNeighbour = zone.Links.Count;
				var offset = rnd.Next (nbNeighbour - 1);
				var scoring = new SortedList<int, int> (6);
				// pod logic: scan all neigbour, choose one I do not own
				for (var j = 0; j < nbNeighbour; j++) {
					var neighbour = zone.Links [(j + offset) % nbNeighbour];
					scoring [neighbour.ComputeScore (1)] = neighbour.ID;
				}

				Console.Error.Write ("Pod #{0} from {1}", i, pod.CurrentZone);
				foreach (var dump in scoring) {
					Console.Error.Write ("{0} {1};", dump.Value, dump.Key);
				}
				Console.Error.WriteLine ();
				var nextZone = scoring.Last ().Value;
				zones [nextZone].VisitCount++;
				Console.Write ("1 {0} {1} ", pod.CurrentZone, nextZone);
			}
			Console.WriteLine(""); // first line for movement commands, second line for POD purchase (see the protocol in the statement for details)
			Console.WriteLine("WAIT");
			firstRound = false;
		}
	}
} 
