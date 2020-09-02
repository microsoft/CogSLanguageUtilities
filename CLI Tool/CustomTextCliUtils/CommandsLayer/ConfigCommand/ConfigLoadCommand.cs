using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using Microsoft.CustomTextCliUtils.Configs;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand
{
    [Command("load", Description = "loads app configs from file")]
    public class ConfigLoadCommand
    {
        [Option("--path <absolute_path>", Description = "absolute path to configs file")]
        public string configsFilePath { get; }

        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.LoadConfigsFromFile(configsFilePath);
            }

            return 1;
        }
    }
}
