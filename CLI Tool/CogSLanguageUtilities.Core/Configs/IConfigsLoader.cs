

using Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Configs;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public interface IConfigsLoader
    {
        public BlobStorageConfigModel GetBlobConfigModel();
        public LocalStorageConfigModel GetLocalConfigModel();
        public MSReadConfigModel GetMSReadConfigModel();
        public StorageConfigModel GetStorageConfigModel();
        public ChunkerConfigModel GetChunkerConfigModel();
        public PredictionConfigModel GetPredictionConfigModel();
        public TextAnalyticsConfigModel GetTextAnalyticsConfigModel();
    }
}
