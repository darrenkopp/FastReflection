using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastReflection
{
    public interface IFastGetter
    {
        object Get(object instance);
    }
}
