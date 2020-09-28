using Autofac;
using Microsoft.CognitiveSearchIntegration.Core.Controllers;
using Microsoft.CognitiveSearchIntegration.Core.Services.CognitiveSearch;
using Microsoft.CognitiveSearchIntegration.Core.Services.Storage;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Core.Helpers.HttpHandler;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public class DependencyInjectionController
    {
        public static IContainer BuildIndexCommandDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LocalStorageService>().As<IStorageService>();
            builder.RegisterType<CustomTextIndexingService>().As<ICustomTextIndexingService>();
            builder.Register(c =>
            {
                // TODO: get configuration
                return new CognitiveSearchService(new HttpHandler(), "https://shaban-search.search.windows.net", "2CD90E19736D4DDF8DE53805A2FB61A7");
            }).As<ICognitiveSearchService>();
            builder.Register<IndexingController>(c =>
            {
                // TODO: get configs
                return new IndexingController(
                    c.Resolve<IStorageService>(),
                    c.Resolve<ICustomTextIndexingService>(),
                    c.Resolve<ICognitiveSearchService>(),
                    "DefaultEndpointsProtocol=https;AccountName=toolingctlutest;AccountKey=BJFJ1KxfHf7UlPN0vruKiDETUMdousYl9AyUiYg8aXsFQnqnKMXvrFa053mO8Qqbwvs36yGzw5Y/RDgChytegQ==;EndpointSuffix=core.windows.net",
                    "paperschunked");
            });
            return builder.Build();
        }
    }
}
