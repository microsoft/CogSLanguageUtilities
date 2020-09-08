using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Misc;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using System;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public class DestinationStorageFactory : IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType storageType, StorageConfigModel storageConfigModel)
        {
            switch (storageType)
            {
                case StorageType.Blob:
                    return new BlobStorageService(storageConfigModel.Blob.ConnectionString, storageConfigModel.Blob.DestinationContainer);
                case StorageType.Local:
                    return new LocalStorageService(storageConfigModel.Local.DestinationDirectory);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}