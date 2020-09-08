using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.TextAnalyticsCommand
{
    [Command("textanalytics", Description = "shows help for text analytics command")]
    [Subcommand(
        typeof(PredictCommand))]
    public class TextAnalyticsCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
