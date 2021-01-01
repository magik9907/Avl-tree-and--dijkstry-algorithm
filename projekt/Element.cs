using System;
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
        public int index = -1;
        public Element(string cityName)
        {
            city = cityName;
        }
    }
}
