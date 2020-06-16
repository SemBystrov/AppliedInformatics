using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppliedInformatics.LaboratoryWork1;
using AppliedInformatics.LaboratoryWork4;

namespace AppliedInformatics.LaboratoryWork5
{
    /// <summary>
    /// Класс для работы с системой нелинейных уравнений 
    /// </summary>
    public class NonLinearSystem
    {
        /// <summary>
        /// Делегат для пользовательской функции, возвращающей значения функций при заданном векторе X
        /// </summary>
        public delegate Matrix Fx(Matrix X);

        /// <summary>
        /// Функция, представлящая систему нелинейных уравнений
        /// </summary>
        protected Fx fx;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="fx">Пользовательская функция, представляющая систему нелинейных уравнений</param>
        public NonLinearSystem(Fx fx)
        {
            this.fx = fx;
        }

        // Значение производных по i-му неизвестному в точке
        /// <summary>
        /// Метод для вычисления частных производных
        /// </summary>
        /// <param name="X"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public Matrix DFx(Matrix X, int i)
        {
            double epsilon = 1e-3;

            Matrix XpE = Matrix.CopyMatrix(X);
            XpE[i, 0] += epsilon;
            Matrix XmE = Matrix.CopyMatrix(X);
            XmE[i, 0] -= epsilon;

            Matrix Yp = this.fx(XpE);
            Matrix Ym = this.fx(XmE);
            Matrix dY = (Yp + Ym.Coef(-1)).Coef(1/(2*epsilon));

            return dY;
        }

        /// <summary>
        /// Поиск решения системы методом Ньютона
        /// </summary>
        /// <param name="X">Вектор приближённых значений корней</param>
        /// <param name="calculationError">Допустимая погрешность</param>
        /// <returns>Корни системы</returns>
        /// <exception cref="ArgumentException">Вызывается, при превышении допустимого числа итераций алгоритма (10000)</exception>
        public Matrix SolutionNewton(Matrix X, double calculationError)
        {
            int i = 0;

            bool solved = false;

            while (!solved)
            {
                Matrix newX = Matrix.CopyMatrix(X);

                // Составляю линейную систему из производных и значения функций
                Matrix forSystem = DFx(X, 0);

                for (int j = 1; j < X.CountStrings; j++)
                    forSystem = forSystem.Concatenation(DFx(X, j));

                forSystem = forSystem.Concatenation(this.fx(X).Coef(-1));

                // Решаю систему методом Гаусса
                Matrix systemSolution = (new LinearSystem(forSystem)).SolutionGaussMethod();

                // Получаю новые значения
                newX = newX + systemSolution;
                
                // Проверяю погрешность 
                Matrix check = newX + X.Coef(-1);
                
                double sum = 0;
                for (int j = 0; j < check.CountStrings; j++)
                    sum += Math.Abs(check[j, 0]);


                if (sum < calculationError)
                    solved = true;
                else
                    solved = false;

                X = newX;

                i++;

                if (i > 10000)
                    throw new ArgumentException("Превышено допустимое количество итераций. Попробуйте другие стартовые значения");
            }

            return X;
        }

        

    }

    /// <summary>
    /// Расширение класса матриц
    /// </summary>
    public static class MatrixExtension2
    {
        /// <summary>
        /// Метод, осуществляющий умножение матрицы на коэффициент
        /// </summary>
        /// <param name="initial">Исходная матрица</param>
        /// <param name="coef">Коэффициент</param>
        /// <returns></returns>
        public static Matrix Coef (this Matrix initial, double coef)
        {
            if (coef != 0) 
            { 
                Matrix coefinit = Matrix.CopyMatrix(initial);

                for (int i = 0; i < coefinit.CountStrings; i++)
                    coefinit.DivideString(i, 1 / coef);
                
                return coefinit;
            }
            else
            {
                Matrix coefinit = new Matrix(initial.CountStrings, initial.CountColumns);
                return coefinit;
            }
        }
    }
}
