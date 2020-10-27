using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CustomTextCliUtils.Configs;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.Import
{
    [Command("customtext")]
    class ImportCustomTextCommand
    {
        [Option("--schema-path <ABSOLUTE_PATH>")]
        public string SchemaPath { get; }

        private async Task OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildImportCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<IImportController>();
                await controller.ImportSchemaAsync(StorageType.Local, SchemaPath);
            }
        }
    }
}
