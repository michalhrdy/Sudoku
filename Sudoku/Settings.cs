using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    static class Settings
    {
        public const int matrixSize = 9;
        public const int subMatrixSize = 3;
        public const char delimiter = ',';

        public static int[,] defaultData = 
        {
            { 6,0,7,1,4,3,8,2,0 },
            { 9,2,0,5,0,0,3,0,0 },
            { 0,1,3,2,9,0,7,0,5 },
            { 0,6,0,8,7,0,9,0,3 },
            { 0,9,0,0,0,1,0,0,4 },
            { 0,4,5,0,0,0,6,8,1 },
            { 0,7,0,9,6,8,4,0,2 },
            { 4,3,6,0,0,2,0,9,0 },
            { 2,0,9,4,0,5,0,6,7 }
        };
    }
}
