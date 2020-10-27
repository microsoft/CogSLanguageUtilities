// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class ImportController : IImportController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly ILoggerService _loggerService;
        private ICustomTextAuthoringService _customTextAuthoringService;
        private IStorageFactoryFactory _storageFactoryFactory;
        private IStorageService _sourceStorageService;

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

        private void InitializeStorage(StorageType sourceStorageType)
        {
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
        }

        public async Task ImportSchemaAsync(StorageType sourceStorageType, string schemaPath)
        {
            InitializeStorage(sourceStorageType);

            // load schema
            string schemaString = await _sourceStorageService.ReadAsStringFromAbsolutePathAsync(schemaPath);
            var customTextSchema = JsonConvert.DeserializeObject<CustomTextSchema>(schemaString);

            // prepare model ids
            var modelsDictionary = new Dictionary<string, string>(); // old model id -> new model id

            _loggerService.LogOperation(OperationType.CreatingModels);

            // add models: classifiers
            await AddClassifierModelsToAppAsync(customTextSchema, modelsDictionary);

            // add models: extractors
            //await AddExtractorModelsToAppAsync(customTextSchema, modelsDictionary);

            _loggerService.LogOperation(OperationType.AddingLabels);

            // add labels: classifiers
            await AddClassifierLabelsToAppAsync(customTextSchema, modelsDictionary);

            // add labels: extractors
            //await AddExtractorLabelsToAppAsync(customTextSchema, modelsDictionary);
        }

        private async Task AddExtractorLabelsToAppAsync(CustomTextSchema customTextSchema, Dictionary<string, string> modelsDictionary)
        {
            foreach (var example in customTextSchema.Examples)
            {
                foreach (var miniDoc in example.MiniDocs)
                {
                    var mappedMiniDoc = ImportCustomTextSchemaMapper.MapMiniDoc(miniDoc);
                    mappedMiniDoc.ModelId = modelsDictionary[mappedMiniDoc.ModelId];
                    UpdateModelIdInExtractorLabels(mappedMiniDoc.PositiveExtractionLabels, modelsDictionary);
                    await _customTextAuthoringService.AddDocumentExtractorMiniDocsAsync(
                        documentId: example.Document.DocumentId,
                        extractorId: mappedMiniDoc.ModelId,
                        miniDocs: new List<MiniDoc> { mappedMiniDoc }
                    );
                }
            }
        }

        private async Task AddClassifierLabelsToAppAsync(CustomTextSchema customTextSchema, Dictionary<string, string> modelsDictionary)
        {
            foreach (var example in customTextSchema.Examples)
            {
                foreach (var classificationLabel in example.ClassificationLabels)
                {
                    await _customTextAuthoringService.AddDocumentClassifierLabelAsync(
                        documentId: example.Document.DocumentId,
                        classifierId: modelsDictionary[classificationLabel.ModelId],
                        classifierLabel: classificationLabel.Label
                    );
                }
            }
        }

        private async Task AddExtractorModelsToAppAsync(CustomTextSchema customTextSchema, Dictionary<string, string> modelsDictionary)
        {
            foreach (var extractor in customTextSchema.Extractors)
            {
                // map model
                var mappedExtractor = ImportCustomTextSchemaMapper.MapExtractor(extractor);
                // send request
                var response = await _customTextAuthoringService.AddApplicationExtractorAsync(mappedExtractor);

                // add created extractor id to dictionary
                AddExtractorsToDictionaryRecursively(mappedExtractor, response, modelsDictionary);
            }
        }

        private async Task AddClassifierModelsToAppAsync(CustomTextSchema customTextSchema, Dictionary<string, string> modelsDictionary)
        {
            foreach (var classifier in customTextSchema.Classifiers)
            {
                // map model
                var mappedClassifier = ImportCustomTextSchemaMapper.MapClassifier(classifier);
                // send request
                var response = await _customTextAuthoringService.AddApplicationClassifierAsync(mappedClassifier);

                // add created classifier id to dictionary
                modelsDictionary.Add(classifier.Id, response.Id);
            }
        }

        private void AddExtractorsToDictionaryRecursively(CustomTextModel schemaExtractor, CustomTextModel createdExtractor, Dictionary<string, string> modelsDictionary)
        {
            if (schemaExtractor?.Id != null && createdExtractor?.Id != null)
            {
                modelsDictionary[schemaExtractor.Id] = createdExtractor.Id;
                if (schemaExtractor.Children?.Count != createdExtractor.Children?.Count)
                {
                    // TODO: error handling
                }
                if (schemaExtractor.Children != null && createdExtractor.Children != null)
                {
                    for (var i = 0; i < schemaExtractor.Children.Count; i++)
                    {
                        AddExtractorsToDictionaryRecursively(schemaExtractor.Children[i], createdExtractor.Children[i], modelsDictionary);
                    }
                }
            }
            else
            {
                // TODO: error handling
            }
        }

        private void UpdateModelIdInExtractorLabels(List<PositiveExtractionLabel> labels, Dictionary<string, string> modelsDictionary)
        {
            if (labels != null)
            {
                for (var i = 0; i < labels.Count; i++)
                {
                    labels[i].ModelId = modelsDictionary[labels[i].ModelId];
                    UpdateModelIdInExtractorLabels(labels[i].Children, modelsDictionary);
                }
            }
        }
    }
}
