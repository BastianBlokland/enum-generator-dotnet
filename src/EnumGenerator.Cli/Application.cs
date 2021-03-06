using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

using EnumGenerator.Core.Utilities;
using EnumGenerator.Core.Mapping;
using EnumGenerator.Core.Mapping.Exceptions;
using EnumGenerator.Core.Definition;
using EnumGenerator.Core.Exporter;

namespace EnumGenerator.Cli
{
    /// <summary>
    /// Application logic for the cli tool.
    /// </summary>
    public sealed class Application
    {
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        private readonly Core.ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="logger">Optional logger to use during execution</param>
        public Application(ILogger<Application> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.logger = new LoggerAdapter(logger);
        }

        /// <summary>
        /// Run the generator tool.
        /// </summary>
        /// <param name="inputFile">Path to the json file to generate the enum for</param>
        /// <param name="outputFile">Path where to save the generated enum</param>
        /// <param name="outputType">Type of output file to generate</param>
        /// <param name="collectionJPath">JPath to the collection in the input file</param>
        /// <param name="entryNameJPath">
        /// JPath to the name field in an entry in the input file</param>
        /// <param name="entryValueJPath">
        /// Optional JPath to the value field in an entry in the input file
        /// </param>
        /// <param name="entryCommentJPath">
        /// Optional JPath to the comment field in an entry in the input file
        /// </param>
        /// <param name="enumComment">
        /// Optional comment to add to the generated enum.
        /// </param>
        /// <param name="enumNamespace">
        /// Optional namespace to add the generated enum to.
        /// </param>
        /// <param name="headerMode">Mode to use when adding a header</param>
        /// <param name="indentMode">Mode to use when indenting text</param>
        /// <param name="indentSize">When indenting with spaces this controls how many</param>
        /// <param name="newlineMode">Mode to use when adding newlines to text</param>
        /// <param name="storageType">Storage type for the exported enum</param>
        /// <param name="curlyBracketMode">Mode to use when writing curly-brackets</param>
        /// <returns>Exit code</returns>
        public int Run(
            string inputFile,
            string outputFile,
            OutputType outputType,
            string collectionJPath,
            string entryNameJPath,
            string entryValueJPath,
            string entryCommentJPath,
            string enumComment,
            string enumNamespace,
            Core.Exporter.HeaderMode headerMode,
            CodeBuilder.IndentMode indentMode,
            int indentSize,
            CodeBuilder.NewlineMode newlineMode,
            Core.Exporter.StorageType storageType,
            Core.Exporter.CurlyBracketMode curlyBracketMode)
        {
            if (string.IsNullOrEmpty(inputFile))
                throw new ArgumentException($"Invalid path: '{inputFile}'", nameof(inputFile));
            if (string.IsNullOrEmpty(outputFile))
                throw new ArgumentException($"Invalid path: '{outputFile}'", nameof(outputFile));
            if (string.IsNullOrEmpty(collectionJPath))
                throw new ArgumentException($"Invalid JPath: '{collectionJPath}'", nameof(collectionJPath));
            if (string.IsNullOrEmpty(entryNameJPath))
                throw new ArgumentException($"Invalid JPath: '{entryNameJPath}'", nameof(entryNameJPath));

            // Resolve relative paths to absolute paths.
            var fullInputPath = this.GetFullPath(inputFile);
            var fullOutputPath = this.GetFullPath(outputFile);
            if (fullInputPath == null || fullOutputPath == null)
                return 1;

            // Get input.
            var inputJson = GetInputJson();
            if (inputJson == null)
                return 1;

            // Generate enum name.
            var enumName = GetEnumName();
            if (enumName == null)
                return 1;

            // Create mapping context.
            var context = Context.Create(
                collectionJPath,
                entryNameJPath,
                entryValueJPath,
                entryCommentJPath,
                this.logger);

            // Map enum.
            EnumDefinition enumDefinition = null;
            try
            {
                enumDefinition = context.MapEnum(inputJson, enumName, enumComment);
            }
            catch (JsonParsingFailureException)
            {
                this.logger.LogCritical("Failed to parse input file: invalid json");
                return 1;
            }
            catch (MappingFailureException e)
            {
                this.logger.LogCritical($"Failed to map enum: {e.InnerException.Message}");
                return 1;
            }

            // Export.
            var output = Export(enumDefinition);
            if (output == null)
                return 1;

            // Save to file.
            try
            {
                if (!fullOutputPath.EndsWith(GetRequiredExtension(outputType), StringComparison.OrdinalIgnoreCase))
                    fullOutputPath = $"{fullOutputPath}{GetDesiredExtension(outputType)}";
                Directory.GetParent(fullOutputPath).Create();
                using (var stream = new FileStream(fullOutputPath, FileMode.Create, FileAccess.Write))
                {
                    stream.Write(output);
                }

                this.logger.LogInformation($"Written enum to: '{fullOutputPath}'");
            }
            catch (Exception e)
            {
                this.logger.LogCritical($"Failed to save to '{fullOutputPath}': {e.Message}");
                return 1;
            }

            return 0;

            byte[] Export(EnumDefinition enumDef)
            {
                switch (outputType)
                {
                    case OutputType.CSharp:
                        try
                        {
                            return Utf8NoBom.GetBytes(enumDef.ExportCSharp(
                                enumNamespace,
                                headerMode,
                                indentMode,
                                indentSize,
                                newlineMode,
                                storageType,
                                curlyBracketMode));
                        }
                        catch (Exception e)
                        {
                            this.logger.LogCritical($"Failed to generate csharp: {e.Message}");
                            return null;
                        }

                    case OutputType.FSharp:
                        try
                        {
                            return Utf8NoBom.GetBytes(enumDef.ExportFSharp(
                                string.IsNullOrEmpty(enumNamespace) ? "Generated" : enumNamespace,
                                headerMode,
                                indentSize,
                                newlineMode,
                                storageType));
                        }
                        catch (Exception e)
                        {
                            this.logger.LogCritical($"Failed to generate fsharp: {e.Message}");
                            return null;
                        }

                    case OutputType.VisualBasic:
                        try
                        {
                            return Utf8NoBom.GetBytes(enumDef.ExportVisualBasic(
                                enumNamespace,
                                headerMode,
                                indentMode,
                                indentSize,
                                newlineMode,
                                storageType));
                        }
                        catch (Exception e)
                        {
                            this.logger.LogCritical($"Failed to generate csharp: {e.Message}");
                            return null;
                        }

                    case OutputType.Cil:
                        try
                        {
                            return Utf8NoBom.GetBytes(enumDef.ExportCil(
                                assemblyName: enumName,
                                enumNamespace,
                                headerMode,
                                indentMode,
                                indentSize,
                                newlineMode,
                                storageType,
                                curlyBracketMode));
                        }
                        catch (Exception e)
                        {
                            this.logger.LogCritical($"Failed to generate cil: {e.Message}");
                            return null;
                        }

                    case OutputType.ClassLibrary:
                        try
                        {
                            return enumDef.ExportClassLibrary(
                                assemblyName: enumName,
                                enumNamespace,
                                storageType);
                        }
                        catch (Exception e)
                        {
                            this.logger.LogCritical($"Failed to generate class-library: {e.Message}");
                            return null;
                        }

                    default:
                        this.logger.LogCritical($"Unsupported output-type '{outputType}'");
                        return null;
                }
            }

            string GetInputJson()
            {
                if (!File.Exists(fullInputPath))
                {
                    this.logger.LogCritical($"No file found at: '{fullInputPath}'");
                    return null;
                }

                try
                {
                    var result = File.ReadAllText(fullInputPath);
                    this.logger.LogDebug($"'{result.Length}' characters read from: '{fullInputPath}'");
                    return result;
                }
                catch (Exception e)
                {
                    this.logger.LogCritical($"Unable to read file '{fullInputPath}': {e.Message}");
                    return null;
                }
            }

            string GetEnumName()
            {
                // Determine file-name.
                string fileName;
                try
                {
                    fileName = Path.GetFileNameWithoutExtension(fullOutputPath);
                    if (fileName.EndsWith(".g", StringComparison.OrdinalIgnoreCase))
                        fileName = fileName.Substring(0, fileName.Length - 2);
                }
                catch (Exception)
                {
                    this.logger.LogCritical($"Unable to get file-name from path: '{fullOutputPath}'");
                    return null;
                }

                // Convert file-name into valid identifier.
                if (IdentifierCreator.TryCreateIdentifier(fileName, out var nameId))
                {
                    this.logger.LogDebug($"Generated enum-name: '{nameId}' from file-name: '{fileName}'");
                    return nameId;
                }

                this.logger.LogCritical($"Unable to create valid identifier from file-name: '{fileName}'");
                return null;
            }
        }

        private static string GetRequiredExtension(OutputType outputType)
        {
            switch (outputType)
            {
                case OutputType.CSharp:
                    return ".cs";
                case OutputType.FSharp:
                    return ".fs";
                case OutputType.VisualBasic:
                    return ".vb";
                case OutputType.Cil:
                    return ".il";
                case OutputType.ClassLibrary:
                    return ".dll";
                default:
                    throw new InvalidOperationException($"Unknown output: '{outputType}'.");
            }
        }

        private static string GetDesiredExtension(OutputType outputType)
        {
            switch (outputType)
            {
                case OutputType.CSharp:
                    return ".g.cs";
                case OutputType.FSharp:
                    return ".g.fs";
                case OutputType.VisualBasic:
                    return ".g.vb";
                case OutputType.Cil:
                    return ".g.il";
                case OutputType.ClassLibrary:
                    return ".dll";
                default:
                    throw new InvalidOperationException($"Unknown output: '{outputType}'.");
            }
        }

        private string GetFullPath(string relativePath)
        {
            try
            {
                var result = Path.GetFullPath(relativePath);
                this.logger.LogDebug($"Resolved absolute path: '{result}'");
                return result;
            }
            catch
            {
                this.logger.LogCritical($"Unable to determine absolute path for: '{relativePath}'");
                return null;
            }
        }
    }
}
