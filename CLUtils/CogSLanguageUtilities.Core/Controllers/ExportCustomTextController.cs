// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.ExportCustomText;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class ExportCustomTextController : IExportCustomTextController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly ILoggerService _loggerService;
        private ICustomTextAuthoringService _customTextAuthoringService;
        private IStorageFactoryFactory _storageFactoryFactory;
        private IStorageService _destinationStorageService;

        public ExportCustomTextController(
            IConfigsLoader configurationService,
            ILoggerService loggerService,
            ICustomTextAuthoringService customTextAuthoringService,
            IStorageFactoryFactory storageFactoryFactory)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _loggerService = loggerService;
            _customTextAuthoringService = customTextAuthoringService;
        }

        private void InitializeStorage(StorageType destinationStorageType)
        {
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _destinationStorageService = destinationFactory.CreateStorageService(destinationStorageType, storageConfigModel);
        }

        public async Task ExportSchema(StorageType destinationStorageType)
        {
            InitializeStorage(destinationStorageType);

            // read models from app
            _loggerService.LogOperation(OperationType.ReadingModels);
            var customTextModels = await _customTextAuthoringService.GetApplicationModels();
            // map to schema format
            _loggerService.LogOperation(OperationType.GeneratingSchema);
            var schema = CustomTextSchemaMapper.MapCustomTextModelsToSchema(customTextModels.Models);
            // write schema to file
            _loggerService.LogOperation(OperationType.StoringResult, Constants.CustomTextSchemaFileName);
            await _destinationStorageService.StoreDataAsync(JsonConvert.SerializeObject(schema, Formatting.Indented), Constants.CustomTextSchemaFileName);
        }
    }
}
