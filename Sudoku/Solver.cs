using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Solver
    {
        //Stores a copy of data to work with
        private int[,] sudokuMatrixCopy;
        public Solver()
        {

        }

        //Makes a copy of input data and runs Solve()
        public int[,] RunSolve(int[,] sudokuMatrix)
        {
            this.sudokuMatrixCopy = sudokuMatrix;

            Solve(sudokuMatrixCopy);

            return sudokuMatrixCopy;
        }

        //Solves the Sudoku and stores result in sudokuMatrixCopy
        private bool Solve(int[,] sudokuMatrix)
        {
            int row = 0;
            int col = 0;

            //If there are no empty fields left copy the result
            if (!FindEmpty(ref row,ref col,ref sudokuMatrix))
            {
                this.sudokuMatrixCopy = sudokuMatrix;
                return true;
            }

            //Try to find viable number to fit in an empty cell
            for (int n = Settings.minValue; n <= sudokuMatrix.GetLength(0); n++)
            {
                //If number is unique in column, row and submatrix
                //place it in an empty spot and call Solve recursively
                if (IsUnique(sudokuMatrix, row, col, n))
                {
                    sudokuMatrix[row, col] = n;

                    if (Solve(sudokuMatrix))
                    {
                        return true;
                    }
                    else
                    {
                        sudokuMatrix[row, col] = 0;
                    }
                }
            }
            return false;
        }

        //Sets row and col to first found empty position in sudokuMatrix
        //Returns false if there are no more epmty cells left
        private bool FindEmpty(ref int row, ref int col, ref int[,] sudokuMatrix)
        {
            for (int i = 0; i < sudokuMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < sudokuMatrix.GetLength(1); j++)
                {
                    if (sudokuMatrix[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        return true;
                    }
                }
            }
            return false;
        }

        //Checks if the number is unique in row, column and sub-matrix
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

            //Calculate Sub-Matrix offset
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
