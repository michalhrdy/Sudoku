using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuData
    {
        public int[,] NumberMatrix { get; set; }
        public SudokuData()
        {
            FillMatrix();
        }

        public void LoadFromFile(string fileName)
        {
            var stream = new StreamReader(fileName);
            int[,] tempMatrix = new int[Settings.matrixSize, Settings.matrixSize];

            for (int i = 0; i < NumberMatrix.GetLength(0); i++)
            {
                string[] line = stream.ReadLine().Split(',');

                if (line.Length == NumberMatrix.GetLength(0))
                {
                    for (int j = 0; j < NumberMatrix.GetLength(0); j++)
                    {
                        if (line[j].Length > 1)
                        {
                            throw new Exception("File format error: Numbers must be single digit");
                        };

                        tempMatrix[i, j] = Int32.Parse(line[j]);
                    }
                }
                else
                {
                    throw new Exception("File format error: Wrong number of columns");
                }
            }

            NumberMatrix = tempMatrix;
        }

        public void SaveToFile(string fileName)
        {
            string[] lines = new string[NumberMatrix.GetLength(0)];

            for (int i = 0; i < NumberMatrix.GetLength(0); i++)
            {
                string line = string.Empty;
                for (int j = 0; j < NumberMatrix.GetLength(0); j++)
                {
                    line = line + NumberMatrix[i, j].ToString() + Settings.delimiter;
                }
                line = line.Remove(line.Length - 1);
                lines[i] = line;
            }

            System.IO.File.WriteAllLines(fileName, lines);
        }

        //Checks if data contains a valid sudoku
        public bool CheckSudokuDataValidity()
        {
            //Check rows
            for (int i = 0; i < NumberMatrix.GetLength(0); i++)
            {
                int flag = 0;

                for (int j = 0; j < NumberMatrix.GetLength(0); j++)
                {
                    if (NumberMatrix[i, j] != 0)
                    {
                        int bit = 1 << NumberMatrix[i, j];
                        if ((flag & bit) != 0) return false;
                        flag |= bit;
                    }
                }
            }

            //Check columns
            for (int i = 0; i < NumberMatrix.GetLength(1); i++)
            {
                int flag = 0;

                for (int j = 0; j < NumberMatrix.GetLength(1); j++)
                {
                    if (NumberMatrix[j, i] != 0)
                    {
                        int bit = 1 << NumberMatrix[j, i];
                        if ((flag & bit) != 0) return false;
                        flag |= bit;
                    }
                }
            }

            int subSize = (int)Math.Sqrt(NumberMatrix.GetLength(0));
            //Check sub-matrices
            for (int i = 0; i < NumberMatrix.GetLength(0); i += subSize)
            {
                for (int j = 0; j < NumberMatrix.GetLength(1); j += subSize)
                {
                    if (!CheckSubMatrix(subSize, i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Sub-matrix checking method called from CheckSudokuDataValidity()
        private bool CheckSubMatrix(int subSize, int startRow, int startCol)
        {
            int flag = 0;

            for (int i = startRow; i < startRow + subSize; i++)
            {
                for (int j = startCol; j < startCol + subSize; j++)
                {
                    if (NumberMatrix[i, j] != 0)
                    {
                        int bit = 1 << NumberMatrix[i, j];
                        if ((flag & bit) != 0) return false;
                        flag |= bit;
                    }
                }
            }

            return true;
        }

        private void FillMatrix()
        {
            //NumberMatrix = Settings.defaultData;

            NumberMatrix = new int[Settings.matrixSize,Settings.matrixSize];
            for (int i = 0; i < NumberMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < NumberMatrix.GetLength(1); j++)
                {
                    NumberMatrix[i, j] = 0;
                }
            }
        }
    }
}
