using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastReflection
{
    class FastSetter : IFastSetter
    {
        private readonly Type InstanceType;
        private readonly Type PropertyType;
        private readonly Action<object, object> Setter;

        public FastSetter(Type instanceType, Type propertyType, Action<object, object> setter)
        {
            InstanceType = instanceType;
            PropertyType = propertyType;
            Setter = setter;
        }

        public void Set(object instance, object value)
        {
            if (!InstanceType.IsAssignableFrom(instance.GetType()))
                throw new InvalidOperationException("The instance is not not an instance or derived instance of " + InstanceType.FullName);

            if (value != null && !PropertyType.IsAssignableFrom(value.GetType()))
                throw new InvalidOperationException("The value is not an instance or derived instance of " + PropertyType.FullName);

            Setter(instance, value);
        }
    }
}
