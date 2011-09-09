using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastReflection.Tests
{
    public class Color
    {
        public Color()
        {
            Red = Green = Blue = 128;
        }

        public int? Red { get; set; }

        public int? Green { get; set; }

        public int? Blue { get; set; }
    }
}
