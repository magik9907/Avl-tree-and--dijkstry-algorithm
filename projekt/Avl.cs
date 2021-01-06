﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace projekt
{

    public class Avl
    {
        public Element tree = null;

        public int CountPrefix(string prefix)
        {

            return (tree != null) ? PrefixCounter(prefix, tree) : 0;
        }

        // Regexp /^(pre)\w*/gi
        private int PrefixCounter(string pre, Element curr)
        {
            if (curr == null) return 0;
            Regex regex = new Regex(@"^(" + pre + @")\w*");
            bool status = regex.IsMatch(curr.city);
            int counter = 0;
            if (status)
            {
                counter++;
                counter += PrefixCounter(pre, curr.right);
                counter += PrefixCounter(pre, curr.left);
            }
            else
            {
                switch (CompareString(pre, curr.city))
                {
                    case 1:
                        counter = PrefixCounter(pre, curr.left);
                        break;
                    case -1:
                        counter = PrefixCounter(pre, curr.right);
                        break;
                }
            }
            return counter;
        }

        public void IsCity(string city)
        {
            Element curr = tree;
            while (curr != null && curr.city != city)
            {
                switch (CompareString(city, curr.city))
                {
                    case -1: curr = curr.right; break;
                    case 1: curr = curr.left; break;
                }
            }

            Console.WriteLine((curr == null) ? "NIE" : "TAK");
        }

        public void Insert(string city)
        {
            tree = addToBranch(new Element(city), tree);
        }

        public void Insert(Element city)
        {
            tree = addToBranch(city, tree);
        }

        private Element addToBranch(Element city, Element curr)
        {
            if (curr == null)
                return city;
            if (curr.city == city.city)
            {
                city.index = -2;
                return curr;
            }
            short compareStatus = CompareString(city.city, curr.city);
            switch (compareStatus)
            {
                case 1:
                    curr.left = addToBranch(city, curr.left);
                    break;
                case -1:
                    curr.right = addToBranch(city, curr.right);
                    break;
            }
            CountScale(curr);
            return CheckRotation(curr); ;
        }

        public void Delete(string city, ref int index)
        {
            if (tree == null)
            {
                return;
            }
            tree = DelateFromBranch(city, tree, ref index);
        }

        private Element DelateFromBranch(string city, Element curr, ref int index)
        {
            //side == true -lewa: 
            //side == false -prawa:
            if (curr == null)
                return null;
            Element copy = null;
            if (curr.city == city)
            {
                index = curr.index;
                copy = getNewRoot(curr, curr.city);
                if (copy == null) return copy;
                if (curr.left != null && copy.city != curr.left.city)
                    copy.left = curr.left;
                if (curr.right != null && copy.city != curr.right.city)
                    copy.right = curr.right;
                curr = copy;
            }
            else
            {
                short sideSearch = CompareString(city, curr.city);
                switch (sideSearch)
                {
                    case 1: curr.left = DelateFromBranch(city, curr.left, ref index); break;
                    case -1: curr.right = DelateFromBranch(city, curr.right, ref index); break;
                }
            }
            CountScale(curr);
            curr = CheckRotation(curr);
            return curr;
        }

        private Element getNewRoot(Element curr, string name)
        {
            Element newRoot = null;
            if (curr.right != null)
            {
                curr.right = searchNewRoot(curr.right, name,ref newRoot);
            }
            else if (curr.left != null)
            {
                curr.left = searchNewRoot(curr.left, name, ref newRoot);
            }
            return newRoot;
        }

        private Element searchNewRoot(Element curr, string name, ref Element newRoot)
        {
            if (curr == null)
                return null;
            int selectedSide = CompareString(name, curr.city);
            switch (selectedSide)
            {
                case 1:
                    curr.left = searchNewRoot(curr.left, name, ref newRoot);
                    if (curr.left == null && newRoot == null) { newRoot = curr; return null; }
                    /*if (elem != null && elem.left != null)
                    {
                        curr.left = elem.left;
                        elem.left = null;
                    }*/
                    break;
                case -1:
                    curr.right = searchNewRoot(curr.right, name, ref newRoot);
                    if (curr.right == null && newRoot == null) { newRoot = curr; return null; }

                    /*if (elem != null && elem.right != null)
                    {
                        curr.right = elem.right;
                        elem.right = null;
                    }*/
                    break;
            }
            if(newRoot.right != null)
            {
                curr.left = newRoot.right;
                newRoot.right = null;
            }
            if (newRoot.left != null)
            {
                curr.right = newRoot.left;
                newRoot.left = null;
            }
            CountScale(curr);
            curr = CheckRotation(curr);
            return curr;
        }

        private Element CheckRotation(Element child)
        {
            Element copy = child;
            if (child.scale == 2)
            {
                if (child.left.scale == 1)
                    copy = RR(child);
                else if (child.left.scale == -1)
                    copy = LR(child);
            }
            else if (child.scale == -2)
            {
                if (child.right.scale == -1)
                    copy = LL(child);
                else if (child.right.scale == 1)
                    copy = RL(child);
            }
            return copy;
        }

        private Element RR(Element curr)
        {
            Element copy = curr.left;
            curr.left = null;
            curr.left = copy.right;
            copy.right = curr;
            CountScale(curr);
            CountScale(copy);
            return copy;
        }

        private Element LL(Element curr)
        {
            Element copy = curr.right;
            curr.right = null;
            curr.right = copy.left;
            copy.left = curr;
            CountScale(curr);
            CountScale(copy);
            return copy;
        }

        private Element LR(Element curr)
        {
            curr.left = LL(curr.left);
            curr = RR(curr);
            return curr;
        }

        private Element RL(Element curr)
        {
            curr.right = RR(curr.right);
            curr = LL(curr);
            return curr;
        }

        private void CountScale(Element elem)
        {
            if (elem == null) return;
            int levelLeft = (elem.left == null) ? 0 : elem.left.level;
            int levelRight = (elem.right == null) ? 0 : elem.right.level;
            int heigherLevel = (levelLeft > levelRight) ? levelLeft : levelRight;
            elem.level = heigherLevel + 1;
            elem.scale = levelLeft - levelRight;
        }

        public void Print()
        {
            Print(tree, "-");
        }

        private void Print(Element curr, string prefix)
        {
            string text = prefix + ((curr == null) ? "NULL" : curr.city + " L:" + curr.level + " S:" + curr.scale + " I:" + curr.index);
            if (curr != null)
            {
                Print(curr.right, "|" + prefix);
                Console.WriteLine(text);
                Print(curr.left, "|" + prefix);
            }
            else Console.WriteLine(text);
        }

        /// <returns> 1 when is closer to char A; -1 - when is closer to Z</returns>
        private short CompareString(string newCity, string currCity)
        {
            currCity = currCity.ToLower();
            newCity = newCity.ToLower();
            try
            {
                for (short i = 0; i < newCity.Length; i++)
                {
                    if ((int)newCity[i] > (int)currCity[i])
                        return -1;
                    if ((int)newCity[i] < (int)currCity[i])
                        return 1;
                }
            }
            catch (Exception e)
            {
                if (newCity.Length > currCity.Length)
                    return -1;
                return 1;
            }
            return 1;
        }

        public int GetIndex(string city)
        {
            Element curr = tree;
            while (curr != null)
            {
                if (curr.city == city) return curr.index;
                switch (CompareString(city, curr.city))
                {
                    case -1: curr = curr.right; break;
                    case 1: curr = curr.left; break;
                }
            }
            return -1;
        }

    }
}
