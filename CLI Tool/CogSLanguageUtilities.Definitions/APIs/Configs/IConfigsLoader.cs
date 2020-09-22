﻿using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.TextAnalytics;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs
{
    public interface IConfigsLoader
    {
        public BlobStorageConfigModel GetBlobConfigModel();
        public ChunkerConfigModel GetChunkerConfigModel();
        public LocalStorageConfigModel GetLocalConfigModel();
        public MSReadConfigModel GetMSReadConfigModel();
        public CustomTextConfigModel GetCustomTextConfigModel();
        public StorageConfigModel GetStorageConfigModel();
        public TextAnalyticsConfigModel GetTextAnalyticsConfigModel();
    }
}