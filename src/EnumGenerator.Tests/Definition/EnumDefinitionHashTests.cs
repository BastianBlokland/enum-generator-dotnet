using System.Globalization;
using Xunit;

using EnumGenerator.Core.Builder;
using EnumGenerator.Core.Definition;

namespace EnumGenerator.Tests.Builder
{
    public sealed class EnumDefinitionHashTests
    {
        [Fact]
        public void HashIs128Bits()
        {
            var enum1 = CreateDefinition();
            var hash = enum1.Get128BitHash();
            Assert.Equal(128, hash.Length * 8);

            EnumDefinition CreateDefinition()
            {
                var builder = new EnumBuilder("TestEnum");
                builder.PushEntry("A", 1);
                builder.PushEntry("B", 2);
                return builder.Build();
            }
        }

        [Fact]
        public void SameEnumsHaveSameHash()
        {
            var enum1 = CreateDefinition();
            var enum2 = CreateDefinition();

            var hash1 = enum1.Get128BitHash();
            var hash2 = enum2.Get128BitHash();
            Assert.Equal(hash1, hash2);

            EnumDefinition CreateDefinition()
            {
                var builder = new EnumBuilder("TestEnum");
                builder.PushEntry("A", 1);
                builder.PushEntry("B", 2);
                return builder.Build();
            }
        }

        [Fact]
        public void EnumsWithDifferentNameHaveDifferentHash()
        {
            var enum1 = CreateDefinition(name: "EnumA");
            var enum2 = CreateDefinition(name: "EnumB");

            var hash1 = enum1.Get128BitHash();
            var hash2 = enum2.Get128BitHash();
            Assert.NotEqual(hash1, hash2);

            EnumDefinition CreateDefinition(string name)
            {
                var builder = new EnumBuilder(name);
                builder.PushEntry("A", 1);
                builder.PushEntry("B", 2);
                return builder.Build();
            }
        }

        [Fact]
        public void EnumsWithDifferentEntryNameHaveDifferentHash()
        {
            var enum1 = CreateDefinition(entry1Name: "A");
            var enum2 = CreateDefinition(entry1Name: "B");

            var hash1 = enum1.Get128BitHash();
            var hash2 = enum2.Get128BitHash();
            Assert.NotEqual(hash1, hash2);

            EnumDefinition CreateDefinition(string entry1Name)
            {
                var builder = new EnumBuilder("TestEnum");
                builder.PushEntry(entry1Name, 1);
                builder.PushEntry("C", 2);
                return builder.Build();
            }
        }

        [Fact]
        public void EnumsWithDifferentEntryValueHaveDifferentHash()
        {
            var enum1 = CreateDefinition(entry1Value: 1);
            var enum2 = CreateDefinition(entry1Value: 2);

            var hash1 = enum1.Get128BitHash();
            var hash2 = enum2.Get128BitHash();
            Assert.NotEqual(hash1, hash2);

            EnumDefinition CreateDefinition(int entry1Value)
            {
                var builder = new EnumBuilder("TestEnum");
                builder.PushEntry("A", entry1Value);
                builder.PushEntry("B", 3);
                return builder.Build();
            }
        }

        [Fact]
        public void EnumsWithDifferentAmountsOfEntriesHaveDifferentHash()
        {
            var enum1 = CreateDefinition(entries: 2);
            var enum2 = CreateDefinition(entries: 3);

            var hash1 = enum1.Get128BitHash();
            var hash2 = enum2.Get128BitHash();
            Assert.NotEqual(hash1, hash2);

            EnumDefinition CreateDefinition(int entries)
            {
                var builder = new EnumBuilder("TestEnum");
                for (int i = 0; i < entries; i++)
                    builder.PushEntry("Entry" + i.ToString(CultureInfo.InvariantCulture), i);

                return builder.Build();
            }
        }
    }
}
