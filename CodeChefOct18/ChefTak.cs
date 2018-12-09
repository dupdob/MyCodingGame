//#define TEST
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CodeChefOct18
{
    public class ChefTak
    {
        abstract class Runner
        {
            private double coins = 10.0;
            private double _tugreks;
            public readonly string Label;

            public double Coins => coins;

            public double Score => Coins - 10;
            
            public double Tugreks => _tugreks;

            protected Runner(string label)
            {
                Label = label;
            }

            public double ExecuteRound(int t, double qty, double price, int remainingRounds)
            {
                var execQty = Action(t, qty, price, remainingRounds);

                var nominal = Round(execQty * price);
                if (t == 1)
                {
                    if (_tugreks < nominal)
                    {
                        Debug.WriteLine("Trying to spend more than what we have");
                    }

                    coins += execQty;
                    _tugreks -= nominal;
                    // remove fees
                    coins -= ComputeFees(execQty);
                }
                else
                {
                    if (execQty > coins)
                    {
                        Debug.WriteLine("Trying to sell more than what we have");
                    }

                    coins -= execQty;
                    _tugreks += nominal;
                    // remove fees
                    _tugreks -= ComputeFees(nominal);
                }

                coins = Round(coins);
                _tugreks = Round(_tugreks);
                return execQty;  
            }

            protected abstract double Action(int dir, double qty, double price, int remaining);

            public override string ToString()
            {
                return $"{Label}: Result : {coins-10} ({coins:G8} coins, {_tugreks:G8} tugreks)";
            }
        }
       
        
        private static readonly IList<Runner> runners = new List<Runner>();
        
        private static void MainTak(string[] args)
        {
#if !TEST
            var testCases = 5;//int.Parse(Console.ReadLine());
            for(var t= 0 ; t < testCases; t++)
            {
                var N = int.Parse(Console.ReadLine());
                runners.Clear();
                runners.Add(new ScalpingStrategyRunner(.20, .50));
                for (var i = 0; i < N; i++)
                {
                    var instructions = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    var dir = int.Parse(instructions[0]);
                    var qty = double.Parse(instructions[1]);
                    var price = double.Parse(instructions[2]);
                    var execQty = ExecuteRound(dir, qty, price, N-i-1);

                    Console.WriteLine($"{execQty:G8}");
                }

                if (runners[0].Coins<20)
                {
                    throw new Exception();
                }
             }
#else
            runners.Clear();
            runners.Add(new BasicRunner());
            runners.Add(new ScalpingStrategyRunner(.15, .20));
            runners.Add(new ScalpingStrategyRunner(.20, .50));
            runners.Add(new ScalpingStrategyRunner(.20, .50, .10));
            var rnd = new Random();
            var N = 10000;
            var P = rnd.Next(25, 75);
            var Z = 10.0;
            var minPrice = 20.0;
            var maxPrice = 0.0;
            
            for (var i = 0; i < N; i++)
            {
                var t = rnd.Next(1, 100) > P ? 2 : 1;
                var Y = rnd.Next(1, 100);
                Z += rnd.Next(-100, 100) / 1000.0;
                if (Z > 15)
                {
                    Z -= 2 * (Z - 15);
                }
                else if (Z<5)
                {
                    Z += 2 * (5 - Z );
                }
                if (rnd.Next(1, 1000) == 1000)
                {
                    Z += rnd.Next(-1000, 1000) / 1000.0;
                    if (Z > 15)
                    {
                        Z -= 2 * (Z - 15);
                    }
                    else if (Z<5)
                    {
                        Z += 2 * (5 - Z );
                    }
                }
        
                if (minPrice > Z)
                {
                    minPrice = Z;
                }
                else if (maxPrice < Z)
                {
                    maxPrice = Z;
                }

                Z = Round(Z);
                ExecuteRound(t, Y, Z, N - i-1);
            }
            Console.WriteLine($"Last {Z}, (min-max {minPrice:G8}-{maxPrice:G8})");
            var max = double.MinValue;
            var label = string.Empty;
            foreach (var runner in runners)
            {
                Console.WriteLine(runner);
                if (runner.Score > max)
                {
                    max = runner.Score;
                    label = runner.Label;
                }
            }
            Console.WriteLine($"Best is {label} with {max}.");
#endif
        }

        class ScalpingStrategyRunner : Runner
        {
            private double savings;
            private double AvailableCoins => Coins - savings;
            private const int SavingThreshold = 10;
            private bool savingDone;
            private double lastSell = 10.0;
            private readonly double marginThreshold = .15;
            private readonly double stopLoss = .2;
            private readonly double actionRatio;
            
            
            protected override double Action(int dir, double qty, double price, int remaining)
            {
                var execQty = 0.0;

                if (dir == 1)
                {
                    if (remaining == 0)
                    {
                        // we buy what we can
                        execQty = Math.Min(qty, Tugreks*actionRatio / price);
                    }

                    if (price < lastSell * (1 - marginThreshold) || price > lastSell * (1 + stopLoss))
                    {
                        execQty = Math.Min(qty, Tugreks*actionRatio / price);
                        lastSell = price;
                    }
                    savingDone = false;
                }
                else if (remaining>=20)
                {
                    if (price > lastSell * (1 + marginThreshold) || price < lastSell * (1 - stopLoss))
                    {
                        execQty = Math.Min(qty, AvailableCoins*actionRatio);
                    }
                }

                return execQty;
            }

            public ScalpingStrategyRunner(double margin, double stopLoss, double actionRatio = 1.0) : base($"Scalping ({margin}-{stopLoss} {actionRatio*100}%)")
            {
                this.marginThreshold = margin;
                this.stopLoss = stopLoss;
                this.actionRatio = actionRatio;
            }
            
        }
        
        class BasicRunner: Runner
        {
            protected override double Action(int dir, double qty, double price, int remaining)
            {
                var execQty = 0.0;
                if (dir == 1)
                {
                    if (remaining == 0)
                    {
                        // we buy what we can
                        execQty = Math.Min(qty, Tugreks / price);
                    }
                    // we can buy coins
                    else if (price < 9)
                    {
                        execQty = Math.Min(qty, Tugreks*.75 / price);
                    }
                    else if (price < 9.5 || remaining<10) 
                        // arbitrary threshold, for test
                    {
                        execQty = Math.Min(qty, Tugreks*.5 / price);
                    }
                }
                else
                {
                    // we can sell coins
                    if (price > 11)
                    {
                        execQty = Math.Min(qty, .75*Coins);
                    }
                    else if (price > 10.5)
                        // arbitrary threshold, for test
                    {
                        execQty = Math.Min(qty, .5 * Coins);
                    }
                    else if (price > 14.5)
                    {
                        execQty = Math.Min(qty, Coins);
                    }
                }

                return execQty;
            }

            public BasicRunner() : base("Basic")
            {
                
            }
        }

        private static double ExecuteRound(int dir, double qty, double price, int remainingRounds)
        {
            var execQty = 0.0;
            foreach (var runner in runners)
            {
                execQty = runner.ExecuteRound(dir, qty, price, remainingRounds);
            }
            return execQty;
        }


        private static double ComputeFees(double value)
        {
            return Round(value * 0.0005);
        }

        private static double Round(double toRound)
        {
            if (toRound == 0.0)
            {
                return toRound;
            }

            if (Math.Abs(toRound) < 1.0E-10)
            {
                return 0;
            }
            var firstDigit = (int)Math.Log10(toRound);
            if (firstDigit < 0)
            {
                return toRound;
            }
            return firstDigit < 8 ? Math.Round(toRound, Math.Max(8 - firstDigit, 8), MidpointRounding.AwayFromZero) : toRound;
        }
    }
}