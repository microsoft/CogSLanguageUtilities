

using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public interface IConfigsLoader
    {
        BlobStorageConfigModel GetBlobConfigModel();
        LocalStorageConfigModel GetLocalConfigModel();
        MSReadConfigModel GetMSReadConfigModel();
        StorageConfigModel GetStorageConfigModel();
        ChunkerConfigModel GetChunkerConfigModel();
        PredictionConfigModel GetPredictionConfigModel();
    }
}
