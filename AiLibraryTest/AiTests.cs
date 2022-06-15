using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AiLibrary;

namespace AiLibraryTest
{
    public class AiTests
    {
        #region World tests
        [Fact]
        public void ObstructionCount_TooManyObstructions_ShouldFail()
        {
            Assert.Throws<ArgumentException>(()=> new World
            {
                Size = new Size(10, 10),
                ObstructionCount = 101
            });
        }
        [Fact]
        public void FoodCellCount_TooManyObstructions_ShouldFail()
        {
            Assert.Throws<ArgumentException>(() => new World
            {
                Size = new Size(10, 10),
                ObstructionCount = 90,
                FoodCellCount = 100
            });
        }
        [Fact]
        public void InitialPopulation_TooManyObstructions_ShouldFail()
        {
            Assert.Throws<ArgumentException>(() => new World
            {
                Size = new Size(10, 10),
                ObstructionCount = 90,
                InitialPopulation = 100
            });
        }
        [Fact]
        public void SetWorldSize_PassValidArgs_ShouldWork()
        {
            ushort height = 500;
            ushort width = 400;

            World world = new World
            {
                Size = new Size(width, height)
            };
            Assert.Equal(height, world.Size.Height);
            Assert.Equal(width, world.Size.Width);
        }
        [Fact]
        public void SetWorldSize_WidthGreaterThanMax_ShouldThrowError()
        {
            //1920 x 1080
            ushort height = 500;
            ushort width = 5000;
            World world = new World();
            Assert.Throws<ArgumentException>(() => world.Size = new Size(width, height));
        }
        [Fact]
        public void SetWorldSize_HeightGreaterThanMax_ShouldThrowError()
        {
            ushort height = 5000;
            ushort width = 500;
            World world = new World();
            Assert.Throws<ArgumentException>(() => world.Size = new Size(width, height));
        }
        [Fact]
        public void SetInitialPopulation_ValidValue_ShouldWork()
        {
            ushort initPop = 1000;
            World world = new World
            {
                Size = new Size(50, 50),
                InitialPopulation = initPop
            };
            Assert.Equal(initPop, world.InitialPopulation);
        }
        [Fact]
        public void SetNumberOfGenerations_ValidValue_ShouldWork()
        {
            ushort gens = 4000;
            World world = new World
            {
                GenerationsCount = gens
            };
            Assert.Equal(gens, world.GenerationsCount);
        }
        [Fact]
        public void SetStepsPerGeneration_ValidValue_ShouldWork()
        {
            ushort steps = 3000;
            World world = new World
            {
                StepsPerGeneration = steps
            };
            Assert.Equal(steps, world.StepsPerGeneration);
        }
        [Fact]
        public void CreateObstructions_ShouldWork()
        {
            Random r = new Random();
            World w = new World
            {
                Size = new Size(50, 30),
                ObstructionCount = 30
            };
            w.CreateCells();
            w.CreateObstructions();
            Assert.Equal(w.ObstructionCount, w.Obstructions.Count);
        }
        [Fact]
        public void CreateObstructions_CellsAreEmpty_ShouldFail()
        {
            Random r = new Random();
            var w = new World
            (
                size: new Size(50, 30),
                initPop: 1000,
                numOfGens: 100000,
                stepsPerGen: 300,
                numOfGenes: 8,
                numOfObstruct: 30,
                numOfFoodCells: 0,
                maxFoodValue: 200,
                fitnessFunction: null, //TODO: FIX this
                sensorCount: 3,
                hiddenNeuronCount: 3,
                outputNeuronCount: 3
            );
            Assert.Throws<InvalidOperationException>(()=> w.CreateObstructions());
        }
        [Fact]
        public void CreateObstructions_ImproperInitialisation_ShouldFail()
        {
            Random r = new Random();
            World w = new World
            {
                Size = new Size(50, 30),
                ObstructionCount = 30
            };
            Assert.Throws<InvalidOperationException>(() => w.CreateObstructions());
        }
        [Fact]
        public void CreateObstructions_DefaultConstructor_ShouldFail()
        {
            World w = new World();
            w.CreateCells();
            Assert.Throws<InvalidOperationException>(() => w.CreateObstructions());
        }
        [Theory]
        [InlineData(0, 0)]
        [InlineData(10, 30)]
        [InlineData(1000, 30)]
        public void CreateFood_ShouldWork(ushort food, ushort obs)
        {
            Random r = new Random();
            World w = new World
            {
                Size = new Size(50, 30),
                ObstructionCount = obs,
                FoodCellCount = food,
                MaxFoodValue = 20,
            };
            w.CreateCells();
            w.CreateObstructions();
            w.CreateFood();
            Assert.Equal(w.FoodCellCount, w.FoodCells.Count);
        }
        [Fact]
        public void CreateFood_CellsAreNull_ShouldFail()
        {
            Random r = new Random();
            World w = new World
            {
                Size = new Size(50, 30),
                ObstructionCount = 10,
                FoodCellCount = 100,
                MaxFoodValue = 20,
            };
            Assert.Throws<InvalidOperationException>(() => w.CreateFood());
        }
        [Fact]
        public void CreateFood_CellsAreEmpty_ShouldFail()
        {
            Random r = new Random();
            var w = new World
            (
                size: new Size(50, 30),
                initPop: 100,
                numOfGens: 100000,
                stepsPerGen: 300,
                numOfGenes: 8,
                numOfObstruct: 30,
                numOfFoodCells: 100,
                maxFoodValue: 200,
                fitnessFunction: null, //TODO: FIX this
                sensorCount: 3,
                hiddenNeuronCount: 3,
                outputNeuronCount: 3
            );
            Assert.Throws<InvalidOperationException>(() => w.CreateFood());
        }
        //[Fact]
        //public void CreateFood_DefaultConstructor_ShouldFail()
        //{
        //    //Can no longer throw this error after changing the setter for FoodCellCount
        //    Random r = new Random();
        //    World w = new World();
        //    w.Size.Width = 10;
        //    w.Size.Height = 10;
        //    w.FoodCellCount = 10;
        //    w.MaxFoodValue = 10;
        //    w.CreateCells();
        //    Assert.Throws<InvalidOperationException>(() => w.CreateFood(r));
        //}
        [Theory]
        [InlineData(10, 30, 10)]
        [InlineData(10, 30, 270)]
        [InlineData(700, 700, 50)]
        public void CreateGoggas_ShouldWork(ushort width, ushort height, ushort goggaCount)
        {
            World w = new World
            {
                Size = new Size(width, height),
                InitialPopulation = goggaCount,
                GeneCount = 8
            };
            w.CreateCells();
            w.CreateObstructions();
            w.CreateFood();
            w.CreateGoggas(w.InitialPopulation, w.Goggas);
            Assert.Equal(goggaCount, w.Goggas.Count);
        }
        #endregion
        #region Neuron test
        [Fact]
        public void SetNeuronId_ValidValue_ShouldWork()
        {
            byte id = 127;
            Neuron n = new Neuron
            {
                Id = id
            };
            Assert.Equal(id, n.Id);
        }
        [Fact]
        public void SetNeuronId_InvalidValue_ShouldThrowError()
        {
            byte id = 128;
            
            Assert.Throws<ArgumentException>(()=> new Neuron
            {
                Id = id
            });
        }
        [Theory]
        [InlineData(5, 0.993)]
        [InlineData(0, 0.5)]
        [InlineData(-5, 0.007)]
        public void Neuron_CalculateOutput(double input, double output)
        {
            var n = new Neuron
            {
                TransferFunction = Utils.Sigmoid
            };
            double result = n.CalcOutput(input);
            Assert.Equal(output, result, 3);
        }
        #endregion
        #region Gene tests
        [Fact]
        public void GetLayer_ShouldReturnFalse()
        {
            var g = new Gene(0b0111_1111_1111_1111_1111_1111_1111_1111);
            bool result = g.GetLayer();
            Assert.False(result);
        }
        [Fact]
        public void GetLayer_ShouldReturnTrue()
        {
            var g = new Gene(0b1000_0000_0000_0000_0000_0000_0000_0000);
            bool result = g.GetLayer();
            Assert.True(result);
        }
        [Theory]
        [InlineData(0b0111_1111, 0b1111_1111_1111_1111_1111_1111_1111_1111)]
        [InlineData(0b0100_0001, 0b1100_0001_1111_1111_1111_1111_1111_1111)]
        [InlineData(0b0000_1111, 0b0000_1111_1111_1111_1111_1111_1111_1111)]
        [InlineData(0b0000_0000, 0b0000_0000_1111_1111_1111_1111_1111_1111)]
        [InlineData(0b0001_1000, 0b0001_1000_1111_1111_1111_1111_1111_1111)]
        public void GetSourceAddress_ShouldWork(byte expected, uint geneVal)
        {
            var g = new Gene(geneVal);
            byte actual = g.GetSourceAddress();
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(0b0111_1111, 0b1111_1111_0111_1111_1111_1111_1111_1111)]
        [InlineData(0b0100_0001, 0b1111_1111_0100_0001_1111_1111_1111_1111)]
        [InlineData(0b0000_1111, 0b1111_1111_0000_1111_1111_1111_1111_1111)]
        [InlineData(0b0000_0000, 0b1111_1111_0000_0000_1111_1111_1111_1111)]
        [InlineData(0b0001_1000, 0b1111_1111_0001_1000_1111_1111_1111_1111)]
        public void GetSinkAddress_ShouldWork(byte expected, uint geneVal)
        {
            var g = new Gene(geneVal);
            byte actual = g.GetSinkAddress();
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData("00007FFF", Gene.SCALE, 4.29483622)]
        [InlineData("00000001", Gene.SCALE, 0.00013107)]
        [InlineData("80000000", Gene.SCALE, 0.0)]
        [InlineData("00000000", Gene.SCALE, 0)]
        [InlineData("0000FFFF", Gene.SCALE, -0.00013107)] 
        [InlineData("00008000", Gene.SCALE, -4.2949673)]
        public void GetWeight_ShouldWork(string hexStr, double scale, double expected)
        {
            var g = new Gene(Utils.HexStringToUnit32(hexStr));
            double actual = g.GetWeight(scale);
            Assert.Equal(expected, actual, 8);
        }
        #endregion
        #region Genome tests
        [Theory]
        [InlineData(0)]
        [InlineData(1001)]
        public void GetNewRandomGenome_ShouldThrowError(ushort count)
        {
            Random r = new Random();
            Assert.Throws<ArgumentException>(()=> Genome.GetNewRandomGenome(count, r));
        }
        [Fact]
        public void GetNewRandomGenome_ShouldWork()
        {
            Random r = new Random();
            ushort geneCount = 8;
            var g = Genome.GetNewRandomGenome(geneCount, r);
            Assert.Equal(geneCount, g.Genes.Length);
        }
        #endregion
        #region Input and transfer function tests
        [Theory]
        [InlineData(5, 0.993)]
        [InlineData(0, 0.5)]
        [InlineData(-5, 0.007)]
        public void Sigmoid_ValidValue_ShouldCalculate(double input, double output)
        {          
            double result = Utils.Sigmoid(input);
            Assert.Equal(output, result, 3);
        }
        [Fact]
        public void NeighboursNorth_3Neighbours_ShouldCalculate()
        {
            World w = new World();
            w.Size.Height = 50;
            w.Size.Width = 30;
            w.CreateCells(); //todo:create test for CreateCells
            Position p = new Position(10, 5);
            Gogga g = new Gogga(8);
            g.CurrentPosition = p;
            w.Cells[p.X - 1, p.Y + 1].IsOccupied = true;
            w.Cells[p.X, p.Y + 1].IsOccupied = true;
            w.Cells[p.X + 1, p.Y + 1].IsOccupied = true;
            double actual = Utils.NeighboursNorth(w, g);
            Assert.Equal(0.99, actual);
        }
        [Fact]
        public void NeighboursNorth_2NeighboursCenterRight_ShouldCalculate()
        {
            World w = new World();
            w.Size.Height = 50;
            w.Size.Width = 30;
            w.CreateCells(); //todo:create test for CreateCells
            Position p = new Position(10, 5);
            Gogga g = new Gogga(8);
            g.CurrentPosition = p;
            w.Cells[p.X, p.Y + 1].IsOccupied = true;
            w.Cells[p.X + 1, p.Y + 1].IsOccupied = true;
            double actual = Utils.NeighboursNorth(w, g);
            Assert.Equal(0.66, actual);
        }
        [Fact]
        public void NeighboursNorth_2NeighboursLeftCenter_ShouldCalculate()
        {
            World w = new World();
            w.Size.Height = 50;
            w.Size.Width = 30;
            w.CreateCells(); //todo:create test for CreateCells
            Position p = new Position(10, 5);
            Gogga g = new Gogga(8);
            g.CurrentPosition = p;
            w.Cells[p.X - 1, p.Y + 1].IsOccupied = true;
            w.Cells[p.X, p.Y + 1].IsOccupied = true;
            double actual = Utils.NeighboursNorth(w, g);
            Assert.Equal(0.66, actual);
        }
        [Fact]
        public void NeighboursNorth_2NeighboursLeftRight_ShouldCalculate()
        {
            World w = new World();
            w.Size.Height = 50;
            w.Size.Width = 30;
            w.CreateCells(); //todo:create test for CreateCells
            Position p = new Position(10, 5);
            Gogga g = new Gogga(8);
            g.CurrentPosition = p;
            w.Cells[p.X - 1, p.Y + 1].IsOccupied = true;
            w.Cells[p.X + 1, p.Y + 1].IsOccupied = true;
            double actual = Utils.NeighboursNorth(w, g);
            Assert.Equal(0.66, actual);
        }
        [Fact]
        public void NeighboursNorth_1NeighbourCenter_ShouldCalculate()
        {
            World w = new World();
            w.Size.Height = 50;
            w.Size.Width = 30;
            w.CreateCells(); //todo:create test for CreateCells
            Position p = new Position(10, 5);
            Gogga g = new Gogga(8);
            g.CurrentPosition = p;
            w.Cells[p.X, p.Y + 1].IsOccupied = true;
            double actual = Utils.NeighboursNorth(w, g);
            Assert.Equal(0.33, actual);
        }
        [Fact]
        public void NeighboursNorth_0Neighbours_ShouldCalculate()
        {
            World w = new World();
            w.Size.Height = 50;
            w.Size.Width = 30;
            w.CreateCells(); //todo:create test for CreateCells
            Position p = new Position(10, 5);
            Gogga g = new Gogga(8);
            g.CurrentPosition = p;
            double actual = Utils.NeighboursNorth(w, g);
            Assert.Equal(0.0, actual);
        }
        [Fact]
        public void NeighboursNorth_xIs0_2Neighbours_ShouldCalculate()
        {
            World w = new World();
            w.Size.Height = 50;
            w.Size.Width = 30;
            w.CreateCells(); //todo:create test for CreateCells
            Position p = new Position(0, 5);
            Gogga g = new Gogga(8);
            g.CurrentPosition = p;
            w.Cells[p.X, p.Y + 1].IsOccupied = true;
            w.Cells[p.X + 1, p.Y + 1].IsOccupied = true;
            double actual = Utils.NeighboursNorth(w, g);
            Assert.Equal(0.66, actual);
        }
        [Fact]
        public void NeighboursNorth_xIsMax_2Neighbours_ShouldCalculate()
        {
            World w = new World();
            w.Size.Width = 30;
            w.Size.Height = 50;
            w.CreateCells(); //todo:create test for CreateCells
            Position p = new Position(29, 5);
            Gogga g = new Gogga(8);
            g.CurrentPosition = p;
            w.Cells[p.X - 1, p.Y + 1].IsOccupied = true;
            w.Cells[p.X, p.Y + 1].IsOccupied = true;
            double actual = Utils.NeighboursNorth(w, g);
            Assert.Equal(0.66, actual);
        }
        #endregion
        #region Cell tests
        [Fact]
        public void CreateCells_ValidSize_ShouldCalculate()
        {
            ushort height = 50;
            ushort width = 30;
            World w = new World();
            w.Size.Height = height;
            w.Size.Width = width;
            w.CreateCells(); //todo:create test for CreateCells
            int cellCount = w.Cells.GetLength(0) * w.Cells.GetLength(1);
            Assert.Equal(width*height, cellCount);
        }
        #endregion
        #region Position tests
        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(0, 0, 3, 4, 5)]
        [InlineData(3, 4, 0, 0, 5)]
        [InlineData(2, 1, 6, 9, 8.94)]
        public void GetDistBetweenTwoPositions_shouldWork(ushort x1, ushort y1, ushort x2, ushort y2, double expected)
        {
            var p1 = new Position(x1, y1);
            var p2 = new Position(x2, y2);
            double actual = Position.GetDistBetweenTwoPositions(p1, p2);
            Assert.Equal(expected, actual, 2);
        }
        #endregion
        #region Utilities tests
        [Fact]
        public void MMult_ShouldWork()
        {
            double[,] matrix = new double[4, 3];
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;
            matrix[0, 2] = 3;
            matrix[1, 0] = 4;
            matrix[1, 1] = 5;
            matrix[1, 2] = 6;
            matrix[2, 0] = 7;
            matrix[2, 1] = 8;
            matrix[2, 2] = 9;
            matrix[3, 0] = 10;
            matrix[3, 1] = 11;
            matrix[3, 2] = 12;

            double[] vector = new double[3];
            vector[0] = 1;
            vector[1] = 2;
            vector[2] = 3;

            double[] result = Utils.MMult(matrix, vector);

            Assert.Equal(14, result[0]);
            Assert.Equal(32, result[1]);
            Assert.Equal(50, result[2]);
            Assert.Equal(68, result[3]);
        }
        [Fact]
        public void MMult_PassNullVector_ShouldThrowError()
        {
            double[,] matrix = new double[4, 3];
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;
            matrix[0, 2] = 3;
            matrix[1, 0] = 4;
            matrix[1, 1] = 5;
            matrix[1, 2] = 6;
            matrix[2, 0] = 7;
            matrix[2, 1] = 8;
            matrix[2, 2] = 9;
            matrix[3, 0] = 10;
            matrix[3, 1] = 11;
            matrix[3, 2] = 12;

            double[] vector = null;

            Assert.Throws<ArgumentException>(() => Utils.MMult(matrix, vector));

        }
        [Fact]
        public void MMult_PassNullMatrix_ShouldThrowError()
        {
            double[,] matrix = null;

            double[] vector = new double[3];
            vector[0] = 1;
            vector[1] = 2;
            vector[2] = 3;

            Assert.Throws<ArgumentException>(() => Utils.MMult(matrix, vector));

        }
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 4)]
        public void PowOf2_ShouldWork(uint y, uint result)
        {
            Assert.Equal(result, Utils.PowOf2(y));
        }
        #endregion
        #region Gogga tests
        //Test the move funtion
        [Theory]
        [InlineData(1, 1, 1, 2, Direction.North)]
        [InlineData(1, 1, 2, 2, Direction.NorthEast)]
        [InlineData(1, 1, 2, 1, Direction.East)]
        [InlineData(1, 1, 2, 0, Direction.SouthEast)]
        [InlineData(1, 1, 1, 0, Direction.South)]
        [InlineData(1, 1, 0, 0, Direction.SouthWest)]
        [InlineData(1, 1, 0, 1, Direction.West)]
        [InlineData(1, 1, 0, 2, Direction.NorthWest)]
        public void Move_NoObstruct_ShouldWork(ushort xStart, ushort yStart, ushort xEnd, ushort yEnd, Direction d)
        {
            var w = new World
            (
                size: new Size(5, 5),
                initPop: 1,
                numOfGens: 1,
                stepsPerGen: 1,
                numOfGenes: 1,
                numOfObstruct: 0,
                numOfFoodCells: 0,
                maxFoodValue: 0,
                fitnessFunction: null, //TODO: FIX this
                sensorCount: 4,
                hiddenNeuronCount: 4,
                outputNeuronCount: 4
            );
            w.CreateCells();
            Gogga g = new Gogga(1);
            g.CurrentPosition = new Position(xStart, yStart);
            w.Cells[g.CurrentPosition.X, g.CurrentPosition.Y].IsOccupied = true;
            g.Move(w, d);
            Assert.Equal(xEnd, g.CurrentPosition.X);
            Assert.Equal(yEnd, g.CurrentPosition.Y);
        }
        [Theory]
        [InlineData(1, 4, Direction.North)]
        [InlineData(1, 4, Direction.NorthEast)]
        [InlineData(4, 3, Direction.NorthEast)]
        [InlineData(4, 1, Direction.East)]
        [InlineData(4, 1, Direction.SouthEast)]
        [InlineData(1, 0, Direction.SouthEast)]
        [InlineData(1, 0, Direction.South)]
        [InlineData(0, 1, Direction.SouthWest)]
        [InlineData(1, 0, Direction.SouthWest)]
        [InlineData(0, 1, Direction.West)]
        [InlineData(0, 1, Direction.NorthWest)]
        [InlineData(1, 4, Direction.NorthWest)]
        public void Move_OffEdge_ShouldReturnFalse(ushort xStart, ushort yStart, Direction d)
        {
            var w = new World
            (
                size: new Size(5, 5),
                initPop: 1,
                numOfGens: 1,
                stepsPerGen: 1,
                numOfGenes: 1,
                numOfObstruct: 0,
                numOfFoodCells: 0,
                maxFoodValue: 0,
                fitnessFunction: null, //TODO: FIX this
                sensorCount: 4,
                hiddenNeuronCount: 4,
                outputNeuronCount: 4
            );
            w.CreateCells();
            Gogga g = new Gogga(1);
            g.CurrentPosition = new Position(xStart, yStart);
            w.Cells[g.CurrentPosition.X, g.CurrentPosition.Y].IsOccupied = true;
            Assert.False(g.Move(w, d));
        }
        [Theory]
        [InlineData(2, 2, 2, 3, Direction.North)]
        [InlineData(2, 2, 3, 3, Direction.NorthEast)]
        [InlineData(2, 2, 3, 2, Direction.East)]
        [InlineData(2, 2, 3, 1, Direction.SouthEast)]
        [InlineData(2, 2, 2, 1, Direction.South)]
        [InlineData(2, 2, 1, 1, Direction.SouthWest)]
        [InlineData(2, 2, 1, 2, Direction.West)]
        [InlineData(2, 2, 1, 3, Direction.NorthWest)]
        public void Move_Obstruction_ShouldReturnFalse(ushort xStart, ushort yStart, ushort xObs, ushort yObs, Direction d)
        {
            var w = new World
            (
                size: new Size(5, 5),
                initPop: 1,
                numOfGens: 1,
                stepsPerGen: 1,
                numOfGenes: 1,
                numOfObstruct: 0,
                numOfFoodCells: 0,
                maxFoodValue: 0,
                fitnessFunction: null, //TODO: FIX this
                sensorCount: 4,
                hiddenNeuronCount: 4,
                outputNeuronCount: 4
            );
            w.CreateCells();
            Gogga g = new Gogga(1);
            g.CurrentPosition = new Position(xStart, yStart);
            w.Cells[xObs, yObs].IsObstructed = true;
            Assert.False(g.Move(w, d));
        }

        //[Fact]
        //public void Move_OffEdge_ShouldThrowError()
        //{
        //    //var w = new World
        //    //(
        //    //    size: new Size(5, 5),
        //    //    initPop: 1,
        //    //    numOfGens: 1,
        //    //    stepsPerGen: 1,
        //    //    numOfGenes: 1,
        //    //    numOfObstruct: 0,
        //    //    numOfFoodCells: 0,
        //    //    maxFoodValue: 0,
        //    //    fitnessFunction: null, //TODO: FIX this
        //    //    sensorCount: 4,
        //    //    hiddenNeuronCount: 4,
        //    //    outputNeuronCount: 4
        //    //);
        //    var w = new World();
        //    w.CreateCells();
        //    Gogga g = new Gogga(1);
        //    g.CurrentPosition = new Position(1, 1);
        //    w.Cells[g.CurrentPosition.X, g.CurrentPosition.Y].IsOccupied = true;
        //    //Assert.Throws<InvalidOperationException>(() => g.Move(w, Direction.North));
        //}

        #endregion



    }
}
