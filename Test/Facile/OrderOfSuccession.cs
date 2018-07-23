namespace CodingGame.Facile
{
    using System;
    using System.Collections.Generic;

    public class OrderOfSuccession
    {
        static void MainOrderOfSuccession(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            Dictionary<string, Heir> family = new Dictionary<string, Heir>(n);
            for (int i = 0; i < n; i++)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                string name = inputs[0];
                string parent = inputs[1];
                int birth = int.Parse(inputs[2]);
                string death = inputs[3];
                string religion = inputs[4];
                string gender = inputs[5];
                
                var heir = new Heir();
                heir.Name = name;
                heir.Parent = parent;
                heir.birth = birth;
                heir.canInherit = death == "-" && religion == "Anglican";
                heir.isMale = gender == "M";
                family[name] = heir;
            }

            foreach (var heir in family.Values)
            {
                if (heir.Parent != "-")
                {
                    family[heir.Parent].heirs.Add(heir);
                }
            }

            foreach (var heir in family.Values)
            {
                if (heir.Parent == "-")
                {
                    heir.Scan((x)=>Console.WriteLine(x.Name));
                    break;
                }
            }

        }

        public class Heir
        {
            public string Name;
            public string Parent;
            public int birth;
            public bool isMale;
            public bool canInherit;
            public List<Heir> heirs = new List<Heir>();

            public void Scan(Action<Heir> func)
            {
                if (this.canInherit)
                {
                    func(this);
                }
                List<Heir> children = new List<Heir>(this.heirs);
                
                children.Sort(Comparer<Heir>.Create((heir, heir1) =>
                {
                    if (heir.isMale)
                    {
                        return !heir1.isMale || heir.birth < heir1.birth ? -1 : 1;
                    }
                    return !heir1.isMale && heir.birth < heir1.birth ? -1 : 1;
                }));

                foreach (var child in children)
                {
                    child.Scan(func);
                }                
            }
        }
    }
}