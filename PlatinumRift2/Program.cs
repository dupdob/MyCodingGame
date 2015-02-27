using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Diagnostics;

enum ZoneType {Neutral, MyBase, EnnemyBase};
class Zone
{
	static private Random rnd = new Random();
	static public int MyID;
	static public int MaxDepth = 10;

	private int owner=-1;
	public ZoneType type = ZoneType.Neutral;

	public List<Zone> Links = new List<Zone>();
	public int ID;

	public List<int> DeadFrom = new List<int>();

	public Dictionary<int, int> Pods = new Dictionary<int, int>(); 
	public int Platinum;
	public bool Visible;
	public int Owner
	{
		get { return this.owner;}
		set {
			if (this.owner != value) {
				this.owner = value;
				//Player.LogLine("Z#{0}: New Owner: {1}", this.ID, this.owner);
				this.DeadFrom.Clear ();
				if (value != MyID)
					VisitCount = 0;
			}
		}
	}

	public bool Fighting
	{
		get {
			bool podsFound = false;
			foreach (var pod in this.Pods)
				if (pod.Value > 0)
				if (podsFound)
					return true;
				else
					podsFound = true;
			return false;
		}
	}

	public int VisitCount;

	public int ComputeScore(int from)
	{
		var offset = rnd.Next (20);
		if (this.type == ZoneType.EnnemyBase)
			return int.MaxValue;
		var path = new List<int> ();
		path.Add (from);
		if (DeadEndPath (path)) {
			return int.MinValue;
		}
		if (owner == MyID) {
			offset = 10- VisitCount;
		} else if (Platinum > 0) {
			offset += 500 * Platinum + LinksToVisit();
			if (owner > 0)
				offset += 10;
		} else if (Owner== -1) {
			offset += 20 * LinksToVisit();
		} else {
			offset += 10 + LinksToVisit();
		}
		return offset;
	}

	public int LinksToVisit()
	{
		var res = 0;
		foreach(var zone in Links)
		{
			if (zone.Owner != MyID)
				res++;
		}
		return res;
	}

	public bool DeadEndPath(List<int> from)
	{
		var lastZone = from.Last();
		if (this.DeadFrom.Contains(lastZone))
			return true;
		if (((Platinum > 0 || !this.Visible) && Owner!=MyID) || this.type==ZoneType.EnnemyBase 
			|| this.LinksToVisit()>0)
			return false;

		if (from.Count >= MaxDepth)
			return false;
		var nextList = new List<int> (from);
		nextList.Add (ID);
		foreach (var next in this.Links) {
			if (nextList.Contains(next.ID))
				continue;
			if (!next.DeadEndPath (nextList))
				return false;
		}
		//Player.LogLine("Zone #{0} is dead end from {1}!", ID, lastZone);
		this.DeadFrom.Add(lastZone);
		return true;
	}
}

class Pod
{
	public int CurrentZone;
}

class Player
{
	static void qMain(string[] args)
	{
		LogLine ("dupdob: Algo V2.3");
		string[] inputs = Console.ReadLine().Split(' ');
		//		int playerCount = int.Parse(inputs[0]); // the amount of players (2 to 4)
		Zone.MyID = int.Parse(inputs[1]); // my player ID (0, 1, 2 or 3)
		LogLine ("MyId:{0}", Zone.MyID);
		int zoneCount = int.Parse(inputs[2]); // the amount of zones on the map
		int linkCount = int.Parse(inputs[3]); // the amount of links between all zones
		var zones = new Dictionary<int, Zone>(zoneCount);
		int maxEnemyPods;

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
			zones[z1].Links.Add(zones [z2]);
			zones[z2].Links.Add(zones [z1]);
		}

		//		DumpTerrain (zones);

		var turnCount = 0;
		// game loop
		for(;;)
		{
			var clock = new Stopwatch ();
			var myPods = new List<Pod>(10);
			maxEnemyPods = 0;
			clock.Start ();
			int platinum = int.Parse(Console.ReadLine()); // my available Platinum
			for (int i = 0; i < zoneCount; i++)
			{
				inputs = Console.ReadLine().Split(' ');
				var zone = zones[int.Parse(inputs[0])];
				zone.Owner = int.Parse(inputs[1]);
				for (var j = 0; j < 2; j++)
				{
					var pods = int.Parse(inputs[2 + j]);
					zone.Pods[j] = pods;

					if (turnCount == 0 && pods>0) {
						if (j == Zone.MyID) {
							zone.type = ZoneType.MyBase;
							LogLine("MyBase is at #{0}.", i);
						}
						else {
							zone.type = ZoneType.EnnemyBase;
							LogLine("EnnemyBase is at #{0}.", i);
						}
					}
					if (j == Zone.MyID) {
						var pod = new Pod ();
						pod.CurrentZone = i;
						if (zone.type == ZoneType.MyBase) {
							LogLine ("Save some pods at #{0}!", zone.ID);
							if (turnCount == 0)
								pods--;
							else if (turnCount < 10)
								pods -= 4;
							else
								pods -= Math.Max(4, maxEnemyPods);

						}
						for (var k = 0; k < pods; k++)
							myPods.Add (pod);
					} else {
						maxEnemyPods = Math.Max (maxEnemyPods, pods);
					}
				}
				zone.Visible = int.Parse(inputs[4]) > 0;
				if (zone.Visible)
				{
					zone.Platinum = int.Parse(inputs[5]);
				}
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			for (var i = 0; i < myPods.Count; i++) {
				var pod = myPods [i];
				var zone = zones [pod.CurrentZone];
				var nbNeighbour = zone.Links.Count;
				var scoring = new SortedList<int, int> (6);
				// pod logic
				// check if we are on an ennemy zone, because movements are limited then
				if (zone.Fighting) {
					// we can stay, or we retreat
					for (var j = 0; j < nbNeighbour; j++) {
						var neighbour = zone.Links [j];
						if (zone.Owner != Zone.MyID)
							continue;

						scoring [neighbour.Platinum] = neighbour.ID;
					}
				} else {
					// scan all neigbour, pick best one
					for (var j = 0; j < nbNeighbour; j++) {
						var neighbour = zone.Links [j];
						scoring [neighbour.ComputeScore (pod.CurrentZone)] = neighbour.ID;
					}

				}
				/*
				Log ("Pod #{0} @ {1}:", i, pod.CurrentZone);
				foreach (var dump in scoring) {
					Log ("{0} {1};", dump.Value, dump.Key);
				}
				LogLine ();*/
				var nextZone = scoring.Last ().Value;
				zones [nextZone].VisitCount++;
				zones [nextZone].Owner = Zone.MyID;
				Console.Write ("1 {0} {1} ", pod.CurrentZone, nextZone);

				if (clock.ElapsedMilliseconds > 50) {
					LogLine ("*** Interrupting our play, too much time spent ***");
					break;
				}
			}
			Console.WriteLine(); // first line for movement commands, second line for POD purchase (see the protocol in the statement for details)
			Console.WriteLine("WAIT");
			turnCount++;
		}
	}

	static void DumpTerrain (Dictionary<int, Zone> zones)
	{
		foreach (var zone in zones) {
			Log ("#{0}: P:{1} O:{2};" , zone.Value.ID, zone.Value.Platinum, zone.Value.Owner);
			if (zone.Value.type == ZoneType.EnnemyBase) {
				Console.Error.Write ("Enemmy base; ");
			}
			Log ("Next to: ");
			foreach(var next in zone.Value.Links)
			{
				Log ("{0},", next.ID);
			}
			LogLine ();
		}
	}	

	public static void Log(string fmt, params object[] pars)
	{
		Console.Error.Write (fmt, pars);
	}
	public static void LogLine(string fmt, params object[] pars)
	{
		Console.Error.WriteLine (fmt, pars);
	}
	public static void LogLine()
	{
		Console.Error.WriteLine ();
	}
} 
