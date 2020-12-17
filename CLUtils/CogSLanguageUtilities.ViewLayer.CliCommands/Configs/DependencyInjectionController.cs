// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Autofac;
using Microsoft.IAPUtilities.Core.Controllers;
using Microsoft.IAPUtilities.Core.Services.IAP;
using Microsoft.IAPUtilities.Core.Services.Logger;
using Microsoft.IAPUtilities.Core.Services.Luis;
using Microsoft.IAPUtilities.Core.Services.Storage;
using Microsoft.IAPUtilities.Definitions.APIs.Controllers;
using Microsoft.IAPUtilities.Definitions.APIs.Services;

namespace Microsoft.IAPUtilities.ViewLayer.CliCommands.Configs
{
    public class DependencyInjectionController
    {
        public static IContainer BuildIAPControllerDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ConsoleLoggerService()).As<ILoggerService>();
            builder.RegisterType<ConfigsLoader>();
            builder.RegisterType<TranscriptParser>().As<ITranscriptParser>();
            builder.RegisterType<IAPResultGenerator>().As<IIAPResultGenerator>();
            builder.Register(c =>
            {
                var configs = c.Resolve<ConfigsLoader>();
                return new LuisPredictionService(
                    configs.GetLuisConfigModel().Endpoint,
                    configs.GetLuisConfigModel().Key,
                    configs.GetLuisConfigModel().AppId);
            }).As<ILuisPredictionService>();

            builder.Register(c =>
            {
                var configs = c.Resolve<ConfigsLoader>();
                return new DiskStorageService(
                    configs.GetStorageConfigModel().SourcePath,
                    configs.GetStorageConfigModel().DestinationPath);
            }).As<IStorageService>();

            builder.RegisterType<IAPProccessController>().As<IIAPProccessController>();
            return builder.Build();
        }
    }
}
