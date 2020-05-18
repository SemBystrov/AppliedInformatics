using System;
using System.Collections.Generic;
using System.Linq;

namespace AppliedInformatics.LaboratoryWork3
{
    /// <summary>
    ///     Класс<c> Graph</c> используется для хранения данных о вершинах и рёбрах графа.
    ///     В данной реализации поддерживаются только ориентированные взвешенные рёбра (по умолчанию вес ребра - 0).
    /// </summary>
    /// <remarks>
    ///     Особенности хранения графа обеспечивают следующую сложность для базовых операций:
    ///     <list type="table">
    ///         <item>
    ///             <term>Добавление вершины (AddNode)</term>
    ///             <description>O(log N)</description>
    ///         </item>
    ///         <item>
    ///             <term>Добавление ребра (AddEdge)</term>
    ///             <description>O(log^2 N)</description>
    ///         </item>
    ///         <item>
    ///             <term>Удаление вершины (RemoveNode)</term>
    ///             <description>O(NlogN)</description>
    ///         </item>
    ///         <item>
    ///             <term>Удаление ребра (RemoveEdge)</term>
    ///             <description>O(log^2 N)</description>
    ///         </item>
    ///         <item>
    ///             <term>Проверка смежности вершин (Adjacency)</term>
    ///             <description>O(log^2 N)</description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public class Graph
    {

        /// <summary>
        ///     Класс <c>Node</c> используется для представления вершины. 
        /// </summary>

        private class Node
        {
            /// <summary>
            ///     Поле хранит информацию о рёбрах, выходящих из данной вершины.
            /// </summary>
            /// <remarks>
            ///     Поле представлено в виде коллекции <see cref="SortedDictionary{TKey, TValue}"/>.
            ///     В данной реализации Key - вершина, в которую ведёт ребро, Value - вес ребра.
            /// </remarks>
            public SortedDictionary<int, ushort> FromHereToNodes;
            public Node()
            {
                this.FromHereToNodes = new SortedDictionary<int, ushort>();
            }
            /// <summary>
            ///     Выводит в консоль данные о вершине 
            /// </summary>
            public void Display()
            {
                foreach (KeyValuePair<int, ushort> i in this.FromHereToNodes)
                {
                    Console.WriteLine("Ведёт в вершину " + i.Key.ToString() + ", вес ребра - " + i.Value.ToString());
                }
            }
        }

        /// <summary>
        ///     Коллекция хранит информацию о вершинах в <see cref="SortedDictionary{TKey, TValue}"/> обращение осуществляется по пользовательским идентификаторам.
        /// </summary>

        private SortedDictionary<int, Node> nodes;

        private SortedDictionary<int, Node> Nodes { get => nodes; set => nodes = value; }

        /// <summary>
        ///     Конструктор класса 
        /// </summary>

        public Graph()
        {
            this.Nodes = new SortedDictionary<int, Node>();
        }

        /// <summary>
        /// Метод <c>GetAdjacentNodes</c> возвращает коллекцию вершин смежных с переданной
        /// </summary>
        /// <param name="id">Идентификатор вершины</param>
        /// <exception cref="ArgumentException">Если вершины с переданным <paramref name="id"/> не существует</exception>
        /// <returns>Коллекция смежных вершин для данной</returns>

        public SortedDictionary<int, ushort>.KeyCollection GetAdjacentNodes(int id)
        {
            try
            {
                return this.Nodes[id].FromHereToNodes.Keys;
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("В данном графе нет вершины с id: " + id);
            }
        }

        /// <summary>
        ///     Метод <c>GetWeightEdge</c> возвращает вес ребра из вершины <paramref name="fromNode"/> к вершине <paramref name="toNode"/>
        /// </summary>
        /// <param name="fromNode">Вершина из которой выходит ребро</param>
        /// <param name="toNode">Вершина в которую направлено ребро</param>
        /// <exception cref="ArgumentException">Если вершины с переданным идентификатором не существует</exception>
        /// <returns>Вес ребра</returns>

        public ushort GetWeightEdge(int fromNode, int toNode)
        {
            try
            {
                return this.Nodes[fromNode].FromHereToNodes[toNode];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Нет вершины с таким идентификатором");
            }
        }

        /// <summary>
        ///     Метод <c>AddNode</c> добавляет новую вершину с переданным идентификатором к графу
        /// </summary>
        /// <exception cref="ArgumentException">Вызывается, если <paramref name="id"/> уже присутствует в графе</exception>
        /// <param name="id">Идентификатор вершины</param>

        public void AddNode(int id)
        {
            try
            {
                this.Nodes[id] = new Node();
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("В данном графе уже есть вершина с id: " + id);
            }
        }

        /// <summary>
        ///     Метод <c>RemoveNode</c> удаляет вершину с переданным идентификатором из графа
        /// </summary>
        /// <param name="id">Идентификатор вершины</param>
        /// <remarks>
        ///     Если вершины с данным <paramref name="id"/> не существует исключение не генерируется, граф остаётся неизменным.
        /// </remarks>

        public void RemoveNode(int id)
        {
            if (this.NodeIdCheck(id))
            {
                foreach (Node n in this.Nodes.Values)
                {
                    n.FromHereToNodes.Remove(id);
                }
            }
        }

        /// <summary>
        ///     Добавляет ориентированное ребро графа от <paramref name="fromNode"/> к <paramref name="toNode"/> c нулевым весом
        ///     Если ребро уже существует, то вес будет перезаписан на новый.
        /// </summary>
        /// <param name="fromNode">Вершина из которой выходит ребро</param>
        /// <param name="toNode">Вершина в которую направлено ребро</param>

        public void AddEdge(int fromNode, int toNode)
        {
            try
            {
                this.AddEdge(fromNode, toNode, 0);
            }
            catch (ArgumentException err)
            {
                throw err;
            }

        }

        /// <summary>
        ///     Добавляет ориентированное ребро графа от <paramref name="fromNode"/> к <paramref name="toNode"/> c весом <paramref name="weight"/>.
        ///     Если ребро уже существует, то вес будет перезаписан на новый.
        /// </summary>
        /// <param name="fromNode">Вершина из которой выходит ребро</param>
        /// <param name="toNode">Вершина в которую направлено ребро</param>
        /// <param name="weight">Вес ребра</param>

        public void AddEdge(int fromNode, int toNode, ushort weight)
        {
            if (!this.NodeIdCheck(fromNode))
                throw new ArgumentException("Неверное значение параметра fromNode. В данном графе нет вершины с id: " + fromNode);

            if (!this.NodeIdCheck(toNode))
                throw new ArgumentException("Неверное значение параметра toNode. В данном графе нет вершины с id: " + toNode);

            this.Nodes[fromNode].FromHereToNodes[toNode] = weight;
        }

        /// <summary>
        ///     Удаляет ребро из <paramref name="fromNode"/> к <paramref name="toNode"/>. 
        /// </summary>
        /// <param name="fromNode">Вершина из которой выходит ребро</param>
        /// <param name="toNode">Вершина в которую направлено ребро</param>
        /// <remarks>
        ///     Если вершины с данным идентификатором не существует исключение не генерируется, граф остаётся неизменным.
        /// </remarks>

        public void RemoveEdge(int fromNode, int toNode)
        {
            this.Nodes[fromNode].FromHereToNodes.Remove(toNode);
        }

        /// <summary>
        ///     Проверяет наличие идентификатора вершины в графе
        /// </summary>
        /// <param name="id">Идентификатор вершины</param>
        /// <returns><c>true</c> - в графе есть вершина с данным <paramref name="id"/>, иначе <c>false</c></returns>

        public bool NodeIdCheck(int id)
        {
            return this.Nodes.ContainsKey(id);
        }
    }

    /// <summary>
    ///     Сборник алгоритмов для графа
    /// </summary>
    public class GraphAlgorithms
    {
        /// <summary>
        ///     Метод обхода графа BFS
        /// </summary>
        /// <param name="graph">Граф, который обходит алгоритм</param>
        /// <param name="fromNode">Вершина, с которой начинается обход</param>
        /// <returns>Итератор</returns>
        public static IEnumerable<int> BFS(Graph graph, int fromNode)
        {
            Queue<int> queue = new Queue<int>();
            HashSet<int> visited = new HashSet<int>();
            int node;
            queue.Enqueue(fromNode);
            visited.Add(fromNode);
            while (queue.Count > 0)
            {
                node = queue.Dequeue();
                foreach (int n in graph.GetAdjacentNodes(node))
                {
                    if (!visited.Contains(n))
                    {
                        queue.Enqueue(n);
                        visited.Add(n);
                    }
                }
                yield return node;
            }
        }

        /// <summary>
        ///     Метод обхода графа DFS
        /// </summary>
        /// <param name="graph">Граф, который обходит алгоритм</param>
        /// <param name="fromNode">Вершина, с которой начинается обход</param>
        /// <returns>Итератор</returns>
        public static IEnumerable<int> DFS(Graph graph, int fromNode)
        {
            Stack<int> stack = new Stack<int>();
            HashSet<int> visited = new HashSet<int>();
            int node;
            stack.Push(fromNode);
            while (stack.Count > 0)
            {
                node = stack.Pop();
                if (!visited.Contains(node))
                {
                    visited.Add(node);
                    foreach (int n in graph.GetAdjacentNodes(node))
                    {
                        if (!visited.Contains(n))
                        {
                            stack.Push(n);
                        }
                    }
                    yield return node;
                }
            }
        }

        /// <summary>
        ///     Алгоритм Дейекстры 
        /// </summary>
        /// <param name="graph">Граф, который обходит алгоритм</param>
        /// <param name="fromNode">Вершина, с которой начинается обход</param>
        /// <returns>Итератор</returns>
        public static SortedDictionary<int, int> Dijkstra(Graph graph, int fromNode)
        {

            SortedDictionary<int, int> answer = new SortedDictionary<int, int>();

            SortedDictionary<int, int> weight = new SortedDictionary<int, int>();

            int node, minWeight;
            weight[fromNode] = 0;

            while (weight.Count > 0)
            {
                minWeight = weight.First().Value;
                node = weight.First().Key;
                foreach (KeyValuePair<int, int> w in weight)
                {
                    if (w.Value < minWeight)
                    {
                        minWeight = w.Value;
                        node = w.Key;
                    }
                }

                answer.Add(node, minWeight);
                weight.Remove(node);


                foreach (int n in graph.GetAdjacentNodes(node))
                {
                    if (!answer.ContainsKey(n))
                    {
                        if (weight.ContainsKey(n))
                        {
                            weight[n] = Math.Min(weight[n], minWeight + graph.GetWeightEdge(node, n));
                        }
                        else
                        {
                            weight[n] = minWeight + graph.GetWeightEdge(node, n);
                        }
                    }
                }


            }

            return answer;
        }
    }
}
