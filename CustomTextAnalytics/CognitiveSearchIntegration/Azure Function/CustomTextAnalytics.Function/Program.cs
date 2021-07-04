using AzureCognitiveSearch.PowerSkills.Helpers;
using AzureCognitiveSearch.PowerSkills.ViewModels;
using CustomTextAnalytics.MiniSDK.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.PowerSkills.Text.LUISExtractor
{
    public static class Program
    {
        private static readonly string _endpointUrl = Environment.GetEnvironmentVariable("CustomTextEndpointUrl");
        private static readonly string _serviceKey = Environment.GetEnvironmentVariable("CustomTextApiKey");
        private static readonly string _modelId = Environment.GetEnvironmentVariable("CustomTextModelId");

        [FunctionName("customtext-extractor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext executionContext)
        {
            log.LogInformation("LUIS-D Extractor and Classifier function");


            #region Input Mapping
            string skillName = executionContext.FunctionName;
            IEnumerable<WebApiRequestRecord> requestRecords = WebApiSkillHelpers.GetRequestRecords(req);
            if (requestRecords == null)
            {
                return new BadRequestObjectResult($"{skillName} - Invalid request record array.");
            }
            #endregion Input Mapping

            WebApiSkillResponse response = WebApiSkillHelpers.ProcessRequestRecords(skillName, requestRecords,
                (inRecord, outRecord) =>
                {
                    // extract input
                    var text = inRecord.Data["text"] as string;

                    try
                    {
                        // processing
                        var client = new CustomTextAnalyticsClient(_endpointUrl, _serviceKey);
                        var entities = client.AnalyzeCustomEntitiesAsync(text, _modelId).ConfigureAwait(false).GetAwaiter().GetResult();

                        // get result
                        entities.ToList().ForEach(entity =>
                        {
                            outRecord.Data.Add(entity.Text, entity.Text);
                        });

                        outRecord.Data["result"] = "success";
                    }
                    catch (Exception _)
                    {
                        outRecord.Data["result"] = "failed";
                    }

                    return outRecord;
                });

            return new OkObjectResult(response);
        }
    }
}