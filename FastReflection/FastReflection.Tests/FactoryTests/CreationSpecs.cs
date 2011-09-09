using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Diagnostics;

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

        [Fact]
        public void can_make_nested_property()
        {
            var animal = new Animal();
            var property = PropertyFactory.Create(typeof(Animal), "Color.Red");
            Assert.DoesNotThrow(() => Assert.Equal(animal.Color.Red, property.Get(animal)));
            Assert.DoesNotThrow(() => property.Set(animal, 20));
            Assert.Equal(20, animal.Color.Red);
        }
    }
}
