using Xunit;

using EnumGenerator.Core.Mapping;
using EnumGenerator.Core.Mapping.Exceptions;
using EnumGenerator.Core.Builder;
using EnumGenerator.Core.Definition;

namespace EnumGenerator.Tests.Builder
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
        public void ExplictValuesOfWrongTypesAreIgnored()
        {
            var context = Context.Create("[*]", "name", "value");
            var enumDefinition = context.MapEnum(
            @"[
                { ""name"": ""A"", ""value"": 10 },
                { ""name"": ""B"", ""value"": [ 20 ] },
                { ""name"": ""C"", ""value"": 30 }
            ]",
            "TestEnum");

            Assert.Equal(new EnumEntry("A", 10), enumDefinition.Entries[0]);
            Assert.Equal(new EnumEntry("B", 1), enumDefinition.Entries[1]);
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

            Assert.Equal("This is A", enumDefinition.Entries[0].Comment);
            Assert.Equal("This is B", enumDefinition.Entries[1].Comment);
            Assert.Equal("This is C", enumDefinition.Entries[2].Comment);
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

            Assert.Equal(new EnumEntry("A", 1), enumDefinition.Entries[0]);
            Assert.Equal(new EnumEntry("C", 3), enumDefinition.Entries[1]);
        }

        [Fact]
        public void EntriesWithNamesOfWrongTypesAreSkipped()
        {
            var context = Context.Create("[*]", "name", "value");
            var enumDefinition = context.MapEnum(
            @"[
                { ""name"": ""A"" },
                { ""name"": [ 1 ] },
                { ""name"": ""C"" },
            ]",
            "TestEnum");

            Assert.Equal(new EnumEntry("A", 0), enumDefinition.Entries[0]);
            Assert.Equal(new EnumEntry("C", 1), enumDefinition.Entries[1]);
        }
    }
}
