using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema
{
    public class CustomTextSchemaModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<CustomTextSchemaModel> Children { get; set; }
    }
}