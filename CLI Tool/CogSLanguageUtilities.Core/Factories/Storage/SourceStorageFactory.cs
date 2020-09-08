using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Misc;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using System;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public class SourceStorageFactory : IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType storageType, StorageConfigModel storageConfigModel)
        {
            switch (storageType)
            {
                case StorageType.Blob:
                    return new BlobStorageService(storageConfigModel.Blob.ConnectionString, storageConfigModel.Blob.SourceContainer);
                case StorageType.Local:
                    return new LocalStorageService(storageConfigModel.Local.SourceDirectory);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}