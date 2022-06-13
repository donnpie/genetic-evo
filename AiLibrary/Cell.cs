using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    /// <summary>
    /// Represents a cell on the map of the world
    /// </summary>
    public class Cell
    {
        public Position Position {get;set;}
        /// <summary>
        /// Indicates if the cell is occupied by a Gogga
        /// </summary>
        public bool IsOccupied { get; set; }
        /// <summary>
        /// Indicates that a cell contains a permanet obstruction
        /// </summary>
        public bool IsObstructed { get; set; }
        public ushort FoodValue { get; set; }
        public ushort PheromoneValue { get; set; }
        public Cell()
        {
            Position = new Position();
            IsOccupied = false;
            IsObstructed = false;
            FoodValue = 0;
            PheromoneValue = 0;
        }

        //Todo: Should methods that operate on collections of cells be located here?
    }
}
