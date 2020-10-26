// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.ExportCustomText;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class ImportController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly ILoggerService _loggerService;
        private ICustomTextAuthoringService _customTextAuthoringService;
        private IStorageFactoryFactory _storageFactoryFactory;
        private IStorageService _destinationStorageService;

        public ImportController(
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

        public async Task ImportSchema(StorageType destinationStorageType)
        {
            InitializeStorage(destinationStorageType);

            // load schema
            var customTextSchema = new CustomTextSchema();

            // prepare model ids
            var modelsDictionary = new Dictionary<string, string>(); // old model id -> new model id

            // add models: classifiers
            customTextSchema.Classifiers.ForEach(async classifier =>
            {
                // map model
                var mappedClassifier = new CustomTextModel
                {
                    Name = classifier.Name,
                    Description = classifier.Description
                };
                // send request
                var response = await _customTextAuthoringService.AddApplicationClassifierAsync(mappedClassifier);

                // get created classifier id
                modelsDictionary.Add(classifier.Id, "asd");
            });

            // add models: extractors
            customTextSchema.Extractors.ForEach(async extractor =>
            {
                // map model
                var mappedExtractor = new CustomTextModel
                {
                    Name = extractor.Name,
                    Description = extractor.Description,
                    Children = null
                };
                // send request
                var response = await _customTextAuthoringService.AddApplicationExtractorAsync(mappedExtractor);

                // get created classifier id
                modelsDictionary.Add(extractor.Id, "asd");
            });

            // add labels: classifiers
            customTextSchema.Examples.ForEach(example =>
            {
                example.ClassificationLabels.ForEach(async classificationLabel =>
                {
                    await _customTextAuthoringService.AddDocumentClassifierLabelAsync(
                        documentId: example.Document.DocumentId,
                        classifierId: modelsDictionary[classificationLabel.ModelId],
                        classifierLabel: classificationLabel.Label
                    );
                });
            });

            // add labels: extractors
            customTextSchema.Examples.ForEach(async example =>
            {
                example.MiniDocs.ForEach(async miniDoc =>
                {
                    await _customTextAuthoringService.AddDocumentExtractorMiniDocsAsync(
                        documentId: example.Document.DocumentId,
                        extractorId: modelsDictionary[miniDoc.ModelId],
                        miniDocs: new List<MiniDoc> { miniDoc /* mapper */ }
                    );
                });
                
            });
        }
    }
}
