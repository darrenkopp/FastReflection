using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastReflection.Tests
{
    public class Animal
    {
        public Animal()
        {
            Color = new Color();
            Name = "unknown";
        }

        public string Name { get; set; }

        public Color Color { get; set; }
    }
}
