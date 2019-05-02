using System;
using Xunit;

using EnumGenerator.Core.Builder;
using EnumGenerator.Core.Exporter;
using EnumGenerator.Core.Exporter.Exceptions;

namespace EnumGenerator.Tests.Builder
{
    public sealed class CSharpExporterTests
    {
        [Fact]
        public void ThrowsIfExportedWithInvalidNamespace() => Assert.Throws<InvalidNamespaceException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDef = builder.Build();

            enumDef.Export(@namespace: "0Test");
        });

        [Fact]
        public void BasicEnumIsPresentAfterExporting()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.Export(@namespace: "Test");

            Assert.Contains(
                expectedSubstring:
@"  public enum TestEnum
    {
        A = 1,

        B = 2,
    }",
                actualString: export,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
