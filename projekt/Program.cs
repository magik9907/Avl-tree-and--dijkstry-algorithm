using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            Avl avl = new Avl();
            string name = "z";
            avl.Insert(name);
            name = "q";
            avl.Insert(name);
            name = "p";
            avl.Insert(name);
            name = "d";
            avl.Insert(name);
            name = "b";
            avl.Insert(name);
            name = "o";
            avl.Insert(name);
            name = "n";
            avl.Insert(name);
            name = "m";
            avl.Insert(name);
            name = "i";
            avl.Insert(name);
            name = "l";
            avl.Insert(name);
            name = "e";
            avl.Insert(name);
            name = "a";
            avl.Insert(name);
            name = "g";
            avl.Insert(name);
            name = "w";
            avl.Insert(name);
            name = "c";
            avl.Insert(name);
            name = "u";
            avl.Insert(name);
            name = "f";
            avl.Insert(name);
            name = "v";
            avl.Insert(name);

            avl.Print();
            avl.Delete("m");
            avl.Delete("n");
            avl.Print();

            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
