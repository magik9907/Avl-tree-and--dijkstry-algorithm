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
        //przechowuje liste miast (id w drzewie avl)
        List<GElem> array = new List<GElem>();
        private class GElem
        {
            // przechowuje liste sasiadow
            public List<int> incList = new List<int>();
        }
        List<List<int>> edges = new List<List<int>>();

        class TableElement
        {
            public int prevVertex = -1;
            public int length = -1;
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

        private void SetRoads(int i)
        {
            try
            {
                if (edges[i] == null)
                    edges.Add(new List<int>());
            }
            catch (ArgumentOutOfRangeException)
            {
                edges.Add(new List<int>());
            }
            for (int j = 0; j <= i; j++)
            {
                try
                {
                    edges[i][j] = 0;
                }
                catch (ArgumentOutOfRangeException)
                {
                    edges[i].Add(0);
                }

                try
                {
                    edges[j][i] = 0;
                }
                catch (ArgumentOutOfRangeException)
                {
                    edges[j].Add(0);
                }
            }
        }

        public void Insert(Element city)
        {
            if (empty.Count == 0)
            {
                array.Add(new GElem());
                city.index = array.Count() - 1;
                SetRoads(city.index);
            }
            else
            {
                array[empty[0]] = new GElem();
                city.index = empty[0];
                empty.RemoveAt(0);
            }
        }

        public void Delete(int index)
        {
            GElem node = array[index];
            array[index] = null;
            empty.Add(index);
            SetRoads(index);
        }

        void AddRoad(GElem e, int i, int j, int scale)
        {
            e.incList.Add(j);
            edges[i][j] = scale;
            edges[j][i] = scale;
        }

        public void AddRoad(int indexOne, int indexTwo, int length)
        {
            AddRoad(array.ElementAt(indexOne), indexOne, indexTwo, length);
            AddRoad(array.ElementAt(indexTwo), indexTwo, indexOne, length);
        }

        void RemoveRoad(GElem e, int i, int j)
        {
            edges[i][j] = 0;
            edges[j][i] = 0;
        }

        public void RemoveRoad(int indexOne, int indexTwo)
        {
            RemoveRoad(array.ElementAt(indexOne), indexOne, indexTwo);
            RemoveRoad(array.ElementAt(indexTwo), indexTwo, indexOne);
        }

        private List<HeapElem> InsertHeap(List<HeapElem> heap, int val, int vertex)
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

        public int FindRoad(int v, int end)
        {
            List<HeapElem> heap = new List<HeapElem>();
            heap = InsertHeap(heap, 0, v);
            /*heap = InsertHeap(heap, 5, 1);
            heap = InsertHeap(heap, 2, 1);
            heap = InsertHeap(heap, 8, 1);
            heap = InsertHeap(heap, 4, 1);
            heap = InsertHeap(heap, 6, 1);
            heap = RemoveHeap(heap);*/
            HeapElem el;
            bool[] visited = new bool[array.Count];
            int vertex;
            TableElement[] table = new TableElement[array.Count];
            for (int x = 0; x < table.Length; x++)
                table[x] = new TableElement();
            table[v].length = 0;
            table[v].prevVertex = v;
            while (heap.Count > 0)
            {
                el = heap[0];
                heap = RemoveHeap(heap);
                vertex = el.vertex;
                if (visited[vertex] == true) continue;

                Console.WriteLine(vertex);
                visited[vertex] = true;
                foreach (var x in array[vertex].incList)
                {
                    if (table[vertex].length + edges[vertex][x] <= table[x].length || table[x].length == -1)
                    {
                        table[x].length = table[vertex].length + edges[vertex][x];
                        table[x].prevVertex = vertex;
                    }
                    if (visited[x] == false)
                        heap = InsertHeap(heap, table[x].length, x);
                }
            }

            Console.WriteLine("Najkrosza droga " + table[end].length);
            return 0;
        }

    }
}
