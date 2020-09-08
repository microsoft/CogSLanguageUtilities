﻿
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;
using System.IO;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Chunker;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public class ConfigsLoader : IConfigsLoader
    {
        readonly ConfigModel _configModel;

        public ConfigsLoader()
        {
            var filePath = Path.Combine(Constants.ConfigsFileLocalDirectory, Constants.ConfigsFileName);
            if (File.Exists(filePath))
            {
                var configsFile = File.ReadAllText(filePath);
                _configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            }
            else
            {
                throw new MissingConfigsException();
            }
        }

        public BlobStorageConfigModel GetBlobConfigModel()
        {
            return _configModel.Storage.Blob;
        }

        public ChunkerConfigModel GetChunkerConfigModel()
        {
            return _configModel.Chunker;
        }

        public LocalStorageConfigModel GetLocalConfigModel()
        {
            return _configModel.Storage.Local;
        }

        public MSReadConfigModel GetMSReadConfigModel()
        {
            return _configModel.Parser.MsRead;
        }

        public CustomTextConfigModel GetPredictionConfigModel()
        {
            return _configModel.Prediction;
        }

        public StorageConfigModel GetStorageConfigModel()
        {
            return _configModel.Storage;
        }

        public TextAnalyticsConfigModel GetTextAnalyticsConfigModel()
        {
            return _configModel.TextAnalytics;
        }
    }
}