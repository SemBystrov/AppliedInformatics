using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using AppliedInformatics.LaboratoryWork1;
using AppliedInformatics.LaboratoryWork4;

namespace AppliedInformatics.LaboratoryWork7
{
    public class DifferentialSystem
    {
        public delegate double Fx(Matrix YX);

        protected Fx[] fx;
        public DifferentialSystem(Fx[] fx)
        {
            this.fx = fx;
        }

        private Matrix RKi(int countY, double k)
        {
            Matrix RKi = new Matrix(countY + 1, 1);
            for (int i = 0; i < countY; i++)
                RKi[i, 0] = k;
            return RKi;
        }

        public Matrix SolutionRungeKutta4(Matrix YX0, double h0, double m)
        {
            Matrix YX = Matrix.CopyMatrix(YX0);
            double h = h0 / m;
            Console.WriteLine(h);
            for (int i = 1; i <= m; i++)
            {
                Matrix Ki = new Matrix(4, YX.CountStrings);
                Matrix RKi;
                // Вычисление k1

                for (int j = 0; j < this.fx.Length; j++)
                    Ki[0, j] = h * this.fx[j](YX);

                Ki[0, Ki.CountColumns - 1] = h;
                Ki[1, Ki.CountColumns - 1] = h;
                Ki[2, Ki.CountColumns - 1] = h;
                Ki[3, Ki.CountColumns - 1] = h;
                
                // Вычисление k2

                RKi = Ki.Slice(0, 0, 0, Ki.CountColumns - 1);
                RKi.DivideString(0, 2);

                for (int j = 0; j < this.fx.Length; j++)    
                    Ki[1, j] = h * this.fx[j](YX + Matrix.TransposedMatrix(RKi));


                // Вычисление k3

                RKi = Ki.Slice(1, 0, 1, Ki.CountColumns - 1);
                RKi.DivideString(0, 2);

                for (int j = 0; j < this.fx.Length; j++)
                    Ki[2, j] = h * this.fx[j](YX + Matrix.TransposedMatrix(RKi));

                // Вычисление k4

                RKi = Ki.Slice(2, 0, 2, Ki.CountColumns - 1);

                for (int j = 0; j < this.fx.Length; j++)
                    Ki[3, j] = h * this.fx[j](YX + Matrix.TransposedMatrix(RKi));

                Ki.Display();

                // Вычисление y
                for (int j = 0; j < this.fx.Length; j++)
                    YX[j, 0] += (1.0 / 6) * (Ki[0, j] + 2 * Ki[1, j] + 2 * Ki[2, j] + Ki[3, j]);
                
                YX[YX.CountStrings - 1, 0] += h;

                YX.Display();
            }

            return YX;
        }
    }
}
