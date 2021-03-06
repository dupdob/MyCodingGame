﻿using System;
using System.IO;
using System.Text;

namespace Test.Tooling
{
    class InFromText : IDisposable
    {
        private readonly StreamReader fileIn;
        private readonly TextReader oldInput;

        /// <summary>
        /// Create a new object to redirect the input
        /// </summary>
        /// <param name="text">
        /// Text to use as an input
        /// </param>
        public InFromText(string text)
        {
            this.oldInput = Console.In;
            this.fileIn = new StreamReader(
                new MemoryStream(Encoding.Unicode.GetBytes(text))
            );
            Console.SetIn(this.fileIn);
        }

        // Dispose() is called automatically when the object
        // goes out of scope
        public void Dispose()
        {
            Console.SetIn(this.oldInput);  // Restore the console output
            this.fileIn.Close();        // Done with the file
        }
    }
}