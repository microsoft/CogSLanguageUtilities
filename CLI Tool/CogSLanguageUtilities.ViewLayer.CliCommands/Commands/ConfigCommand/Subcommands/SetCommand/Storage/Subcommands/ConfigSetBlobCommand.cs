﻿using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("blob", Description = "sets configs for blob storage")]
    public class ConfigSetBlobCommand
    {
        [Option(CommandOptionTemplate.BlobStorageConnectionstring, Description = "azure storage account connection string")]
        public string ConnectionString { get; }

        [Option(CommandOptionTemplate.BlobStorageSourceContainer, Description = "name of source container")]
        public string SourceContainer { get; }

        [Option(CommandOptionTemplate.BlobStorageDestinationContainer, Description = "name of destination container")]
        public string DestinationContainer { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                await controller.SetBlobStorageConfigsAsync(ConnectionString, SourceContainer, DestinationContainer);
            }
        }
    }
}
