using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema
{
    public class CustomTextSchema
    {
        [JsonProperty("classifiers")]
        public List<CustomTextSchemaModel> Classifiers { get; set; }

        [JsonProperty("extractors")]
        public List<CustomTextSchemaModel> Extractors { get; set; }
    }
}
