using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    public class Genome
    {
        public Gene[] Genes { get; set; }
        public Genome(ushort numberOfGenes)
        {
            Genes = new Gene[numberOfGenes];
        }
        public Genome(Genome other)
        {
            Genes = new Gene[other.Genes.Length];
            for (int i = 0; i < Genes.Length; i++)
            {
                Genes[i] = new Gene(other.Genes[i]);
            }
        }
        public static Genome GetNewRandomGenome(ushort geneCount, Random rand)
        {
            const int maxGeneCount = 1000; //This is somewhat arbitrary
            const int minGeneCount = 1;
            if ((geneCount < minGeneCount) || (geneCount > maxGeneCount)) throw new ArgumentException($"geneCount value {geneCount} is invalid");
            Genome genome = new Genome(geneCount);
            for (int i = 0; i < geneCount; i++)
            {
                genome.Genes[i] = Gene.GetNewRandomGene(rand);
            }
            return genome;
        }
        /// <summary>
        /// Creates an exact copy of a genome without mutation
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Genome CloneExact()
        {
            var genome = new Genome((ushort)this.Genes.Length);
            for (int i = 0; i < this.Genes.Length; i++)
            {
                genome.Genes[i] = new Gene(this.Genes[i]);
            }
            return genome;
        }
        /// <summary>
        /// Creates a copy of a genome with possible mutation
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Genome Mutate(Genome other)
        {
            //TODO: change to include mutation
            var genome = new Genome((ushort)other.Genes.Length);
            for (int i = 0; i < other.Genes.Length; i++)
            {
                genome.Genes[i] = new Gene(other.Genes[i]);
            }
            return genome;
        }
        public int Length()
        {
            return Genes.Length;
        }
        public static void PrintGenome(Gene[] genes)
        {
            int geneCount = genes.Length;
            for (int i = 0; i < geneCount; i++)
            {
                Gene.PrintGene(genes[i]);
            }
            Console.WriteLine();
        }
        /// <summary>
        /// Creates a new Genome with possibility of mutation.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="mutationHappened">Determines the likelihood that mutation will occur</param>
        /// <returns></returns>
        public Genome MutateGenome(Random r, out bool mutationHappened, double mutationRate = 0.001)
        {
            if (mutationRate < 0.000001) throw new ArgumentException("Mutation rate must be greater than 0.000001.");
            mutationHappened = r.NextDouble() < mutationRate;
            if (!mutationHappened) 
            {
                return new Genome(this);
            }
            else
            {
                var genome = new Genome(this);
                int geneIdx = r.Next(genome.Genes.Length);
                genome.Genes[geneIdx] = this.Genes[geneIdx].Mutate(r);
                return genome;
            }
        }
    }
}
