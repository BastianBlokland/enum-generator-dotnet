using System;

namespace Enum.Generator.Cli
{
    /// <summary>
    /// Main program class, responsible for parsing arguments and running Application logic.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for the cli-tool.
        /// </summary>
        /// <param name="args">Arguments provided to the tool</param>
        /// <returns>Exit code</returns>
        public static int Main(string[] args)
        {
            Console.WriteLine($"Hello world, arg-length: {args.Length}");
            return 0;
        }
    }
}
