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
        SortedSet<HeapElem> heap = null;
        Dictionary<string, TableElement> table = null;
        string prevCityPath = null;
        Dictionary<string, bool> visited = null;

        private class GElem
        {
            public string city;
            // przechowuje liste sasiadow
            public Dictionary<string, int> incList = new Dictionary<string, int>();
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
                if (x.length > y.length) return 1;
                if (x.length < y.length) return -1;
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
            }
        }

        public void Delete(string index)
        {
            if (!array.ContainsKey(index)) return;
            Dictionary<string, int> incList = array[index].incList;
            array.Remove(index);
            foreach (KeyValuePair<string, int> x in incList)
            {
                array[x.Key].incList.Remove(index);
            }
        }

        void AddRoad(GElem e, string j, int scale)
        {
            if (!e.incList.ContainsKey(j))
            {
                e.incList.Add(j, scale);
            }
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

        public string FindRoad(string start, string end)
        {
            if (prevCityPath != start)
            {
                prevCityPath = start;
                heap = new SortedSet<HeapElem>(new ByLength());
                heap.Add(new HeapElem(0, start));
                visited = new Dictionary<string, bool>();
                visited.Add(start, false);
                table = new Dictionary<string, TableElement>();
                table.Add(start, new TableElement());
                table[start].length = 0;
                table[start].prevVertex = start;
                Dijkstry(start, end);
            }
            else
            {
                if (visited.ContainsKey(end) && !visited[end])
                {
                    Dijkstry(start, end);
                }
            }

            if (!table.ContainsKey(end) || table[end].length == int.MaxValue) return "NIE";
            //return table[end].length.ToString();
            string prev = end;
            string line = array[prev].city;
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
            int oldValNeighbour, lengthToCurrCity, sum;
            while (heap.Count > 0)
            {
                smallestPathFromCity = heap.First();
                //heap = RemoveHeap(heap);
                heap.Remove(smallestPathFromCity);
                vertexFromHeap = smallestPathFromCity.vertex;
                if (visited[vertexFromHeap] == true) continue;
                lengthToCurrCity = smallestPathFromCity.length;
                visited[vertexFromHeap] = true;

                foreach (KeyValuePair<string, int> x in array[vertexFromHeap].incList)
                {
                    if (!table.ContainsKey(x.Key))
                    {
                        table.Add(x.Key, new TableElement());
                        visited.Add(x.Key, false);
                    }
                    key = x.Key;
                    oldValNeighbour = table[key].length;
                    sum = lengthToCurrCity + x.Value;
                    if (sum < oldValNeighbour)
                    {
                        table[key].length = sum;
                        table[key].prevVertex = vertexFromHeap;

                        if (visited[key] == false)
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
                heap = new SortedSet<HeapElem>(new ByLength());
                visited = new Dictionary<string, bool>();
                visited.Add(start, false);
                heap.Add(new HeapElem(0, start));
                table = new Dictionary<string, TableElement>();
                table.Add(start, new TableElement());
                table[start].length = 0;
                table[start].prevVertex = start;
                Dijkstry(start, startNewRoad);
                if (!visited.ContainsKey(endNewRoad) || !visited[endNewRoad])
                    Dijkstry(start, endNewRoad);
            }
            else
            {
                if (!visited.ContainsKey(startNewRoad) || !visited[startNewRoad])
                    Dijkstry(start, startNewRoad);
                if (!visited.ContainsKey(endNewRoad) || !visited[endNewRoad])
                    Dijkstry(start, endNewRoad);
            }
            SortedSet<HeapElem> heapNR = new SortedSet<HeapElem>(new ByLength());
            //[x,1] -is on heap
            Dictionary<string, bool[]> isVisitedHeap = new Dictionary<string, bool[]>();
            int cityShortestPath = 0;
            int oldValue = table[endNewRoad].length;
            int newValue = roadLength + table[startNewRoad].length;
            bool onHeap, isVisited;
            HeapElem t, p;
            if (oldValue > newValue)
            {
                cityShortestPath++;
                isVisitedHeap.Add(endNewRoad, new bool[2]);
                heapNR.Add(new HeapElem(newValue, endNewRoad));
            }
            else if (table[startNewRoad].length > roadLength + table[endNewRoad].length)
            {
                isVisitedHeap.Add(startNewRoad, new bool[2]);
                cityShortestPath++;
                heapNR.Add(new HeapElem(roadLength + table[endNewRoad].length, startNewRoad));
            }
            else return "0";

            HeapElem el = null;
            while (heapNR.Count > 0 && cityShortestPath < 100)
            {
                el = heapNR.First();
                heapNR.Remove(el);
                isVisitedHeap[el.vertex][1] = false;
                isVisitedHeap[el.vertex][0] = true;
                foreach (KeyValuePair<string, int> x in array[el.vertex].incList)
                {
                    if (!isVisitedHeap.ContainsKey(x.Key))
                        isVisitedHeap.Add(x.Key, new bool[2]);
                    if (!visited[x.Key])
                    {
                        Dijkstry(start, x.Key);
                    }
                    oldValue = table[x.Key].length;
                    newValue = el.length + x.Value;
                    if (oldValue > newValue)
                    {
                        onHeap = isVisitedHeap[x.Key][1];
                        isVisited = isVisitedHeap[x.Key][0];
                        if (onHeap)
                        {
                            heapNR.Remove(new HeapElem(oldValue, x.Key));
                            heapNR.Add(new HeapElem(newValue, x.Key));
                        }
                        else if (!isVisited)
                        {
                            cityShortestPath++;
                            heapNR.Add(new HeapElem(newValue, x.Key));
                            isVisitedHeap[x.Key][1] = true;
                        }
                    }
                }
            }
            return ((cityShortestPath >= 100) ? "100+" : cityShortestPath.ToString());
        }


    }
}
