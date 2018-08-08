using System;
using System.Collections.Generic;
using System.Text;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace CodingGame.Facile
{
    internal class HiddenWord    
    {
        static void MainHiddenWord(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var words = new string[n];
            for (var i = 0; i < n; i++)
            {
                words[i] = Console.ReadLine();
            }
            var inputs = Console.ReadLine().Split(' ');
            var h = int.Parse(inputs[0]);
            var w = int.Parse(inputs[1]);
            var lines = new List<Line>();
            var letters = new Letter[h,w];
            for (var i = 0; i < h; i++)
            {
                var oneLine = Console.ReadLine();
                for (var j = 0; j < w; j++)
                {
                    letters[i,j] = new Letter(i, j, oneLine[j]);
                }                
            }
            
            // horizontal lines
            for (var i = 0; i < h; i++)
            {   
                var line = new Line();
                for (var j = 0; j < w; j++)
                {
                    line.Add(letters[i, j]);
                }
                lines.Add(line);
                Console.Error.WriteLine($"Add horizontal {i}: {line.Text}");
            }

            // vertical lines
            for (var j = 0; j < w; j++)
            {   
                var line = new Line();
                for (var i = 0; i < h; i++)
                {
                    line.Add(letters[i, j]);
                }
                lines.Add(line);
                Console.Error.WriteLine($"Add vertical {j}: {line.Text}");
            }
            
            // down diagonals
            for (var i = 0; i < h; i++)
            {
                var diagDown = new Line();
                for (var j = 0; j < h - i; j++)
                {
                    diagDown.Add(letters[i+j, j]);
                }
                lines.Add(diagDown);
                Console.Error.WriteLine($"Add downDiag {i}: {diagDown.Text}");
            }
            for (var i = 0; i < w; i++)
            {
                var diagDown = new Line();
                for (var j = 0; j < h - i; j++)
                {
                    diagDown.Add(letters[j, i+j]);
                }
                lines.Add(diagDown);
                Console.Error.WriteLine($"Add downDiag {i}: {diagDown.Text}");
            }
            // up diagnoals
            for (var i = 0; i < h; i++)
            {
                var diagUp = new Line();
                for (var j = 0; j < i+1; j++)
                {
                    diagUp.Add(letters[i-j, j]);
                }
                lines.Add(diagUp);
                Console.Error.WriteLine($"Add diagUp {i}: {diagUp.Text}");
            }
            for (var i = 0; i < w; i++)
            {
                var diagUp = new Line();
                for (var j = 0; j < w - i; j++)
                {
                    diagUp.Add(letters[h-j-1, i+j]);
                }
                lines.Add(diagUp);
                Console.Error.WriteLine($"Add diagUp {i}: {diagUp.Text}");
            }

            // find words
            foreach (var word in words)
            {
                var temp = new StringBuilder();
                for (var i = word.Length - 1; i >= 0; i--)
                {
                    temp.Append(word[i]);
                }

                var reversed = temp.ToString();
                foreach (var line in lines)
                {
                    var start = line.Text.IndexOf(word);
                    if (start == -1)
                    {
                        start = line.Text.IndexOf(reversed);
                    }

                    if (start == -1)
                    {
                        continue;
                    }
                    Console.Error.WriteLine("found "+word);
                    for (var i = start; i < start + word.Length; i++)
                    {
                        // we erase found letters
                        var letter = line.Letters[i];
                        letters[letter.Y, letter.X] = null;
                    }
            }
            }
            // get remaining letters
            var lastWord = new StringBuilder();
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++)
                {
                    if (letters[i, j] != null)
                    {
                        lastWord.Append(letters[i, j].Char);
                    }
                }
            }
            Console.WriteLine(lastWord.ToString());
        }

        private class Letter
        {
            public int X { get; }
            public int Y { get; }
            public char Char { get; }

            public Letter(int y, int x, char letter)
            {
                X = x;
                Y = y;
                Char = letter;
            }
        }

        private class Line
        {
            public IList<Letter> Letters { get; }

            public string Text
            {
                get
                {
                    var builder = new StringBuilder(Letters.Count);
                    foreach (var letter in Letters)
                    {
                        builder.Append(letter.Char);
                    }

                    return builder.ToString();
                }
            }

            public Line()
            {
                Letters = new List<Letter>();
            }

            public void Add(Letter letter)
            {
                Letters.Add(letter);
            }
        }
    }
}