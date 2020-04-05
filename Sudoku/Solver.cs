using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Solver
    {
        private int[,] sudokuMatrixCopy;
        public Solver()
        {

        }

        public int[,] RunSolve(int[,] sudokuMatrix)
        {
            //Store a copy of data to work with
            this.sudokuMatrixCopy = sudokuMatrix;

            Solve(sudokuMatrixCopy);

            return sudokuMatrixCopy;
        }

        private bool Solve(int[,] sudokuMatrix)
        {
            int row = 0;
            int col = 0;
            bool foundEmpty = false;

            for (int i = 0; i < sudokuMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < sudokuMatrix.GetLength(1); j++)
                {
                    if (sudokuMatrix[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        foundEmpty = true;
                        break;
                    }
                }
                if (foundEmpty)
                {
                    break;
                }
            }


            if (foundEmpty)
            {
                for(int n = 1; n <= sudokuMatrix.GetLength(0); n++)
                {
                    if (IsUnique(sudokuMatrix, row, col, n))
                    {
                        sudokuMatrix[row, col] = n;
                        Solve(sudokuMatrix);
                    }
                }


            } else
            {
                this.sudokuMatrixCopy = sudokuMatrix;
                return true;
            }
        }

        private bool IsUnique(int[,] sudokuMatrix,int row, int col, int num)
        {
            //Check uniqueness in column
            for (int r = 0; r < sudokuMatrix.GetLength(0); r++)
            {
                if (sudokuMatrix[r, col] == num)
                {
                    return false;
                }
            }

            //Check uniqueness in row
            for (int c = 0; c < sudokuMatrix.GetLength(0); c++)
            {
                if (sudokuMatrix[row, c] == num)
                {
                    return false;
                }
            }

            int squareRoot = (int)Math.Sqrt(sudokuMatrix.GetLength(0));
            int subMatrixRowOffset = row - row % squareRoot;
            int subMatrixColOffset = col - col % squareRoot;

            //Check uniqueness in sub-matrix
            for (int r = subMatrixRowOffset; r < subMatrixRowOffset + squareRoot; r++)
            {
                for (int c = subMatrixColOffset; c < subMatrixColOffset + squareRoot; c++)
                {
                    if (sudokuMatrix[r, c] == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
