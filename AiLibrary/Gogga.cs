using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    /// <summary>
    /// Represents a single organism in the population
    /// </summary>
    public class Gogga
    {
        public ushort Id { get; set; } = 0;
        public ushort Age { get; set; } = 0;
        public double Excitation { get; set; } = 0;
        public bool IsAlive { get; set; } = true;
        public double Fitness {get {return FitnessFunction(this);}}
        public Direction LastDirection { get; set; } = Direction.North;
        public Position CurrentPosition { get; set; }
        public Position StartPosition { get; set; }
        public Genome Genome { get; set; }
        public List<double[,]> WeightMatrices { get; set; }
        public Func<Gogga, double> FitnessFunction;
        public Gogga(ushort geneCount)
        {
            Id = 0;
            Age = 0;
            Excitation = 0;           
            LastDirection = Direction.North;
            IsAlive = true;
            CurrentPosition = new Position();
            StartPosition = new Position(CurrentPosition);
            Genome = new Genome(geneCount);
            WeightMatrices = new List<double[,]>();
            FitnessFunction = null;
        }
        public Gogga(ushort id, ushort age, double excitation, Direction direction, 
            Position p, ushort geneCount, Func<Gogga, double> fitnessFunction)
        {
            Id = id;
            Age = age;
            Excitation = excitation;
            LastDirection = direction;
            CurrentPosition = p;
            IsAlive = true;
            StartPosition = new Position(CurrentPosition);
            Genome = new Genome(geneCount);
            WeightMatrices = new List<double[,]>();
            FitnessFunction = fitnessFunction;
        }
        public static double[,] CreateWeightMatrix(int inputCount, int outputCount) 
        {
            return new double[outputCount, inputCount];
        }
        public static Direction GetRandomDirection(Random rand)
        {
            Array directions = Enum.GetValues(typeof(Direction));
            return (Direction)directions.GetValue(rand.Next(directions.Length));
        }
        public void Die(Cell[,] cells)
        {
            IsAlive = false;
            cells[CurrentPosition.X, CurrentPosition.Y].IsOccupied = false;
        }
        /// <summary>
        /// Returns true if the move in the specified direction was successful
        /// </summary>
        /// <param name="w"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public bool Move(World w, Direction d)
        {
            //TODO: Write test code
            ushort curX = CurrentPosition.X;
            ushort curY = CurrentPosition.Y;
            switch (d)
            {
                case Direction.North:
                    if(CurrentPosition.Y < w.Size.Height - 1)
                    {
                        if (!w.Cells[curX, curY + 1].IsOccupied && !w.Cells[curX, curY + 1].IsObstructed)
                        {
                            w.Cells[curX, curY].IsOccupied = false;
                            w.Cells[curX, curY+1].IsOccupied = true;
                            CurrentPosition.Y += 1;
                            return true;
                        }
                    }
                    return false;
                case Direction.NorthEast:
                    if (CurrentPosition.Y < w.Size.Height - 1 && CurrentPosition.X < w.Size.Width - 1)
                    {
                        if (!w.Cells[curX+1, curY + 1].IsOccupied && !w.Cells[curX+1, curY + 1].IsObstructed)
                        {
                            w.Cells[curX, curY].IsOccupied = false;
                            w.Cells[curX+1, curY + 1].IsOccupied = true;
                            CurrentPosition.X += 1;
                            CurrentPosition.Y += 1;
                            return true;
                        }
                    }
                    return false;
                case Direction.East:
                    if (CurrentPosition.X < w.Size.Width - 1)
                    {
                        if (!w.Cells[curX+1, curY].IsOccupied && !w.Cells[curX+1, curY].IsObstructed)
                        {
                            w.Cells[curX, curY].IsOccupied = false;
                            w.Cells[curX + 1, curY].IsOccupied = true;
                            CurrentPosition.X += 1;
                            return true;
                        }
                    }
                    return false;
                case Direction.SouthEast:
                    if (CurrentPosition.X < w.Size.Width - 1 && CurrentPosition.Y > 0)
                    {
                        if (!w.Cells[curX + 1, curY - 1].IsOccupied && !w.Cells[curX + 1, curY - 1].IsObstructed)
                        {
                            w.Cells[curX, curY].IsOccupied = false;
                            w.Cells[curX + 1, curY - 1].IsOccupied = true;
                            CurrentPosition.X += 1;
                            CurrentPosition.Y -= 1;
                            return true;
                        }
                    }
                    return false;
                case Direction.South:
                    if (CurrentPosition.Y > 0)
                    {
                        if (!w.Cells[curX, curY - 1].IsOccupied && !w.Cells[curX, curY - 1].IsObstructed)
                        {
                            w.Cells[curX, curY].IsOccupied = false;
                            w.Cells[curX, curY - 1].IsOccupied = true;
                            CurrentPosition.Y -= 1;
                            return true;
                        }
                    }
                    return false;
                case Direction.SouthWest:
                    if (CurrentPosition.X > 0 && CurrentPosition.Y > 0)
                    {
                        if (!w.Cells[curX - 1, curY - 1].IsOccupied && !w.Cells[curX - 1, curY - 1].IsObstructed)
                        {
                            w.Cells[curX, curY].IsOccupied = false;
                            w.Cells[curX - 1, curY - 1].IsOccupied = true;
                            CurrentPosition.X -= 1;
                            CurrentPosition.Y -= 1;
                            return true;
                        }
                    }
                    return false;
                case Direction.West:
                    if (CurrentPosition.X > 0)
                    {
                        if (!w.Cells[curX - 1, curY].IsOccupied && !w.Cells[curX - 1, curY].IsObstructed)
                        {
                            w.Cells[curX, curY].IsOccupied = false;
                            w.Cells[curX - 1, curY].IsOccupied = true;
                            CurrentPosition.X -= 1;
                            return true;
                        }
                    }
                    return false;
                case Direction.NorthWest:
                    if (CurrentPosition.X > 0 && CurrentPosition.Y < w.Size.Height - 1)
                    {
                        if (!w.Cells[curX - 1, curY + 1].IsOccupied && !w.Cells[curX - 1, curY + 1].IsObstructed)
                        {
                            w.Cells[curX, curY].IsOccupied = false;
                            w.Cells[curX - 1, curY + 1].IsOccupied = true;
                            CurrentPosition.X -= 1;
                            CurrentPosition.Y += 1;
                            return true;
                        }
                    }
                    return false;
                default:
                    throw new InvalidOperationException("Move method recieved invalid parameters");
            }
        }
        public static Direction MoveRandom(Random r)
        {
            return (Direction)r.Next(8);
        }
        /// <summary>
        /// Creates a copy of itself at a given position with exact same genome
        /// </summary>
        /// <returns></returns>
        public Gogga SpawnExact(World w, Position p)
        {
            var g = new Gogga((ushort)this.Genome.Length())
            {
                Id = 0,
                Age = 0,
                Excitation = 0,
                IsAlive = true,
                LastDirection = Gogga.GetRandomDirection(w.Rand),
                CurrentPosition = p,
                StartPosition = new Position(CurrentPosition),
                Genome = this.Genome.CloneExact(),
                WeightMatrices = this.WeightMatrices,
                FitnessFunction = this.FitnessFunction
            };
            return g;
        }
    }
    public enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }
}
