using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * Link has been sent to the Secret Realm to find the Triforce and save Hyrule. But the vile Ganondorf followed Link into the Temple of Time, trapped him, and took the Triforce for himself.

The Temple of Time has been closed for a thousand years now, but the Sages finally found you, one of the greatest Nerds ever born, and they need your help to open the Temple of Time's door and join Link in the ultimate battle!

The Temple of Time's surface contains several incrusted Triforces of different sizes, and the Sages believe that by creating Triforces of the corresponding sizes, the door will open. Even though no magic has worked until now, your programming skills will surely make the difference.

The program:

You must create a program that echoes a Triforce of a given size N.

- A triforce is made of 3 identical triangles
- A triangle of size N should be made of N lines
- A triangle's line starts from 1 star, and earns 2 stars each line
- Take care, a . must be located at the top/left to avoid automatic trimming

For example, a Triforce of size 3 will look like:


.    *
    ***
   *****
  *     *
 ***   ***
***** *****
 **/
class TriForce
{
    static void MainTriForce(string[] args)
    {
        int N = int.Parse(Console.ReadLine());

        var builder = new StringBuilder();

        for (var i = 0; i < N; i++)
        {
            builder.Append(' ', 2*N - i - 1);
            builder.Append('*', i*2 + 1);
            builder.AppendLine();
        }

        for (var i = 0; i < N; i++)
        {
            builder.Append(' ', N - i - 1);
            builder.Append('*', i * 2 + 1);
            builder.Append(' ', 2 * N - i*2 - 1);
            builder.Append('*', i * 2 + 1);
            if (i<N-1)
            builder.AppendLine();
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine('.'+builder.ToString().Substring(1));
    }
}