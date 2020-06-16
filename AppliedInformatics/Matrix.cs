using System;

namespace AppliedInformatics.LaboratoryWork1
{
    /// <summary>
    /// <para>
    /// Класс Matrix создаёт объект, который хранит элементы матрицы в виде двумерного массива matrix.
    /// </para>
    /// <para>
    /// Переопределение операторов:
    /// <list type="bullet">
    ///     <item>
    ///     <c><see cref="this[int, int]"/></c>
    ///     Для удобства обращения к элементам матрицы используется оператор[,].
    ///     </item>
    ///     <item>
    ///     <c><see cref="operator +(Matrix, Matrix)"/> и <see cref="operator *(Matrix, Matrix)"/></c>
    ///     реализуют операции сложения матриц(+) и умножения(*), если над переданными матрицами можно применить
    ///     соответствующие операции, то вернётся новая матрица, в противном случае вызывается исключение.
    ///     </item>
    /// </list>
    /// </para>
    /// <para>
    /// Операции над строками:
    /// <list type="bullet">
    ///    <item>
    ///    <c><see cref="DivideString"/></c> - делит элементы строки на число 
    ///    </item>
    ///    <item>
    ///    <c><see cref="SubtractionStrings"/></c> - вычитает из одной строки другую 
    ///    </item>
    ///    <item>
    ///    <c><see cref="SwapStrings"/></c> - меняет строки местами 
    ///    </item>
    /// </list>
    /// </para>
    /// </summary>
    public class Matrix
    {

        /// <summary>
        /// Поле для хранения элементов матрицы
        /// </summary>

        private readonly double[,] matrix;

        /// <summary>
        /// Свойство, возвращающее информацию о количестве строк в матрице
        /// </summary>

        public int CountStrings
        {
            get
            {
                return matrix.GetLength(0);
            }
        }

        /// <summary>
        /// Свойство, возвращающее информацию о количестве столбцов в матрице
        /// </summary>

        public int CountColumns
        {
            get
            {
                return matrix.GetLength(1);
            }
        }

        /// <summary>
        ///     Конструктор матрицы
        /// </summary>
        /// <param name="CountString">Количество строк</param>
        /// <param name="CountColumn">Количество столбцов в матрице</param>

        public Matrix(int CountString, int CountColumn)
        {
            this.matrix = new double[CountString, CountColumn];
        }

        /// <summary>
        ///     Оператор [,] для обращения к элементу по строке/столбцу
        /// </summary>
        /// <param name="i">Номер строки</param>
        /// <param name="j">Номер стобца</param>

        public double this[int i, int j]
        {
            get
            {
                return this.matrix[i, j];
            }
            set
            {
                this.matrix[i, j] = value;
            }
        }

        /// <summary>
        ///     Определяет операцию сложения для двух матриц
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <exception cref="ArgumentException">Вызывается, если матрицы разных размеров (операция в данном случае не определена)</exception>
        /// <returns>Новая матрица, которая равна left + right</returns>

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if ((left.CountStrings == right.CountStrings) && (right.CountColumns == left.CountColumns))
            {
                Matrix addition = new Matrix(left.CountStrings, left.CountColumns);

                for (int i = 0; i < left.CountStrings; i++)
                {
                    for (int j = 0; j < left.CountColumns; j++)
                    {
                        addition[i, j] += left[i, j] + right[i, j];
                    }
                }
                return addition;
            }
            else
            {
                throw new ArgumentException("Для матриц разных размеров операция сложения не определена");
            }
        }

        /// <summary>
        ///     Определяет операцию умножения для двух матриц
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <exception cref="ArgumentException">Вызывается, если количество столбцов первой матрицы не равно количеству строк во второй</exception>
        /// <returns>Новая матрица, которая равна left * right</returns>

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.CountColumns == right.CountStrings)
            {
                Matrix multiplication = new Matrix(left.CountStrings, right.CountColumns);

                for (int i = 0; i < left.CountStrings; i++)
                {
                    for (int j = 0; j < right.CountColumns; j++)
                    {
                        for (int y = 0; y < right.CountStrings; y++)
                        {
                            multiplication[i, j] += left[i, y] * right[y, j];
                        }
                    }
                }
                return multiplication;

            }
            else
            {
                throw new ArgumentException("Условие 'количество столбцов первой матрицы должно быть равно количеству строк во второй' не выполняется.");
            }
        }

        /// <summary>
        /// Метод класса <c>TransposedMatrix</c> возвращает транспонированную матрицу.
        /// </summary>
        /// <param name="initial">Матрица, для которой нужно получить транспонированную</param>
        /// <returns>Транспонированная матрица</returns>

        public static Matrix TransposedMatrix(Matrix initial)
        {
            Matrix transposed = new Matrix(initial.CountColumns, initial.CountStrings);

            for (int i = 0; i < initial.CountStrings; i++)
            {
                for (int j = 0; j < initial.CountColumns; j++)
                {
                    transposed[j, i] = initial[i, j];
                }
            }
            return transposed;
        }

        /// <summary>
        /// Метод осуществляет копирование матрицы
        /// </summary>
        /// <param name="initial">Матрицы, которую нужно скопировать</param>
        /// <returns>Скопированная матрица</returns>

        public static Matrix CopyMatrix(Matrix initial)
        {
            Matrix copied = new Matrix(initial.CountStrings, initial.CountColumns);

            for (int i = 0; i < initial.CountStrings; i++)
            {
                for (int j = 0; j < initial.CountColumns; j++)
                {
                    copied[i, j] = initial[i, j];
                }
            }

            return copied;
        }

        // Операции над элементами матрицы

        /// <summary>
        /// Делит все элементы строки на заданное число
        /// </summary>
        /// <param name="stringNumber">Номер делимой строки</param>
        /// <param name="divider">Делитель</param>
        /// 

        public void DivideString(int stringNumber, double divider)
        {
            for (int i = 0; i < this.CountColumns; i++)
            {
                this[stringNumber, i] /= divider;
            }
        }

        /// <summary>
        /// Вычитает из строки другую строку
        /// </summary>
        /// <param name="minuendString">Номер уменьшаемой строки</param>
        /// <param name="substrahendString">Номер вычитаемой строки</param>

        public void SubtractionStrings(int minuendString, int substrahendString)
        {
            for (int i = 0; i < this.CountColumns; i++)
            {
                this[minuendString, i] -= this[substrahendString, i];
            }
        }

        /// <summary>
        /// Меняет строки местами
        /// </summary>
        /// <param name="stringNumber1">Номер первой строки</param>
        /// <param name="stringNumber2">Номер второй строки</param>

        public void SwapStrings(int stringNumber1, int stringNumber2)
        {
            double swap;
            for (int i = 0; i < this.CountStrings; i++)
            {
                swap = this[stringNumber1, i];
                this[stringNumber1, i] = this[stringNumber2, i];
                this[stringNumber2, i] = swap;
            }
        }

        /// <summary>
        /// Выводит матрицу в консоль
        /// </summary>

        public void Display()
        {
            for (int i = 0; i < this.CountStrings; i++)
            {
                for (int j = 0; j < this.CountColumns; j++)
                {
                    Console.Write(this[i, j].ToString() + " ");
                }
                Console.Write('\n');
            }
        }
    }
}