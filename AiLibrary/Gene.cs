using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    public class Gene
    {
        public const uint MAX_GENE_VAL = 4294967295U;
        public const double SCALE = 500000000.0;
        public uint GeneValue { get; set; }
        public Gene(uint geneValue)
        {
            GeneValue = geneValue;
        }
        public Gene(Gene other) : base()
        {
            GeneValue = other.GeneValue;
        }
        public static Gene GetNewRandomGene(Random rand)
        {
            int i = 4;
            byte[] byteArr = new byte[i];
            rand.NextBytes(byteArr);
            return new Gene(BitConverter.ToUInt32(byteArr, 0));
        }
        public bool GetLayer()
        {
            return Utils.IsBitSet(GeneValue, 31);
        }
        private static byte ShiftRightAndMask(uint geneVal, int bitsToShift, int mask)
        {
            int gene = (int)geneVal;
            gene >>= bitsToShift; //shift right
            return (byte)(mask & gene);
        }
        private static byte ShiftLeftAndMask(uint geneVal, int bitsToShift, int mask)
        {
            int gene = (int)geneVal;
            gene <<= bitsToShift; //shift left
            return (byte)(mask & gene);
        }
        private static int ShiftLeftAndMask(int rawValue, int bitsToShift, int mask)
        {
            int value = (int)rawValue;
            value <<= bitsToShift; //shift left
            return mask & value;
        }
        public byte GetSourceAddress()
        {
            return Gene.ShiftRightAndMask(GeneValue, 24, 0b0000_0000_0000_0000_0000_0000_0111_1111);
        }
        public byte GetSinkAddress()
        {
            return Gene.ShiftRightAndMask(GeneValue, 16, 0b0000_0000_0000_0000_0000_0000_0111_1111);
        }
        public double GetWeight(double scale)
        {
            int value = unchecked((int)GeneValue);
            value <<= 16;
            return value / scale;
        }
        public DecodedGene DecodeGene(double scale)
        {
            DecodedGene g = new DecodedGene();
            g.Layer = GetLayer();
            g.SourceAddress = GetSourceAddress();
            g.SinkAddress = GetSinkAddress();
            g.Weight = GetWeight(scale);
            return g;
        }
        public static void PrintGene(Gene gene)
        {
            string hexValue = gene.GeneValue.ToString("X");
            Console.Write(hexValue);
            Console.Write(" ");
        }
        public void PrintGene()
        {
            string hexValue = GeneValue.ToString("X");
            Console.Write(hexValue);
            Console.Write(" ");
        }
        /// <summary>
        /// Mutates the gene by flipping 1 bit
        /// </summary>
        /// <returns></returns>
        public Gene Mutate(Random r)
        {
            //TODO: write test code
            uint bitIdx = (uint)r.Next(32);
            uint mask = Utils.PowOf2(bitIdx);
            var g = new Gene(this.GeneValue ^ mask);
            return g;
        }
    }
    public struct DecodedGene
    {
        public bool Layer;
        public byte SourceAddress;
        public byte SinkAddress;
        public double Weight;
    }
}
