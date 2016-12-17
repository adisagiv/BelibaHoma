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
            int size = 6;
            int[,] matrix = new int[size,size];
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = r.Next(1,3000);
                }
            }

            Console.WriteLine("Initial cost");
            for (int i = 0; i < size; i++)
            {
                Console.Write("[");
                for (int j = 0; j < size -1; j++)
                {
                    Console.Write(matrix[i,j] + ", ");
                }
                Console.Write(matrix[i, size-1]);
                Console.Write("]\n");
            }

            MatchingAlgorithm Algo = new MatchingAlgorithm(matrix);
            int[,] mat = Algo.Run();

            Console.WriteLine("Final assignment");
            for (int i = 0; i < size; i++)
            {
                Console.Write("[");
                for (int j = 0; j < size - 1; j++)
                {
                    Console.Write(mat[i, j] + ", ");
                }
                Console.Write(mat[i, size - 1]);
                Console.Write("]\n");
            }
            Console.Read();
        }
    }
}
