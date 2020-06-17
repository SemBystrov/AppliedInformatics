using AppliedInformatics.LaboratoryWork1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AppliedInformatics.LaboratoryWork6
{
    public class DifferentialEquation
    {
        public delegate double Fx(Matrix YX);
        
        protected Fx fx; 
        public DifferentialEquation(Fx fx)
        {
            this.fx = fx;
        }

        public Matrix SolutionRungeKutta4(Matrix YX0, double h0, double m)
        {
            Matrix YX = Matrix.CopyMatrix(YX0);
            double h = h0 / m;

            for (int j = 1; j <= m; j++)
            {
                double k1 = this.fx(YX);
                Matrix RKi = new Matrix(2, 1);
                RKi[0, 0] = (h * k1) / 2;
                RKi[1, 0] = h / 2;
                double k2 = this.fx(YX + RKi);
                RKi[0, 0] = (h * k2) / 2;
                double k3 = this.fx(YX + RKi);
                RKi[0, 0] = h * k3;
                RKi[1, 0] = h;
                double k4 = this.fx(YX + RKi);

                YX[0, 0] += (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                YX[1, 0] = YX0[1, 0] + j * h;
            }

            return YX;
        }
    }
}
