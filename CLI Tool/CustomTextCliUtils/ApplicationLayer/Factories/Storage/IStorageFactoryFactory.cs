

using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public interface IStorageFactoryFactory
    {
        IStorageFactory CreateStorageFactory(TargetStorage targetStorage);
    }
}
