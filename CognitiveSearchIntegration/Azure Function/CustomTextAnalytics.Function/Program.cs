using AzureCognitiveSearch.PowerSkills.Helpers;
using AzureCognitiveSearch.PowerSkills.ViewModels;
using CustomTextUtilities.MiniSDK.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.PowerSkills.Text.LUISExtractor
{
    public static class Program
    {
        private static string _endpointUrl = "";
        private static string _serviceKey = "";
        private static string _modelId = "";

        [FunctionName("customtext-extractor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            Microsoft.Azure.WebJobs.ExecutionContext executionContext)
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

                    // processing
                    var client = new CustomTextAnalyticsClient(_endpointUrl, _serviceKey);
                    var entities = client.AnalyzeCustomEntitiesAsync(text, _modelId).ConfigureAwait(false).GetAwaiter().GetResult();

                    // result mocking
                    outRecord.Data["result"] = "success";

                    // mock entities
                    outRecord.Data.Add("account", "account");
                    outRecord.Data.Add("card", "card");
                    outRecord.Data.Add("loan", "loan");

                    return outRecord;
                });

            return new OkObjectResult(response);
        }
    }
}
