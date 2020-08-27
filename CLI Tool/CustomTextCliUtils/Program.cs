﻿using CustomTextCliUtils.ApplicationLayer.Exceptions;
using CustomTextCliUtils.ApplicationLayer.Services.Logger;
using CustomTextCliUtils.CommandsLayer;
using CustomTextCliUtils.CommandsLayer.ConfigCommand;
using CustomTextCliUtils.Configs.Consts;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Reflection;

namespace CustomTextCliUtils
{
    [Command(Constants.ToolName)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(ParseCommand),
        typeof(PredictCommand),
        typeof(ConfigCommand),
        typeof(ChunkCommand))]
    class Program
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
            if (ex?.InnerException is CliException)
            {
                loggerService.LogError(ex.InnerException);
            }
            if (ex?.InnerException?.InnerException is CliException)
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
