﻿using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.Configs.Consts;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
{
    [Command("prediction", Description = "sets configs for prediction")]
    public class ConfigSetPredictionCommand
    {
        [Option(CommandOptionTemplate.CustomTextAzureResourceKey, Description = "custom text app prediction resource key")]
        public string CustomTextKey { get; }
        [Option(CommandOptionTemplate.CustomTextAzureResourceEndpoint, Description = "custom text app prediction resource endpoint url")]
        public string EndpointUrl { get; }
        [Option(CommandOptionTemplate.CustomTextAppId, Description = "custom text app id")]
        public string AppId { get; }

        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.SetPredictionConfigsAsync(CustomTextKey, EndpointUrl, AppId).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            return 1;
        }
    }
}
