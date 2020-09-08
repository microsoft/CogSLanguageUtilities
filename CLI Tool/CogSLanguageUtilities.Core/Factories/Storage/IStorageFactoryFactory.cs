using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public interface IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage);
    }
}
