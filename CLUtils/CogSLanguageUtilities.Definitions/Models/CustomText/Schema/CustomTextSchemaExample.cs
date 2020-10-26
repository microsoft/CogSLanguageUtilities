using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema
{
    public class CustomTextSchemaExample
    {
        [JsonProperty("document")]
        public CustomTextSchemaDocument Document { get; set; }

        [JsonProperty("classificationLabels")]
        public List<CustomTextSchemaClassificationLabel> ClassificationLabels { get; set; }

        [JsonProperty("miniDocs")]
        public List<CustomTextSchemaMiniDoc> MiniDocs { get; set; }
    }
}
