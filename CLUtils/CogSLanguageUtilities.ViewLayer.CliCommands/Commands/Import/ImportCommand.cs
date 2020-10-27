using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.Import
{
    [Command("import")]
    [Subcommand(typeof(ImportCustomTextCommand))]
    class ImportCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
