using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastReflection
{
    public interface IFastProperty
    {
        bool CanRead { get; }

        bool CanWrite { get; }

        void Set(object instance, object value);

        object Get(object instance);
    }
}
