using System;

namespace Enum.Generator.GlobalTool
{
    /// <summary>
    /// Main program class, responsible for parsing arguments and running Application logic.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for the global-tool.
        /// </summary>
        /// <remarks>Implementation forwards to the Enum.Generator.Cli project</remarks>
        /// <param name="args">Arguments provided to the tool</param>
        /// <returns>Exit code</returns>
        public static int Main(string[] args) => Cli.Program.Main(args);
    }
}
