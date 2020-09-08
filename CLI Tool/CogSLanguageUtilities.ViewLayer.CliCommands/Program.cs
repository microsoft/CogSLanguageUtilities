using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CogSLanguageUtilities.Core.Services.Logger;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Reflection;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.UtilitiesCommand;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs.Consts;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.PredictCommand;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.EvaluateCommand;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands
{
    [Command(Constants.ToolName)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(ConfigCommand),
        typeof(UtilitiesCommand),
        typeof(PredictCommand),
        typeof(EvaluateCommand))]
    public class Program
    {
        public static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            CommandLineApplication.Execute<Program>(args);
        }

        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }

        private static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        // TODO: Refactor unhandled exception handler
        // Where to place universal exception handler ?
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            ILoggerService loggerService = new ConsoleLoggerService();
            Exception ex = e.ExceptionObject as Exception;
            if (ex is CliException)
            {
                loggerService.LogError(ex);
            }
            else if (ex?.InnerException is CliException)
            {
                loggerService.LogError(ex.InnerException);
            }
            else if (ex?.InnerException?.InnerException is CliException)
            {
                loggerService.LogError(ex.InnerException.InnerException);
            }
            else
            {
                loggerService.LogError(ex);
            }
            Environment.Exit(1);
        }
    }
}
