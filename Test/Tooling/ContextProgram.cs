namespace CSharpContestProject
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Test.Tooling;
    /// <summary>
    /// Helper bootstrap to have local testing environement
    /// </summary>
    class ContextProgram
    {
        static void MainTest(string[] args)
        {
            Console.WriteLine("Starting test");
            Console.WriteLine("________________");
            using (new InFromFile("Input.txt"))
            {
                using (new OutToFile("Outtest.txt"))
                {
                    //Program.Main2(args);
                }
            }

            Console.WriteLine("________________");
            Console.WriteLine("Finishing test");

            var expected = File.OpenText("Output.txt");
            var actual = File.OpenText("OutTest.txt");
            string expectedLine;
            var i = 0;
            while((expectedLine = expected.ReadLine()) != null)
            {
                var actualLine = actual.ReadLine();

                if (actualLine != expectedLine)
                {
                    Console.WriteLine("Error line {0} (actual/expected):", i);
                    Console.WriteLine(actualLine);
                    Console.WriteLine(expectedLine);
                    Console.ReadKey();
                }
                i++;
            }
            var lastLine = actual.ReadLine();
            if (lastLine != null)
            {
                Console.WriteLine("Extra line {0} actual", i);
                Console.WriteLine(lastLine);
                Console.ReadKey();
            }
        }
    }
}
