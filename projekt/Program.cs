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

            string[] add = { "z", "d", "f", "q", "qq", "b", "dd", "aa", "oo", "p", "t", "e", "yw", "hgf", "sdf", "qwe", "rbx", "gsw", "mcz", "mmkz", "akr", "zxv", "wra", "gud", "fds", "sdge", "hgc", "gtse", "hyd", "zggdz", "frsf", "bhddsx" };
            Avl avl = new Avl();

            for (int i = 0;i<add.Length;i++)
                avl.Insert(add[i]);
            avl.Print();
            Console.WriteLine();
            string[] del = { "z"};
            for (int i = 0; i < del.Length; i++)
                avl.Delete(del[i]);
            avl.Print();

            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
