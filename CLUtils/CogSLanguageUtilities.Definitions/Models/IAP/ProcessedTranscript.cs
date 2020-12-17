// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Enums.IAP;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.IAP
{
    public partial class ProcessedTranscript
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("conversation")]
        public List<Conversation> Conversation { get; set; }
    }

    public partial class Conversation
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("extractions")]
        public List<Extraction> Extractions { get; set; }
    }

    public partial class Extraction
    {
        [JsonProperty("entityName")]
        public string EntityName { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<Extraction> Children { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("transcriptId")]
        public string TranscriptId { get; set; }

        [JsonProperty("channel")]
        public ChannelType Channel { get; set; }
    }

}
