using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiLibrary;


namespace GeneticEvolution
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Program flow
            //1. Initial setup (stays same for entire simulation)
            ////world setup
            //////world size Done
            //////starting population Done
            //////number of generations Done
            //////number of steps per generation Done
            //////impassible regions / barriers TODO
            //////food TODO
            //////select number of genes Done
            //////Create goggas without weight matrices
            ////////random starting position Done
            ////////Generate random genome Done
            //neural net setup (stays same for entire simulation)
            ////neuron id Done
            ////transfer function Done
            ////Configure input neurons (and number) Done
            ////select number of and Configure hidden neurons Done
            ////Configure output neurons (and number) Done
            //Genomic setup
            //create brain
            ////create weight matrices Done
            //////for each gene in genome:
            ////////decode Done
            ////////fill weight matrices Done
            //Define survival criteria
            //2. Run simulation
            //Run generation
            //For each step in generation
            //for each creature in generation
            //calculate outputs of input neurons
            //Calculate the inputs for hidden neurons
            //calculate outputs of hidden neurons
            //calculate the inputs for output neurons
            //calculate the outputs of output neurons
            //perform actions as per neural output
            //record world snapshot
            //Run suvival criteria for each gogga
            //if gogga survives
            //run mutation algorithm
            //if mutated, recreate brain
            //else kill
            //Run next generation
            //3. Inspect results
            //Survival rate
            //most prevalant genomes

            //Test area


            #region World setup
            var w = new World
            (
                size: new Size(500, 500),
                initPop: 1000,
                numOfGens: 1,
                stepsPerGen:500,
                numOfGenes: 16,
                numOfObstruct: 500,
                numOfFoodCells: 3,
                maxFoodValue: 10,
                fitnessFunction: TestFitnessFunction, //TODO: improve how to select the fitness function. Maybe use enums?
                sensorCount: 4,
                hiddenNeuronCount: 3,
                outputNeuronCount: 4
                );
            w.CreateCells();
            w.CreateObstructions();
            w.CreateFood();
            w.CreateGoggas(w.InitialPopulation, w.Goggas);

            #endregion
            #region Neuron setup
            //TODO: Neurons should be encapsulated in world
            //Input neurons

            for (int i = 0; i < w.Sensors.Length; i++)
            {
                //Give the sensors ids
                Sensor n = new Sensor();
                n.Id = (byte)i;
                w.Sensors[i] = n;              
            }
            //Configure individual input neurons - Number of neurons here must equal inputNeuronCount
            w.Sensors[0].SensorFunction = Utils.NeighboursNorth;
            w.Sensors[1].SensorFunction = Utils.RandomInput;
            w.Sensors[2].SensorFunction = Utils.RandomInput;
            w.Sensors[3].SensorFunction = Utils.RandomInput;

            //Hidden neurons
            for (int i = 0; i < w.HiddenNeurons.Length; i++)
            {
                Neuron n = new Neuron();
                n.Id = (byte)i;
                n.TransferFunction = Utils.Linear;
                w.HiddenNeurons[i] = n;              
            }

            //Output neurons
            for (int i = 0; i < w.OutputNeurons.Length; i++)
            {
                Neuron n = new Neuron();
                n.Id = (byte)i;
                n.TransferFunction = Utils.Step05;
                w.OutputNeurons[i] = n;               
            }
            #endregion
            #region Neural net setup
            //TODO: Write test code for this section.
            w.CreateNeuralNets();
            #endregion
            #region Main program loop
            for (int i = 0; i < w.GenerationsCount; i++)
            {
                var genWatch = new System.Diagnostics.Stopwatch();
                genWatch.Start();
                for (int j = 0; j < w.StepsPerGeneration; j++)
                {
                    //var stepWatch = new System.Diagnostics.Stopwatch();
                    //stepWatch.Start();
                    for (int k = 0; k < w.Goggas.Count; k++)
                    {
                        //individual gogga code goes here
                        //var goggaWatch = new System.Diagnostics.Stopwatch();
                        //goggaWatch.Start();
                        Gogga currentGogga = w.Goggas[k];
                        //calculate outputs of input neurons
                        double[] sensorOutput = new double[w.SensorCount];
                        foreach (var sensor in w.Sensors)
                        {
                            sensorOutput[sensor.Id] = sensor.CalcOutput(w,currentGogga);
                        }
                        //Calculate the inputs for hidden neurons
                        double[] hiddenNeuronInput = new double[w.HiddenNeuronCount];
                        hiddenNeuronInput = Utils.MMult(currentGogga.WeightMatrices[0], sensorOutput);
                        //calculate outputs of hidden neurons
                        double[] hiddenNeuronOutput = new double[w.HiddenNeuronCount];
                        foreach (var neuron in w.HiddenNeurons)
                        {
                            hiddenNeuronOutput[neuron.Id] = neuron.CalcOutput(hiddenNeuronInput[neuron.Id]);
                        }
                        //calculate the inputs for output neurons
                        double[] outputNeuronInput = new double[w.OutputNeuronCount];
                        outputNeuronInput = Utils.MMult(currentGogga.WeightMatrices[1], hiddenNeuronOutput);
                        //calculate the outputs of output neurons
                        double[] outputNeuronOutput = new double[w.OutputNeuronCount];
                        foreach (var neuron in w.OutputNeurons)
                        {
                            outputNeuronOutput[neuron.Id] = neuron.CalcOutput(outputNeuronInput[neuron.Id]);
                        }
                        //perform actions as per neural output (move, deposit pheromones etc)
                        //First 4 output neurons determines movement
                        byte movement = 0;
                        for (int m = 0; m < 4; m++)
                        {
                            movement += (byte)(Math.Pow(2, m) * outputNeuronOutput[m]);
                        }
                        DecodeMove(movement, currentGogga, w);

                        //Update gogga properties (eg age)
                        currentGogga.Age += 1;

                        //goggaWatch.Stop();
                        //Console.WriteLine($"Time to execute gogga: {goggaWatch.Elapsed}");
                    }
                    //end of step code goes here
                    //record world snapshot
                    //public void EndOfStep...
                    //Console.WriteLine($"Time to execute step: {stepWatch.Elapsed}");
                }
                //end of generation code goes here incl survival calcs

                //Calculate who survives
                //TODO: Make this a method of world - this needs to be performed at the end of every generation
                double sumOfFitness = 0;
                foreach (var g in w.Goggas)
                {
                    sumOfFitness += g.Fitness;
                }
                double avgFitness = sumOfFitness / w.Goggas.Count;
                Console.WriteLine($"Average fitness: {avgFitness}");
                
                int SurvivorCount = 0;
                int currentPopulationCount = w.Goggas.Count;
                List<Gogga> nextGen = new List<Gogga>();
                foreach (var g in w.Goggas)
                {
                    if (g.Fitness < avgFitness)
                    {
                        //Unfit ones die
                        g.Die(w.Cells);
                    }
                    else
                    {
                        //Survivors reproduce and then die
                        w.Cells[g.CurrentPosition.X, g.CurrentPosition.Y].IsOccupied = false;
                        Position p = Position.GetNewRandomPositionNotOnObstruction(w);
                        w.Cells[p.X, p.Y].IsOccupied = true;
                        var newGogga = g.SpawnExact(w, p); //TODO: add mutation
                        nextGen.Add(newGogga);
                        SurvivorCount++;
                    }
                }
                Console.WriteLine($"Death count: {SurvivorCount}");
                //Replace dead ones with new ones with random genes
                int topUp = SurvivorCount; 
                w.CreateGoggas((ushort)topUp, nextGen);              
                w.Goggas = nextGen;
                Console.WriteLine($"Occupied cells: {w.GetNumberOfOccupiedCells()}");
                Console.WriteLine($"Occupation audit: {w.AuditCellOccupation()}");

                Console.WriteLine($"Time to execute generation: {genWatch.Elapsed}");
            }
            //end of simulation code goes here
            #endregion




        }



        
        //TODO: find a more elegant way to declare various fitness functions (Maybe use enums??)
        public static double TestFitnessFunction(Gogga g)
        {
            return Position.GetDistBetweenTwoPositions(g.CurrentPosition, g.StartPosition);
        }

        public static void DecodeMove(byte b, Gogga g, World w)
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
                    g.Move(w, Direction.North);
                    return;
                case 2:
                    g.Move(w, Direction.East);
                    return;
                case 3:
                    g.Move(w, Direction.NorthEast);
                    return;
                case 4:
                    g.Move(w, Direction.South);
                    return;
                case 5:
                    g.Move(w, Gogga.MoveRandom(w.Rand));
                    return;
                case 6:
                    g.Move(w, Direction.SouthEast);
                    return;
                case 7:
                    g.Move(w, Gogga.MoveRandom(w.Rand));
                    return;
                case 8:
                    g.Move(w, Direction.West);
                    return;
                case 9:
                    g.Move(w, Direction.NorthWest);
                    return;
                case 10:
                    g.Move(w, Gogga.MoveRandom(w.Rand));
                    return;
                case 11:
                    g.Move(w, Gogga.MoveRandom(w.Rand));
                    return;
                case 12:
                    g.Move(w, Direction.SouthWest);
                    return;
                case 13:
                    g.Move(w, Gogga.MoveRandom(w.Rand));
                    return;
                case 14:
                    g.Move(w, Gogga.MoveRandom(w.Rand));
                    return;
                case 15:
                    g.Move(w, Gogga.MoveRandom(w.Rand));
                    return;
                default:
                    break;
            }
        }


    }
    //How to convert hex string to number
    //Method 1
    //long hex = long.Parse("FFFFFFFF", System.Globalization.NumberStyles.HexNumber);
    //Console.WriteLine($"{hex}");

    //Method 2
    //string hexString = "0xFFFFFFFF";
    //long hexL = Convert.ToInt64(hexString, 16);
    //Console.WriteLine($"{hexL}");

    //Create a bunch of random genes
    //Random rand = new Random();
    //for (int j = 0; j < 100; j++)
    //{
    //    long number = GetRandomGene(4294967296, rand);
    //    Console.WriteLine(Convert.ToString(number, 2));
    //}

    //How to convert a string binary representation of a number into a decimal number
    //int binNum = 0b1111111; //Max address num
    //Console.WriteLine(binNum);

    //int inputNeuronCount = 3;
    //int hiddenNeuronCount = 1;
    //int outputNeuronCount = 2;
    //float[,] weightsFirstLayer = new float[hiddenNeuronCount,inputNeuronCount];
    //float[,] weightsSecondLayer = new float[outputNeuronCount, hiddenNeuronCount];

    //Fill weights
    //for (int r = 0; r < weightsFirstLayer.GetLength(0); r++)
    //{
    //    for (int c = 0; c < weightsFirstLayer.GetLength(1); c++)
    //    {
    //        weightsFirstLayer[r, c] = 0f;
    //    }
    //}

    ////Gene data
    //Random rand = new Random();
    //bool isSecondLayer = false;
    ////int sourceId = rand.Next(127);
    ////int sinkId = rand.Next(127);
    //int sourceId = 2;
    //int sinkId = 0;
    //Console.WriteLine($"source: {sourceId}, sink: {sinkId}");
    //float weight = 3.4f;

    //if (!isSecondLayer)
    //{
    //    int c = sourceId % inputNeuronCount;
    //    int r = sinkId % hiddenNeuronCount;
    //    Console.WriteLine($"r: {r}, c: {c}");
    //    weightsFirstLayer[r, c] = weight;
    //}
    //else
    //{
    //    int c = sourceId % inputNeuronCount;
    //    int r = sinkId % hiddenNeuronCount;
    //    Console.WriteLine($"r: {r}, c: {c}");
    //    weightsSecondLayer[r, c] = weight;
    //}

    //for (int r = 0; r < weightsFirstLayer.GetLength(0); r++)
    //{
    //    for (int c = 0; c < weightsFirstLayer.GetLength(1); c++)
    //    {
    //        Console.WriteLine(weightsFirstLayer[r, c]);
    //    }
    //}

    //int geneCount = 8;
    //const long MAXVAL = 4294967296;
    //Random rand = new Random();
    //long[] genome = GetRandomGenome(geneCount, MAXVAL, rand);
    //PrintGenome(genome);


    //"FFFFFFFF"
    //"0FFFFFFF"
    //"F0000000"
    //long hex = long.Parse("0000FFFF", System.Globalization.NumberStyles.HexNumber);
    //PrintGene(hex);
    //Console.WriteLine();
    //PrintBinary(hex);
    //Console.WriteLine(GetLayer(hex));
    //GetSourceAddress(hex);
    //GetSinkAddress(hex);
    //Console.WriteLine(GetWeight(hex, 0));
    //float max = 0;
    //float min = 0;
    //for (int i = 0; i < 100; i++)
    //{
    //    long gene = GetRandomGene(MAXVAL, rand);
    //    float result = GetWeight(gene, 500000000.0f);
    //    max = result > max ? result : max;
    //    min = result < min ? result : min;
    //}
    //Console.WriteLine($"{max} {min}");


}
