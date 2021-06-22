﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CustomTextUtilities.MiniSDK
{
    public class HttpPipeline
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<HttpResponseMessage> SendHttpRequestAsync<T>(HttpRequestMethod method, string url, Dictionary<string, string> urlParameters, Dictionary<string, string> headers, T body = default)
        {
            var requestMethod = GetHttpRequestMethod(method);
            var fullUrl = CreateUrlWithParameters(url, urlParameters);
            try
            {
                var requestMessage = new HttpRequestMessage(requestMethod, fullUrl);
                PopulateRequestMessageHeaders(headers, requestMessage);
                var requestBodyAsJson = JsonConvert.SerializeObject(body);
                requestMessage.Content = new StringContent(requestBodyAsJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.SendAsync(requestMessage);
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public enum HttpRequestMethod
        {
            GET,
            POST
        }

        #region Helpers
        private string CreateUrlWithParameters(string url, Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                return url;
            }
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            parameters.ToList().ForEach(p => query[p.Key] = p.Value);
            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();
            return url;
        }

        private void PopulateRequestMessageHeaders(Dictionary<string, string> headers, HttpRequestMessage requestMessage)
        {
            foreach (KeyValuePair<string, string> h in headers)
            {
                requestMessage.Headers.Add(h.Key, h.Value);
            }
        }

        private HttpMethod GetHttpRequestMethod(HttpRequestMethod method)
        {
            switch (method)
            {
                case HttpRequestMethod.GET:
                    return HttpMethod.Get;
                case HttpRequestMethod.POST:
                    return HttpMethod.Post;
                default:
                    return HttpMethod.Get;
            }
        }
        #endregion Helpers
    }
}
