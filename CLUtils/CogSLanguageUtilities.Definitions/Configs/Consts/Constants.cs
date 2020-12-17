// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts
{
    public class Constants
    {
        // tool name
        public const string ToolName = "clutils";
        // configs file
        public static readonly string ConfigsFileLocalDirectory = ".";
        public static readonly string ConfigsFileName = "configs.json";
        // text analytics
        public const int TextAnalyticsPredictionMaxCharLimit = 5000;
        public const int TextAnaylticsApiCallDocumentLimit = 5;
        // Mapping
        public const string InstanceKey = "$instance";
        public const string TypeKey = "type";
        public const string StartIndexKey = "startIndex";
        public const string LengthKey = "length";
        public const string TextKey = "text";
    }
}