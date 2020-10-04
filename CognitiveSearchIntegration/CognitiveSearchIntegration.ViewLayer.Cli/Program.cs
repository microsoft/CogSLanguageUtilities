﻿using McMaster.Extensions.CommandLineUtils;
using Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Commands;
using System.Reflection;

namespace Microsoft.CognitiveSearchIntegration.ViewLayer.Cli
{
    [Command("cognitivesearch")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(IndexCommand))]
    class Program
    {
        public static void Main(string[] args)
        {
            //AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            CommandLineApplication.Execute<Program>(args);
        }

        private static string GetVersion()
           => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        //static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        //{
        //    Environment.Exit(1);
        //}

    }
}