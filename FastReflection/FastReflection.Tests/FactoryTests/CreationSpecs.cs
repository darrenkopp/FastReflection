using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FastReflection.Tests.FactoryTests
{
    public class CreationSpecs
    {
        [Fact]
        public void should_allow_set_when_property_is_writeable()
        {
            var property = PropertyFactory.Create(typeof(Animal), "Name");
            var animal = new Animal { Name = "before" };

            Assert.True(property.CanWrite);
            Assert.DoesNotThrow(() => property.Set(animal, "after"));
            Assert.Equal("after", animal.Name);
        }
    }
}
