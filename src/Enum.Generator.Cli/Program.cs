using System;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            // Parse Options from args and call 'MainWithOptions' method if succeeded, otherwise show help
            var parser = new Parser(settings =>
            {
                settings.CaseInsensitiveEnumValues = true;
            });
            var parseResult = parser.ParseArguments<ProgramOptions>(args);
            return parseResult.MapResult(
                options => MainWithOptions(options),
                errors =>
                {
                    // Show help
                    var help = HelpText.AutoBuild(parseResult);
                    help.Heading = $"enum-generator - {GetVersion()}";
                    help.Copyright = string.Empty;
                    help.AutoHelp = false;
                    help.AutoVersion = false;
                    help.AddEnumValuesToHelpText = true;
                    help.MaximumDisplayWidth = 100;
                    help.AddOptions(parseResult);

                    Console.Error.Write(help);

                    // Exit with error
                    return 1;
                });

            string GetVersion() => typeof(Program).Assembly.GetName().Version.ToString();
        }

        /// <summary>
        /// Entry point with structured options.
        /// </summary>
        /// <param name="options">Structured options for the application logic</param>
        /// <returns>Exitcode</returns>
        public static int MainWithOptions(ProgramOptions options)
        {
            // Configure services
            var services = new ServiceCollection();
            ConfigureServices(services, options.Verbose);

            // Run application
            using (var provider = services.BuildServiceProvider())
            {
                return provider.GetService<Application>().Run(
                    options.InputFile,
                    options.OutputFile,
                    options.CollectionJPath,
                    options.EntryNameJPath,
                    options.EntryValueJPath,
                    options.EntryCommentJPath,
                    options.EnumComment,
                    options.EnumNamespace,
                    options.IndentMode,
                    options.IndentSize,
                    options.NewlineMode);
            }
        }

        private static void ConfigureServices(IServiceCollection services, bool verboseLogging)
        {
            // Logging
            services.AddLogging(logConfig =>
            {
                logConfig.AddProvider(new ConsoleLoggerProvider(verboseLogging)).
                    SetMinimumLevel(verboseLogging ? LogLevel.Trace : LogLevel.Information);
            });

            // Application
            services.AddTransient<Application>();
        }
    }
}
