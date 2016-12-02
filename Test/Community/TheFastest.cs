using System;

class TheFastest
{
    static void MainFastest(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        string minTime = new string('Z', 10);
        for (int i = 0; i < N; i++)
        {
            string t = Console.ReadLine();
            if (t.CompareTo(minTime)<0)
            {
                minTime = t;
            }
        }

        Console.WriteLine(minTime);
    }
}