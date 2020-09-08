using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Misc;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;


namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public interface IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType targetStorage, StorageConfigModel storageConfigModel);
    }
}
