﻿using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("blob", Description = "shows configs for blob storage")]
    public class ConfigShowBlobCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowStorageBlobConfigs();
            }
        }
    }
}