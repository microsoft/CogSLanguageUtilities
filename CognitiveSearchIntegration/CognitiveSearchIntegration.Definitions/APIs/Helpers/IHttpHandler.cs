﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Definitions.APIs.Helpers
{
    public interface IHttpHandler
    {
        public Task<HttpResponseMessage> SendGetRequestAsync(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters);

        public Task<HttpResponseMessage> SendJsonPostRequestAsync(string url, object body, Dictionary<string, string> headers, Dictionary<string, string> parameters);
    }
}
