using Xunit;

using Enum.Generator.Core.Mapping;
using Enum.Generator.Core.Mapping.Exceptions;
using Enum.Generator.Core.Builder;
using Enum.Generator.Core.Definition;

namespace Enum.Generator.Tests.Builder
{
    public sealed class JsonMapperTests
    {
        [Fact]
        public void ThrowsIfPassedInvalidJson() => Assert.Throws<JsonParsingFailureException>(() =>
        {
            var context = Context.Create("[*]", "name");
            context.MapEnum("Invalid json", "TestEnum");
        });

        [Fact]
        public void ThrowsIfEnumNameIsNotValidIdentifier() => Assert.Throws<MappingFailureException>(() =>
        {
            var context = Context.Create("[*]", "name");
            context.MapEnum("[]", "Invalid Enum Name");
        });

        [Fact]
        public void ThrowsIfPassedInvalidCollectionJPath() => Assert.Throws<MappingFailureException>(() =>
        {
            var context = Context.Create("Invalid jPath", "name");
            context.MapEnum("[]", "TestEnum");
        });

        [Fact]
        public void EmptyEnumCanBeMapped()
        {
            var context = Context.Create("[*]", "name");
            var enumDefinition = context.MapEnum("[]", "TestEnum");

            Assert.Empty(enumDefinition.Entries);
            Assert.Equal(new EnumBuilder("TestEnum").Build(), enumDefinition);
        }

        [Fact]
        public void BasicEnumCanBeMapped()
        {
            var context = Context.Create("[*]", "name");
            var enumDefinition = context.MapEnum(
            @"[
                { ""name"": ""A"" },
                { ""name"": ""B"" },
                { ""name"": ""C"" }
            ]",
            "TestEnum");

            Assert.Equal(new EnumEntry("A", 0), enumDefinition.Entries[0]);
            Assert.Equal(new EnumEntry("B", 1), enumDefinition.Entries[1]);
            Assert.Equal(new EnumEntry("C", 2), enumDefinition.Entries[2]);
        }

        [Fact]
        public void ExplictValuesAreRespected()
        {
            var context = Context.Create("[*]", "name", "value");
            var enumDefinition = context.MapEnum(
            @"[
                { ""name"": ""A"", ""value"": 10 },
                { ""name"": ""B"", ""value"": 20 },
                { ""name"": ""C"", ""value"": 30 }
            ]",
            "TestEnum");

            Assert.Equal(new EnumEntry("A", 10), enumDefinition.Entries[0]);
            Assert.Equal(new EnumEntry("B", 20), enumDefinition.Entries[1]);
            Assert.Equal(new EnumEntry("C", 30), enumDefinition.Entries[2]);
        }

        [Fact]
        public void CommentsAreParsed()
        {
            var context = Context.Create("[*]", "name", null, "comment");
            var enumDefinition = context.MapEnum(
            @"[
                { ""name"": ""A"", ""comment"": ""This is A"" },
                { ""name"": ""B"", ""comment"": ""This is B"" },
                { ""name"": ""C"", ""comment"": ""This is C"" }
            ]",
            "TestEnum");

            Assert.Equal(new EnumEntry("A", 0, "This is A"), enumDefinition.Entries[0]);
            Assert.Equal(new EnumEntry("B", 1, "This is B"), enumDefinition.Entries[1]);
            Assert.Equal(new EnumEntry("C", 2, "This is C"), enumDefinition.Entries[2]);
        }

        [Fact]
        public void ThrowsIfContainsDuplicateValues() => Assert.Throws<MappingFailureException>(() =>
        {
            var context = Context.Create("[*]", "name", "value");
            context.MapEnum(
            @"[
                { ""name"": ""A"", ""value"": 1 },
                { ""name"": ""B"", ""value"": 1 },
            ]",
            "TestEnum");
        });

        [Fact]
        public void EntriesWithoutNamesAreSkipped()
        {
            var context = Context.Create("[*]", "name", "value");
            var enumDefinition = context.MapEnum(
            @"[
                { ""name"": ""A"", ""value"": 1 },
                { ""value"": 2 },
                { ""name"": ""C"", ""value"": 3 },
            ]",
            "TestEnum");

            Assert.Equal(new EnumEntry("A", 1, "This is A"), enumDefinition.Entries[0]);
            Assert.Equal(new EnumEntry("C", 3, "This is B"), enumDefinition.Entries[1]);
        }
    }
}
