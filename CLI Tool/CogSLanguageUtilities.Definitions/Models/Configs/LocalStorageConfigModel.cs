using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Configs
{
    public class LocalStorageConfigModel
    {
        [JsonProperty(ConfigKeys.LocalStorageSourceDir)]
        public string SourceDirectory { get; set; }

        [JsonProperty(ConfigKeys.LocalStorageDestinationDir)]
        public string DestinationDirectory { get; set; }
    }
}