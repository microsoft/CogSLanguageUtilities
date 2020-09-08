using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.EvaluateCommand
{
    [Command("evaluate", Description = "")]
    public class EvaluateCommand
    {
        private async Task OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
