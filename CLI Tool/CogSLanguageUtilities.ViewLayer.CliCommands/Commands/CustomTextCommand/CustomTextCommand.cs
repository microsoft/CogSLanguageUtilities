using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.CustomTextCommand
{
    [Command("customtext", Description = "shows help for custom text command")]
    [Subcommand(
        typeof(PredictCommand))]
    public class CustomTextCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
