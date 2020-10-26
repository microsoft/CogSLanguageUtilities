using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema
{
    public class CustomTextSchemaExample
    {
        [JsonProperty("document")]
        public CustomTextSchemaDocument Document { get; set; }

        [JsonProperty("classificationLabels")]
        public CustomTextSchemaClassificationLabel[] ClassificationLabels { get; set; }

        [JsonProperty("miniDocs")]
        public List<CustomTextSchemaMiniDoc> MiniDocs { get; set; }
    }
}
