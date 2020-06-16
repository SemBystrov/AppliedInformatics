using System;
using AppliedInformatics.LaboratoryWork4;
using AppliedInformatics.LaboratoryWork3;
using AppliedInformatics.LaboratoryWork1;
using AppliedInformatics.LaboratoryWork5;

namespace AppliedInformatics
{
    class Program
    {
        static public Matrix SystemTest1(Matrix X)
        {
            double x1 = X[0, 0];
            double x2 = X[1, 0];
            Matrix result = new Matrix(X.CountStrings, X.CountColumns);
            result[0, 0] = 0.1 * x1 * x1 + x1 + 0.2 * x2 * x2 - 0.3;
            result[1, 0] = 0.2 * x1 * x1 + x2 - 0.1 * x1 * x2 - 0.7;
            return result;
        }

        /// <summary>
        /// Точка входа в программу 
        /// </summary>
        static void Main(string[] args)
        {
            Matrix X = new Matrix(2, 1);
            X[0, 0] = 3;
            X[1, 0] = 1;

            NonLinearSystem.Fx systemTest = SystemTest1;

            NonLinearSystem someSystem = new NonLinearSystem(systemTest);

            X = someSystem.SolutionNewton(X, 0.00000001);
        }
    }
}
