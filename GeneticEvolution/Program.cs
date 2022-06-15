using AiLibrary;
using System;
using System.Collections.Generic;


namespace GeneticEvolution
{
    class Program
    {       
        static void Main(string[] args)
        {
            var simWatch = new System.Diagnostics.Stopwatch();
            simWatch.Start();
            //Test area
            {
                //double[,] matrix = new double[4, 3];

                //double[] vector = new double[3];
                //vector[0] = 1;
                //vector[1] = 2;
                //vector[2] = 3;

                //double[] result = Utils.MMult(matrix, vector);



            }

            #region World setup
            var w = new World
            (
                size: new Size(700, 700),
                initPop: 3000,
                numOfGens: 100,
                stepsPerGen:500,
                numOfGenes: 32,
                numOfObstruct: 49000,
                numOfFoodCells: 10000,
                maxFoodValue: 100,
                fitnessFunction: TestFitnessFunction, //TODO: improve how to select the fitness function. Maybe use enums?
                sensorCount: 4,
                hiddenNeuronCount: 3,
                outputNeuronCount: 4
                );
            w.CreateCells();
            w.CreateObstructions();
            w.CreateFood();
            w.CreateGoggas(w.InitialPopulation, w.Goggas);
            //TODO: How many goggas created?
            //todo: hOW much food was created?
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
            #region MAIN PROGRAM LOOP
            for (int i = 0; i < w.GenerationsCount; i++)
            {
                Console.WriteLine($"Generation: {i}");
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
                        if (currentGogga.WeightMatrices.Count == 0) throw new InvalidOperationException("Runtime error: WeightMatrices not set for current gogga. Did you forget to run the CreateNeuralNet method?");
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
                        currentGogga.DecodeMove(movement, w);

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
                //Thought: make it such that different goggas have different survival criteria
                //eg, predators must hunt to survive, prey must evade to survive
                double sumOfFitness = 0;
                double maxFitness = 0;
                double minFitness = double.MaxValue;
                foreach (var g in w.Goggas)
                {
                    sumOfFitness += g.Fitness;
                    maxFitness = Math.Max(maxFitness, g.Fitness);
                    minFitness = Math.Min(minFitness, g.Fitness);
                }
                double avgFitness = sumOfFitness / w.Goggas.Count;
                Console.WriteLine($"Average fitness for generation: {avgFitness}");
                Console.WriteLine($"Max fitness: {maxFitness}");
                Console.WriteLine($"Min fitness: {minFitness}");
                
                int survivorCount = 0;
                int currentPopulationCount = w.Goggas.Count;
                List<Gogga> nextGen = new List<Gogga>();
                bool mutationHappened;
                int mutationCount = 0;
                foreach (var g in w.Goggas)
                {
                    if (g.Fitness < avgFitness)
                    {
                        //Unfit ones die without repoducing
                        g.Die(w.Cells);
                    }
                    else
                    {
                        //Survivors reproduce and then die
                        w.Cells[g.CurrentPosition.X, g.CurrentPosition.Y].IsOccupied = false;
                        Position p = Position.GetNewRandomPositionNotOnObstruction(w);
                        w.Cells[p.X, p.Y].IsOccupied = true;
                        //var newGogga = g.SpawnExact(w, p); //TODO: add mutation
                        var newGogga = g.SpawnWithMutation(w, p, out mutationHappened); //TODO: add mutation
                        if (mutationHappened) mutationCount += 1;
                        nextGen.Add(newGogga);
                        survivorCount++;
                    }
                }
                Console.WriteLine($"Number of survivors: {survivorCount} / {currentPopulationCount}");
                Console.WriteLine($"Number of mutations: {mutationCount}");
                //Add new goggas with with random genes to get back to the original count
                int topUp = currentPopulationCount - survivorCount; 

                w.CreateGoggas((ushort)topUp, nextGen);        
                for (int m = survivorCount; m < nextGen.Count; m++)
                {
                    nextGen[m].CreateNeuralNet(w.SensorCount, w.HiddenNeuronCount, w.OutputNeuronCount);
                }                                               //
                w.Goggas = nextGen;
                //Console.WriteLine($"Occupied cells: {w.GetNumberOfOccupiedCells()}");
                //Console.WriteLine($"Occupation audit: {w.AuditCellOccupation()}");
                //todo: most prevalant genomes

                Console.WriteLine($"Time to execute generation: {genWatch.Elapsed}");
            }
            //end of simulation code goes here (when all generations have run)
            #endregion

            Console.WriteLine($"Time to execute simulation: {simWatch.Elapsed}");
        }

        //TODO: find a more elegant way to declare various fitness functions (Maybe use enums??)
        public static double TestFitnessFunction(Gogga g)
        {
            return Position.GetDistBetweenTwoPositions(g.CurrentPosition, g.StartPosition);
        }
    }
}
