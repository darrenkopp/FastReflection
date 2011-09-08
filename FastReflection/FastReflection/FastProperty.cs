using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastReflection
{
    public class FastProperty : IFastProperty
    {
        private readonly IFastGetter Getter;
        private readonly IFastSetter Setter;
        public FastProperty(IFastGetter getter, IFastSetter setter)
        {
            Getter = getter;
            Setter = setter;
            CanRead = getter != null;
            CanWrite = setter != null;
        }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }

        public void Set(object instance, object value)
        {
            if (!CanWrite)
                throw new InvalidOperationException("There is no setter for the property.");

            Setter.Set(instance, value);
        }

        public object Get(object instance)
        {
            if (!CanRead)
                throw new InvalidOperationException("There is not getter for the property.");

            return Getter.Get(instance);
        }
    }
}
