using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class SafeCracking
{
    static void MainSafe(string[] args)
    {
        string msg = Console.ReadLine();
        var parts = msg.Split(':');
        var alphabet = parts[0].Split(' ');
        var message = parts[1].Split('-');
        foreach (var alphat in alphabet)
        {
            Console.Error.WriteLine("- Alphabet {0}", alphat);
        }

        foreach (var dig in message)
        {
            var response = 0;
            var digit = dig.Trim();
            switch (digit.Length)
            {
                case 3:
                    if (digit[0] == alphabet[2][1])
                        response = 1;
                    else if (digit[2] == alphabet[2][1])
                        response = 2;
                    else if (digit[0] == alphabet[1][0])
                        response = 6;
                    else
                        Console.Error.WriteLine("Failed to recognize {0}", digit);
                    break;
                case 4:
                    if (digit[0] == alphabet[2][5])
                        response = 9;
                    else if (digit[1] == alphabet[0][2])
                        response = 0;
                    else if (digit[1] == alphabet[2][1])
                        response = 4;
                    else if (digit[1] == alphabet[2][4])
                        response = 5;
                    else
                        Console.Error.WriteLine("Failed to recognize {0}", digit);
                    break;
                case 5:
                    if (digit[1] == alphabet[0][1])
                        response = 3;
                    else if (digit[0] == alphabet[1][0])
                        response = 7;
                    else if (digit[0] == alphabet[0][2])
                        response = 8;
                    else
                        Console.Error.WriteLine("Failed to recognize {0}", digit);
                    break;
            }
            Console.Write(response);
        }

        Console.WriteLine();
    }
}