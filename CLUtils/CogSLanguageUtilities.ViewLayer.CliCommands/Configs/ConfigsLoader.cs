// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Configs;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Models.Configs;
using Newtonsoft.Json;
using System.IO;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs
{
    public class ConfigsLoader
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

        public StorageConfigModel GetStorageConfigModel()
        {
            return _configModel.Storage;
        }

        public LuisConfigModel GetLuisConfigModel()
        {
            return _configModel.Luis;
        }
    }
}
