using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    class Graf
    {
        // przechowuje puste miejsca w tablicy array
        List<int> empty = new List<int>();
        //przechowuje liste miast sasiadujacych i dlugosc drogi (id z drzewa avl)
        List<GElem> array = new List<GElem>();
        private class GElem
        {
            // przechowuje liste sasiadow
            public string city;
            public List<NeigbourVertex> incList = new List<NeigbourVertex>();
            public GElem(string name)
            {
                city = name;
            }
        }

        class NeigbourVertex
        {
            public int index = 0;
            public int length = 0;

            public NeigbourVertex(int i, int l)
            {
                index = i;
                length = l;
            }
        }

        class TableElement
        {
            public int prevVertex = -1;
            public int length = int.MaxValue;
        }

        class HeapElem
        {
            public int length;
            public int vertex;

            public HeapElem(int d, int v)
            {
                this.length = d;
                this.vertex = v;
            }
        }

        public void Insert(Element city)
        {
            if (empty.Count == 0)
            {
                array.Add(new GElem(city.city));
                city.index = array.Count() - 1;
            }
            else
            {
                array[empty[0]] = new GElem(city.city);
                city.index = empty[0];
                empty.RemoveAt(0);
            }
        }

        public void Delete(int index)
        {
            GElem node = array[index];
            array[index] = null;
            empty.Add(index);
        }

        void AddRoad(GElem e, int j, int scale)
        {
            foreach (var x in e.incList)
            {
                if (x.index == j)
                {
                    x.length = scale;
                    return;
                }
            }
            e.incList.Add(new NeigbourVertex(j, scale));
        }

        public void AddRoad(int indexOne, int indexTwo, int length)
        {
            AddRoad(array.ElementAt(indexOne), indexTwo, length);
            AddRoad(array.ElementAt(indexTwo), indexOne, length);
        }

        void RemoveRoad(GElem e, int j)
        {
            List<NeigbourVertex> incList = e.incList;
            for (int i = 0; i < incList.Count; i++)
            {
                if (incList[i].index == j) { incList.RemoveAt(i); return; }
            }
        }

        public void RemoveRoad(int indexOne, int indexTwo)
        {
            RemoveRoad(array.ElementAt(indexOne), indexTwo);
            RemoveRoad(array.ElementAt(indexTwo), indexOne);
        }

        private List<HeapElem> InsertHeap(List<HeapElem> heap, int vertex, int val)
        {
            HeapElem temp;
            int tempPos;
            heap.Add(new HeapElem(val, vertex));
            int valPos = heap.Count - 1;

            tempPos = valPos / 2;
            while (valPos > 0 && heap[tempPos].length > val)
            {
                temp = heap[tempPos];
                heap.RemoveAt(tempPos);
                heap.Add(temp);
                temp = heap[valPos - 1];
                heap.RemoveAt(valPos - 1);
                heap.Insert(tempPos, temp);
                valPos = tempPos;
                tempPos = valPos / 2;
            }
            return heap;
        }

        private List<HeapElem> RemoveHeap(List<HeapElem> heap)
        {
            HeapElem temp, tempNew;
            int tempPos = heap.Count;
            int pos;
            temp = heap[tempPos - 1];
            int len = temp.length;
            heap.RemoveAt(tempPos - 1);
            if (heap.Count == 0)
                return heap;
            heap.RemoveAt(0);
            heap.Insert(0, temp);
            tempPos = 1;
            try
            {
                while ((len > heap[2 * tempPos - 1].length) && len > heap[2 * tempPos].length)
                {
                    if (heap[2 * tempPos - 1].length > heap[2 * tempPos].length)
                    {
                        pos = 2 * tempPos + 1;
                    }
                    else
                    {
                        pos = 2 * tempPos;
                    }
                    tempNew = heap[pos - 1];
                    heap.RemoveAt(pos - 1);
                    heap.Insert(tempPos - 1, tempNew);
                    tempPos = pos;
                }
            }
            catch (Exception e)
            {
                if (heap.Count == 2 * tempPos)
                {
                    pos = 2 * tempPos;
                    tempNew = heap[pos - 1];
                    heap.RemoveAt(pos - 1);
                    heap.Insert(tempPos - 1, tempNew);
                    tempPos = pos;
                }
            }
            return heap;
        }

        public string FindRoad(int v, int end)
        {
            List<HeapElem> heap = new List<HeapElem>();
            heap = InsertHeap(heap, v, 0);

            HeapElem el;
            bool[] visited = new bool[array.Count];
            int vertex;
            TableElement[] table = new TableElement[array.Count];
            table[v] = new TableElement();
            table[v].length = 0;
            table[v].prevVertex = v;
            while (heap.Count > 0)
            {
                el = heap[0];
                heap = RemoveHeap(heap);
                vertex = el.vertex;
                if (visited[vertex] == true) continue;

                visited[vertex] = true;
                foreach (var x in array[vertex].incList)
                {
                    if (table[x.index] == null)
                        table[x.index] = new TableElement();
                    if (table[vertex].length + x.length <= table[x.index].length)
                    {
                        table[x.index].length = table[vertex].length + x.length;
                        table[x.index].prevVertex = vertex;
                    }
                    if (visited[x.index] == false)
                        heap = InsertHeap(heap, x.index, table[x.index].length);
                }
            }
            if (table[end] == null) return "NIE";
            int prev = end;
            string line = array[prev].city;

            int sum = 0;

            do
            {
                foreach (var x in array[prev].incList)
                {
                    if (x.index == table[prev].prevVertex)
                    {
                        sum += x.length;
                    }
                }



                line = array[table[prev].prevVertex].city + "-" + line;
                prev = table[prev].prevVertex;
            }
            while (prev != v && prev != -1);
            line = table[end].length + " " + sum.ToString() + ": ";
            return line;
            return table[end].length.ToString();
        }

    }
}
