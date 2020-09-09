// Copyright (c) Microsoft Corporation. All rights reserved.

using Newtonsoft.Json;

namespace Microsoft.LUIS.Runtime.DataStructures
{
    public class EntityNameAndLocation
    {
        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "startCharIndex")]
        public int StartPosition { get; set; }

        [JsonProperty(PropertyName = "endCharIndex")]
        public int EndPosition { get; set; }
    }
}