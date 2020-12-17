// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Autofac;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Core.Services.IAP;
using Microsoft.CogSLanguageUtilities.Core.Services.Logger;
using Microsoft.CogSLanguageUtilities.Core.Services.Luis;
using Microsoft.CogSLanguageUtilities.Core.Services.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public class DependencyInjectionController
    {
        public static IContainer BuildIAPControllerDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ConsoleLoggerService()).As<ILoggerService>();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.RegisterType<TranscriptParser>().As<ITranscriptParser>();
            builder.RegisterType<IAPTranscriptGenerator>().As<IIAPTranscriptGenerator>();
            builder.Register(c =>
            {
                var configs = c.Resolve<IConfigsLoader>();
                return new LuisPredictionService(
                    configs.GetLuisConfigModel().Endpoint,
                    configs.GetLuisConfigModel().Key,
                    configs.GetLuisConfigModel().AppId);
            }).As<ILuisPredictionService>();

            builder.Register(c =>
            {
                var configs = c.Resolve<IConfigsLoader>();
                return new LocalStorageService(
                    configs.GetStorageConfigModel().SourcePath,
                    configs.GetStorageConfigModel().DestinationPath);
            }).As<IStorageService>();

            builder.RegisterType<IAPProccessController>();
            return builder.Build();
        }
    }
}
