using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using Generic.Models;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var trainingset = new double[100,4];

            for (int i = 0; i < 100; i++)
            {
                Random r = new Random();
                trainingset[i, 0] = r.Next(1, 3000);

                Random r1 = new Random();
                if (i%2 == 1)
                {
                    trainingset[i, 3] = 1;
                    if (r1.Next(0, 100) < 80)
                    {
                        trainingset[i, 1] = 1;
                    }
                    else
                    {
                        trainingset[i, 1] = 0;
                    }
                }
                else
                {
                    trainingset[i, 3] = 0;
                    if (r1.Next(0, 100) < 50)
                    {
                        trainingset[i, 1] = 0;
                    }
                    else
                    {
                        trainingset[i, 1] = 1;
                    }
                }
                
                Random r2 = new Random();
                if (i%2 == 1)
                {
                    trainingset[i, 2] = r2.Next(60, 100);
                }
                else
                    trainingset[i, 2] = r2.Next(0, 60);
            }

            int NPoints = 100;//     -   training set size, NPoints>=1
            int NVars = 3;//       -   number of independent variables, NVars>=1
            int NClasses = 1;
            int NTrees = 20;//     -   number of trees in a forest, NTrees>=1.
                           // recommended values: 50-100.
            double R = 0.5;

            int info = 0;

            alglib.decisionforest df;
            alglib.dfreport rep;

            alglib.dfbuildrandomdecisionforest(trainingset, NPoints, NVars, NClasses, NTrees, R, out info, out df, out rep);

            Console.WriteLine(info);

            //int size = 6;
            //int[,] matrix = new int[size,size];
            //Random r = new Random();
            //for (int i = 0; i < size; i++)
            //{
            //    for (int j = 0; j < size; j++)
            //    {
            //        matrix[i, j] = r.Next(1,3000);
            //    }
            //}

            //Console.WriteLine("Initial cost");
            //for (int i = 0; i < size; i++)
            //{
            //    Console.Write("[");
            //    for (int j = 0; j < size -1; j++)
            //    {
            //        Console.Write(matrix[i,j] + ", ");
            //    }
            //    Console.Write(matrix[i, size-1]);
            //    Console.Write("]\n");
            //}

            //MatchingAlgorithm Algo = new MatchingAlgorithm(matrix);
            //int[,] mat = Algo.Run();

            //Console.WriteLine("Final assignment");
            //for (int i = 0; i < size; i++)
            //{
            //    Console.Write("[");
            //    for (int j = 0; j < size - 1; j++)
            //    {
            //        Console.Write(mat[i, j] + ", ");
            //    }
            //    Console.Write(mat[i, size - 1]);
            //    Console.Write("]\n");
            //}
            //Console.Read();
        }
    }
}
