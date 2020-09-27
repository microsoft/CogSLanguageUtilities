using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.Export
{
    [Command("export", Description = "Export application models")]
    [Subcommand(typeof(ExportCustomTextCommand))]
    public class ExportCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
