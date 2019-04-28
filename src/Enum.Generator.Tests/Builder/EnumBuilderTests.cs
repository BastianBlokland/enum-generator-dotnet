using System;
using Xunit;

using Enum.Generator.Core.Builder;
using Enum.Generator.Core.Builder.Exceptions;
using Enum.Generator.Core.Definition;

namespace Enum.Generator.Tests.Builder
{
    public sealed class EnumBuilderTests
    {
        [Fact]
        public void ThrowsIfEnumHasDuplicateValue() => Assert.Throws<DuplicateEnumValueException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            builder.PushEntry("C", 2);
        });

        [Fact]
        public void ThrowsIfEnumIsEmpty() => Assert.Throws<EmptyEnumException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            var enumDefinition = builder.Build();
        });

        [Fact]
        public void AllEntriesArePresent()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDefinition = builder.Build();

            Assert.True(enumDefinition.HasEntry("A"));
            Assert.True(enumDefinition.HasEntry("B"));

            Assert.True(enumDefinition.HasEntry(1));
            Assert.True(enumDefinition.HasEntry(2));
        }

        [Fact]
        public void NameCanBeFoundFromValue()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDefinition = builder.Build();

            Assert.True(enumDefinition.TryGetName(1, out var name));
            Assert.Equal("A", name);
        }

        [Fact]
        public void ValueCanBeFoundFromName()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDefinition = builder.Build();

            Assert.True(enumDefinition.TryGetValue("A", out var value));
            Assert.Equal(1, value);
        }

        [Fact]
        public void NonExistingEntriesAreNotFound()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDefinition = builder.Build();

            Assert.False(enumDefinition.HasEntry("B"));
            Assert.False(enumDefinition.HasEntry(2));
            Assert.False(enumDefinition.TryGetName(2, out var name));
            Assert.False(enumDefinition.TryGetValue("B", out var value));
        }
    }
}
