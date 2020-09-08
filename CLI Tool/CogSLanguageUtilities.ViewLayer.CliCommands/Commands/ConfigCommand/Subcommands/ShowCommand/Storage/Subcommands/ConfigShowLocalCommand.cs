﻿using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("local", Description = "shows configs for local storage")]
    public class ConfigShowLocalCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowStorageLocalConfigs();
            }
        }
    }
}
