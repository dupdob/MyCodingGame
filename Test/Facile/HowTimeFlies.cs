using System;
using System.Text;
using System.Globalization;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 https://www.codingame.com/ide/puzzle/how-time-flies
 **/
class HowTimeFlies
{
    static void MainTimeFiles(string[] args)
    {
        var BEGIN = Console.ReadLine();
        var END = Console.ReadLine();

        var begin = DateTime.ParseExact(BEGIN, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        var end = DateTime.ParseExact(END, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        var elapsed = end - begin;
        var nbDays = elapsed.TotalDays;
        var nbYears = end.Year - begin.Year ;
        if (end.DayOfYear < begin.DayOfYear)
        {
            nbYears--;
        }

        var nbMonths = end.Month - begin.Month;
        if (nbMonths < 1)
        {
            nbMonths += 12;
            nbMonths %= 12;
        }

        if (end.Day < begin.Day)
        {
            nbMonths--;
        }
        var builder = new StringBuilder();
        if (nbYears > 0)
        {
            builder.Append(nbYears);
            builder.Append(" year");
            if (nbYears > 1)
            {
                builder.Append('s');
            }

            builder.Append(", ");
        }

        if (nbMonths > 0)
        {
            builder.Append(nbMonths);
            builder.Append(" month");
            if (nbMonths > 1)
            {
                builder.Append('s');
            }            
            builder.Append(", ");
        }

        builder.Append($"total {(int) nbDays} days");
        Console.WriteLine(builder.ToString());
    }
}