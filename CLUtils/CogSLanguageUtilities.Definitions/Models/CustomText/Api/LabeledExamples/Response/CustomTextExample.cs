// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response
{
    public class CustomTextExample
    {
        [JsonProperty("document")]
        public Document Document { get; set; }

        [JsonProperty("classificationLabels")]
        public List<ClassificationLabel> ClassificationLabels { get; set; }

        [JsonProperty("miniDocs")]
        public List<MiniDoc> MiniDocs { get; set; }
    }
}
