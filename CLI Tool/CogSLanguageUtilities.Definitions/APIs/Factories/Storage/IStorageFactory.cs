﻿using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage
{
    public interface IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType targetStorage, StorageConfigModel storageConfigModel);
    }
}