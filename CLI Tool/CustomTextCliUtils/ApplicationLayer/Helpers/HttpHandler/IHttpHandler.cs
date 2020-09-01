using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.HttpHandler
{
    public interface IHttpHandler
    {
        HttpResponseMessage SendGetRequest(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters);

        HttpResponseMessage SendJsonPostRequest(string url, object body, Dictionary<string, string> headers, Dictionary<string, string> parameters);
    }
}
