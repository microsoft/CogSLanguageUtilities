﻿using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Evaluation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.TextAnalytics;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs
{
    public class ConfigModel
    {
        [JsonProperty("storage")]
        public StorageConfigModel Storage { get; set; }

        [JsonProperty("parser")]
        public ParserConfigModel Parser { get; set; }

        [JsonProperty("chunker")]
        public ChunkerConfigModel Chunker { get; set; }

        [JsonProperty("customtext")]
        public CustomTextConfigModel Prediction { get; set; }

        [JsonProperty("textanalytics")]
        public TextAnalyticsConfigModel TextAnalytics { get; set; }
        
        [JsonProperty("evaluation")]
        public EvaluationConfigModel Evaluation { get; set; }

        public ConfigModel()
        {
            Storage = new StorageConfigModel();
            Parser = new ParserConfigModel();
            Chunker = new ChunkerConfigModel();
            Prediction = new CustomTextConfigModel();
            TextAnalytics = new TextAnalyticsConfigModel();
            Evaluation = new EvaluationConfigModel();
        }
    }
}
