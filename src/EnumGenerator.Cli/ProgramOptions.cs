using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Cli
{
    /// <summary>
    /// Class containing all options that can be passed to the cli tool.
    /// </summary>
    public sealed class ProgramOptions
    {
        /// <summary>List of examples used for generating the helptext</summary>
        [Usage(ApplicationAlias = "enum-generator")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Generate a enum", new ProgramOptions
                {
                    InputFile = "Path/To/Example.json",
                    OutputFile = "Path/To/Enum.g.cs"
                });
            }
        }

        /// <summary>Path to the json-file to generate the enum from</summary>
        [Option('i', "input", Required = true, HelpText = "Json file to generate the enum from.")]
        public string InputFile { get; set; }

        /// <summary>Path where to save the generated enum</summary>
        [Option('o', "output", Required = true, HelpText = "Target path for the generated enum.")]
        public string OutputFile { get; set; }

        /// <summary>Type of output file to generate</summary>
        [Option('t', "type", Required = false, Default = OutputType.CSharp, HelpText = "Type of output file to generate.")]
        public OutputType OutputType { get; set; }

        /// <summary>JPath to the collection to base the enum on</summary>
        [Option('c', "collection", Required = false, Default = "[*]", HelpText = "JPath to the collection to base the enum on.")]
        public string CollectionJPath { get; set; }

        /// <summary>JPath to a name field of a entry in the input file</summary>
        [Option('e', "entryname", Required = false, Default = "name", HelpText = "JPath to the name field of a entry in the input file.")]
        public string EntryNameJPath { get; set; }

        /// <summary>JPath to a value field of a entry in the input file</summary>
        [Option("entryvalue", Required = false, HelpText = "JPath to a value field of a entry in the input file.")]
        public string EntryValueJPath { get; set; }

        /// <summary>JPath to a comment field of a entry in the input file</summary>
        [Option("entrycomment", Required = false, HelpText = "JPath to a comment field of a entry in the input file.")]
        public string EntryCommentJPath { get; set; }

        /// <summary>Comment to add to the generated enum</summary>
        [Option("comment", Required = false, HelpText = "Comment to add to the generated enum.")]
        public string EnumComment { get; set; }

        /// <summary>Namespace to add the created enum to</summary>
        [Option("namespace", Required = false, HelpText = "Namespace to add the created enum to.")]
        public string EnumNamespace { get; set; }

        /// <summary>Mode to use when adding a header</summary>
        [Option("headermode", Required = false, Default = Core.Exporter.HeaderMode.Default, HelpText = "Mode to use when adding a header.")]
        public Core.Exporter.HeaderMode HeaderMode { get; set; }

        /// <summary>Indentation to use while writing output text</summary>
        [Option("indentmode", Required = false, Default = CodeBuilder.IndentMode.Spaces, HelpText = "Indentation to use when writing output text.")]
        public CodeBuilder.IndentMode IndentMode { get; set; }

        /// <summary>When indenting with spaces this controls how many</summary>
        [Option("indentsize", Required = false, Default = 4, HelpText = "When indenting with spaces this controls how many.")]
        public int IndentSize { get; set; }

        /// <summary>Mode to use for ending lines</summary>
        [Option("newlinemode", Required = false, Default = CodeBuilder.NewlineMode.Unix, HelpText = "Which newline mode to use when writing output text.")]
        public CodeBuilder.NewlineMode NewlineMode { get; set; }

        /// <summary>Storage type for the exported enum</summary>
        [Option("storagetype", Required = false, Default = Core.Exporter.StorageType.Implicit, HelpText = "Underlying storage type for the exported enum.")]
        public Core.Exporter.StorageType StorageType { get; set; }

        /// <summary>Mode to use when writing curly-brackets</summary>
        [Option("curlybracket", Required = false, Default = Core.Exporter.CurlyBracketMode.NewLine, HelpText = "Which curlybracket-mode to use.")]
        public Core.Exporter.CurlyBracketMode CurlyBracketMode { get; set; }

        /// <summary>Switch to enable verbose logging</summary>
        [Option('v', "verbose", Required = false, HelpText = "Enable verbose output.")]
        public bool Verbose { get; set; }
    }
}
