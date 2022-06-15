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
        /// <summary>
        /// Returns an empty array of type double with rows = outputCount
        /// and columns = inputCount
        /// </summary>
        /// <param name="inputCount"></param>
        /// <param name="outputCount"></param>
        /// <returns></returns>
        public static double[,] CreateWeightMatrix(int inputCount, int outputCount) 
        {
            return new double[outputCount, inputCount];
        }
        /// <summary>
        /// Returns a random Direction object
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Direction GetRandomDirection(Random r)
        {
            return (Direction)r.Next(8);
        }
        /// <summary>
        /// Sets IsAlive = false and sets corresponding cell's IsOccupied to false
        /// </summary>
        /// <param name="cells"></param>
        public void Die(Cell[,] cells)
        {
            IsAlive = false;
            cells[CurrentPosition.X, CurrentPosition.Y].IsOccupied = false;
        }
        /// <summary>
        /// Decodes the output of the neural network and moves gogga accordingly
        /// </summary>
        /// <param name="b"></param>
        /// <param name="w"></param>
        public void DecodeMove(byte b, World w)
        {
            //output encoding: 
            // 0: 0000: Don't move
            // 1: 0001: move north 
            // 2: 0010: move east
            // 3: 0011: move northeast
            // 4: 0100: move south
            // 5: 0101: move random
            // 6: 0110: move southeast
            // 7: 0111: move random
            // 8: 1000: move west
            // 9: 1001: Move northwest
            //10: 1010: move random
            //11: 1011: move random
            //12: 1100: Move southwest
            //13: 1101: move random
            //14: 1110: move random
            //15: 1111: move random
            switch (b)
            {
                case 0:
                    return;
                case 1:
                    Move(w, Direction.North);
                    return;
                case 2:
                    Move(w, Direction.East);
                    return;
                case 3:
                    Move(w, Direction.NorthEast);
                    return;
                case 4:
                    Move(w, Direction.South);
                    return;
                case 5:
                    Move(w, Gogga.GetRandomDirection(w.Rand));
                    return;
                case 6:
                    Move(w, Direction.SouthEast);
                    return;
                case 7:
                    Move(w, Gogga.GetRandomDirection(w.Rand));
                    return;
                case 8:
                    Move(w, Direction.West);
                    return;
                case 9:
                    Move(w, Direction.NorthWest);
                    return;
                case 10:
                    Move(w, Gogga.GetRandomDirection(w.Rand));
                    return;
                case 11:
                    Move(w, Gogga.GetRandomDirection(w.Rand));
                    return;
                case 12:
                    Move(w, Direction.SouthWest);
                    return;
                case 13:
                    Move(w, Gogga.GetRandomDirection(w.Rand));
                    return;
                case 14:
                    Move(w, Gogga.GetRandomDirection(w.Rand));
                    return;
                case 15:
                    Move(w, Gogga.GetRandomDirection(w.Rand));
                    return;
                default:
                    break;
            }
        }
        /// <summary>
        /// Returns true if the move in the specified direction was successful
        /// </summary>
        /// <param name="w"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        /// 
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
        /// <summary>
        /// Create the weight matrices for the gogga according to the genome
        /// </summary>
        /// <param name="sensorCount"></param>
        /// <param name="hiddenNeuronCount"></param>
        /// <param name="outputNeuronCount"></param>
        public void CreateNeuralNet(int sensorCount, int hiddenNeuronCount, int outputNeuronCount)
        {
            double[,] m1 = Gogga.CreateWeightMatrix(sensorCount, hiddenNeuronCount);
            double[,] m2 = Gogga.CreateWeightMatrix(hiddenNeuronCount, outputNeuronCount);
            WeightMatrices.Add(m1);
            WeightMatrices.Add(m2);
            for (int j = 0; j < Genome.Length(); j++)
            {
                DecodedGene dg = Genome.Genes[j].DecodeGene(Gene.SCALE);
                if (!dg.Layer)
                {
                    m1[dg.SinkAddress % hiddenNeuronCount, dg.SourceAddress % sensorCount] = dg.Weight;
                }
                else
                    m2[dg.SinkAddress % outputNeuronCount, dg.SourceAddress % hiddenNeuronCount] = dg.Weight;
            }
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
        /// <summary>
        /// Creates a copy of itself at a given position with a genome that is possibly mutated.
        /// If the genome is mutated, the weight matrices are recalculated
        /// </summary>
        /// <param name="w"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public Gogga SpawnWithMutation(World w, Position p, out bool mutationHappened)
        {
            //TODO: Create test code
            var g = new Gogga((ushort)this.Genome.Length())
            {
                Id = 0,
                Age = 0,
                Excitation = 0,
                IsAlive = true,
                LastDirection = Gogga.GetRandomDirection(w.Rand),
                CurrentPosition = p,
                StartPosition = new Position(CurrentPosition),
                Genome = this.Genome.Mutate(w.Rand, out mutationHappened, w.MutationRate),
                FitnessFunction = this.FitnessFunction
            };
            if (mutationHappened)
            {
                g.CreateNeuralNet(w.SensorCount, w.HiddenNeuronCount, w.OutputNeuronCount);
            }
            else g.WeightMatrices = this.WeightMatrices;
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
