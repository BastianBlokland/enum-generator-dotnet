using System;
using System.Linq;
using System.Text.RegularExpressions;

using EnumGenerator.Core.Definition;
using EnumGenerator.Core.Exporter.Exceptions;
using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Exporter for creating fsharp source-code.
    /// </summary>
    public static class FSharpExporter
    {
        /// <summary>
        /// Create fsharp source-code representation of a <see cref="EnumDefinition"/>.
        /// </summary>
        /// <exception cref="Exceptions.InvalidNamespaceException">
        /// Thrown when a invalid namespace identifier is given.
        /// </exception>
        /// <exception cref="Exceptions.OutOfBoundsValueException">
        /// Thrown when enum value does not fit in given storage-type.
        /// </exception>
        /// <param name="enumDefinition">Enum to generate fsharp source-code for</param>
        /// <param name="namespace">Namespace to add the enum to</param>
        /// <param name="headerMode">Mode to use when adding a header</param>
        /// <param name="indentSize">How many spaces should be used for indents</param>
        /// <param name="newlineMode">Mode to use for ending lines</param>
        /// <param name="storageType">Underlying enum storage-type to use</param>
        /// <returns>String containing the genenerated fsharp sourcecode</returns>
        public static string ExportFSharp(
            this EnumDefinition enumDefinition,
            string @namespace,
            HeaderMode headerMode = HeaderMode.Default,
            int indentSize = 4,
            CodeBuilder.NewlineMode newlineMode = CodeBuilder.NewlineMode.Unix,
            StorageType storageType = StorageType.Implicit)
        {
            if (enumDefinition == null)
                throw new ArgumentNullException(nameof(enumDefinition));

            if (!IdentifierValidator.ValidateNamespace(@namespace))
                throw new InvalidNamespaceException(@namespace);

            foreach (var oobEntry in enumDefinition.Entries.Where(e => !storageType.Validate(e.Value)))
                throw new OutOfBoundsValueException(storageType, oobEntry.Value);

            var builder = new CodeBuilder(CodeBuilder.IndentMode.Spaces, indentSize, newlineMode);
            if (headerMode != HeaderMode.None)
            {
                builder.AddHeader();
                builder.WriteEndLine();
            }

            // Add namespace.
            builder.WriteLine($"namespace {@namespace}");
            builder.WriteEndLine();

            // Open System.CodeDom.Compiler (for the 'GeneratedCode' attribute)
            builder.WriteLine("open System.CodeDom.Compiler");
            builder.WriteEndLine();

            // Add type comment.
            if (!string.IsNullOrEmpty(enumDefinition.Comment))
                builder.AddSummary(enumDefinition.Comment);

            // Add enum definition.
            var assemblyName = typeof(FSharpExporter).Assembly.GetName();
            builder.WriteLine($"[<GeneratedCode(\"{assemblyName.Name}\", \"{assemblyName.Version}\")>]");
            builder.WriteLine($"type {enumDefinition.Identifier} =");

            // Add entries.
            foreach (var entry in enumDefinition.Entries)
            {
                var literalSuffix = storageType == StorageType.Implicit ?
                    string.Empty :
                    storageType.GetFSharpLiteralSuffix();

                builder.WriteLine($"| {entry.Name} = {entry.Value}{literalSuffix}", additionalIndent: 1);
            }

            return builder.Build();
        }

        private static void AddSummary(this CodeBuilder builder, string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException($"Invalid text: '{text}'", nameof(text));

            // Strip newlines from the text. (At the moment only single line summaries are supported).
            text = Regex.Replace(text, "(\n|\r)+", " ");

            builder.WriteLine("/// <summary>");
            builder.WriteLine($"/// {text}");
            builder.WriteLine("/// </summary>");
        }

        private static void AddHeader(this CodeBuilder builder)
        {
            var assemblyName = typeof(FSharpExporter).Assembly.GetName();
            builder.WriteLine("//------------------------------------------------------------------------------");
            builder.WriteLine("<auto-generated>", prefix: "// ");
            builder.WriteLine($"Generated by: {assemblyName.Name} - {assemblyName.Version}", prefix: "// ", additionalIndent: 1);
            builder.WriteLine("</auto-generated>", prefix: "// ");
            builder.WriteLine("//------------------------------------------------------------------------------");
        }
    }
}