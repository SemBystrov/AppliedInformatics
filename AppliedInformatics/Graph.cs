using System;
using System.Collections.Generic;

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
    class Graph
    {

        /// <summary>
        ///     Класс <c>Node</c> используется для хранения данных об одной вершине.
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
            public SortedDictionary<int, int> FromHereToNodes;
            public Node()
            {
                this.FromHereToNodes = new SortedDictionary<int, int>();
            }
            /// <summary>
            ///     Выводит в консоль данные о вершине 
            /// </summary>
            public void Display()
            {
                foreach (KeyValuePair<int, int> i in this.FromHereToNodes)
                {
                    Console.WriteLine("Ведёт в вершину " + i.Key.ToString() + ", вес ребра - " + i.Value.ToString());
                }
            }
        }

        /// <summary>
        ///     Коллекция хранит информацию о вершинах по идентификаторам в <see cref="SortedDictionary{TKey, TValue}"/>
        /// </summary>

        private SortedDictionary<int, Node> _nodes;

        /// <summary>
        ///     Конструктор класса 
        /// </summary>

        public Graph()
        {
            this._nodes = new SortedDictionary<int, Node>();
        }

        /// <summary>
        ///     Метод <c>AddNode</c> добавляет новую вершину с переданным идентификатором к графу
        /// </summary>
        /// <exception cref="System.ArgumentException">Вызывается, если <paramref name="id"/> уже присутствует в графе</exception>
        /// <param name="id">Идентификатор вершины</param>

        public void AddNode(int id)
        {
            if (this.NodeIdCheck(id))
                throw new ArgumentException("В данном графе уже есть вершина с id: " + id);

            this._nodes[id] = new Node();
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
                foreach (Node n in this._nodes.Values)
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

            this._nodes[fromNode].FromHereToNodes[toNode] = weight;
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
            if (this.NodeIdCheck(fromNode))
            {
                this._nodes[fromNode].FromHereToNodes.Remove(toNode);
            }
        }

        /// <summary>
        ///     Проверяет наличие идентификатора вершины в графе
        /// </summary>
        /// <param name="id">Идентификатор вершины</param>
        /// <returns><c>true</c> - в графе есть вершина с данным <paramref name="id"/>, иначе <c>false</c></returns>

        public bool NodeIdCheck(int id)
        {
            return this._nodes.ContainsKey(id);
        }
    }
}
