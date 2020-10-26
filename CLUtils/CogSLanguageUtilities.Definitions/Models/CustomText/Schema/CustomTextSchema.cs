﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Schema
{
    public class CustomTextSchema
    {
        [JsonProperty("classifiers")]
        public List<CustomTextSchemaModel> Classifiers { get; set; }

        [JsonProperty("extractors")]
        public List<CustomTextSchemaModel> Extractors { get; set; }

        [JsonProperty("examples")]
        public List<CustomTextSchemaExample> Examples { get; set; }
    }
}