using System;
using System.Collections.Generic;
using System.Data;

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
        public int[] Run()
        {
            throw new NotImplementedException();
            //step 1:
            //find min in each row and substruct
            for (int row = 0; row < _size; row++)
            {
                //initial smallest
                int smallest = Int32.MaxValue;
                //find smallest
                for (int col = 0; col < _size; col++)
                {                 
                    if (_workMatrix[row,col] < smallest)
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

            //step 2:
            int[] _rowsCovered = new int[_size];
            int[] _colsCovered= new int[_size];
            List<int> assignedRows = new List<int>();
            List<int> assignedCols = new List<int>();
            List<int> unassignedRows = new List<int>();
            int[,] zeroes = new int[_size, _size];
            //for each row, assign a zero (in case the col doesn't have a zero assigned yet)
            for (int row = 0; row < _size; row++)
            {
                bool unassigned = true;
                for(int col = 0; col < _size; col++)
                {
                    if (_workMatrix[row, col] == 0)

                    {
                        if (unassigned)
                        {
                            if (!assignedCols.Contains(col))
                            {
                                zeroes[row, col] = 1;
                                assignedCols.Add(col);
                                assignedRows.Add(row);
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
                    if ((col == _size - 1) && unassigned)
                    {
                        unassignedRows.Add(row);
                    }
                }

            }
            //mark all rows which are unnassigned to a zero
            //for each of these rows - mark the columns with unassigned zeroes.
            //                       - in these columns, mark rows with assigned zeros
            List<int> markedCols = new List<int>();
            foreach (var row in unassignedRows)
            {
                for (int col = 0; col < _size; col++)
                {
                    if (zeroes[row, col] == 2)
                    {
                        if (!markedCols.Contains(col))
                        {
                            markedCols.Add(col);
                        }
                    }
                }
            }
        List<int> markedRows = new List<int>();        
        foreach (var col in markedCols)
        {
            for (int row = 0; row < _size; row++)
            {
                if (zeroes[row, col] == 1)
                {
                    if (!markedRows.Contains(row))
                    {
                        markedRows.Add(row);
                    }
                }
            }
        }

            //draw lines: through all marked columns and -unmarked- rows!
            //count the number of lines - if equals to n - we finished -> go to done
            int cover = markedCols.Count + (_size - markedRows.Count);

            //TODO:fIND AND RETURN THE ASSIGNMENT

            //step 3: (creating more zeros)
            //find the minimum unmarked element
            int smallestUncovered = Int32.MaxValue;
            for (int row = 0; row < _size; row++)
            {
                if(_rowsCovered[row] == 1) continue;
                for (int col = 0; col < _size; col++)
                {
                    if (_colsCovered[col] == 1) continue;
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
                    if (_rowsCovered[row] == 1 && _colsCovered[col] == 1)
                    {
                        _workMatrix[row, col] += smallestUncovered;
                    }
                    else if (_rowsCovered[row] == 0 && _colsCovered[col] == 0)
                    {
                        _workMatrix[row, col] -= smallestUncovered;
                    }
                }
            }

            //back to step 2.
        }

        //private void InitMatches()
        //{
            
        //}
    }
}