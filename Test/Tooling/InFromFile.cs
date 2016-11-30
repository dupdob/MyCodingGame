namespace Test.Tooling
{
    using System;
    using System.IO;

    class InFromFile: IDisposable
    {
        private StreamReader fileIn;
        private TextReader oldInput;

        /// <summary>
        /// Create a new object to redirect the output
        /// </summary>
        /// <param name="outFileName">
        /// The name of the file to capture console output
        /// </param>
        public InFromFile(string outFileName)
        {
            this.oldInput = Console.In;
            this.fileIn = new StreamReader(
                new FileStream(outFileName, FileMode.Open)
                );
            Console.SetIn(this.fileIn);
        }

        /// <summary>
        /// Create a new object to redirect the output
        /// </summary>
        /// <param name="index">
        /// The name of the file to capture console output
        /// </param>
        public InFromFile(int index)
        {
            var filename = string.Format("..\\..\\input{0}.txt", index);
            this.oldInput = Console.In;
            this.fileIn = new StreamReader(
                new FileStream(filename, FileMode.Open)
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
