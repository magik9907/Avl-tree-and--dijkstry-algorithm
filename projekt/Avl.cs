using System;
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
            return (tree != null) ? PrefixCounter(prefix, tree, new Regex(@"^(" + prefix + @")\w*")) : 0;
        }

        // Regexp /^(pre)\w*/gi
        private int PrefixCounter(string pre, Element curr, Regex regex)
        {
            if (curr == null) return 0;

            int counter = 0;
            if (regex.IsMatch(curr.city))
            {
                counter++;
                counter += PrefixCounter(pre, curr.right, regex);
                counter += PrefixCounter(pre, curr.left, regex);
            }
            else
            {
                switch (CompareString(curr.city, pre))
                {
                    case 1:
                        counter = PrefixCounter(pre, curr.left, regex);
                        break;
                    case -1:
                        counter = PrefixCounter(pre, curr.right, regex);
                        break;
                }
            }
            return counter;
        }

        public void IsCity(string city)
        {
            Element curr = tree;
            while (curr != null)
            {
                switch (CompareString(curr.city, city))
                {
                    case -1: curr = curr.right; break;
                    case 1: curr = curr.left; break;
                    case 0: Console.WriteLine("Tak"); return;
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
            switch (CompareString(curr.city, city.city))
            {
                case 1:
                    curr.left = addToBranch(city, curr.left);
                    break;
                case -1:
                    curr.right = addToBranch(city, curr.right);
                    break;
                case 0:
                    return curr;
            }
            CountScale(curr);
            return CheckRotation(curr); ;
        }

        public void Delete(string city)
        {
            if (tree == null)
            {
                return;
            }
            tree = DelateFromBranch(city, tree);
        }

        private Element DelateFromBranch(string city, Element curr)
        {
            //side == true -lewa: 
            //side == false -prawa:
            if (curr == null)
                return null;
            Element copy = null;
            switch (CompareString(curr.city, city))
            {
                case 0:
                    copy = getNewRoot(curr, curr.city);
                    if (copy == null) return copy;
                    if (curr.left != null && copy.city != curr.left.city)
                        copy.left = curr.left;
                    if (curr.right != null && copy.city != curr.right.city)
                        copy.right = curr.right;
                    curr = copy;
                    break;
                case 1: curr.left = DelateFromBranch(city, curr.left); break;
                case -1: curr.right = DelateFromBranch(city, curr.right); break;

            }
            CountScale(curr);
            return CheckRotation(curr);

        }

        private Element getNewRoot(Element curr, string name)
        {
            Element newRoot = null;
            if (curr.right != null)
            {
                curr.right = searchNewRoot(curr.right, name, ref newRoot);
            }
            else if (curr.left != null)
            {
                curr.left = searchNewRoot(curr.left, name, ref newRoot);
            }
            return newRoot;
        }

        private Element searchNewRoot(Element curr, string name, ref Element newRoot)
        {
            switch (CompareString(curr.city, name))
            {
                case 1:
                    if (curr.left != null)
                        curr.left = searchNewRoot(curr.left, name, ref newRoot);
                    if (curr.left == null && newRoot == null) { newRoot = curr; return null; }
                    break;
                case -1:
                    if (curr.right != null)
                        curr.right = searchNewRoot(curr.right, name, ref newRoot);
                    if (curr.right == null && newRoot == null) { newRoot = curr; return null; }
                    break;
            }
            if (newRoot.right != null && newRoot.right != null)
            {
                curr.left = newRoot.right;
                newRoot.right = null;
            }
            if (newRoot.left != null && newRoot.left != null)
            {
                curr.right = newRoot.left;
                newRoot.left = null;
            }
            CountScale(curr);
            return CheckRotation(curr);

        }

        private Element CheckRotation(Element child)
        {
            Element copy = child;
            if (child.scale == 2)
            {
                if (child.left.scale == 1 || child.left.scale == 0 )
                    copy = RR(child);
                else if (child.left.scale == -1)
                    copy = LR(child);
            }
            else if (child.scale == -2 )
            {
                if (child.right.scale == -1 || child.right.scale == 0)
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
            if (curr == null)
                return;
            string text = prefix + curr.city;
            if (curr != null)
            {
                Print(curr.right, "|" + prefix);
                Console.WriteLine(text);
                Print(curr.left, "|" + prefix);
            }
            else Console.WriteLine(text);
        }

        /// <returns> 1 when is closer to char A; -1 - when is closer to Z</returns>
        private int CompareString(string cityOne, string cityTwo)
        {
            return String.Compare(cityOne, cityTwo);
        }

        public bool GetIndex(string city)
        {
            Element curr = tree;
            while (curr != null)
            {
                switch (CompareString(curr.city, city))
                {
                    case -1: curr = curr.right; break;
                    case 1: curr = curr.left; break;
                    case 0: return true;
                }
            }
            return false;
        }

    }
}
