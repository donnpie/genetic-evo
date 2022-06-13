using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    /// <summary>
    /// Represents a position on a two-dimensional map
    /// The value (0,0) represents the bottom left most position on the map
    /// </summary>
    public class Position
    {
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public Position()
        {
            X = 0;
            Y = 0;
        }
        public Position(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }
        public Position(Position other)
        {
            X = other.X;
            Y = other.Y;
        }
        /// <summary>
        /// Creates a new position, ignoring obstructions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static Position GetNewRandomPosition(ushort width, ushort height, Random rand)
        {
            Position p = new Position((ushort)rand.Next(width), (ushort)rand.Next(height));
            return p;
        }
        /// <summary>
        /// Creates a new position, taking into account obstructed and occupied cells
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static Position GetNewRandomPositionNotOnObstruction(World w)
        {
            const int maxTries = 100;
            int i = 0;
            Position p;
            while (i < maxTries)
            {
                p = Position.GetNewRandomPosition(w.Size.Width, w.Size.Height, w.Rand);
                if (w.Cells[p.X, p.Y].IsObstructed || w.Cells[p.X, p.Y].IsOccupied)
                {
                    i++;
                    continue;
                }
                else
                {
                    return p;
                }        
            }
            throw new InvalidOperationException("Could not find a valid position");
        }
        public static double GetDistBetweenTwoPositions(Position p1, Position p2)
        {
            int xDist = p1.X - p2.X;
            int yDist = p1.Y - p2.Y;
            double xsq = (double)(xDist * xDist);
            double ysq = (double)(yDist * yDist);
            double result = Math.Sqrt(xsq+ysq);
            return result;
        }
    }
}
