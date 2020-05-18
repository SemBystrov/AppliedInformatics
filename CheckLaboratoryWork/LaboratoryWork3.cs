using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppliedInformatics;
using AppliedInformatics.LaboratoryWork3;

namespace CheckLaboratoryWork
{
    [TestClass]
    public class LaboratoryWork3
    {
        [TestMethod]
        public void BFSTest1()
        {
            Graph SomeGraph = new Graph();

            int[] correctBFS = { 1, 2, 3, 4 };
            int[] correctBFS2 = { 1, 3, 2, 4 };
            int[] tryBFS = new int[4];

            SomeGraph.AddNode(1);
            SomeGraph.AddNode(2);
            SomeGraph.AddNode(3);
            SomeGraph.AddNode(4);
            SomeGraph.AddEdge(1, 2);
            SomeGraph.AddEdge(1, 3);
            SomeGraph.AddEdge(3, 4);

            int i = 0;

            foreach (int n in GraphAlgorithms.BFS(SomeGraph, 1))
            {
                tryBFS[i] = n;
                i += 1;
            }

            try
            {
                CollectionAssert.AreEqual(correctBFS, tryBFS);
            }
            catch
            {
                CollectionAssert.AreEqual(correctBFS2, tryBFS);
            }
        }

        [TestMethod]
        public void DFSTest1()
        {
            Graph SomeGraph = new Graph();

            int[] correctDFS = { 1, 2, 3, 4 };
            int[] correctDFS2 = { 1, 3, 4, 2 };
            int[] tryDFS = new int[4];

            SomeGraph.AddNode(1);
            SomeGraph.AddNode(2);
            SomeGraph.AddNode(3);
            SomeGraph.AddNode(4);
            SomeGraph.AddEdge(1, 2);
            SomeGraph.AddEdge(1, 3);
            SomeGraph.AddEdge(3, 4);

            int i = 0;

            foreach (int n in GraphAlgorithms.DFS(SomeGraph, 1))
            {
                tryDFS[i++] = n;
            }

            try
            {
                CollectionAssert.AreEqual(correctDFS, tryDFS);
            }
            catch
            {
                CollectionAssert.AreEqual(correctDFS2, tryDFS);
            }
        }

        [TestMethod]
        public void DijkstraTest1()
        {
            Graph SomeGraph = new Graph();

            SortedDictionary<int, int> correctDijkstraAnswer = new SortedDictionary<int, int>
            {
                { 1, 0 },
                { 2, 8 },
                { 3, 3 },
                { 4, 9 }
            };

            SomeGraph.AddNode(1);
            SomeGraph.AddNode(2);
            SomeGraph.AddNode(3);
            SomeGraph.AddNode(4);
            SomeGraph.AddEdge(1, 2, 8);
            SomeGraph.AddEdge(1, 3, 3);
            SomeGraph.AddEdge(2, 4, 1);
            SomeGraph.AddEdge(3, 4, 10);
            SomeGraph.AddEdge(1, 4, 100);

            SortedDictionary<int, int> tryDijkstra = GraphAlgorithms.Dijkstra(SomeGraph, 1);

            CollectionAssert.AreEqual(correctDijkstraAnswer, tryDijkstra);
        }

        [TestMethod]
        public void DijkstraTest2()
        {
            Graph SomeGraph = new Graph();

            SortedDictionary<int, int> correctDijkstraAnswer = new SortedDictionary<int, int>
            {
                { 1, 0 },
                { 2, 1 },
                { 3, 100 }
            };

            SomeGraph.AddNode(1);
            SomeGraph.AddNode(2);
            SomeGraph.AddNode(3);
            SomeGraph.AddEdge(1, 2, 1);
            SomeGraph.AddEdge(1, 3, 100);
            SomeGraph.AddEdge(2, 3, 101);

            SortedDictionary<int, int> tryDijkstra = GraphAlgorithms.Dijkstra(SomeGraph, 1);

            CollectionAssert.AreEqual(correctDijkstraAnswer, tryDijkstra);
        }
    }
}
