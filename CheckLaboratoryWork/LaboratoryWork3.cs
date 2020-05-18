using System;
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
                for (int j = 0; j < 4; j++)
                {
                    Assert.AreEqual(tryBFS[j], correctBFS[j]);
                }
            }
            catch
            {
                for (int j = 0; j < 4; j++)
                {
                    Assert.AreEqual(tryBFS[j], correctBFS2[j]);
                }
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
                for (int j = 0; j < 4; j++)
                {
                    Assert.AreEqual(tryDFS[j], correctDFS[j]);
                }
            }
            catch
            {
                for (int j = 0; j < 4; j++)
                {
                    Assert.AreEqual(tryDFS[j], correctDFS2[j]);
                }
            }
        }
    }
}
