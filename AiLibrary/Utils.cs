using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    public class Utils
    {
        #region Transfer functions for neurons
        public static double Sigmoid(double input)
        {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }
        public static double Tanh(double input)
        {
            return Math.Tanh(input);
        }
        public static double Linear(double input)
        {
            if (input >= -1 && input <= 1) return input;
            else if (input > 1) return 1;
            else return -1;          
        }
        /// <summary>
        /// Step function with threshold = 0.5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Step05(double input)
        {
            if (input >= 0.5) return 1;
            else return 0;
        }
        #endregion
        #region Functions for sensors
        /// <summary>
        /// Scans the three cells north of the current cell for neighbours
        /// </summary>
        /// <param name="w">Represents the current world of the simulation</param>
        /// <param name="g">Represents the current instance of Gogga</param>
        /// <returns>Returns an double between -1 and +1</returns>
        public static double NeighboursNorth(World w, Gogga g)
        {
            ushort x = g.CurrentPosition.X;
            ushort y = g.CurrentPosition.Y;
            ushort northBoundary = w.Size.Height;
            ushort eastBoundary = w.Size.Width;
            bool validX = x > 0 && x < eastBoundary - 1;
            bool validY = y >= 0 && y < northBoundary - 1;
            double result = 0;
            if (validX && validY)
            {
                List<Cell> cells = new List<Cell>();
                cells.Add(w.Cells[x-1,y+1]);
                cells.Add(w.Cells[x,y+1]);
                cells.Add(w.Cells[x+1,y+1]);
                result = CalcOccupiedCells(cells);
            }
            else if (x == 0 && validY)
            {
                List<Cell> cells = new List<Cell>();
                cells.Add(w.Cells[x, y + 1]);
                cells.Add(w.Cells[x + 1, y + 1]);
                result = CalcOccupiedCells(cells);
            }
            else if (validY)
            {
                List<Cell> cells = new List<Cell>();
                cells.Add(w.Cells[x-1, y + 1]);
                cells.Add(w.Cells[x, y + 1]);
                result = CalcOccupiedCells(cells);
            }
            return result;
        }
        public static double RandomInput(World w, Gogga g)
        {
            return w.Rand.NextDouble();
        }
        private static double CalcOccupiedCells(List<Cell> cells)
        {
            double result = 0;
            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i].IsOccupied)
                {
                    //TODO: when moving/creating a gogga, indicate which cell it occupies
                    result += 0.33;
                }
            }
            return result;
        }
        #endregion
        #region Linear algebra functions
        /// <summary>
        /// Returns the n*1 vector from multiplying (n*m) matrix and (m*1) vector 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double[] MMult(double[,] matrix, double[] vector)
        {
            int mRows = matrix.GetLength(0);
            int mCols = matrix.GetLength(1);
            int vRows = vector.GetLength(0);
            int vCols = vector.Rank;
            if (mCols != vRows) throw new ArgumentException("Dimensions of matrix and vector are not compatible for multiplication");
            if (vCols != 1) throw new ArgumentException("Dimensions of input vector are not compatible for multiplication");
            double[] result = new double[mRows];
            for (int i = 0; i < mRows; i++)
            {
                for (int j = 0; j < mCols; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }
            return result;
        }
        #endregion
        #region Other functions
        public static void PrintBinary(long value)
        {
            string binary = Convert.ToString(value, 2);
            Console.WriteLine(binary);
        }
        public static void PrintBinary(int value)
        {
            string binary = Convert.ToString(value, 2);
            Console.WriteLine(binary);
        }
        public static void PrintBinary(uint value)
        {
            string binary = Convert.ToString(value, 2);
            Console.WriteLine(binary);
        }
        public static void PrintHex(uint value)
        {
            string binary = Convert.ToString(value, 16);
            Console.WriteLine(binary);
        }
        public static void PrintBinary(ushort value)
        {
            string binary = Convert.ToString(value, 2);
            Console.WriteLine(binary);
        }
        public static bool IsBitSet(long value, int pos)
        {
            long mask = 0b1;
            mask <<= pos;
            return (value & mask) != 0;
        }
        public static uint HexStringToUnit32(string hexString)
        {
            return uint.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
        }
        /// <summary>
        /// Raises 2 to the power of y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static uint PowOf2(uint y)
        {
            if (y == 0) return 1;
            uint result = 2;
            for (int i = 1; i < y; i++)
            {
                result *= 2;
            }
            return result;
        }
        #endregion

    }
}
