﻿using Autofac;
using Microsoft.CogSLanguageUtilities.Core.Factories.Storage;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Core.Services.Logger;
using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Core.Services.Storage;
using System;
using Microsoft.CogSLanguageUtilities.Core.Services.Chunker;
using Microsoft.CogSLanguageUtilities.Core.Services.Prediction;
using Microsoft.CogSLanguageUtilities.Core.Helpers.HttpHandler;
using Microsoft.CogSLanguageUtilities.Core.Services.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Core.Services.Concatenation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public class DependencyInjectionController
    {
        private static ContainerBuilder BuildCommonDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ConsoleLoggerService()).As<ILoggerService>();
            return builder;
        }

        public static IContainer BuildConfigCommandDependencies()
        {
            var builder = BuildCommonDependencies();
            builder.RegisterInstance(new LocalStorageService(Constants.ConfigsFileLocalDirectory)).As<IStorageService>();
            builder.RegisterType<ConfigsController>();
            return builder.Build();
        }

        public static IContainer BuildChunkerCommandDependencies()
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterType<PlainTextChunkerService>().As<IChunkerService>();
            builder.RegisterType<ChunkerController>();
            return builder.Build();
        }

        public static IContainer BuildParseCommandDependencies(ParserType parserType)
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return CreateParserService(parserType, configService);
            }).As<IParserService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var loggerService = c.Resolve<ILoggerService>();
                var parserservice = c.Resolve<IParserService>();
                var chunkerService = CreateChunkerService(parserType);
                return new ParserController(configService, new StorageFactoryFactory(), parserservice,
                    loggerService, chunkerService);
            }).As<ParserController>();
            return builder.Build();
        }

        public static IContainer BuildPredictCommandDependencies(ParserType parserType)
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return CreateParserService(parserType, configService);
            }).As<IParserService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var predictionConfigs = configService.GetPredictionConfigModel();
                return new CustomTextPredictionService(new HttpHandler(), predictionConfigs.CustomTextKey, predictionConfigs.EndpointUrl,
                    predictionConfigs.AppId);
            }).As<ICustomTextService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var loggerService = c.Resolve<ILoggerService>();
                var parserservice = c.Resolve<IParserService>();
                var chunkerService = CreateChunkerService(parserType);
                var predictionService = c.Resolve<ICustomTextService>();
                return new PredictionsController(configService, new StorageFactoryFactory(), parserservice,
                    loggerService, chunkerService, predictionService);
            }).As<PredictionsController>();
            return builder.Build();
        }

        public static IContainer BuildTextAnalyticsCommandDependencies(ParserType parserType)
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.RegisterType<ConcatenationService>().As<IConcatenationService>();
            builder.RegisterInstance<IChunkerService>(CreateChunkerService(parserType));
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return CreateParserService(parserType, configService);
            }).As<IParserService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return new TextAnalyticsPredictionService(
                    configService.GetTextAnalyticsConfigModel().AzureResourceKey,
                    configService.GetTextAnalyticsConfigModel().AzureResourceEndpoint,
                    configService.GetTextAnalyticsConfigModel().DefaultLanguage);
            }).As<ITextAnalyticsPredictionService>();
            builder.RegisterType<TextAnalyticsController>();
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterType<PredictionsController>();
            return builder.Build();
        }

        private static IParserService CreateParserService(ParserType parserType, IConfigsLoader configService)
        {
            if (parserType.Equals(ParserType.MSRead))
            {
                var msReadConfig = configService.GetMSReadConfigModel();
                return new MSReadParserService(msReadConfig.CognitiveServiceEndPoint, msReadConfig.CongnitiveServiceKey);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static IChunkerService CreateChunkerService(ParserType parserType)
        {
            if (parserType.Equals(ParserType.MSRead))
            {
                return new MsReadChunkerService();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
