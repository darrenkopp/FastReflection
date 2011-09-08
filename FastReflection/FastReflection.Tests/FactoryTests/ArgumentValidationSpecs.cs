using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FastReflection.Tests.FactoryTests
{
    public class ArgumentValidationSpecs
    {
        [Fact]
        public void should_throw_if_type_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => PropertyFactory.Create(null, "Nothing"));
        }

        [Fact]
        public void should_throw_if_path_is_null()
        {
            Assert.Throws<ArgumentException>(() => PropertyFactory.Create(typeof(Animal), null));
        }

        [Fact]
        public void should_throw_if_path_empty()
        {
            Assert.Throws<ArgumentException>(() => PropertyFactory.Create(typeof(Animal), ""));
        }
    }
}
