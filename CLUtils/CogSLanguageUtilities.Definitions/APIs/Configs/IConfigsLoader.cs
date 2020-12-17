// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs
{
    public interface IConfigsLoader
    {
        public StorageConfigModel GetStorageConfigModel();

        public LuisConfigModel GetLuisConfigModel();
    }
}