﻿using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CustomTextCliUtils.Configs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Helpers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers
{
    public class ParserServiceController : IParserController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private readonly IParserService _parserService;
        private IStorageService _sourceStorageService;
        private IStorageService _destinationStorageService;
        private readonly ILoggerService _loggerService;
        private readonly IChunkerService _chunkerService;

        public ParserServiceController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            IParserService parserService,
            ILoggerService loggerService,
            IChunkerService chunkerService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _parserService = parserService;
            _loggerService = loggerService;
            _chunkerService = chunkerService;
        }

        private void InitializeStorage(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
            _destinationStorageService = destinationFactory.CreateStorageService(destinationStorageType, storageConfigModel);
        }

        public async Task ExtractText(StorageType sourceStorageType, StorageType destinationStorageType, ChunkMethod chunkType)
        {
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;
            var convertedFiles = new ConcurrentBag<string>();
            var failedFiles = new ConcurrentDictionary<string, string>();

            // read files from source storage
            var fileNames = await _sourceStorageService.ListFilesAsync();
            // parse files
            var tasks = fileNames.Select(async fileName =>
            {
                try
                {
                    // validate types
                    _parserService.ValidateFileType(fileName);
                    // read file
                    _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                    var file = await _sourceStorageService.ReadFileAsync(fileName);
                    // parse file
                    _loggerService.LogOperation(OperationType.ParsingFile, fileName);
                    var parseResult = await _parserService.ParseFile(file);
                    // chunk file
                    _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                    var chunkedText = _chunkerService.Chunk(parseResult, chunkType, charLimit);
                    // store file
                    _loggerService.LogOperation(OperationType.StoringResult, fileName);
                    foreach (var item in chunkedText.Select((value, i) => (value, i)))
                    {
                        var newFileName = ChunkInfoHelper.GetChunkFileName(fileName, item.i);
                        await _destinationStorageService.StoreDataAsync(item.value.Text, newFileName);
                    }
                    convertedFiles.Add(fileName);
                }
                catch (CliException e)
                {
                    failedFiles[fileName] = e.Message;
                    _loggerService.LogError(e);
                }
            });
            await Task.WhenAll(tasks);
            _loggerService.LogParsingResult(convertedFiles, failedFiles);
        }


    }
}
