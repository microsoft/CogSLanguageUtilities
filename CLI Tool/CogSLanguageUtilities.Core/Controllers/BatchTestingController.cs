using Microsoft.CogSLanguageUtilities.Core.Helpers.Utilities;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Concatenation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Response.Result;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Prediction;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.LuisModelEvaluation.Models.Input;
using Microsoft.LuisModelEvaluation.Models.Result;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
        private readonly ITextAnalyticsService _textAnalyticsPredictionService;
        private readonly ICustomTextService _customTextPredictionService;
        private readonly IBatchTestingService _batchTestingService;

        public BatchTestingController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            ILoggerService loggerService,
            ITextAnalyticsService textAnalyticsPredictionService,
            ICustomTextService CustomTextPredictionService,
            IBatchTestingService batchTestingService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _loggerService = loggerService;
            _textAnalyticsPredictionService = textAnalyticsPredictionService;
            _customTextPredictionService = CustomTextPredictionService;
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

        public async Task EvaluateModelAsync(StorageType sourceStorageType, StorageType destinationStorageType, CognitiveServiceType service)
        {
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;
            var defaultOps = _configurationService.GetTextAnalyticsConfigModel().DefaultOperations;
            var convertedFiles = new ConcurrentBag<string>();
            var failedFiles = new ConcurrentDictionary<string, string>();
            // Check which service to run
            var runCustomText = CognitiveServiceType.CustomText.Equals(service) || CognitiveServiceType.Both.Equals(service);
            var runTextAnalytics = service == CognitiveServiceType.TextAnalytics || service == CognitiveServiceType.Both;

            var predictedData = new Dictionary<string, CustomTextPredictionResponse>();
            var labeledData = new Dictionary<string, PredictionObject>();
            // read files from source storage
            var fileNames = await _sourceStorageService.ListFilesAsync();
            // parse files
            var tasks = fileNames.Select(async fileName =>
            {
                try
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    if (Path.GetExtension(fileName) == ".json")
                    {
                        string labeledFile = await _sourceStorageService.ReadFileAsStringAsync(fileName);
                        labeledData[fileNameWithoutExtension] = JsonConvert.DeserializeObject<PredictionObject>(labeledFile);
                    }
                    // read file
                    _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                    var fileString = await _sourceStorageService.ReadFileAsStringAsync(fileName);
                    // prediction service
                    _loggerService.LogOperation(OperationType.RunningPrediction, fileName);
                    var queries = new List<string> { fileString };
                    var customTextresponse = runCustomText ? await _customTextPredictionService.GetPredictionBatchAsync(queries) : null;
                    var sentimentResponse = runTextAnalytics && defaultOps.Sentiment ? await _textAnalyticsPredictionService.PredictSentimentBatchAsync(queries) : null;
                    var nerResponse = runTextAnalytics && defaultOps.Ner ? await _textAnalyticsPredictionService.PredictNerBatchAsync(queries) : null;
                    var keyphraseResponse = runTextAnalytics && defaultOps.Keyphrase ? await _textAnalyticsPredictionService.PredictKeyphraseBatchAsync(queries) : null;
                    // concatenation service
                    var concatenatedResponse = new PredictionResultChunkInfo
                    {
                        CustomTextResponse = customTextresponse[0]
                    };
                    var responseAsJson = JsonConvert.SerializeObject(concatenatedResponse, Formatting.Indented);
                    // store file
                    _loggerService.LogOperation(OperationType.StoringResult, fileName);
                    var newFileName = fileNameWithoutExtension + ".json";
                    await _destinationStorageService.StoreDataAsync(responseAsJson, newFileName);
                    convertedFiles.Add(fileName);
                    predictedData[fileNameWithoutExtension] = customTextresponse[0];
                }
                catch (CliException e)
                {
                    failedFiles[fileName] = e.Message;
                    _loggerService.LogError(e);
                }
            });
            await Task.WhenAll(tasks);
            // evaluate model
            _loggerService.LogOperation(OperationType.EvaluatingResults);
            List<TestingExample> testingExamples = BatchTestingMapper.MapCustomText(predictedData, labeledData);
            BatchTestResponse batchTestResponse = _batchTestingService.RunBatchTest(testingExamples);
            // store file
            var outFileName = "batchTesting.json";
            _loggerService.LogOperation(OperationType.StoringResult, outFileName);
            var responseAsJson = JsonConvert.SerializeObject(batchTestResponse, Formatting.Indented);
            await _destinationStorageService.StoreDataAsync(responseAsJson, outFileName);
            _loggerService.LogParsingResult(convertedFiles, failedFiles);
        }
    }
}
