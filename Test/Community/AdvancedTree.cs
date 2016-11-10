using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

/**
Your goal is to recreate the behaviour of the UNIX tree program with support for some flags.
You'll be given a starting path S, a list of flags F, and a list of files.

The tree will be printed with characters pipe | backtick ` hyphen - and with spaces.
Refer to the first example to understand how you need to print the tree.

You'll also need to handle three flags inspired by the original tree program:
- -a : Display hidden directories and files.
- -d: Only display directories (modifies the report line, see below).
- -L depth: Limit the "depth" of the tree.

A hidden file or directory has a name starting with a dot: .

The input path S must be printed followed by the text [error opening dir] if:
- the path is a file, or
- the path doesn't exist

Finally, you'll need to print a new line plus the number of directories and files found as follows:

x directories, y files

where x is the number of directories and y is the number of files. If the -d option is in effect, that last line reads like this instead:

x directories

If x or y are equal to 1, then you need to print directory and file respectively.

The tree needs to be sorted in ascending alphabetical order, letters in lowercase having priority on uppercases and without considering the leading and first character '.' (dot) from hidden files or directories names.

Incorrect flags must be ignored.
You're only given valid paths, which are only starting with one "current directory" (./) and without "parent directory" (../)
There are no empty directories.
There are no spaces in files or directory names.
There are no duplicated directories or files with the same name as either a hidden or non-hidden file/directory on the same depth (i.e: you will never see both .Directory1 and Directory1 in the same folder)

 **/
class AdvancedTree
{
    class Node
    {
        public string Name;
        public bool Hidden;
        public bool IsDirectory;
        public List<Node> SubNodes = new List<Node>();

        public string CleanName => this.Name[0] == '.' ? this.Name.Substring(1) : this.Name;

        public Node(string name, bool isDirectory)
        {
            if (name[0] == '.')
            {
                this.Hidden = true;
            }
            this.Name = name;
            this.IsDirectory = isDirectory;
        }

        public Node FindDirectory(string name, bool create=true)
        {
            foreach (var subNode in this.SubNodes)
            {
                if (subNode.Name == name && subNode.IsDirectory)
                    return subNode;
            }
            if (!create)
                return null;
            var item = new Node(name, true);
            this.SubNodes.Add(item);
            return item;
        }

        public Node FindFile(string name)
        {
            foreach (var subNode in this.SubNodes)
            {
                if (subNode.Name == name && !subNode.IsDirectory)
                    return subNode;
            }
            var item = new Node(name, false);
            this.SubNodes.Add(item);
            return item;
        }

        public Tuple<int, int> Dump(string tabs, int maxDepth, bool dirOnly, bool withHidden)
        {
            if (maxDepth==0)
                return  new Tuple<int, int>(0, 0);

            int nbFiles = 0, nbDirs = 0;
            var output =
                this.SubNodes.Select(x => x)
                .Where(_ => (!_.Hidden || withHidden) && (_.IsDirectory || !dirOnly))
                .OrderBy(x => x.CleanName).ToArray();
            for (int i = 0; i < output.Length; i++)
            {
                var builder = new StringBuilder();
                var header = (i < output.Length - 1) ? "|-- " : "`-- ";
                var subtabs = (i < output.Length - 1) ? "|   " : "    ";
                builder.Append(tabs);
                builder.Append(header);
                builder.Append(output[i].Name);
                Console.WriteLine(builder.ToString());
                if (output[i].IsDirectory)
                {
                    var res = output[i].Dump(tabs + subtabs, maxDepth-1, dirOnly, withHidden);
                    nbDirs += 1 + res.Item2;
                    nbFiles += res.Item1;
                }
                else
                {
                    nbFiles++;
                }

            }
            return new Tuple<int, int>(nbFiles, nbDirs);
        }
    }

    private static Node root;

    static void MainTree(string[] args)
    {
        var S = Console.ReadLine();
        var F = Console.ReadLine();
        var N = int.Parse(Console.ReadLine());
        root = new Node(".",true);
        // build tree structure
        for (var i = 0; i < N; i++)
        {
            var line = Console.ReadLine();
            var path = line.Split('/');
            var node = AdvancedTree.root;
            var filename = path.Last();
            for (var j= 0; j < path.Length-1; j++)
            {
                if (path[j]==".")
                    continue;
                node = node.FindDirectory(path[j]);
            }
            if (filename != string.Empty)
            {
                node.FindFile(filename);
            }
        }
        // parse flags
        var flags = F.Split(',');
        bool withHidden = false, dirOnly = false;
        int maxDepth = int.MaxValue;
        foreach (var flag in flags)
        {
            var cleanFlag = flag.Trim();
            if (cleanFlag == string.Empty)
                continue;
            if (cleanFlag[0] !='-')
                continue;
            switch (cleanFlag[1])
            {
                case 'a':
                    withHidden = true;
                    break;
                case 'L':
                    var pars = cleanFlag.Split(' ');
                    if (pars.Length<2)
                        continue;
                    maxDepth = int.Parse(pars[1]);
                    break;
                case 'd':
                    dirOnly = true;
                    break;
            }
        }
        // find starting point

        Console.Write(S);
        var startPath = S.Split('/');
        var index = 0;
        if (startPath.Length>0 && startPath[0] == ".")
        {
            index = 1;
        }
        var rootNode = root;
        for (var i = index; i < startPath.Length; i++)
        {
            rootNode = rootNode.FindDirectory(startPath[i], false);
            if (rootNode == null)
            {
                Console.Write(" [error opening dir]");
            }
        }
        Console.WriteLine();

        var res = rootNode != null ? rootNode.Dump("", maxDepth, dirOnly, withHidden) : new Tuple<int, int>(0, 0);
        Console.WriteLine();
        var dirLabel = res.Item2 == 1 ? "directory" : "directories";
        var fileLabel = res.Item1 == 1 ? "file" : "files";
        if (dirOnly)
        {
            Console.WriteLine("{0} {1}", res.Item2, dirLabel);
        }
        else
        {
            Console.WriteLine("{0} {1}, {2} {3}", res.Item2, dirLabel, res.Item1, fileLabel);
        }
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

    }
}