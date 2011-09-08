using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastReflection
{
    class FastGetter : IFastGetter
    {
        private readonly Type InstanceType;
        private readonly Func<object, object> Getter;

        public FastGetter(Type instanceType, Func<object, object> getter)
        {
            InstanceType = instanceType;
            Getter = getter;
        }

        public object Get(object instance)
        {
            // check to make sure object is ok type
            if (InstanceType.IsAssignableFrom(instance.GetType()))
                throw new InvalidOperationException("The instance is not not an instance or derived instance of " + InstanceType.FullName);

            return Getter(instance);
        }
    }
}
