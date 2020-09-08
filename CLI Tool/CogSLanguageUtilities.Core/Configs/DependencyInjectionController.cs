﻿using Autofac;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using System;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.HttpHandler;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.TextAnalytics;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Concatenation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums;

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
            builder.RegisterType<ConfigServiceController>();
            return builder.Build();
        }

        public static IContainer BuildChunkerCommandDependencies()
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterType<PlainTextChunkerService>().As<IChunkerService>();
            builder.RegisterType<ChunkerServiceController>();
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
                return new ParserServiceController(configService, new StorageFactoryFactory(), parserservice,
                    loggerService, chunkerService);
            }).As<ParserServiceController>();
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
            }).As<IPredictionService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var loggerService = c.Resolve<ILoggerService>();
                var parserservice = c.Resolve<IParserService>();
                var chunkerService = CreateChunkerService(parserType);
                var predictionService = c.Resolve<IPredictionService>();
                return new PredictionServiceController(configService, new StorageFactoryFactory(), parserservice,
                    loggerService, chunkerService, predictionService);
            }).As<PredictionServiceController>();
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
            builder.RegisterType<PredictionServiceController>();
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
