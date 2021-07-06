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
        private static readonly string _endpointUrl = "https://cognitivesearchintegrationcustomtextresource.cognitiveservices.azure.com";
        private static readonly string _serviceKey = "0a6b7248fa794016bcbeed4442fbdbed";
        private static readonly string _modelId = "a0b4c4c0-ec29-418d-967f-0e59a29b0dac_Extraction_latest";

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

            var customTextClient = new CustomTextAnalyticsClient(_endpointUrl, _serviceKey);

            WebApiSkillResponse response = WebApiSkillHelpers.ProcessRequestRecords(skillName, requestRecords,
                (inRecord, outRecord) =>
                {
                    var text = inRecord.Data["text"] as string;
                    try
                    {
                        // processing
                        var entities = customTextClient.AnalyzeCustomEntitiesAsync(text, _modelId).ConfigureAwait(false).GetAwaiter().GetResult();

                        // get result
                        entities.ToList().ForEach(entity =>
                        {
                            outRecord.Data.Add(entity.Category, entity.Category);
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