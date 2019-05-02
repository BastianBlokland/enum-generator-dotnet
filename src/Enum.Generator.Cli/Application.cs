using System;
using Microsoft.Extensions.Logging;

using Enum.Generator.Core.Utilities;

namespace Enum.Generator.Cli
{
    /// <summary>
    /// Application logic for the cli tool.
    /// </summary>
    public sealed class Application
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="logger">Optional logger to use during execution</param>
        public Application(ILogger<Application> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.logger = logger;
        }

        /// <summary>
        /// Run the generator tool.
        /// </summary>
        /// <param name="inputFile">Path to the json file to generate the enum for</param>
        /// <param name="outputFile">Path where to save the generated enum</param>
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
        /// <param name="indentMode">Mode to use when indenting text</param>
        /// <param name="indentSize">When indenting with spaces this controls how many</param>
        /// <param name="newlineMode">Mode to use when adding newlines to text</param>
        /// <returns>Exit code</returns>
        public int Run(
            string inputFile,
            string outputFile,
            string collectionJPath,
            string entryNameJPath,
            string entryValueJPath,
            string entryCommentJPath,
            string enumComment,
            string enumNamespace,
            CodeBuilder.IndentMode indentMode,
            int indentSize,
            CodeBuilder.NewlineMode newlineMode)
        {
            if (inputFile == null)
                throw new ArgumentNullException(nameof(inputFile));
            if (outputFile == null)
                throw new ArgumentNullException(nameof(outputFile));
            if (collectionJPath == null)
                throw new ArgumentNullException(nameof(collectionJPath));
            if (entryNameJPath == null)
                throw new ArgumentNullException(nameof(entryNameJPath));

            this.logger.LogInformation("Hello world");
            return 0;
        }
    }
}
