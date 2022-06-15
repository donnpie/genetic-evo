using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    public class World
    {
        public Size Size { get; set; }
        public uint GenerationsCount { get; set; }
        public ushort StepsPerGeneration { get; set; }
        public ushort GeneCount { get; set; }
        ushort obstructionCount;
        public ushort ObstructionCount { get { return obstructionCount; } set {
                if (Size.GetArea() < value)
                    throw new ArgumentException($"Value of ObstructionCount ({value}) cannot be greater than number of cells ({Size.GetArea()}).");
                else obstructionCount = value;
            } }
        ushort foodCellCount;
        public ushort FoodCellCount { get { return foodCellCount; } set {
                int maxFoodPossible = Size.GetArea() - ObstructionCount;
                if (maxFoodPossible < value || maxFoodPossible < 0) throw new ArgumentException($"Value of FoodCellCount {value} cannot be greater than number of available cells {Math.Max(maxFoodPossible, 0)}.Reduce FoodCellCount and/or ObstructionCount");
                else foodCellCount = value;
            }
        }
        public ushort MaxFoodValue { get; set; }
        ushort initialPopulation;
        public ushort InitialPopulation { get { return initialPopulation; } set {
                int maxGoggasPossible = Size.GetArea() - ObstructionCount;
                if (maxGoggasPossible < value || maxGoggasPossible < 0) throw new ArgumentException($"Value of InitialPopulation {value} cannot be greater than number of available cells {Math.Max(maxGoggasPossible, 0)}.Reduce InitialPopulation and/or ObstructionCount");
                else initialPopulation = value;
            }
        }
        double mutationRate;
        public double MutationRate { get { return mutationRate; }
            set
            {
                if (value < 0) {throw new ArgumentException("Mutation rate cannot be less than 0."); }
                mutationRate = value;
            }
        }
        //Fitness function setup - TODO: add as property to world
        public Func<Gogga, double> FitnessFunction;
        public Random Rand { get; private set; }
        //public NeuralNet neuralNet { get; set; }
        public Cell[,] Cells { get; set; }
        public List<Gogga> Goggas { get; set; }
        public List<Cell> Obstructions { get; set; }
        public List<Cell> FoodCells { get; set; }
        public int SensorCount {get; set;} //Must be greater than 0
        public Sensor[] Sensors { get; set; }
        public int HiddenNeuronCount { get; set; } //Must be greater than 0
        public Neuron[] HiddenNeurons { get; set; }
        public int OutputNeuronCount { get; set; } //Must be greater than 0
        public Neuron[] OutputNeurons { get; set; }
        public World()
        {
            //This constructor should be used for testing purposes only
            Size = new Size();
            GenerationsCount = 0;
            StepsPerGeneration = 0;
            ObstructionCount = 0;
            FoodCellCount = 0;
            InitialPopulation = 0;
            SensorCount = 0;
            Rand = new Random();
            Cells = null;
            Goggas = new List<Gogga>();
            Obstructions = new List<Cell>();
            FoodCells = new List<Cell>();
            FitnessFunction = null;
            Sensors = null;
            HiddenNeuronCount = 0;
            OutputNeuronCount = 0;
        }
        /// <summary>
        /// Correctly initialises a World object. After object is initialised, the following actions must be performed:
        /// CreateCells
        /// CreateObstructions
        /// CreateFood
        /// CreateGoggas
        /// Set up neurons
        /// CreateNeuralNets
        /// </summary>
        /// <param name="size"></param>
        /// <param name="initPop"></param>
        /// <param name="numOfGens"></param>
        /// <param name="stepsPerGen"></param>
        /// <param name="numOfGenes"></param>
        /// <param name="numOfObstruct"></param>
        /// <param name="numOfFoodCells"></param>
        /// <param name="maxFoodValue"></param>
        /// <param name="fitnessFunction"></param>
        /// <param name="sensorCount"></param>
        /// <param name="hiddenNeuronCount"></param>
        /// <param name="outputNeuronCount"></param>
        public World(Size size, ushort initPop, uint numOfGens, ushort stepsPerGen,
                ushort numOfGenes, ushort numOfObstruct, ushort numOfFoodCells,
                ushort maxFoodValue, Func<Gogga, double> fitnessFunction,
                int sensorCount, int hiddenNeuronCount, int outputNeuronCount
        )
        {
            Size = size;
            GenerationsCount = numOfGens;
            StepsPerGeneration = stepsPerGen;
            GeneCount = numOfGenes;
            ObstructionCount = numOfObstruct;
            FoodCellCount = numOfFoodCells;
            MaxFoodValue = maxFoodValue;
            InitialPopulation = initPop;
            Rand = new Random();
            Cells = new Cell[Size.Width, Size.Height];
            Goggas = new List<Gogga>();
            Obstructions = new List<Cell>();
            FoodCells = new List<Cell>();
            FitnessFunction = fitnessFunction;
            SensorCount = sensorCount;
            Sensors = new Sensor[SensorCount];
            HiddenNeuronCount = hiddenNeuronCount;
            HiddenNeurons = new Neuron[HiddenNeuronCount];
            OutputNeuronCount = outputNeuronCount;
            OutputNeurons = new Neuron[OutputNeuronCount];
        }
        public void CreateCells()
        {
            if (Size == null) throw new InvalidOperationException("Size is null - cannot create cells. Use a constructor to initialise World object");
            Cells = new Cell[Size.Width, Size.Height];
            for (int i = 0; i < Size.Width; i++)
            {
                for (int j = 0; j < Size.Height; j++)
                {
                    Cell cell = new Cell();
                    cell.Position = new Position((ushort)i, (ushort)j);
                    Cells[i, j] = cell;
                }
            }
        }
        /// <summary>
        /// Creates random obstructions on the map. The number of obstructions is set by <paramref name="NumberOfObstructions"/>,
        /// You must run <c>CreateCells</c> before running this method.
        /// </summary>
        /// <param name="rand"></param>
        public void CreateObstructions()
        {
            if (Cells is null) throw new InvalidOperationException("World object not initialised properly. Use a constructor and not inline intitialisation");
            if (Cells.Length == 0) throw new InvalidOperationException("World object not initialised properly. Size property cannot be set to (0,0)");
            if (Cells[0, 0] is null) throw new InvalidOperationException("Method CreateCells must be called before method CreateObstructions");
            for (int i = 0; i < ObstructionCount; i++)
            {
                var p = Position.GetNewRandomPosition(Size.Width, Size.Height, Rand);
                Cells[p.X, p.Y].IsObstructed = true;
                Obstructions.Add(Cells[p.X, p.Y]);
            }
        }
        /// <summary>
        /// Creates random food on the map. The number of cells with food is set by <paramref name="NumberOfFoodCells"/>.
        /// The maximum value of food that can be deposited on a cell is set by <paramref name="MaxFoodValue"/>.
        /// You must run <c>CreateObstructions</c> before running this method.
        /// </summary>
        /// <param name="rand"></param>
        public void CreateFood()
        {
            if (Cells is null) throw new InvalidOperationException("World object not initialised properly. Use a constructor and not inline intitialisation");
            if (Cells.Length == 0) throw new InvalidOperationException("World object not initialised properly. Size property cannot be set to (0,0)");
            if (Cells[0, 0] is null) throw new InvalidOperationException("Method CreateCells must be called before method CreateObstructions");
            if (FoodCellCount == 0) return;
            if (MaxFoodValue == 0) return;
            int i = 0;
            while (i < FoodCellCount)
            {
                var p = Position.GetNewRandomPosition(Size.Width, Size.Height, Rand);
                if (Cells[p.X, p.Y].IsObstructed)
                {
                    continue;
                }
                Cells[p.X, p.Y].FoodValue = (ushort)Math.Abs(Rand.Next(MaxFoodValue));
                FoodCells.Add(Cells[p.X, p.Y]);
                i++;
            }
        }
        /// <summary>
        /// Creates a list of new Goggas with random positions and random genomes. 
        /// Obstructed and occupied cells are ignored so that Goggas are only placed on available cells.
        /// Returns the actual number of goggas that were created - this may not always be equal to
        /// the number that was requested.
        /// </summary>
        /// <param name="numberOfGoggas">The number of new goggas to be created</param>
        /// <param name="goggaList">The list to which the goggas will be added</param>
        public int CreateGoggas(ushort numberOfGoggas, List<Gogga> goggaList)
        {
            if (numberOfGoggas == 0) throw new ArgumentException("numberOfGoggas cannot be 0.");
            int maxGoggasPossible = Size.GetArea() - Obstructions.Count - goggaList.Count;
            if (maxGoggasPossible < 1) throw new ArgumentException("maxNumberOfGoggas cannot be less than 1.");
            
            List<Cell> availableCells = new List<Cell>();
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (!Cells[i, j].IsObstructed && !Cells[i, j].IsOccupied)
                        availableCells.Add(Cells[i, j]);
                    else
                        continue;
                }
            }
            int availableCellCount = availableCells.Count;
            int numberOfGoggasToBuild = Math.Min(numberOfGoggas, maxGoggasPossible);
            int k = 0;
            int watchdog = 0;
            while (k < numberOfGoggasToBuild)
            {
                int randomCellIdx = Rand.Next(availableCellCount);
                Cell randomCell = availableCells[randomCellIdx];
                var p = new Position(randomCell.Position);         
                if (randomCell.IsObstructed || randomCell.IsOccupied) 
                {
                    //Note: because this method uses a random value to allocate cells,
                    //it will not always be possible to create the target number of cells
                    //especially when the map has very few available cells left
                    watchdog++;
                    if (watchdog > 500) break;
                    continue;
                }
                var g = new Gogga((ushort)k, 0, 0, Gogga.GetRandomDirection(Rand), p, 8, FitnessFunction);
                g.Genome = Genome.GetNewRandomGenome(GeneCount, Rand);
                randomCell.IsOccupied = true; //Need to mark cells as occupied
                goggaList.Add(g);
                k++;
            }
            return k;
        }
        /// <summary>
        /// Create weight matrices for all goggas in Goggas list according to each gogga's genome
        /// </summary>
        public void CreateNeuralNets()
        {
            //TODO: write test code
            for (int i = 0; i < Goggas.Count; i++)
            {
                Goggas[i].CreateNeuralNet(SensorCount, HiddenNeuronCount, OutputNeuronCount);
            }
        }
        /// <summary>
        /// Checks if the number of occupied cells is the same as the number of goggas
        /// </summary>
        public bool AuditCellOccupation()
        {
            int occupiedCells = GetNumberOfOccupiedCells();            
            return occupiedCells == Goggas.Count;
        }
        public int GetNumberOfOccupiedCells() {
            int occupiedCells = 0;
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (Cells[i, j].IsOccupied) occupiedCells++;
                }
            }
            return occupiedCells;
        }
    }
}
