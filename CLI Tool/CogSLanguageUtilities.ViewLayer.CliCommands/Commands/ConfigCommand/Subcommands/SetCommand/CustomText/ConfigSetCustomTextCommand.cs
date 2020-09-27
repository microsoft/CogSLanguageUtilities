using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("customtext", Description = "sets configs for Custom Text")]
    [Subcommand(
        typeof(ConfigSetCustomTextAuthoringCommand),
        typeof(ConfigSetCustomTextPredictionCommand))]
    public class ConfigSetCustomTextCommand
    {
        private void OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.SetPredictionConfigsAsync(CustomTextKey, EndpointUrl, AppId).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}
