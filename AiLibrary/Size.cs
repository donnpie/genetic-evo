using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLibrary
{
    public class Size
    {
        public const ushort MaxWidth = 700;
        public const ushort MaxHeight = 700;
        ushort width;
        ushort height;
        public ushort Width { get { return width; } set { width = value <= MaxWidth ? value : throw new ArgumentException($"Value passed for width is invalid ({value} received, but only {MaxWidth} allowed)"); } }
        public ushort Height { get { return height; } set { height = value <= MaxHeight ? value : throw new ArgumentException($"Value passed for height is invalid ({value} received, but only {MaxHeight} allowed)"); } }
        public Size()
        {
            Width = 0;
            Height = 0;
        }
        public Size(ushort width, ushort height)
        {
            Width = width;
            Height = height;
        }
        public int GetArea()
        {
            return Height * Width;
        }

    }
}
