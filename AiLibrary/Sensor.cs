using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    public class Sensor
    {
        public const byte MaxId = 127;
        byte id;
        public byte Id { get { return id; } set { id = value <= MaxId ? value : 
                    throw new ArgumentException("Value passed for Id is invalid"); } }
        public Func<World, Gogga, double> SensorFunction { get; set; }
        public double CalcOutput(World w, Gogga g)
        {
            return SensorFunction(w, g);
        }
    }
}
