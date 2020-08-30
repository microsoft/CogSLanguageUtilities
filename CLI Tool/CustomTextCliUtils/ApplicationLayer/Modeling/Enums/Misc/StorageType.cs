﻿using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StorageType
    {
        Local,
        Blob
    }
}



