﻿using CliTool.Configs.Models.Enums;
using CliTool.Exceptions;
using CliTool.Services.Configuration;
using CliTool.Services.Logger;
using CliTool.Services.Parser;
using CliTool.Services.Storage;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CliTool.ServiceControllers.Controllers
{
    class ParserServiceController : IParserServiceController
    {
        readonly IConfigurationService _configurationService;
        readonly IStorageFactory _storageFactory;
        readonly IParserService _parserService;
        IStorageService _sourceStorageService;
        IStorageService _destinationStorageService;
        readonly ILoggerService _loggerService;

        public ParserServiceController(IConfigurationService configurationService, IStorageFactory storageFactory, IParserService parserService, ILoggerService loggerService)
        {
            _configurationService = configurationService;
            _storageFactory = storageFactory;
            _parserService = parserService;
            _loggerService = loggerService;
        }

        public void SetStorageServices(StorageType source, StorageType destination)
        {
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = _storageFactory.CreateSourceStorageService(source, storageConfigModel);
            _destinationStorageService = _storageFactory.CreateDestinationStorageService(destination, storageConfigModel);
        }
        public async Task ExtractText()
        {
            // read files from source storage
            var fileNames = _sourceStorageService.ListFiles();
            // parse files
            var tasks = fileNames.Select(async fileName =>
            {
                _loggerService.Log("started processing " + fileName);
                try
                {
                    _parserService.ValidateFileType(Path.GetExtension(fileName));
                    Stream file = await _sourceStorageService.ReadFile(fileName);
                    string text = await _parserService.ExtractText(file);
                    _destinationStorageService.StoreData(text, Path.ChangeExtension(fileName, "txt"));
                }
                catch (CliException e)
                {
                    _loggerService.LogCustomError(e);
                }
                _loggerService.Log("finished processing " + fileName);
            });
            await Task.WhenAll(tasks);
        }
    }
}
