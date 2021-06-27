﻿using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CustomTextCliUtils.Configs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Concatenation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class PredictionsController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private readonly IParserService _parserService;
        private IStorageService _sourceStorageService;
        private readonly ILoggerService _loggerService;
        private readonly IChunkerService _chunkerService;
        private readonly ICustomTextService _predictionService;

        public PredictionsController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            IParserService parserService,
            ILoggerService loggerService,
            IChunkerService chunkerService,
            ICustomTextService predictionService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _parserService = parserService;
            _loggerService = loggerService;
            _chunkerService = chunkerService;
            _predictionService = predictionService;
        }

        private void InitializeStorage(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
        }

        public async Task Predict(StorageType sourceStorageType, StorageType destinationStorageType, string fileName, ChunkMethod chunkType)
        {
            // initialize storage
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;

            // prediction task
            try
            {
                // validate type
                _parserService.ValidateFileType(Path.GetExtension(fileName));
                // read file
                _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                Stream file = await _sourceStorageService.ReadFileAsync(fileName);
                // parse file
                _loggerService.LogOperation(OperationType.ParsingFile, fileName);
                ParseResult parseResult = await _parserService.ParseFile(file);
                // chunk file
                _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                List<ChunkInfo> chunkedText = _chunkerService.Chunk(parseResult, chunkType, charLimit);
                // run prediction
                var chunkPredictionResults = new List<CustomTextPredictionChunkInfo>();
                _loggerService.LogOperation(OperationType.RunningPrediction, fileName);
                var tasks = chunkedText.Select(async (value, i) =>
                {
                    var customTextPredictionResponse = await _predictionService.GetPredictionAsync (value.Text);
                    var chunkInfo = new CustomTextPredictionChunkInfo
                    {
                        // chunk data
                        ChunkInfo = value,
                        // prediction data
                        CustomTextPredictionResponse = customTextPredictionResponse
                    };
                    chunkPredictionResults.Add(chunkInfo);
                });
                await Task.WhenAll(tasks);
                // store or display result
                _loggerService.LogOperation(OperationType.DisplayingResult, fileName);
                var concatenatedResult = JsonConvert.SerializeObject(chunkPredictionResults, Formatting.Indented);
                _loggerService.Log(concatenatedResult);
            }
            catch (CliException e)
            {
                _loggerService.LogError(e);
            }
        }
    }
}
