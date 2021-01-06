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
            if (elem.index != -2)
                graf.Insert(elem);
        }
        /*usuniecie miasta*/
        public static void UM(string city)
        {
            int index = -1;
            avl.Delete(city, ref index);
            if (index > -1)
                graf.Delete(index);
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
            int indexOne = avl.GetIndex(cityOne);
            int indexTwo = avl.GetIndex(cityTwo);
            if (indexTwo == -1 || indexOne == -1) { Console.WriteLine("NIE"); return; };
            graf.AddRoad(indexOne, indexTwo, length);
        }
        /*usuniecie drogi*/
        public static void UD(string cityOne, string cityTwo)
        {
            int indexOne = avl.GetIndex(cityOne);
            int indexTwo = avl.GetIndex(cityTwo);
            if (indexTwo == -1 || indexOne == -1) { Console.WriteLine("NIE"); return; };
            graf.RemoveRoad(indexOne, indexTwo);
        }
        /*najkrotsza droga*/
        public static void ND(string startCity, string endCity)
        {/*
            ND miasto1 miasto2– obliczenie najkrótszej drogi z jednego miasta do drugiego(wypisanie jej długości)*/
            int indexOne = avl.GetIndex(startCity);
            int indexTwo = avl.GetIndex(endCity);
            if (indexTwo == -1 || indexOne == -1) { Console.WriteLine("NIE"); return; };
            Console.WriteLine(graf.FindRoad(indexOne, indexTwo));
        }
        /*sprawdzenie do ilu sie skroci droga*/
        public static void IS(string startCity, string cityOne, string cityTwo, int length)
        {
            /*   
             *   UWAGA: dla uproszczenia zakładamy, że w testach zapytania
dotyczące ostatniej funkcjonalności zawsze pojawiają się na końcu pliku testowego (gdy graf już
się nie zmienia) i dla danego testu miasto A jest zawsze takie samo we wszystkich zapytaniach.


             *   IS miasto1 miasto2 miasto3 długość – obliczenie do ilu miast skróci się najkrótsza droga z
                 miasta1 po potencjalnym dodaniu drogi pomiędzy miastem2 i miastem3 o zadanej długości
            */
            Console.WriteLine(0);
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

                    //  string[] add = { "1", "2", "3", "4", "5", "6" };


                    for (int i = 0; i < add.Length; i++)
                        DM(add[i]);
                    //WY();
                    string[] del = { /*, "sdge", "hgc", "gtse", "hyd", "sdge", "hgc",  "prefdshyd","z", "gtse", "hyd", "sdf", "fd", "fds", "prefdshyd", "fe", */ "oo", "mcz", };
                    for (int i = 0; i < del.Length; i++)
                        UM(del[i]);

                    Console.WriteLine();
                    string[] city = { "bhddsx", "aaaaaaaaaaaa" };
                    for (int i = 0; i < city.Length; i++)
                    {
                        Console.Write(city[i] + ' ');
                        WM(city[i]);
                    }
                    //WY();
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

                    StreamReader sr = new StreamReader("projekt1_in8.txt");
                    string lines = sr.ReadLine();
                    string line;
                    int k = 0;
                    while (k < int.Parse(lines))
                    {
                        line = sr.ReadLine();
                        parts = line.Split(' ');
                        k++;
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
