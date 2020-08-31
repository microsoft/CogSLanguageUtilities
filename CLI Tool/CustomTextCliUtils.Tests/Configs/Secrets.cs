using System;

namespace Microsoft.CustomTextCliUtils.Tests.Configs
{
    public class Secrets
    {
        public static readonly string MSReadCognitiveServiceEndPoint = Environment.GetEnvironmentVariable("MSReadCognitiveServiceEndPoint");
        public static readonly string MSReadCongnitiveServiceKey = Environment.GetEnvironmentVariable("MSReadCongnitiveServiceKey");
        public static readonly string StorageAccountConnectionString = Environment.GetEnvironmentVariable("StorageAccountConnectionString");
        public static readonly string CustomTextKey = Environment.GetEnvironmentVariable("CustomTextKey");
        public static readonly string CustomTextEndpoint = Environment.GetEnvironmentVariable("CustomTextEndpoint");
        public static readonly string CustomTextAppId = Environment.GetEnvironmentVariable("CustomTextAppId");
    }
}
