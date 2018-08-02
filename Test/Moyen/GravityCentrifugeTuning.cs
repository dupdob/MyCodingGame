using System;
using System.Collections.Generic;
using System.Numerics;

namespace CodingGame.Moyen
{
    public class GravityCentrifugeTuning
    {
        static void MainCentrifugeééé(string[] args)
        {

            var input = BigInteger.Parse(Console.ReadLine());
            
            var fibos = new List<BigInteger>();
            var current = new BigInteger(1);
            fibos.Add(current);
            fibos.Add(current);
            while (true)
            {
                current = fibos[fibos.Count - 1] + fibos[fibos.Count - 2];
                if (current > input)
                {
                    break;
                }
                fibos.Add(current);
            }

            fibos.RemoveAt(0);
            var bits = new List<bool>();
            for (var i = fibos.Count - 1; i >= 0; i--)
            {
                if (input >= fibos[i])
                {
                    input -= fibos[i];
                    bits.Add(true);
                }
                else
                {
                    bits.Add(false);
                }
            }
            var digit = 0;
            var bitsForDigit = (bits.Count-1) % 3;
            for (int i = 0; i < bits.Count; i++)
            {
                digit *= 2;
                if (bits[i])
                {
                    digit += 1;
                }

                if (bitsForDigit == 0)
                {
                    Console.Write(digit);
                    bitsForDigit = 2;
                    digit = 0;
                }
                else
                {
                    bitsForDigit--;
                }
            }
            Console.WriteLine();
        }
  
    }
}