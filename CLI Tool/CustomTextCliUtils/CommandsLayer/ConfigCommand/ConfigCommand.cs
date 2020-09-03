﻿using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand
{
    [Command("config", Description = "shows or sets app configs")]
    [Subcommand(
        typeof(ConfigShowCommand),
        typeof(ConfigSetCommand),
        typeof(ConfigLoadCommand))]
    public class ConfigCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
