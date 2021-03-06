﻿// Redirection Utility
// Author: Hai Vu (haivu2004 on Google mail)

namespace Test.Tooling
{
    using System;
    using System.IO;

    /// <summary>
    /// OutToFile is an easy way to redirect console output to a file.
    ///
    /// Usage:
    ///    Console.WriteLine("This text goes to the console by default");
    ///    using (OutToFile redir = new OutToFile("out.txt"))
    ///    {
    /// 	   Console.WriteLine("Contents of out.txt");
    ///    }
    ///    Console.WriteLine("This text goes to console again");
    ///
    /// </summary>
    public class OutToFile : IDisposable
    {
        private StreamWriter fileOutput;
        private TextWriter oldOutput;

        /// <summary>
        /// Create a new object to redirect the output
        /// </summary>
        /// <param name="outFileName">
        /// The name of the file to capture console output
        /// </param>
        public OutToFile(string outFileName)
        {
            this.oldOutput = Console.Out;
            this.fileOutput = new StreamWriter(
                new FileStream(outFileName, FileMode.Create)
                );
            this.fileOutput.AutoFlush = true;
            Console.SetOut(this.fileOutput);
        }

        // Dispose() is called automatically when the object
        // goes out of scope
        public void Dispose()
        {
            Console.SetOut(this.oldOutput);  // Restore the console output
            this.fileOutput.Close();        // Done with the file
        }
    }
}
