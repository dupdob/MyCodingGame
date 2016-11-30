using System;
using System.IO;
using System.Text;

namespace Test.Tooling
{
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

        public string Text => Encoding.Unicode.GetString(this.buffer.ToArray());

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