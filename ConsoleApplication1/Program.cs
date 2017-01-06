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
            var testset = new double[20, 4];
            var testset_noans = new double[20, 3];

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

            for (int i = 0; i < 20; i++)
            {
                Random r = new Random();
                testset[i, 0] = r.Next(1, 3000);
                testset_noans[i, 0] = testset[i, 0];

                Random r1 = new Random();
                if (i % 2 == 1)
                {
                    testset[i, 3] = 1;
                    if (r1.Next(0, 100) < 80)
                    {
                        testset[i, 1] = 1;
                        testset_noans[i, 1] = testset[i, 1];
                    }
                    else
                    {
                        testset[i, 1] = 0;
                        testset_noans[i, 1] = testset[i, 1];
                    }
                }
                else
                {
                    testset[i, 3] = 0;
                    if (r1.Next(0, 100) < 50)
                    {
                        testset[i, 1] = 0;
                        testset_noans[i, 1] = testset[i, 1];
                    }
                    else
                    {
                        testset[i, 1] = 1;
                        testset_noans[i, 1] = testset[i, 1];
                    }
                }

                Random r2 = new Random();
                if (i%2 == 1)
                {
                    testset[i, 2] = r2.Next(60, 100);
                    testset_noans[i, 2] = testset[i, 2];
                }
                else
                {
                    testset[i, 2] = r2.Next(0, 60);
                    testset_noans[i, 2] = testset[i, 2];
                }
            }

            int NPoints = 70;//     -   training set size, NPoints>=1
            int NVars = 3;//       -   number of independent variables, NVars>=1
            int NClasses = 2;
            int NTrees = 20;//     -   number of trees in a forest, NTrees>=1.
                           // recommended values: 50-100.
            double R = 0.5;

            int info = 0;

            var forest = new alglib.dforest.decisionforest();
            var report = new alglib.dforest.dfreport();
            var prediction = new double[20,2];
            var prediction1 = new double[2];
            


            alglib.dforest.dfbuildrandomdecisionforest(trainingset, NPoints, NVars, NClasses, NTrees, R, ref info, forest, report);

            Console.WriteLine(info);

            for (int i = 0; i < 20; i++)
            {
                var test1 = new double[3];
                test1[0] = testset_noans[i, 0];
                test1[1] = testset_noans[i, 1];
                test1[2] = testset_noans[i, 2];
                alglib.dforest.dfprocess(forest, test1, ref prediction1);
                prediction[i, 0] = prediction1[0];
                prediction[i, 1] = prediction1[1];
            }
            

            Console.WriteLine("break");

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Result was " + testset[i, 3]);
                Console.WriteLine("Prediction is - zero: " + prediction[i,0] + "    one: " + prediction[i,1]);
            }

            Console.Write("");
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
