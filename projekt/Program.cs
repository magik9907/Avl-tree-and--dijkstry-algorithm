using System;
using System.Collections.Generic;
using System.Linq;
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
        public static int LM(string pref)
        {
            return avl.CountPrefix(pref);
        }
        /*wypisanie struktury*/
        public static void WY()
        {
            avl.Print();
        }
        /*dodanie drogi*/
        public static void DD(string cityOne, string cityTwo, int length)
        {
            int indexOne = avl.GetIndex(cityOne);
            if (indexOne == -1) Console.WriteLine("NIE");
            int indexTwo = avl.GetIndex(cityTwo);
            if (indexTwo == -1) Console.WriteLine("NIE");
            graf.AddRoad(indexOne, indexTwo, length);
        }
        /*usuniecie drogi*/
        public static void UD(string cityOne, string cityTwo)
        {
            int indexOne = avl.GetIndex(cityOne);
            if (indexOne == -1) Console.WriteLine("NIE");
            int indexTwo = avl.GetIndex(cityTwo);
            if (indexTwo == -1) Console.WriteLine("NIE");
            graf.RemoveRoad(indexOne, indexTwo);
        }
        /*najkrotsza droga*/
        public static void ND(string startCity, string endCity)
        {/*
            ND miasto1 miasto2– obliczenie najkrótszej drogi z jednego miasta do drugiego(wypisanie jej długości)*/
            int indexOne = avl.GetIndex(startCity);
            if (indexOne == -1) Console.WriteLine("NIE");
            int indexTwo = avl.GetIndex(endCity);
            if (indexTwo == -1) Console.WriteLine("NIE");
            Console.WriteLine(graf.FindRoad(indexOne, indexTwo));
        }
        /*sprawdzenie do ilu sie skroci droga*/
        public static void IS(string startCity, string cityOne, string cityTwo, int length)
        {
            /*            IS miasto1 miasto2 miasto3 długość – obliczenie do ilu miast skróci się najkrótsza droga z
                 miasta1 po potencjalnym dodaniu drogi pomiędzy miastem2 i miastem3 o zadanej długości
            */
        }
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            /*
                        string[] add = { "1", "2", "3", "4", "5", "6", "z", "d", "f", "q", "qq", "b", "dd", "aa", "oo", "p", "t", "e", "yw", "hgf", "sdf", "qwe", "rbx", "gsw", "mcz", "mmkz", "akr", "zxv", "wra", "gud", "fds", "sdge", "hgc", "gtse", "hyd", "prehyd", "prefdshyd", "prehydeee", "prehyds", "prehydf", "preehyd", "preahyd", "preqhyd", "prevbchyd", "zggdz", "frsf", "bhddsx" };*/

            string[] add = { "1", "2", "3", "4", "5", "6" };


            for (int i = 0; i < add.Length; i++)
                DM(add[i]);
            WY();
            string[] del = { "z", "sdf", "oo" };
            for (int i = 0; i < del.Length; i++)
                UM(del[i]);
            WY();

            Console.WriteLine();
            string[] city = { "bhddsx", "aaaaaaaaaaaa" };
            for (int i = 0; i < city.Length; i++)
            {
                Console.Write(city[i] + ' ');
                WM(city[i]);
            }
            Console.WriteLine("prefix: pre; count: " + LM("p"));
            watch.Stop();

            string[] dd = {
                "1 2 7",
                "1 3 9",
                "1 6 14",
                "2 3 10",
                "2 4 15",
                "3 4 11",
                "3 6 2",
                "6 5 9",
                "4 5 6",
            };
            string[] parts;
            for (int i = 0; i < dd.Length; i++)
            {
                parts = dd[i].Split(' ');
                DD(parts[0], parts[1], Int32.Parse(parts[2]));
            }

            ND("1", "5");
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
