﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.HttpHandler
{
    public class HttpHandler : IHttpHandler
    {
        private static HttpClient httpClient = new HttpClient();

        public async Task<HttpResponseMessage> SendGetRequestAsync(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            var urlWithParameters = parameters == null ? url : CreateUrlWithParameters(url, parameters);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, urlWithParameters))
            {
                headers?.ToList().ForEach(h => requestMessage.Headers.Add(h.Key, h.Value));
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                return response;
            }
        }

        public async Task<HttpResponseMessage> SendJsonPostRequestAsync(string url, object body, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            var urlWithParameters = parameters == null ? url : CreateUrlWithParameters(url, parameters);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, urlWithParameters))
            {
                headers?.ToList().ForEach(h => requestMessage.Headers.Add(h.Key, h.Value));
                var requestBodyAsJson = JsonConvert.SerializeObject(body);
                requestMessage.Content = new StringContent(requestBodyAsJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                return response;
            }
        }

        private string CreateUrlWithParameters(string url, Dictionary<string, string> parameters)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            parameters.ToList().ForEach(p => query[p.Key] = p.Value);
            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();
            return url;
        }
    }
}
