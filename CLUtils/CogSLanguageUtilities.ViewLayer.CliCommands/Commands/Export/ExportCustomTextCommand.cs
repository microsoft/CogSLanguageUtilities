// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CustomTextCliUtils.Configs;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.Export
{
    [Command("customtext", Description = "Export Custom Text application models")]
    class ExportCustomTextCommand
    {
        [Option("--destination <local/blob>", Description = "[required] indicates destination storage type")]
        public StorageType Destination { get; }

        private async Task OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildExportCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<IExportCustomTextController>();
                await controller.ExportSchema(Destination);
            }
        }
    }
}
