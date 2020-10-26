// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema
{
    public class CustomTextSchemaClassificationLabel
    {
        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("label")]
        public bool Label { get; set; }
    }
}