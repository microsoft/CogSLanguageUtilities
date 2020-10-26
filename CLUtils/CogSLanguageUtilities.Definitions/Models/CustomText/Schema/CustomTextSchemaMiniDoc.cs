// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema
{
    public class CustomTextSchemaMiniDoc
    {
        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("startCharIndex")]
        public int StartCharIndex { get; set; }

        [JsonProperty("endCharIndex")]
        public int EndCharIndex { get; set; }

        [JsonProperty("positiveExtractionLabels")]
        public List<CustomTextSchemaPositiveExtractionLabel> PositiveExtractionLabels { get; set; }
    }
}