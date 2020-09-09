// Copyright (c) Microsoft Corporation. All rights reserved.

using Newtonsoft.Json;

namespace Microsoft.LUIS.Runtime.DataStructures
{
    public class ModelStats
    {
        [JsonProperty(PropertyName = "modelName")]
        public string ModelName { get; set; }

        [JsonProperty(PropertyName = "modelType")]
        public string ModelType { get; set; }

        [JsonProperty(PropertyName = "precision")]
        public double Precision { get; set; }

        [JsonProperty(PropertyName = "recall")]
        public double Recall { get; set; }

        [JsonProperty(PropertyName = "fScore")]
        public double FScore { get; set; }

        [JsonProperty(PropertyName = "entityTextFScore", NullValueHandling = NullValueHandling.Ignore)]
        public double? EntityTextFScore { get; set; }

        [JsonProperty(PropertyName = "entityTypeFScore", NullValueHandling = NullValueHandling.Ignore)]
        public double? EntityTypeFScore { get; set; }
    }
}