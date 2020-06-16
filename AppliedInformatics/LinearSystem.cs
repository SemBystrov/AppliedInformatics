using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppliedInformatics.LaboratoryWork1;

namespace AppliedInformatics.LaboratoryWork4
{
    /// <summary>
    /// <para>Класс <c>LinearSystem</c> служит для работы с СЛАУ</para>
    /// </summary>

    public class LinearSystem
    {
        /// <summary>
        /// Исключение, генерируемое при попытке найти решение несовместной системы.
        /// </summary>
        public class IncompatibleSystemException : Exception
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public IncompatibleSystemException() : base("Система несовместна") { }
        }

        // Используется матричное представление системы в виде A * X = B
        private Matrix A;
        private Matrix B;

        /// <summary>
        /// Конструктор класса принимает в себя количество переменных и массив строк, содержащих уравнения системы вида <c>2x1+4x2=6</c>
        /// </summary>
        /// <param name="CountX">Количество переменных</param>
        /// <param name="equations">Массив строк вида <c>"2x0+1x1=6"</c></param>
        public LinearSystem(int CountX, string[] equations)
        {
            this.A = new Matrix(equations.Length, CountX);
            this.B = new Matrix(equations.Length, 1);

            string[] parse;
            for (int i = 0; i < equations.Length; i++)
            {
                parse = equations[i].Split('=');

                // Добавим свободный член в вектор B

                this.B[i, 0] = double.Parse(parse[1]);

                // Распарсим левую часть уравнения

                parse = parse[0].Split('+');

                for (int j = 0; j < parse.Length; j++)
                {
                    string[] x;
                    x = parse[j].Split('x');
                    this.A[i, int.Parse(x[1])] = double.Parse(x[0]);

                }

            }
        }

        /// <summary>
        /// Конструктор класса, принимающий расширенную матрицу системы
        /// </summary>
        /// <param name="extendedA">расширенная матрица системы</param>
        public LinearSystem(Matrix extendedA)
        {
            this.A = extendedA.Slice(0, 0, extendedA.CountStrings - 1, extendedA.CountColumns - 2);
            this.B = extendedA.Slice(0, extendedA.CountColumns - 1, extendedA.CountStrings - 1, extendedA.CountColumns - 1);
        }
        /// <summary>
        /// Реализация метода Гаусса для нахождения решения СЛАУ
        /// </summary>
        /// <returns></returns>
        public Matrix SolutionGaussMethod()
        {
            int rankA, rankE;
            
            Matrix extendedA = this.A.Concatenation(B); // Расширенная матрица

            rankA = this.A.Rank(); // Ранг характеристической матрицы
            rankE = extendedA.Rank(); // Ранг расширенной матрицы
            
            if (rankA == rankE)
            {
                Matrix triangularExA = extendedA.GetTriangularMatrix(); // Треугольный вид расширенной матрицы
                Matrix coefDependetVar = triangularExA.Slice(0, 0, rankA - 1, rankA - 1); // Коэффициенты при зависимых переменных
                Matrix ans;

                if (rankA != this.A.CountColumns) {

                    // Бесконечное множество решений, 
                    // в ответе для каждой зависимой переменной будут переданы соответствующие коэффициенты независимых пременных и в последнем столбце - свободные члены 

                    Matrix coefIndependetVar = triangularExA.Slice(0, rankA, rankA - 1, A.CountColumns - 1);  // Коэффициенты при независимых переменных
                    
                    // Переносим в правую часть уравнения со знаком минус
                    for (int i = 0; i < coefIndependetVar.CountStrings; i++)
                        coefIndependetVar.DivideString(i, -1);

                    // Объединяем со столбцом свободных членов
                    ans = coefIndependetVar.Concatenation(triangularExA.Slice(0, A.CountColumns, rankA - 1, A.CountColumns)); // Объединяю с свободными членами
                }
                else
                {
                    // Система имеет единственное решение

                    ans = triangularExA.Slice(0, A.CountColumns, rankA - 1, A.CountColumns);
                }

                for (int i = rankA - 1; i >= 0; i--)
                {

                    double divider = coefDependetVar[i, i];

                    ans.DivideString(i, divider);

                    // Подставляю найденное значения в другие уравнения

                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (coefDependetVar[j, i] != 0)
                        {
                            ans.DivideString(i, 1 / coefDependetVar[j, i]);
                            ans.SubtractionStrings(j, i);
                            ans.DivideString(i, coefDependetVar[j, i]);
                        }
                    }
                }

                return ans;
            }
            else
            {
                throw new IncompatibleSystemException();
            }
        }
    }

    /// <summary>
    /// <para>Класс расширяет <see cref="Matrix"/> методами преобразований матриц, необходимыми для решения СЛАУ</para>
    /// </summary>
    
    public static class MatrixExtension
    {
        /// <summary>
        /// Метод <c>Rank</c> используется для нахождения ранга матрицы
        /// </summary>
        /// <param name="initial">Матрица для которой осуществляется поиск ранга</param>
        /// <returns>Ранг матрицы</returns>
        
        public static int Rank(this Matrix initial)
        {
            // Получим матрицу треугольного вида для нахождение ранга

            Matrix triangular = initial.GetTriangularMatrix();

            /* 
             * В треугольной матрице ранг равен количеству ненулевых строк
             * 
             * Все нулевые строки алгоритм GetTriangularMatrix оставляет внизу, 
             * что позволяет применить бинарный поиск, 
             * чтобы найти последнюю ненулевую строку, номером которой и будет ранг
             */


            int left = 1, right = Math.Min(triangular.CountStrings, triangular.CountColumns);
            int rank = 0;

            bool isZeroString;

            while (left != right)
            {
                rank = (right + left) / 2;

                rank += (left == rank) ? 1 : 0;

                isZeroString = true;

                // Проверка строки на пустоту

                for (int i = 0; i < triangular.CountColumns; i++) 
                {
                    if (triangular[rank - 1, i] != 0)
                    {
                        isZeroString = false;
                        break;
                    }
                }
                
                if (isZeroString)
                    right = rank - 1;
                else
                    left = rank;
            }
            return right;
        }

        /// <summary>
        /// <para>
        /// Метод <c>GetTriangularMatrix</c> позволяет получить из данной матрицы треугольную с помощью равносильных преобразований
        /// </para>
        /// </summary>
        /// <param name="initial">Матрица из которой надо получить треугольную</param>
        /// <returns>Треугольная матрица</returns>
       
        public static Matrix GetTriangularMatrix(this Matrix initial)
        {
            Matrix triangular = Matrix.CopyMatrix(initial);

            int skipped = 0;
            for (int i = 0; i < Math.Min(triangular.CountColumns, triangular.CountStrings) - 1; i++)
            {
                int substrahendString = i - skipped;
                
                /* 
                 * Поиск ненулевого элемента из множества { элемент главной диагонали и находящиеся под ним }.
                 * 
                 * Используется, чтобы предотвратить ситуацию, возникающую на второй итерации в подобных случаях: 
                 * 
                 * 1 2 3 4
                 * 0 0 1 2
                 * 0 1 2 3
                 * 
                 * Решение: поменять 2 и 3 строки:
                 * 
                 * 1 2 3 4
                 * 0 1 2 3
                 * 0 0 1 2
                 */


                while ((substrahendString < triangular.CountStrings) && (triangular[substrahendString, i] == 0))
                    substrahendString++;

                /*
                 * Замена строк (для описанных выше случаев)
                 */

                if (i != substrahendString)
                    triangular.SwapStrings(i, substrahendString);

                /*
                 * Если все элементы из множества { элемент главной диагонали и находящиеся под ним } нулевые
                 * мы переходим к следующему столбцу, но остаёмся на этой строке, для этого введена переменная skipped 
                 * 
                 * 1 2 3 4 5 6
                 * 0 2 3 4 5 6
                 * 0 0 0 1 5 6
                 * 0 0 0 1 5 6
                 * 0 0 0 0 5 6
                 * 0 0 0 0 0 6
                 */

                if (substrahendString == triangular.CountStrings)
                {
                    skipped += 1;
                    continue;
                }
                    
                
                /*
                 * Вычитанием строк получаем нули в столбце под элементом главной диагонали
                 */

                for (int j = i + 1; j < triangular.CountStrings; j++)
                {
                    
                    if (triangular[j, i] != 0)
                    {
                        double divider = triangular[j, i] / triangular[i, i];
                        triangular.DivideString(j, divider);
                        triangular.SubtractionStrings(j, i);
                        triangular.DivideString(j, 1 / divider);
                    }
                }

            }

            return triangular;
        }

        /// <summary>
        /// <para>Метод <c>Concatenation</c> используется для получения матрицы, объединяющей столбцы двух других матриц.</para>
        /// <para>
        /// Пример:
        /// <code>Матрица C как конкатенациия A и B:</code>
        /// <code>| 2 3 | | 1 2 3 | = | 2 3 1 2 3 |</code>
        /// <code>| 3 2 | | 3 4 5 | = | 3 2 3 4 5 |</code>
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentException">При разном количестве строк</exception>
        /// <param name="initial"></param>
        /// <param name="concatenated"></param>
        /// <returns></returns>

        public static Matrix Concatenation(this Matrix initial, Matrix concatenated)
        {
            if (initial.CountStrings == concatenated.CountStrings)
            {
                Matrix value = new Matrix(initial.CountStrings, initial.CountColumns + concatenated.CountColumns);

                // Копирование элементов из обоих массивов

                for (int i = 0; i < initial.CountStrings; i++)
                    for (int j = 0; j < initial.CountColumns; j++)
                        value[i, j] = initial[i, j];

                for (int i = 0; i < initial.CountStrings; i++)
                    for (int j = initial.CountColumns; j < initial.CountColumns + concatenated.CountColumns; j++)
                        value[i, j] = concatenated[i, j - initial.CountColumns];

                return value;
            }
            else
            {
                throw new ArgumentException("Данные матрицы нельзя объединить, т.к. они имеют разное количество строк");
            }


        }
    
        /// <summary>
        /// Метод Slice возвращает необходимую часть матрицы.
        /// </summary>
        /// <param name="initial">Исходная матрица</param>
        /// <param name="stringStart">Верхняя граница</param>
        /// <param name="columnStart">Левая граница</param>
        /// <param name="stringEnd">Нижняя граница</param>
        /// <param name="columnEnd">Правая граница</param>
        /// <returns>Часть матрицы, ограниценная заданными строками и столбцами</returns>
        public static Matrix Slice(this Matrix initial, int stringStart, int columnStart, int stringEnd, int columnEnd)
        {
            if (((stringStart <= stringEnd) && (stringEnd < initial.CountStrings)) && ((columnStart <= columnEnd) && (columnEnd < initial.CountColumns)))
            {
                Matrix sliceInitial = new Matrix(stringEnd - stringStart + 1, columnEnd - columnStart + 1);
                for (int i = stringStart; i <= stringEnd; i++)
                {
                    for (int j = columnStart; j <= columnEnd; j++)
                    {
                        sliceInitial[i - stringStart, j - columnStart] = initial[i, j];
                    }
                }
                return sliceInitial;
            }
            else
            {
                throw new ArgumentException("Неверно указаны границы");
            }
        }
    }
}
