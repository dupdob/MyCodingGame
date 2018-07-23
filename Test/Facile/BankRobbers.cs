using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class BankRobbers
{
    static void MainRobbers(string[] args)
    {
        int R = int.Parse(Console.ReadLine());
        int V = int.Parse(Console.ReadLine());

        Console.Error.WriteLine($"{R} Robbers, {V} Vaults");
        var vaults = new int[V];
        for (int i = 0; i < V; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int C = int.Parse(inputs[0]);
            int N = int.Parse(inputs[1]);

            int combinaison = (int)(Math.Pow(5, C - N) * Math.Pow(10, N));
            vaults[i] = combinaison;
            Console.Error.WriteLine($"{i}={combinaison}");
        }

        var progress = new int[R];
        var currentVault = 0;
        while (currentVault<V)
        {
            Array.Sort(progress);
            progress[0] += vaults[currentVault++];
            Console.Error.WriteLine($"Progress {progress[0]}");
        }
        Array.Sort(progress);
        Console.WriteLine(progress[R-1]);
    }
}