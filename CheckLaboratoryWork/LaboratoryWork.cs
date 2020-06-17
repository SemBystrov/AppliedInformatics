using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppliedInformatics;
using AppliedInformatics.LaboratoryWork3;
using AppliedInformatics.LaboratoryWork1;
using AppliedInformatics.LaboratoryWork4;
using AppliedInformatics.LaboratoryWork5;
using AppliedInformatics.LaboratoryWork6;
using AppliedInformatics.LaboratoryWork7;
using System.Runtime.CompilerServices;

namespace CheckLaboratoryWork
{
    [TestClass]
    public class LaboratoryWork
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

    [TestClass]
    public class LaboratoryWork4
    {
        /*
         * 1 2 0
         * 0 3 0
         * 0 0 0
         * 
         * Rank = 2
         */
        [TestMethod]
        public void RankTest1()
        {
            
            Matrix SomeMatrix = new Matrix(3, 3);

            int correctRank = 2;

            SomeMatrix[0, 0] = 1;
            SomeMatrix[0, 1] = 2;
            SomeMatrix[0, 2] = 0;
            SomeMatrix[1, 0] = 0;
            SomeMatrix[1, 1] = 3;
            SomeMatrix[1, 2] = 0;
            SomeMatrix[2, 0] = 0;
            SomeMatrix[2, 1] = 0;
            SomeMatrix[2, 2] = 0;

            Assert.AreEqual(correctRank, SomeMatrix.Rank());
        }

        /*
         * 1 2 3
         * 0 3 4
         * 0 0 5
         * 
         * Rank = 3
         */

        [TestMethod]
        public void RankTest2()
        {

            Matrix SomeMatrix = new Matrix(3, 3);

            int correctRank = 3;

            SomeMatrix[0, 0] = 1;
            SomeMatrix[0, 1] = 2;
            SomeMatrix[0, 2] = 3;
            SomeMatrix[1, 0] = 0;
            SomeMatrix[1, 1] = 3;
            SomeMatrix[1, 2] = 4;
            SomeMatrix[2, 0] = 0;
            SomeMatrix[2, 1] = 0;
            SomeMatrix[2, 2] = 5;

            Assert.AreEqual(correctRank, SomeMatrix.Rank());
        }

        [TestMethod]
        public void RankTest3()
        {

            Matrix SomeMatrix = new Matrix(2, 3);

            int correctRank = 1;

            SomeMatrix[0, 0] = 1;
            SomeMatrix[0, 1] = 2;
            SomeMatrix[0, 2] = 6;
            SomeMatrix[1, 0] = 2;
            SomeMatrix[1, 1] = 4;
            SomeMatrix[1, 2] = 12;

            Assert.AreEqual(correctRank, SomeMatrix.Rank());
        }

        [TestMethod]
        public void LinearSystemSolutionTest1()
        {
            int countX = 2;
            string[] eq = { "2x0+1x1=3", "4x0+1x1=5" };

            LinearSystem LS = new LinearSystem(countX, eq);

            Matrix ans = LS.SolutionGaussMethod();

            Matrix correctAns = new Matrix(2, 1);
            correctAns[0, 0] = 1;
            correctAns[1, 0] = 1;

            for (int i = 0; i < 2; i++) {
                Assert.AreEqual(correctAns[i, 0], ans[i, 0]);
            }
            
        }

        [TestMethod]
        public void LinearSystemSolutionTest2()
        {
            int countX = 3;
            string[] eq = { "1x0+1x1+1x2=6", "1x0+2x1+0x2=5", "0x0+1x1+2x2=8" };

            LinearSystem LS = new LinearSystem(countX, eq);

            Matrix ans = LS.SolutionGaussMethod();

            Matrix correctAns = new Matrix(3, 1);
            correctAns[0, 0] = 1;
            correctAns[1, 0] = 2;
            correctAns[2, 0] = 3;

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(correctAns[i, 0], ans[i, 0]);
            }

        }

        [TestMethod]
        public void LinearSystemSolutionTest3()
        {
            int countX = 2;
            string[] eq = { "1x0+2x1=6", "2x0+4x1=12" };

            LinearSystem LS = new LinearSystem(countX, eq);

            Matrix ans = LS.SolutionGaussMethod();

            Matrix correctAns = new Matrix(1, 2);
            correctAns[0, 0] = -2;
            correctAns[0, 1] = 6;

            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(correctAns[0, i], ans[0, i]);
            }
        }
    }

    [TestClass]
    public class LaboratoryWork5
    {
        // Тест от друга 

        public Matrix SystemTest1(Matrix X)
        {
            double x1 = X[0, 0];
            double x2 = X[1, 0];
            Matrix result = new Matrix(X.CountStrings, X.CountColumns);
            result[0, 0] = 0.1 * x1 * x1 + x1 + 0.2 * x2 * x2 - 0.3;
            result[1, 0] = 0.2 * x1 * x1 + x2 - 0.1 * x1 * x2 - 0.7;
            return result;
        }

        [TestMethod]
        public void NewtonTest1()
        {
            Matrix X = new Matrix(2, 1);
            X[0, 0] = 3;
            X[1, 0] = 1;

            NonLinearSystem.Fx systemTest = SystemTest1;

            NonLinearSystem someSystem = new NonLinearSystem(systemTest);

            X = someSystem.SolutionNewton(X, 0.00000001);

            Matrix rightAnswer = new Matrix(2, 1);

            rightAnswer[0, 0] = 0.1964115066987255;
            rightAnswer[1, 0] = 0.7061541850402057;

            Assert.AreEqual(rightAnswer[0, 0], X[0, 0], 0.00000001);
            Assert.AreEqual(rightAnswer[1, 0], X[1, 0], 0.00000001);
        }

        // Собственный тест 
        public Matrix SystemTest2(Matrix X)
        {
            double x1 = X[0, 0];
            double x2 = X[1, 0];
            Matrix result = new Matrix(X.CountStrings, X.CountColumns);
            result[0, 0] = x1 + x2 - 8;
            result[1, 0] = x1 * x1 + x2 * x2 - 82;
            return result;
        }

        [TestMethod]
        public void NewtonTest2()
        {
            Matrix X = new Matrix(2, 1);
            X[0, 0] = 1;
            X[1, 0] = 8;

            NonLinearSystem.Fx systemTest = SystemTest2;

            NonLinearSystem someSystem = new NonLinearSystem(systemTest);

            X = someSystem.SolutionNewton(X, 0.1);

            Matrix rightAnswer = new Matrix(2, 1);

            rightAnswer[0, 0] = -1;
            rightAnswer[1, 0] = 9;

            Assert.AreEqual(rightAnswer[0, 0], X[0, 0], 0.1);
            Assert.AreEqual(rightAnswer[1, 0], X[1, 0], 0.1);
        }
    }

    [TestClass]
    public class LaboratoryWork6
    {
        // Тест от друга 

        public double EquationTest1(Matrix YX)
        {
            double y = YX[0, 0];
            double x = YX[1, 0];
            return y * Math.Cos(x);
        }

        [TestMethod]
        public void RungeKutta4Test1()
        {
            Matrix YX = new Matrix(2, 1);
            YX[0, 0] = 1;
            YX[1, 0] = 0;
            double h0 = 2.5;
            double m = 10;

            DifferentialEquation.Fx equationTest = EquationTest1;

            DifferentialEquation someEquation = new DifferentialEquation(equationTest);

            Matrix newYX = someEquation.SolutionRungeKutta4(YX, h0, m);

            Matrix rightAnswer = new Matrix(2, 1);

            rightAnswer[0, 0] = 1.81930573495;
            rightAnswer[1, 0] = 2.5;

            Assert.AreEqual(rightAnswer[0, 0], newYX[0, 0], 0.00000001);
            Assert.AreEqual(rightAnswer[1, 0], newYX[1, 0], 0.00000001);
        }

        // Собственный тест 
    }

    [TestClass]
    public class LaboratoryWork7
    {
        // Тест от друга 

        public double EquationTest1(Matrix YX)
        {
            double y = YX[0, 0];
            double x = YX[1, 0];
            return y * Math.Cos(x);
        }

        [TestMethod]
        public void RungeKutta4Test1()
        {
            Matrix YX = new Matrix(2, 1);
            YX[0, 0] = 1;
            YX[1, 0] = 0;
            double h0 = 2.5;
            double m = 10;

            DifferentialSystem.Fx[] equationTest = { EquationTest1 };

            DifferentialSystem someEquation = new DifferentialSystem(equationTest);

            Matrix newYX = someEquation.SolutionRungeKutta4(YX, h0, m);

            Matrix rightAnswer = new Matrix(2, 1);

            rightAnswer[0, 0] = 1.81930573495;
            rightAnswer[1, 0] = 2.5;

            Assert.AreEqual(rightAnswer[0, 0], newYX[0, 0], 0.001);
            Assert.AreEqual(rightAnswer[1, 0], newYX[1, 0], 0.001);
        }

        // Собственный тест

        public double EquationTest2(Matrix YX)
        {
            double y = YX[0, 0];
            double y2 = YX[1, 0];
            return -2 * y + 4 * y2;
        }

        public double EquationTest3(Matrix YX)
        {
            double y = YX[0, 0];
            double y2 = YX[1, 0];
            return 3 * y2 - y;
        }

        [TestMethod]
        public void RungeKutta4Test2()
        {
            Matrix YX = new Matrix(3, 1);
            YX[0, 0] = -6;
            YX[1, 0] = -3;
            YX[2, 0] = 0;
            double h0 = 1;
            double m = 10;

            DifferentialSystem.Fx[] equationTest = { EquationTest2, EquationTest3 };

            DifferentialSystem someEquation = new DifferentialSystem(equationTest);

            Matrix newYX = someEquation.SolutionRungeKutta4(YX, h0, m);

            Matrix rightAnswer = new Matrix(3, 1);

            rightAnswer[0, 0] = -16.2496299625;
            rightAnswer[1, 0] = -15.1459916390; 
            rightAnswer[2, 0] = 1;

            Assert.AreEqual(rightAnswer[0, 0], newYX[0, 0], 0.001);
            Assert.AreEqual(rightAnswer[1, 0], newYX[1, 0], 0.001);
            Assert.AreEqual(rightAnswer[2, 0], newYX[2, 0], 0.001);
        }
    }
}
