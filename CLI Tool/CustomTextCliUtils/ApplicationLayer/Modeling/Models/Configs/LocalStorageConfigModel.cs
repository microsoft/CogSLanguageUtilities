﻿using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class LocalStorageConfigModel
    {
        [JsonProperty(ConfigKeys.LocalStorageSourceDir)]
        public string SourceDirectory { get; set; }

        [JsonProperty(ConfigKeys.LocalStorageDestinationDir)]
        public string DestinationDirectory { get; set; }
    }
}