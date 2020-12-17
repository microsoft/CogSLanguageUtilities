// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Models.IAP;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Luis
{
    public class CustomLuisResponse
    {
        public string Query { get; set; }
        public string TopIntent { get; set; }
        public Dictionary<string, double?> Intents { get; set; }
        public List<Extraction> Entities { get; set; }
    }
}
