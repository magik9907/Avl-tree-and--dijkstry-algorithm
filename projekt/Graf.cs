using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    class Graf
    {
        //przechowuje liste miast sasiadujacych i dlugosc drogi (id z drzewa avl)
        Dictionary<string, GElem> array = new Dictionary<string, GElem>();
        //List<HeapElem> heap = new List<HeapElem>();
        SortedSet<HeapElem> heap = null;
        Dictionary<string, TableElement> table = null;
        string prevCityPath = null;
        List<int> empty = new List<int>();
        bool[] visited;
        private class GElem
        {
            public string city;
            // przechowuje liste sasiadow
            public Dictionary<string, int> incList = new Dictionary<string, int>();
            public int index = -1;
            public GElem(string name)
            {
                city = name;
            }
        }

        class NeigbourVertex
        {
            public string city;
            public int length = 0;

            public NeigbourVertex(string i, int l)
            {
                city = i;
                length = l;
            }
        }

        class TableElement
        {
            public string prevVertex = "";
            public int length = int.MaxValue;
        }

        class HeapElem
        {
            public int length;
            public string vertex;

            public HeapElem(int val, string vertex)
            {
                this.length = val;
                this.vertex = vertex;
            }
        }

        class ByLength : IComparer<HeapElem>
        {
            public int Compare(HeapElem x, HeapElem y)
            {
                if (x.length > y.length ) return 1;
                if (x.length < y.length ) return -1;
                if (x.vertex != y.vertex) return -1;
                if (x.vertex == y.vertex) return 0;
                return 0;
            }
        }

        public void Insert(string city)
        {
            if (!array.ContainsKey(city))
            {
                array.Add(city, new GElem(city));
                if (empty.Count == 0)
                    array[city].index = array.Count - 1;
                else
                {
                    array[city].index = empty[0];
                    empty.RemoveAt(0);
                }
            }
        }

        public void Delete(string index)
        {
            if (!array.ContainsKey(index)) return;
            Dictionary<string, int> incList = array[index].incList;
            empty.Add(array[index].index);
            array.Remove(index);
            foreach (KeyValuePair<string, int> x in incList)
            {
                array[x.Key].incList.Remove(index);
            }
        }

        void AddRoad(GElem e, string j, int scale)
        {
            if (!e.incList.ContainsKey(j))
                e.incList.Add(j, scale);
        }

        public void AddRoad(string indexOne, string indexTwo, int length)
        {
            AddRoad(array[indexOne], indexTwo, length);
            AddRoad(array[indexTwo], indexOne, length);
        }

        void RemoveRoad(GElem e, string j)
        {
            Dictionary<string, int> incList = e.incList;
            incList.Remove(j);
        }

        public void RemoveRoad(string indexOne, string indexTwo)
        {
            RemoveRoad(array[indexOne], indexTwo);
            RemoveRoad(array[indexTwo], indexOne);
        }

        private List<HeapElem> InsertHeap(List<HeapElem> heap, string vertex, int val)
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
                if (tempPos * 2 == heap.Count)
                {
                    pos = tempPos;
                    if (len > heap[tempPos * 2 - 1].length)
                    {
                        tempPos = tempPos * 2;
                        temp = heap[tempPos - 1];
                        heap.RemoveAt(tempPos - 1);
                        heap.Insert(pos - 1, temp);
                        temp = heap[pos];
                        heap.RemoveAt(pos);
                        heap.Insert(tempPos - 1, temp);
                    }
                }
            }
            return heap;
        }

        private List<HeapElem> UpdateHeap(List<HeapElem> heap, string index, int newVal)
        {
            int i, j = heap.Count;
            for (i = 1; i <= heap.Count; i++)
            {
                if (heap[i - 1].vertex == index)
                {
                    if (heap[i - 1].length > newVal)
                        heap[i - 1].length = newVal;
                    else
                        return heap;
                    break;
                }
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

        public string FindRoad(string start, string end)
        {
            if (prevCityPath != start)
            {
                prevCityPath = start;
                heap = new SortedSet<HeapElem>(new ByLength());
                // heap = InsertHeap(heap, start, 0);
                heap.Add(new HeapElem(0, start));
                visited = new bool[array.Count];
                table = new Dictionary<string, TableElement>();
                table.Add(start, new TableElement());
                table[start].length = 0;
                table[start].prevVertex = start;
                Dijkstry(start, end);
            }
            else
            {
                if (!visited[array[end].index])
                {
                    Dijkstry(start, end);
                }
            }

            if (!table.ContainsKey(end) || table[end].length == int.MaxValue) return "NIE";
            return table[end].length.ToString();
            string prev = end;
            string line = array[prev].city;
            return table[end].length.ToString();
            do
            {
                line = array[table[prev].prevVertex].city + "-" + line;
                prev = table[prev].prevVertex;
            }
            while (prev != start);
            line = table[end].length + ": " + line;
            return line;
        }

        private void Dijkstry(string start, string end)
        {
            HeapElem smallestPathFromCity = null, p, t;
            string vertexFromHeap, key;
            int index = array[start].index, oldValNeighbour, lengthToCurrCity, sum;
            while (heap.Count > 0)
            {
                smallestPathFromCity = heap.First();
                //heap = RemoveHeap(heap);
                heap.Remove(smallestPathFromCity);
                vertexFromHeap = smallestPathFromCity.vertex;
                index = array[smallestPathFromCity.vertex].index;
                if (visited[index] == true) continue;
                lengthToCurrCity = smallestPathFromCity.length;
                visited[index] = true;

                foreach (KeyValuePair<string, int> x in array[vertexFromHeap].incList)
                {
                    if (!table.ContainsKey(x.Key))
                    {
                        table.Add(x.Key, new TableElement());
                    }
                    key = x.Key;
                    oldValNeighbour = table[key].length;
                    index = array[key].index;
                    sum = lengthToCurrCity + x.Value;
                    if (sum < oldValNeighbour)
                    {
                        table[key].length = sum;
                        table[key].prevVertex = vertexFromHeap;

                        if (visited[index] == false)
                        {
                            t = new HeapElem(oldValNeighbour, key);
                            p = new HeapElem(sum, key);
                            if (heap.TryGetValue(t, out t))
                            {
                                heap.Remove(t);
                                heap.Add(p);
                            }
                            else
                                heap.Add(p);
                            /* if (oldValNeighbour != int.MaxValue)
                                 heap = UpdateHeap(heap, x.Key, table[x.Key].length);
                             else
                                 heap = InsertHeap(heap, x.Key, table[x.Key].length);*/
                        }
                    }
                }
                if (String.Compare(end, vertexFromHeap) == 0) break;
            }
        }


        public string VirtRoad(string start, string startNewRoad, string endNewRoad, int roadLength)
        {
            if (prevCityPath != start || table == null)
            {
                prevCityPath = start;
               // heap = new List<HeapElem>();
                heap = new SortedSet<HeapElem>(new ByLength());
               // heap = InsertHeap(heap, start, 0);
                visited = new bool[array.Count];
                heap.Add(new HeapElem(0,start));
                table = new Dictionary<string, TableElement>();
                table.Add(start, new TableElement());
                table[start].length = 0;
                table[start].prevVertex = start;
                Dijkstry(start, startNewRoad);
                if (!visited[array[endNewRoad].index])
                {
                    Dijkstry(start, endNewRoad);
                }
            }
            else
            {
                if (!visited[array[startNewRoad].index])
                {
                    Dijkstry(start, startNewRoad);
                }
                if (!visited[array[endNewRoad].index])
                {
                    Dijkstry(start, endNewRoad);
                }

            }
            SortedSet<HeapElem> heapNR = new SortedSet<HeapElem>(new ByLength());
            //[x,1] -is on heap
            bool[,] isVisitedHeap = new bool[array.Count, 2];
            int cityShortestPath = 0;
            int oldValue = table[endNewRoad].length;
            int newValue = roadLength + table[startNewRoad].length;
            bool onHeap, isVisited;
            HeapElem t, p;
            if (oldValue > newValue)
            {
                cityShortestPath++;
                heapNR.Add(new HeapElem(newValue, endNewRoad));
              //  heapNR = InsertHeap(heapNR, endNewRoad, newValue);
            }
            else if (table[startNewRoad].length > roadLength + table[endNewRoad].length)
            {
                cityShortestPath++;
                heapNR.Add(new HeapElem(roadLength + table[endNewRoad].length, startNewRoad));
                //heapNR = InsertHeap(heapNR, startNewRoad, roadLength + table[endNewRoad].length);
            }
            else return "0";

            HeapElem el = null;
            while (heapNR.Count > 0 && cityShortestPath < 100)
            {
                el = heapNR.First();
                heapNR.Remove(el);
                //heapNR = RemoveHeap(heapNR);
                isVisitedHeap[array[el.vertex].index, 1] = false;
                isVisitedHeap[array[el.vertex].index, 0] = true;
                foreach (KeyValuePair<string, int> x in array[el.vertex].incList)
                {
                    if (!visited[array[x.Key].index])
                    {
                        Dijkstry(start, x.Key);
                    }
                    oldValue = table[x.Key].length;
                    newValue = el.length + x.Value;
                    if (oldValue > newValue)
                    {
                        onHeap = isVisitedHeap[array[x.Key].index, 1];
                        isVisited = isVisitedHeap[array[x.Key].index, 0];
                        if (onHeap)
                        {
                            heapNR.Remove(new HeapElem(oldValue, x.Key));
                            heapNR.Add(new HeapElem(newValue, x.Key));

                            //heapNR = UpdateHeap(heapNR, x.Key, el.length + x.Value);
                        }
                        else if (!isVisited)
                        {
                            cityShortestPath++;
                            heapNR.Add(new HeapElem(newValue, x.Key));
                            //heapNR = InsertHeap(heapNR, x.Key, newValue);
                            isVisitedHeap[array[x.Key].index, 1] = true;
                        }
                    }
                }
            }
            return ((cityShortestPath >= 100) ? "100+" : cityShortestPath.ToString());
        }


    }
}
