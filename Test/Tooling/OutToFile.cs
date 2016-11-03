// Redirection Utility
// Author: Hai Vu (haivu2004 on Google mail)

namespace Test.Tooling
{
    using System;
    using System.IO;
    using System.Text;

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

    /// <summary>
    /// OutToFile is an easy way to redirect console output to a string.
    ///
    /// Usage:
    ///    Console.WriteLine("This text goes to the console by default");
    ///    using (OutToFile redir = new OutToString())
    ///    {
    /// 	   Console.WriteLine("Contents of out.txt");
    ///    }
    ///    Console.WriteLine("This text goes to console again");
    ///
    /// </summary>
    public class OutToString : IDisposable
    {
        private readonly StreamWriter fileOutput;
        private readonly TextWriter oldOutput;
        private readonly MemoryStream buffer;

        public string Text
        {
            get { return Encoding.Unicode.GetString(this.buffer.ToArray()); }
        }

        /// <summary>
        /// Create a new object to redirect the output
        /// </summary>
        public OutToString()
        {
            this.oldOutput = Console.Out;
            this.buffer = new MemoryStream();
            this.fileOutput = new StreamWriter(
                this.buffer
            ) {AutoFlush = true};
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
