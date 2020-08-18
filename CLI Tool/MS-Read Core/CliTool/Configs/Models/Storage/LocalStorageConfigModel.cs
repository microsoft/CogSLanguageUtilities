﻿using Newtonsoft.Json;

namespace CliTool.Configs
{
    public class LocalStorageConfigModel
    {
        [JsonProperty("sourceDirectory")]
        public string SourceDirectory { get; set; }

        [JsonProperty("destinationDirectory")]
        public string DestinationDirectory { get; set; }
    }
}