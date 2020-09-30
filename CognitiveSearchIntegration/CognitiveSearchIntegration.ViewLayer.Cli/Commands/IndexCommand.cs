using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CognitiveSearchIntegration.Core.Controllers;
using Microsoft.CognitiveSearchIntegration.Enums.Prediction;
using Microsoft.CustomTextCliUtils.Configs;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.CliCommands.Commands
{
    [Command("index", Description = "")]
    public class IndexCommand
    {
        [Option("--schema <absolute_path>", Description = "absolute path to schema file")]
        public string Schema { get; }

        [Option("--cognitive-service <cognitive_service>", Description = "")]
        public CognitiveServiceType CognitiveSerice { get; }

        [Option("--index-name <absolute_path>", Description = "name of index to be created")]
        public string IndexName { get; }

        private async Task OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildIndexCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<IndexingController>();
                await controller.IndexCustomText(Schema, IndexName);
            }
        }
    }
}
