﻿using Autofac;
using CliTool.Configs;
using CliTool.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config.Show
{
    [Command("local")]
    class ConfigShowLocalCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowStorageLocalConfigs();
            }
            return 1;
        }
    }
}