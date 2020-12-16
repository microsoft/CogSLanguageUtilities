// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CustomTextCliUtils.Configs;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.RunPipelineCommand
{
    [Command("run", Description = "")]
    public class RunPipelineCommand
    {
        private async Task OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildIAPControllerDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<IAPProccessController>();
                await controller.Run();
            }
        }
    }
}
