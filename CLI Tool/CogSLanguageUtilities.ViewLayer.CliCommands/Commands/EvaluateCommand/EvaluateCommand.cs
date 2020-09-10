using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Prediction;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CustomTextCliUtils.Configs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.EvaluateCommand
{
    [Command("evaluate", Description = "")]
    public class EvaluateCommand
    {
        [Required]
        [Option("--source <local/blob>", Description = "[required] indicates source storage type")]
        public StorageType Source { get; }
        [Required]
        [Option("--destination <local/blob>", Description = "[required] indicates destination storage type")]
        public StorageType Destination { get; }

        private async Task OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildEvaluateCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<BatchTestingController>();
                await controller.EvaluateModelAsync(Source, Destination, CognitiveServiceType.CustomText);
            }
        }
    }
}

