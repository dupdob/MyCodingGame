using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeChefSept18
{
    public class BadShuffle
    {
        static void MainShuffle(string[] args)
        {
            var cards = int.Parse(Console.ReadLine());

            if (cards == 1)
            {
                Console.WriteLine(1);
                Console.WriteLine(1);
                return;
            }

            var lowestProbaHand = Enumerable.Range(1, cards-1).ToList();
            lowestProbaHand.Insert(0, cards);

            var halfHand = cards / 2;
            var highestHand = Enumerable.Range(2, halfHand-1).ToList();
            highestHand.Add(1);
            var fistCard = halfHand + 1;
            highestHand.AddRange(Enumerable.Range(fistCard+1, cards-fistCard));
            highestHand.Add(fistCard);
            
            Console.WriteLine(ListToString(highestHand));
            Console.WriteLine(ListToString(lowestProbaHand));

        }

        private static string ListToString(IEnumerable<int> cards)
        {
            var text = new StringBuilder();
            foreach (var card in cards)
            {
                text.Append(card.ToString());
                text.Append(' ');
            }

            return text.ToString();            
        }
    }
}