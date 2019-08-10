using Xunit;

using EnumGenerator.Core.Builder;
using EnumGenerator.Core.Exporter;
using EnumGenerator.Core.Exporter.Exceptions;

namespace EnumGenerator.Tests.Builder
{
    public sealed class CSharpExporterTests
    {
        private const string Version = "3.5.0.0";

        [Fact]
        public void ThrowsIfExportedWithInvalidNamespace() => Assert.Throws<InvalidNamespaceException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            var enumDef = builder.Build();

            enumDef.ExportCSharp(@namespace: "0Test");
        });

        [Fact]
        public void ThrowsIfExportedWithOutOfBoundsValue() => Assert.Throws<OutOfBoundsValueException>(() =>
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", -1);
            var enumDef = builder.Build();

            enumDef.ExportCSharp(storageType: StorageType.Unsigned8Bit);
        });

        [Fact]
        public void BasicEnumIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp();

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

[GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
public enum TestEnum
{{
    A = 1,
    B = 2,
}}
",
                actual: export);
        }

        [Fact]
        public void NegativeValuesAreExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", -1);
            builder.PushEntry("B", -2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp();

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

[GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
public enum TestEnum
{{
    A = -1,
    B = -2,
}}
",
                actual: export);
        }

        [Fact]
        public void CommentsAreExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.Comment = "Summary comment.";
            builder.PushEntry("A", 1, "This is entry A.");
            builder.PushEntry("B", 2, "This is entry B.");
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp();

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

/// <summary>
/// Summary comment.
/// </summary>
[GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
public enum TestEnum
{{
    /// <summary>
    /// This is entry A.
    /// </summary>
    A = 1,

    /// <summary>
    /// This is entry B.
    /// </summary>
    B = 2,
}}
",
                actual: export);
        }

        [Fact]
        public void CanBeExportedWithTabs()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp(indentMode: Core.Utilities.CodeBuilder.IndentMode.Tabs);

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
// 	Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

[GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
public enum TestEnum
{{
	A = 1,
	B = 2,
}}
",
                actual: export);
        }

        [Fact]
        public void CanBeExportedWithLessSpaces()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp(spaceIndentSize: 2);

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//   Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

[GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
public enum TestEnum
{{
  A = 1,
  B = 2,
}}
",
                actual: export);
        }

        [Fact]
        public void NamespaceIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp(@namespace: "A.B.C");

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

namespace A.B.C
{{
    [GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
    public enum TestEnum
    {{
        A = 1,
        B = 2,
    }}
}}
",
                actual: export);
        }

        [Fact]
        public void StorageTypeIsExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp(storageType: StorageType.Unsigned8Bit);

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

[GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
public enum TestEnum : byte
{{
    A = 1,
    B = 2,
}}
",
                actual: export);
        }

        [Fact]
        public void SameLineCurlyBracketsAreExported()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp(@namespace: "Test", curlyBracketMode: CurlyBracketMode.SameLine);

            Assert.Equal(
                expected:
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by: EnumGenerator.Core - {Version}
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

namespace Test {{
    [GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
    public enum TestEnum {{
        A = 1,
        B = 2,
    }}
}}
",
                actual: export);
        }

        [Fact]
        public void CanBeExportedWithoutHeader()
        {
            var builder = new EnumBuilder("TestEnum");
            builder.PushEntry("A", 1);
            builder.PushEntry("B", 2);
            var enumDef = builder.Build();

            var export = enumDef.ExportCSharp(headerMode: HeaderMode.None);

            Assert.Equal(
                expected:
$@"using System.CodeDom.Compiler;

[GeneratedCode(""EnumGenerator.Core"", ""{Version}"")]
public enum TestEnum
{{
    A = 1,
    B = 2,
}}
",
                actual: export);
        }
    }
}
