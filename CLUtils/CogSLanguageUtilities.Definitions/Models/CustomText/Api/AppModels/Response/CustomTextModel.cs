// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Enums.CustomText;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response
{
    public class CustomTextModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<CustomTextModel> Children { get; set; }

        [JsonProperty("typeId")]
        public ModelType TypeId { get; set; }

        [JsonProperty("readableType")]
        public string ReadableType { get; set; }
    }
}
