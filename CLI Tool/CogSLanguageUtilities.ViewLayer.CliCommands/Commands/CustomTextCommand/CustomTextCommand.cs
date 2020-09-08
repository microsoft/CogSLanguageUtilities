using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.LanguageUtilities.ViewLayer.CliCommands.Commands.CustomTextCommand
{
    [Command("utilities", Description = "shows help for utilities command")]
    [Subcommand(
        typeof(ParserCommand),
        typeof(ChunkCommand))]
    public class CustomTextCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
