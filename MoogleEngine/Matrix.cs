using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    internal class Matrix
    {
            public double[,] Row { get; private set; }

            public Matrix(double[,] arrays)
            {
                this.Row = arrays;
            }

            public static double[,] Suma(double[,] a, double[,] b)
            {
                double[,] suma = new double[a.GetLength(0), a.GetLength(1)];
                if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                {
                    throw new ArgumentException("NO se pueden sumar matrices de distintas dimensiones");
                }
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    for (int j = 0; j < a.GetLength(1); j++)
                    {
                        suma[i, j] = a[i, j] + b[i, j];
                    }
                }
                return suma;
            }
            public static double[,] Producto(double[,] a, double[,] b)
            {
                double[,] mult = new double[a.GetLength(0), b.GetLength(1)];
                if (a.GetLength(1) != b.GetLength(0))
                {
                    throw new ArgumentException("NO se pueden multiplicar");
                }
                for (int i = 0; i < mult.GetLength(0); i++)
                {
                    for (int j = 0; j < mult.GetLength(1); j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < a.GetLength(1); k++)
                        {
                            sum = sum + a[i, k] * b[k, j];
                        }
                        mult[i, j] = sum;
                    }
                }
                return mult;
            }

            public static double[,] Transpuesta(double[,] a)
            {
                double[,] Ñ = new double[a.GetLength(1), a.GetLength(0)];
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    for (int j = 0; j < a.GetLength(1); j++)
                    {
                        Ñ[i, j] = a[j, i];
                    }
                }

                return Ñ;
            }
            public static double Deter(double[,] a)
            {
                if (a.GetLength(0) != a.GetLength(1))
                {
                    throw new ArgumentException("NO se puede hallar determinante");
                }
                double deter = 0;
                if (a.GetLength(0) == 1)
                {
                    deter = a[0, 0];
                }
                else if (a.GetLength(0) == 2)
                {
                    deter = a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0];
                }
                else
                {
                    for (int i = 0; i < a.GetLength(0); i++)
                    {
                        double[,] menor = new double[a.GetLength(0) - 1, a.GetLength(0) - 1];
                        for (int j = 1; j < a.GetLength(0); j++)
                        {
                            for (int k = 0; k < a.GetLength(0); k++)
                            {
                                if (k < i)
                                {
                                    menor[j - 1, k] = a[j, k];
                                }
                                else if (k > i)
                                {
                                    menor[j - 1, k - 1] = a[j, k];
                                }
                            }
                        }
                        double signo = Math.Pow(-1, i);
                        double cofactor = signo * Deter(menor);
                        deter += a[0, i] * cofactor;
                    }
                }
                return deter;
            }

            public static double[] MatrizXvector(double[,] a, double[] b)
            {
                if (a.GetLength(0) != b.Length)
                {
                    throw new ArgumentException("NO se puede multiplicar");
                }
                double[] MxV = new double[a.GetLength(0)];
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    double sumar = 0;
                    for (int j = 0; j < b.Length; j++)
                    {
                        sumar += a[i, j] * b[j];
                    }
                    MxV[i] = sumar;
                }
                return MxV;
            }
            public static double[,] MatrizXescalar(double[,] a, int b)
            {
                double[,] MxE = new double[a.GetLength(0), a.GetLength(1)];
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    for (int j = 0; j < a.GetLength(1); j++)
                    {
                        MxE[i, j] = MxE[i, j] * b;
                    }
                }
                return MxE;
            }
            public static double[] VectorSuma(double[] a, double[] b)
            {
                if (a.Length != b.Length)
                {
                    throw new ArgumentException("NO se puede sumar");
                }
                double[] Vsuma = new double[a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    Vsuma[i] = a[i] + b[i];
                }
                return Vsuma;
            }
            public static double[] VectorMult(double[] a, double[] b)
            {
                if (a.Length != b.Length)
                {
                    throw new ArgumentException("NO se puede multiplicar");
                }
                double[] Vmult = new double[a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    Vmult[i] += a[i] * b[i];
                }
                return Vmult;
            }
            public static double[] VectorXescalar(double[] a, double b)
            {
                double[] VxE = new double[a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    VxE[i] = a[i] * b;
                }
                return VxE;
            }

        
    }
}
