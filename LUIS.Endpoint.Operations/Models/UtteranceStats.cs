﻿// Copyright (c) Microsoft Corporation. All rights reserved.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.LUIS.Runtime.DataStructures
{
    public class UtteranceStats
    {
        public UtteranceStats()
        {
            FalsePositiveEntities = new List<EntityNameAndLocation>();
            FalseNegativeEntities = new List<EntityNameAndLocation>();
        }

        [JsonProperty(PropertyName = "text")]
        public string UtteranceText { get; set; }

        [JsonProperty(PropertyName = "predictedIntentName")]
        public string PredictedIntentName { get; set; }

        [JsonProperty(PropertyName = "labeledIntentName")]
        public string LabeledIntentName { get; set; }

        [JsonProperty(PropertyName = "falsePositiveEntities")]
        public List<EntityNameAndLocation> FalsePositiveEntities { get; set; }

        [JsonProperty(PropertyName = "falseNegativeEntities")]
        public List<EntityNameAndLocation> FalseNegativeEntities { get; set; }
    }
}