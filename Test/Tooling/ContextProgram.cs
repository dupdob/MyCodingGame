namespace CSharpContestProject
{
    using System;
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
            var input = new InFromText("");
            var output = new OutToString();
            using (input)
            {
                using (output)
                {
                    //Program.Main2(args);
                }
            }

            Console.WriteLine("________________");
            Console.WriteLine("Finishing test");

            var expected = "";
            var actual = output.Text.Split('\n');
            var expectedLines = expected.Split('\n');
            for (int i = 0; i < expectedLines.Length; i++)
            {
                if (i >= actual.Length)
                {
                    Console.WriteLine("{0} lines are missing", expectedLines.Length - i);
                    Console.ReadKey();
                }
                if (expectedLines[i] != actual[i])
                {
                    Console.WriteLine("Error line {0} (actual/expected):", i);
                    Console.WriteLine(actual[i]);
                    Console.WriteLine(expectedLines[i]);
                    Console.ReadKey();
                }
            }
        }
    }
}
