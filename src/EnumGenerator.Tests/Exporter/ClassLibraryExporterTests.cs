using System.Reflection;
using Xunit;

using EnumGenerator.Core.Builder;
using EnumGenerator.Core.Exporter;
using EnumGenerator.Core.Exporter.Exceptions;

namespace EnumGenerator.Tests.Builder
{
    public sealed class ClassLibraryExporterTests
    {
        [Fact]
        public void ThrowsIfExportedWithInvalidNamespace() => Assert.Throws<InvalidNamespaceException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDef = builder.Build();

            enumDef.ExportClassLibrary(@namespace: "0Test", assemblyName: "Test");
        });

        [Fact]
        public void ThrowsIfExportedWithInvalidAssemblyName() => Assert.Throws<InvalidAssemblyNameException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDef = builder.Build();

            enumDef.ExportClassLibrary(@namespace: "Test", assemblyName: "0Test");
        });

        [Fact]
        public void ThrowsIfExportedWithOutOfBoundsValue() => Assert.Throws<OutOfBoundsValueException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", -1);
            var enumDef = builder.Build();

            enumDef.ExportClassLibrary(storageType: StorageType.Unsigned8Bit, assemblyName: "Test");
        });

        [Fact]
        public void BasicEnumIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            // Export pe file.
            var binaryAssembly = enumDef.ExportClassLibrary(assemblyName: "Test");

            // Load the assembly.
            var assembly = Assembly.Load(binaryAssembly);

            // Assert that the assembly contains the generated enum.
            Assert.Equal("Test", assembly.GetName().Name);
            var enumType = assembly.GetType("TestEnum");

            Assert.Equal(new string[] { "A", "B" }, System.Enum.GetNames(enumType));
            Assert.Equal(1, (int)System.Enum.GetValues(enumType).GetValue(0));
            Assert.Equal(2, (int)System.Enum.GetValues(enumType).GetValue(1));
        }

        [Fact]
        public void NameSpaceIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            // Export pe file.
            var binaryAssembly = enumDef.ExportClassLibrary(assemblyName: "Test", @namespace: "A.B.C");

            // Load the assembly.
            var assembly = Assembly.Load(binaryAssembly);

            // Assert that generated enum is contained in correct namespace.
            var enumType = assembly.GetType("A.B.C.TestEnum");
            Assert.Equal("A.B.C.TestEnum", enumType.FullName);
        }

        [Fact]
        public void StorageTypeIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", -1);
            var enumDef = builder.Build();

            // Export pe file.
            var binaryAssembly = enumDef.ExportClassLibrary(
                assemblyName: "Test",
                storageType: StorageType.Signed8Bit);

            // Load the assembly.
            var assembly = Assembly.Load(binaryAssembly);

            // Assert that generated enum is contained in correct namespace.
            var enumType = assembly.GetType("TestEnum");
            Assert.Equal(typeof(sbyte), System.Enum.GetUnderlyingType(enumType));
            Assert.Equal((sbyte)1, (sbyte)System.Enum.GetValues(enumType).GetValue(0));
            Assert.Equal((sbyte)-1, (sbyte)System.Enum.GetValues(enumType).GetValue(1));
        }

        [Fact]
        public void ExportIsDeterministic()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export1 = enumDef.ExportClassLibrary(assemblyName: "Test");
            var export2 = enumDef.ExportClassLibrary(assemblyName: "Test");
            Assert.Equal(export1, export2);
        }
    }
}
