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
        List<HeapElem> heap = new List<HeapElem>();
        Dictionary<string, TableElement> table = null;
        string prevCityPath = null;
        List<int> empty = new List<int>();
        bool[] visited;
        bool[] onHeap;
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
                heap = new List<HeapElem>();
                heap = InsertHeap(heap, start, 0);
                visited = new bool[array.Count];
                onHeap = new bool[array.Count];
                table = new Dictionary<string, TableElement>();
                table.Add(start, new TableElement());
                /*foreach (KeyValuePair<string, GElem> x in array)
                    table.Add(x.Key, new TableElement());
                */
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
            HeapElem smallestPathFromCity = null;
            string vertexFromHeap;
            int index = array[start].index;
            //0- isVisited, 1- is on heap
            int oldValNeighbour;
            int lengthToCurrCity;
            while (heap.Count > 0)
            {
                smallestPathFromCity = heap[0];
                heap = RemoveHeap(heap);
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
                    oldValNeighbour = table[x.Key].length;
                    index = array[x.Key].index;
                    if (lengthToCurrCity + x.Value < oldValNeighbour)
                    {
                        table[x.Key].length = lengthToCurrCity + x.Value;
                        table[x.Key].prevVertex = vertexFromHeap;

                        if (visited[index] == false)
                        {
                            if (oldValNeighbour != int.MaxValue)
                                heap = UpdateHeap(heap, x.Key, table[x.Key].length);
                            else
                                heap = InsertHeap(heap, x.Key, table[x.Key].length);
                        }
                    }
                }
                if (String.Compare(end, vertexFromHeap) == 0) break;
            }
        }


        public string VirtRoad(string start, string startNewRoad, string endNewRoad, int roadLength)
        {
            if (table == null)
                table = GenerateShortestPath(start);
            List<HeapElem> heap = new List<HeapElem>();//[x,0] -is visited
            //[x,1] -is on heap
            bool[,] isVisitedHeap = new bool[array.Count, 2];
            int cityShorterPath = 0;
            int oldValue = table[endNewRoad].length;
            int newValue = roadLength + table[startNewRoad].length;
            bool onHeap, isVisited;
            if (oldValue > newValue)
            {
                cityShorterPath++;
                heap = InsertHeap(heap, endNewRoad, newValue);
            }
            else if (table[startNewRoad].length > roadLength + table[endNewRoad].length)
            {
                cityShorterPath++;
                heap = InsertHeap(heap, startNewRoad, roadLength + table[endNewRoad].length);
            }
            else return "0";

            HeapElem el = null;
            while (heap.Count > 0 && cityShorterPath < 100)
            {
                el = heap[0];
                heap = RemoveHeap(heap);
                isVisitedHeap[array[el.vertex].index, 1] = false;
                isVisitedHeap[array[el.vertex].index, 0] = true;
                foreach (KeyValuePair<string, int> x in array[el.vertex].incList)
                {
                    oldValue = table[x.Key].length;
                    newValue = el.length + x.Value;
                    if (oldValue > newValue)
                    {
                        onHeap = isVisitedHeap[array[x.Key].index, 1];
                        isVisited = isVisitedHeap[array[x.Key].index, 0];
                        if (onHeap)
                        {
                            heap = UpdateHeap(heap, x.Key, el.length + x.Value);
                        }
                        else if (!isVisited)
                        {
                            cityShorterPath++;
                            heap = InsertHeap(heap, x.Key, newValue);
                            isVisitedHeap[array[x.Key].index, 1] = true;
                        }
                    }
                }
            }
            return ((cityShorterPath >= 100) ? "100+" : cityShorterPath.ToString());
        }

        private Dictionary<string, TableElement> GenerateShortestPath(string v)
        {
            List<HeapElem> heap = new List<HeapElem>();
            heap = InsertHeap(heap, v, 0);
            HeapElem el = null;
            bool[] visited = new bool[array.Count];
            bool[] onHeap = new bool[array.Count];
            string vertex;
            Dictionary<string, TableElement> table = new Dictionary<string, TableElement>();
            foreach (KeyValuePair<string, GElem> x in array)
                table.Add(x.Key, new TableElement());
            int index = array[v].index;
            table[v].length = 0;
            table[v].prevVertex = v;
            //0- isVisited, 1- is on heap
            int oldVal;
            while (heap.Count > 0)
            {
                el = heap[0];
                heap = RemoveHeap(heap);
                vertex = el.vertex;
                index = array[el.vertex].index;
                if (visited[index] == true) continue;
                visited[index] = true;
                foreach (KeyValuePair<string, int> x in array[vertex].incList)
                {
                    oldVal = table[x.Key].length;
                    index = array[x.Key].index;
                    if (table[vertex].length + x.Value < oldVal)
                    {
                        table[x.Key].length = table[vertex].length + x.Value;
                        table[x.Key].prevVertex = vertex;

                        if (onHeap[index])
                        {
                            heap = UpdateHeap(heap, x.Key, table[x.Key].length);
                        }
                        else
                        if (visited[index] == false)
                        {
                            heap = InsertHeap(heap, x.Key, table[x.Key].length);
                        }
                    }
                }
            }
            return table;
        }

    }
}
