using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Configs
{
    public class StorageConfigModel
    {
        [JsonProperty("blob")]
        public BlobStorageConfigModel Blob { get; set; }

        [JsonProperty("local")]
        public LocalStorageConfigModel Local { get; set; }

        public StorageConfigModel()
        {
            Blob = new BlobStorageConfigModel();
            Local = new LocalStorageConfigModel();
        }
    }
}
