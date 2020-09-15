using Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.EvaluationNuget;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.LuisModelEvaluation.Models.Input;
using Microsoft.LuisModelEvaluation.Models.Result;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class BatchTestingController : IBatchTestingController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private IStorageService _sourceStorageService;
        private IStorageService _destinationStorageService;
        private readonly ILoggerService _loggerService;
        private readonly ICustomTextPredictionService _customTextPredictionService;
        private readonly IBatchTestingService _batchTestingService;
        private readonly ICustomTextAuthoringService _customTextAuthoringService;

        public BatchTestingController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            ILoggerService loggerService,
            ICustomTextPredictionService CustomTextPredictionService,
            ICustomTextAuthoringService customTextAuthoringService,
            IBatchTestingService batchTestingService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _loggerService = loggerService;
            _customTextPredictionService = CustomTextPredictionService;
            _customTextAuthoringService = customTextAuthoringService;
            _batchTestingService = batchTestingService;
        }

        private void InitializeStorage(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
            _destinationStorageService = destinationFactory.CreateStorageService(destinationStorageType, storageConfigModel);
        }

        public async Task EvaluateCustomTextAppAsync(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            InitializeStorage(sourceStorageType, destinationStorageType);

            // init result containers
            var convertedFiles = new ConcurrentBag<string>();
            var failedFiles = new ConcurrentDictionary<string, string>();

            // get labeled examples
            _loggerService.LogOperation(OperationType.GeneratingTestSet);
            var labeledExamples = await _customTextAuthoringService.GetLabeledExamples();
            var testData = await CreateTestData(labeledExamples);

            // evaluate model
            _loggerService.LogOperation(OperationType.EvaluatingResults);
            BatchTestResponse batchTestResponse = _batchTestingService.RunBatchTest(testData);

            // store file
            var outFileName = Constants.CustomTextEvaluationControllerOutputFileName;
            _loggerService.LogOperation(OperationType.StoringResult, outFileName);
            var responseAsJson = JsonConvert.SerializeObject(batchTestResponse, Formatting.Indented);
            await _destinationStorageService.StoreDataAsync(responseAsJson, outFileName);

            // log result
            _loggerService.LogParsingResult(convertedFiles, failedFiles);
        }

        private async Task<IEnumerable<TestingExample>> CreateTestData(Definitions.Models.CustomText.Api.LabeledExamples.Response.CustomTextGetLabeledExamplesResponse labeledExamples)
        {
            var modelsDictionary = await _customTextAuthoringService.GetModelsDictionary();
            var tasks = labeledExamples.Examples.Select(async e =>
            {
                // document text
                var documentText = await _sourceStorageService.ReadFileAsStringAsync(e.Document.DocumentId);

                // ground truth
                var actualClassId = e.ClassificationLabels.FirstOrDefault(c => c.Label == true).ModelId;
                var actualClassName = modelsDictionary[actualClassId];
                var groundTruth = new PredictionObject
                {
                    Classification = actualClassName,
                    Entities = BatchTestingInputMapper.MapCustomTextExamplesToEntitiesRecursively(e.MiniDocs, modelsDictionary)
                };

                // prediction
                var predictionResponse = await _customTextPredictionService.GetPredictionAsync(documentText);
                var PredictionData = BatchTestingInputMapper.MapCutomTextResponse(predictionResponse);

                // complete example
                return new TestingExample
                {
                    Text = documentText,
                    LabeledData = groundTruth,
                    PredictedData = PredictionData
                };
            });
            return await Task.WhenAll(tasks);
        }

    }
}
