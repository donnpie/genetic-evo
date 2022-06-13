using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    public class Neuron
    {
        public const byte MaxId = 127;
        byte id;
        public byte Id { get { return id; } set { id = value <= MaxId ? value : throw new ArgumentException("Value passed for Id is invalid"); } }
        public double Input { get; set; }
        public Func<double, double> TransferFunction { get; set; }
        public Func<World, Gogga, double> InputFunction { get; set; }
        public double CalcOutput(double input)
        {
            return TransferFunction(input);
        }
        public double CalcInput(World w, Gogga g)
        {
            return InputFunction(w, g);
        }
    }
}
