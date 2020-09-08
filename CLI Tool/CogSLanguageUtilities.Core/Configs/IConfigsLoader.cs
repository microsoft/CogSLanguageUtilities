using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.TextAnalytics;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public interface IConfigsLoader
    {
        public BlobStorageConfigModel GetBlobConfigModel();
        public LocalStorageConfigModel GetLocalConfigModel();
        public MSReadConfigModel GetMSReadConfigModel();
        public StorageConfigModel GetStorageConfigModel();
        public ChunkerConfigModel GetChunkerConfigModel();
        public CustomTextConfigModel GetPredictionConfigModel();
        public TextAnalyticsConfigModel GetTextAnalyticsConfigModel();
    }
}
