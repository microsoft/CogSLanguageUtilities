using System.Linq;
using System.Net.Http;

namespace CustomTextAnalytics.MiniSDK.RestClient.Utilities
{
    public class ValueExtractor
    {
        public static string GetJobId(HttpResponseMessage response)
        {
            var operationLocation = response.Headers.GetValues("operation-location").ToList().First().ToString();
            return operationLocation.Split("/").ToList().Last();
        }
    }
}
