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
        List<HeapElem> heap = new List<HeapElem>();
        TableElement[] table = null;
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
            int valPos = heap.Count;

            tempPos = valPos / 2;
            while (valPos > 1 && heap[tempPos - 1].length > val)
            {
                temp = heap[valPos - 1];
                heap.RemoveAt(valPos - 1);
                heap.Insert(tempPos - 1, temp);
                temp = heap[tempPos];
                heap.RemoveAt(tempPos);
                heap.Insert(valPos - 1, temp);
                valPos = tempPos;
                tempPos = valPos / 2;
            }
            return heap;
        }

        private List<HeapElem> RemoveHeap(List<HeapElem> heap)
        {
            HeapElem temp;
            heap.RemoveAt(0);
            if (heap.Count == 0)
                return heap;
            int tempPos;
            int pos;
            temp = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            int len = temp.length;
            heap.Insert(0, temp);
            tempPos = 1;
            try
            {
                while ((heap[tempPos * 2 - 1] != null && len > heap[tempPos * 2 - 1].length)
                    || (heap[tempPos * 2 + 1 - 1] != null && len > heap[tempPos * 2 + 1 - 1].length))
                {
                    pos = tempPos;
                    switch (heap[tempPos * 2 - 1] != null
                        && len > heap[tempPos * 2 - 1].length
                        && heap[tempPos * 2 - 1].length < heap[tempPos * 2 + 1 - 1].length)
                    {
                        case true: //position 2*index lewo
                            tempPos = tempPos * 2;
                            break;
                        case false://position 2*index +1 prawo
                            tempPos = tempPos * 2 + 1;
                            break;
                    }
                    temp = heap[tempPos - 1];
                    heap.RemoveAt(tempPos - 1);
                    heap.Insert(pos - 1, temp);
                    temp = heap[pos];
                    heap.RemoveAt(pos);
                    heap.Insert(tempPos - 1, temp);
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
            }
            return heap;
        }

        private List<HeapElem> UpdateHeap(List<HeapElem> heap, int index, int newVal)
        {
            int i,j =heap.Count;
            for (i = 1; i <= heap.Count; i++)
            {
                if (heap[i - 1].vertex == index)
                {
                    heap[i - 1].length = newVal;
                    break;
                }
                if (heap[j - i].vertex == index)
                {
                    heap[j - i].length = newVal;
                    i = j;
                    break;
                }
                if (j < i) return heap;
            }
            if (i > heap.Count) return heap;
            HeapElem temp;
            int newPos = i / 2;
            while (i > 1 && newVal < heap[newPos - 1].length)
            {
                temp = heap[i - 1];
                heap.RemoveAt(i - 1);
                heap.Insert(newPos - 1, temp);
                temp = heap[newPos];
                heap.RemoveAt(newPos);
                heap.Insert(i - 1, temp);
                i = newPos;
                newPos /= 2;
            }
            return heap;
        }

        public string FindRoad(int v, int end)
        {
            List<HeapElem> heap = new List<HeapElem>();

            heap = InsertHeap(heap, v, 0);

            HeapElem el = null;
            bool[,] visitedHeap = new bool[array.Count, 2];
            int vertex;
            TableElement[] table = new TableElement[array.Count];
            table[v] = new TableElement();
            table[v].length = 0;
            table[v].prevVertex = v;
            visitedHeap[v, 1] = true;
            int oldVal;
            while (heap.Count > 0)
            {
                el = heap[0];
                heap = RemoveHeap(heap);
                vertex = el.vertex;
                visitedHeap[vertex, 1] = false;
                if (vertex == end) break;
                if (visitedHeap[vertex, 0] == true) continue;
                visitedHeap[vertex, 0] = true;
                foreach (var x in array[vertex].incList)
                {
                    if (table[x.index] == null)
                        table[x.index] = new TableElement();
                    oldVal = table[x.index].length;

                    if (table[vertex].length + x.length <= table[x.index].length)
                    {
                        table[x.index].length = table[vertex].length + x.length;
                        table[x.index].prevVertex = vertex;
                        if (visitedHeap[x.index, 1])
                            heap = UpdateHeap(heap, x.index, table[x.index].length);
                    }

                    if (!visitedHeap[x.index, 0])
                    {
                        heap = InsertHeap(heap, x.index, table[x.index].length);
                        visitedHeap[x.index, 1] = true;
                    }
                }

            }
            if (table[end] == null) return "NIE";
            int prev = end;
            string line = array[prev].city;
            return table[end].length.ToString();
            do
            {
                line = array[table[prev].prevVertex].city + "-" + line;
                prev = table[prev].prevVertex;
            }
            while (prev != v && prev != -1);
            line = table[end].length + ": " + line;
            return line;
        }

        public string VirtRoad(int start, int startNewRoad, int endNewRoad, int roadLength)
        {
            if (table == null)
                table = GenerateShortestPath(start);
            List<HeapElem> heap = new List<HeapElem>();//[x,0] -is visited
            //[x,1] -is on heap
            bool[,] isVisitedHeap = new bool[array.Count, 2];
            int cityShorterPath = 0;
            if (table[endNewRoad].length > roadLength + table[startNewRoad].length)
            {
                cityShorterPath++;
                heap = InsertHeap(heap, endNewRoad, roadLength + table[startNewRoad].length);
            }
            else if (table[startNewRoad].length > roadLength + table[endNewRoad].length)
            {
                cityShorterPath++;
                heap = InsertHeap(heap, startNewRoad, roadLength + table[endNewRoad].length);
            }
            HeapElem el = null;
            while (heap.Count > 0 && cityShorterPath < 100)
            {
                el = heap[0];
                heap = RemoveHeap(heap);
                isVisitedHeap[el.vertex, 1] = false;
                isVisitedHeap[el.vertex, 0] = true;
                foreach (var x in array[el.vertex].incList)
                {
                    if (table[x.index].length > el.length + x.length)
                    {
                        if (isVisitedHeap[x.index, 1])
                        {
                            heap = UpdateHeap(heap, x.index, el.length + x.length);
                        }
                        else
                        {
                            if (!isVisitedHeap[x.index, 0])
                            {
                                cityShorterPath++;
                                heap = InsertHeap(heap, x.index, el.length + x.length);
                                isVisitedHeap[x.index, 1] = true;
                            }
                        }
                    }
                }
            }
            return (cityShorterPath >= 100) ? "100+" : cityShorterPath.ToString();
        }

        private TableElement[] GenerateShortestPath(int v)
        {
            List<HeapElem> heap = new List<HeapElem>();

            heap = InsertHeap(heap, v, 0);

            HeapElem el = null;
            bool[,] visitedHeap = new bool[array.Count, 2];
            int vertex;
            TableElement[] table = new TableElement[array.Count];
            table[v] = new TableElement();
            table[v].length = 0;
            table[v].prevVertex = v;
            visitedHeap[v, 1] = true;
            int oldVal;
            while (heap.Count > 0)
            {
                el = heap[0];
                heap = RemoveHeap(heap);
                vertex = el.vertex;
                visitedHeap[vertex, 1] = false;
                if (visitedHeap[vertex, 0] == true) continue;
                visitedHeap[vertex, 0] = true;
                foreach (var x in array[vertex].incList)
                {
                    if (table[x.index] == null)
                        table[x.index] = new TableElement();
                    oldVal = table[x.index].length;

                    if (table[vertex].length + x.length <= table[x.index].length)
                    {
                        table[x.index].length = table[vertex].length + x.length;
                        table[x.index].prevVertex = vertex;
                        if (visitedHeap[x.index, 1])
                            heap = UpdateHeap(heap, x.index, table[x.index].length);
                    }
                    if (visitedHeap[x.index, 0] == false)
                    {
                        heap = InsertHeap(heap, x.index, table[x.index].length);
                        visitedHeap[x.index, 1] = true;
                    }
                }
            }
            return table;
        }
    }
}
