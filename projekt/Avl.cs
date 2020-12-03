﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    public class Element
    {
        public string city;
        public int scale;
        public Element left = null;
        public Element right = null;
        public int level = 1;
        public Element(string cityName)
        {
            city = cityName;
        }
    }

    public class Avl
    {
        public Element tree = null;
        /// <returns>0 - when is the same; 1 when is closer to char A; -1 - when is closer to Z</returns>
        private short CompareString(string newCity, string currCity)
        {
            currCity = currCity.ToLower();
            newCity = newCity.ToLower();
            for (short i = 0; i < newCity.Length; i++)
            {
                if ((int)newCity[i] > (int)currCity[i])
                    return -1;
                if ((int)newCity[i] < (int)currCity[i])
                    return 1;

            }
            return 1;
        }

        public void add(string city)
        {
            if (tree == null)
            {
                tree = new Element(city);
                return;
            }
            addToBranch(city, tree, null, false);
        }

        public void addToBranch(string city, Element curr, Element parent, bool side)
        {
            short compareStatus = CompareString(city, curr.city);
            short sideAssign = 0;
            switch (compareStatus)
            {
                case 1:
                    if (curr.left == null)
                    {
                        curr.left = new Element(city);
                        city = null;
                    }
                    else
                        addToBranch(city, curr.left, curr, true);
                    sideAssign = 1;
                    break;
                case -1:
                    if (curr.right == null)
                    {
                        curr.right = new Element(city);
                        city = null;
                    }
                    else
                        addToBranch(city, curr.right, curr, false);
                    sideAssign = -1;
                    break;
            }
            CountScale(curr);
            if (city != null)
            {
                if (parent == null)
                    tree = CheckRotation(parent, curr);
                else
                    switch (side)
                    {
                        case true:
                            parent.left = CheckRotation(curr, curr);
                            break;
                        case false:
                            parent.right = CheckRotation(curr, curr);
                            break;
                    }
            }
        }

        private Element CheckRotation(Element parent, Element child)
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
            string text = prefix + ((curr == null) ? "NULL" : curr.city + " L:" + curr.level + " S:" + curr.scale);
            Console.WriteLine(text);
            if (curr != null)
            {
                Print(curr.right, "|" + prefix);
                Print(curr.left, "|" + prefix);
            }
        }

    }
}
