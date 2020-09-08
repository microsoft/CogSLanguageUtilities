using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public interface IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType targetStorage, StorageConfigModel storageConfigModel);
    }
}
