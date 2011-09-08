using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastReflection
{
    public static class ExtensionMethods
    {
        public static IFastProperty ToFastProperty(this Type source, string path)
        {
            return PropertyFactory.Create(source, path);
        }
    }
}
