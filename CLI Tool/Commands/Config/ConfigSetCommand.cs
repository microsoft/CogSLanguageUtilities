﻿using CustomTextCliUtils.Commands.Config.Set;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config
{
    [Command("set", Description = "sets app configs")]
    [Subcommand(
        typeof(ConfigSetStorageCommand),
        typeof(ConfigSetParserCommand),
        typeof(ConfigSetChunkerCommand))]
    class ConfigSetCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}