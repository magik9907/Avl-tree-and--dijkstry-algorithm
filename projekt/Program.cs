using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace projekt
{
    class Program
    {
        static Avl avl = new Avl();
        static Graf graf = new Graf();
        /*dodanie miasta*/
        public static void DM(string city)
        {
            Element elem = new Element(city);
            avl.Insert(elem);
            graf.Insert(city);
        }
        /*usuniecie miasta*/
        public static void UM(string city)
        {
            avl.Delete(city);
            graf.Delete(city);
        }
        /*wyszukanie*/
        public static void WM(string city)
        {
            avl.IsCity(city);
        }
        /*zliczenie z prefiksem*/
        public static void LM(string pref)
        {
            int t = avl.CountPrefix(pref);
            Console.WriteLine(t);
        }
        /*wypisanie struktury*/
        public static void WY()
        {
            avl.Print();
        }
        /*dodanie drogi*/
        public static void DD(string cityOne, string cityTwo, int length)
        {
            graf.AddRoad(cityOne, cityTwo, length);
        }
        /*usuniecie drogi*/
        public static void UD(string cityOne, string cityTwo)
        {
            graf.RemoveRoad(cityOne, cityTwo);
        }
        /*najkrotsza droga*/
        public static void ND(string startCity, string endCity)
        {
            Console.WriteLine(graf.FindRoad(startCity, endCity));
        }
        /*sprawdzenie do ilu sie skroci droga*/
        public static void IS(string startCity, string cityOne, string cityTwo, int length)
        {
            Console.WriteLine(graf.VirtRoad(startCity, cityOne, cityTwo, length));
        }

        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            string[] parts;
            StreamReader sr = new StreamReader("../../projekt1_in7.txt");
            string lines = sr.ReadLine();
            string line;
            bool k = false;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                parts = line.Split(' ');

                switch (parts[0])
                {
                    case "DM":
                        DM(parts[1]);
                        break;
                    case "UM":
                        UM(parts[1]);
                        break;
                    case "WM":
                        WM(parts[1]);
                        break;
                    case "LM":
                        LM(parts[1]);
                        break;
                    case "WY":
                        WY();
                        break;
                    case "DD":
                        DD(parts[1], parts[2], int.Parse(parts[3]));
                        break;
                    case "UD":
                        UD(parts[1], parts[2]);
                        break;
                    case "ND":
                        ND(parts[1], parts[2]);
                        break;
                    case "IS":
                        IS(parts[1], parts[2], parts[3], int.Parse(parts[4]));
                        break;
                }
            }
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
