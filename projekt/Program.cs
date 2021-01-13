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
            Console.WriteLine(/*pref + " " +*/ t);
        }
        /*wypisanie struktury*/
        public static void WY()
        {
            avl.Print();
            Console.WriteLine();
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
            if (!avl.GetIndex(startCity)) { Console.WriteLine("NIE"); return; };
            if (!avl.GetIndex(endCity)) { Console.WriteLine("NIE"); return; };
            Console.WriteLine(graf.FindRoad(startCity, endCity));
        }
        /*sprawdzenie do ilu sie skroci droga*/
        public static void IS(string startCity, string cityOne, string cityTwo, int length)
        {
            if (!avl.GetIndex(startCity)) { Console.WriteLine("NIE"); return; };
            if (!avl.GetIndex(cityOne)) { Console.WriteLine("NIE"); return; };
            if (!avl.GetIndex(cityTwo)) { Console.WriteLine("NIE"); return; };
            Console.WriteLine(graf.VirtRoad(startCity, cityOne, cityTwo, length));
        }
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            string[] parts;


            switch (false)
            {
                case true:

                    string[] add = { "1", "1", "1", "1", "2", "3", "4", "5", "6", "z", "d", "f", "q", "qq", "b", "dd", "aa", "oo", "t", "e", "yw", "hgf", "sdf", "qwe", "rbx", "gsw", "mcz", "mmkz", "akr", "zxv", "wra", "gud", "fds", "sdge", "hgc", "gtse", "hyd", "prehyd", "p", "prefdshyd", "prefdshyd", "prefdshyd", "prefdshyd", "prefdshyd", "prehydeee", "prehyds", "prehydf", "preehyd", "preahyd", "preqhyd", "prevbchyd", "zggdz", "frsf", "bhddsx" };
                    for (int i = 0; i < add.Length; i++)
                        DM(add[i]);
                    WY();
                    string[] del = { /*, "sdge", "hgc", "gtse", "hyd", "sdge", "hgc",  "prefdshyd","z", "gtse", "hyd", "sdf", "fd", "fds", "prefdshyd", "fe", */ "oo", "mcz", };
                    for (int i = 0; i < del.Length; i++)
                        UM(del[i]);

                    Console.WriteLine();
                    string[] city = { "bhddsx", "aaaaaaaaaaaa" };
                    WY();
                    for (int i = 0; i < city.Length; i++)
                    {
                        Console.Write(city[i] + ' ');
                        WM(city[i]);
                    }

                    LM("p");

                    string[] dd = {
                        "1 2 7",
                        "1 3 9",
                        "1 6 14",
                        "2 3 10",
                        "2 4 15",
                        "3 4 11",
                        "3 6 2",
                        "3 6 3",
                        "4 5 6",
                    };

                    for (int i = 0; i < dd.Length; i++)
                    {
                        parts = dd[i].Split(' ');
                        DD(parts[0], parts[1], Int32.Parse(parts[2]));
                    }
                    ND("1", "5");
                    break;
                case false:
                    StreamReader sr = new StreamReader("../../projekt1_in7.txt");
                    string lines = sr.ReadLine();
                    string line;
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
                    break;
            }

            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

        }
    }
}
