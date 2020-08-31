﻿using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class PredictionConfigModel
    {
        [JsonProperty(ConfigKeys.PredictionCustomTextKey)]
        public string CustomTextKey { get; set; }

        [JsonProperty(ConfigKeys.PredictionEndpointUrl)]
        public string EndpointUrl { get; set; }

        [JsonProperty(ConfigKeys.PredictionAppId)]
        public string AppId { get; set; }

        [JsonProperty(ConfigKeys.PredictionVersionId)]
        public string VersionId { get; set; }
    }
}
