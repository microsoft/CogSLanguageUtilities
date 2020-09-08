﻿using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs.Consts;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("msread", Description = "sets configs for msread parser")]
    public class ConfigSetMsReadCommand
    {
        [Option(CommandOptionTemplate.MSReadAzureResourceKey, Description = "azure congnitive services key")]
        public string CognitiveServicesKey { get; }

        [Option(CommandOptionTemplate.MSReadAzureResourceEndpoint, Description = "endpoint url for azure congnitive services")]
        public string EndpointUrl { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                await controller.SetMsReadConfigsAsync(CognitiveServicesKey, EndpointUrl);
            }
        }
    }
}