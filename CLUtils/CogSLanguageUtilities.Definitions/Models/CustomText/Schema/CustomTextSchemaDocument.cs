// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Newtonsoft.Json;
using System;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema
{
    public class CustomTextSchemaDocument
    {
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }

        [JsonProperty("exampleId")]
        public long ExampleId { get; set; }

        [JsonProperty("lastModifiedTimestamp")]
        public DateTimeOffset LastModifiedTimestamp { get; set; }
    }
}