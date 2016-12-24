using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BelibaHoma.BLL.Services
{
    using System.Collections.Generic;

    public class MatchingAlgorithm
    {
        
        private readonly int[,] _costMatrix;
        private int[,] _workMatrix;
        private int _size;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="costMatrix"></param>
        public MatchingAlgorithm(int[,] costMatrix)
        {
            _costMatrix = costMatrix;
            _workMatrix = costMatrix;
            _size = costMatrix.GetLength(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[,] Run()
        {
 
            int step = 1;
            int[] _rowsCovered = new int[_size];
            int[] _colsCovered = new int[_size];
            int[,] zeroes = new int[_size, _size];
            Console.Write("In Algo.Run");
            while (step != 4)
            {
                switch (step)
                {
                    case 1:
                        Console.WriteLine("In step 1");
                        //step 1:
                        //find min in each row and substruct
                        for (int row = 0; row < _size; row++)
                        {
                            //initial smallest
                            int smallest = Int32.MaxValue;
                            //find smallest
                            for (int col = 0; col < _size; col++)
                            {
                                if (_workMatrix[row, col] < smallest)
                                {
                                    smallest = _workMatrix[row, col];
                                }
                                if (smallest == 0) break;
                            }

                            //substruct smallest
                            for (int col = 0; col < _size; col++)
                            {
                                _workMatrix[row, col] -= smallest;
                            }
                        }

                        //find min in each col and substruct 
                        for (int col = 0; col < _size; col++)
                        {
                            int smallest = Int32.MaxValue;
                            //find smallest
                            for (int row = 0; row < _size; row++)
                            {
                                if (_workMatrix[row, col] < smallest)
                                {
                                    smallest = _workMatrix[row, col];
                                }
                                if (smallest == 0) break;
                            }

                            //substruct smallest
                            for (int row = 0; row < _size; row++)
                            {
                                _workMatrix[row, col] -= smallest;
                            }
                        }
                        step = 2;
                        break;

                    case 2:
                        Console.WriteLine("In step 2");
                        //step 2:
                        _rowsCovered = new int[_size];
                        _colsCovered = new int[_size];
                        zeroes = new int[_size, _size];
                        //for each row, assign a zero (in case the col doesn't have a zero assigned yet)
                        for (int row = 0; row < _size; row++)
                        {
                            bool unassigned = true;
                            for (int col = 0; col < _size; col++)
                            {
                                if (_workMatrix[row, col] == 0)
                                {
                                    if (unassigned)
                                    {
                                        if (_colsCovered[col] == 0)
                                        {
                                            zeroes[row, col] = 1;
                                            _colsCovered[col] = 2;
                                            _rowsCovered[row] = 1;
                                            unassigned = false;
                                        }
                                        else
                                        {
                                            zeroes[row, col] = 2;
                                        }
                                    }
                                    else
                                    {
                                        zeroes[row, col] = 2;
                                    }
                                }
                            }
                        }

                        //mark all rows which are unnassigned to a zero
                        //for each of these rows - mark the columns with unassigned zeroes.
                        //                       - in these columns, mark rows with assigned zeros
                        List<int> newlyMarkedRows = new List<int>();
                        List<int> newlyMarkedCols = new List<int>();

                        //initially mark rows unassigned and in them newly mark cols with unassigned zeroes
                        for (int row = 0; row < _size; row++)
                        {
                            if (_rowsCovered[row] == 0)
                            {
                                //mark unassigned rows
                                _rowsCovered[row] = 3;
                                //find cols which have zeroes in that row
                                for (int col = 0; col < _size; col++)
                                {
                                    if (zeroes[row, col] == 2)
                                    {
                                        _colsCovered[col] = 3;
                                        newlyMarkedCols.Add(col);
                                    }
                                }
                            }
                        }

                        //while exist newly marked cols - mark rows with assigned zeroes.
                        //in newly marked rows, mark cols with unassigned zeroes
                        while (newlyMarkedCols.Count != 0)
                        {
                            foreach (var col in newlyMarkedCols)
                            {
                                for (int row = 0; row < _size; row++)
                                {
                                    if (zeroes[row, col] == 1)
                                    {
                                        newlyMarkedRows.Add(row);
                                        _rowsCovered[row] = 3;
                                    }
                                }
                            }
                            newlyMarkedCols = new List<int>();
                            while (newlyMarkedRows.Count != 0)
                            {
                                foreach (var row in newlyMarkedRows)
                                {
                                    for (int col = 0; col < _size; col++)
                                    {
                                        if (zeroes[row, col] != 0 && _colsCovered[col] != 3)
                                        {
                                            newlyMarkedCols.Add(col);
                                            _colsCovered[col] = 3;
                                        }
                                    }
                                }
                                newlyMarkedRows = new List<int>();
                            }
                        }

                        //draw lines: through all marked columns and -unmarked- rows!
                        //count the number of lines - if equals to n - we finished -> go to done
                        int countCover = 0;
                        for (int i = 0; i < _size; i++)
                        {
                            if (_colsCovered[i] == 3) countCover++;
                            if (_rowsCovered[i] != 3) countCover++;
                        }
                        step = countCover < _size ? 3 : 4;
                        break;

                    case 3:
                        Console.WriteLine("In step 3");
                        //step 3: (creating more zeros)
                        //find the minimum unmarked element
                        int smallestUncovered = Int32.MaxValue;
                        for (int row = 0; row < _size; row++)
                        {
                            if (_rowsCovered[row] != 3) continue;
                            for (int col = 0; col < _size; col++)
                            {
                                if (_colsCovered[col] == 3) continue;
                                if (_workMatrix[row, col] < smallestUncovered)
                                {
                                    smallestUncovered = _workMatrix[row, col];
                                }
                            }
                        }
                        //add / substruct smallestUnmarked from all elements needed
                        for (int row = 0; row < _size; row++)
                        {
                            for (int col = 0; col < _size; col++)
                            {
                                if (_rowsCovered[row] != 3 && _colsCovered[col] == 3)
                                {
                                    _workMatrix[row, col] += smallestUncovered;
                                }
                                else if (_rowsCovered[row] == 3 && _colsCovered[col] != 3)
                                {
                                    _workMatrix[row, col] -= smallestUncovered;
                                }
                            }
                        }
                        step = 2;
                        break;
                }
            }

            int[] countRowZeros = new int[_size];
            int[] countColZeros = new int[_size];

            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    if (zeroes[row, col] > 0)
                    {
                        countColZeros[col]++;
                        countRowZeros[row]++;
                    }
                }
            }

            int countAssignments = 0;
            while (countAssignments < _size)
            {
                int minRow = Int32.MaxValue;
                int minRowLoc = -1;
                for (int i = 0; i < _size; i++)
                {
                    if (countRowZeros[i] > 0 && countRowZeros[i] < minRow)
                    {
                        minRow = countRowZeros[i];
                        minRowLoc = i;
                    }
                }

                int minCol = Int32.MaxValue;
                int minColLoc = -1;
                for (int i = 0; i < _size; i++)
                {
                    if (countColZeros[i] > 0 && countColZeros[i] < minCol)
                    {
                        minCol = countColZeros[i];
                        minColLoc = i;
                    }
                }

                if (minRowLoc == -1 && minColLoc == -1)
                {
                    break;
                }

                if (minRow < minCol)
                {
                    bool assigned = false;
                    for (int i = 0; i < _size; i++)
                    {
                        if (assigned == false && zeroes[minRowLoc,i] > 0 && countColZeros[i] > 0)
                        {
                            zeroes[minRowLoc, i] = 5;
                            countRowZeros[minRowLoc] = 0;
                            countColZeros[i] = 0;
                            assigned = true;
                            countAssignments++;
                            for (int j = 0; j < _size; j++)
                            {
                                if (zeroes[j, i] > 0 && j != minRowLoc)
                                {
                                    if (countRowZeros[j] > 0) countRowZeros[j]--;
                                }
                            }
                        }
                        else if (zeroes[minRowLoc, i] > 0)
                        {
                            if (countColZeros[i] > 0) countColZeros[i]--;
                        }
                    }
                }
                else
                {
                    bool assigned = false;
                    for (int i = 0; i < _size; i++)
                    {
                        if (assigned == false && countRowZeros[i] > 0 && zeroes[i, minColLoc] > 0)
                        {
                            zeroes[i, minColLoc] = 5;
                            countColZeros[minColLoc] = 0;
                            countRowZeros[i] = 0;
                            assigned = true;
                            countAssignments++;
                            for (int j = 0; j < _size; j++)
                            {
                                if (zeroes[i, j] > 0 && j != minColLoc)
                                {
                                    if (countColZeros[j] > 0) countColZeros[j]--;
                                }
                            }
                        }
                        else if (zeroes[i, minColLoc] > 0)
                        {
                            if (countRowZeros[i] > 0) countRowZeros[i]--;
                        }
                    }
                }
            }

            return zeroes;
        }
    }
}