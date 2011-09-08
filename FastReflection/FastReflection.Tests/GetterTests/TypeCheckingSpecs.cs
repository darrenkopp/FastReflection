using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FastReflection.Tests.GetterTests
{
    public class TypeCheckingSpecs
    {
        [Fact]
        public void should_throw_when_instance_not_proper_type()
        {
            var setter = new FastGetter(typeof(Animal), i => ((Animal)i).Name);
            Assert.Throws<InvalidOperationException>(() => setter.Get("not an animal"));
        }
    }
}
