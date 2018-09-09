using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeChefSept18
{
    public class BadShuffle
    {
        static void Main(string[] args)
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

            
/*
            IDictionary<Hand, int> score = new Dictionary<Hand, int>();
            for ( var index = 0L;; index++)
            {
                var hand = new Hand(cards);
                var counter = index;
                for (var i = 0; i < cards; i++)
                {
                    var id = counter % cards;
                    counter /= cards;
                    hand.Swap(i, (int)id);
                }

                if (counter > 0)
                {
                    Console.WriteLine($"{index} draws");
                    break;
                }
  
                if (!score.ContainsKey(hand))
                {
                    score.Add(hand, 1);
                }
                else
                {
                    score[hand] += 1;
                }
            }

            Hand minHand = null;
            Hand maxHand = null;
            int minProba = int.MaxValue;
            int maxProba = 0;
            foreach (var pair in score)
            {
                if (pair.Value > maxProba)
                {
                    maxHand = pair.Key;
                    maxProba = pair.Value;
                }

                if (pair.Value <= minProba)
                {
                    minHand = pair.Key;
                    minProba = pair.Value;
                }
            }
            Console.WriteLine(maxHand);
            Console.WriteLine(minHand);
            
            Console.WriteLine($"Highest proba hands ({maxProba})");
            foreach (var entry in score)
            {
                if (entry.Value == maxProba)
                {
                    Console.WriteLine(entry.Key);
                }
            }
            Console.WriteLine($"Lowest proba hands ({minProba})");
            foreach (var entry in score)
            {
                if (entry.Value == minProba)
                {
                    Console.WriteLine(entry.Key);
                }
            }
 */
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

        sealed class Hand
        {
            private int[] cards;

            public Hand(int cardCount)
            {
                cards = Enumerable.Range(1, cardCount).ToArray();
            }

            public void Swap(int i, int j)
            {
                var temp = cards[j];
                cards[j] = cards[i];
                cards[i] = temp;
            }

            public override string ToString()
            {
                var text = new StringBuilder(cards.Length*3);
                foreach (var card in cards)
                {
                    text.Append(card.ToString());
                    text.Append(' ');
                }

                return text.ToString();
            }

            private bool Equals(Hand other)
            {
                if (cards.Length != other.cards.Length)
                {
                    return false;
                }
                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i] != other.cards[i])
                        return false;
                }

                return true;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Hand) obj);
            }

            public override int GetHashCode()
            {
                var hash = 0;
                foreach (var card in cards)
                {
                    hash = hash * 39 ^ card;
                }

                return hash;
            }
        }
    }
}