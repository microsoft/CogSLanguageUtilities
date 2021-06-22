using CustomTextAnalytics.MiniSDK.Helpers;
using Newtonsoft.Json;

namespace CustomTextUtilities.MiniSDK.Models.AnalyzeCustomEntities
{
    public class AnalyzeCustomEntitiesRequestBody
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("analysisInput")]
        public AnalysisInput AnalysisInput { get; set; }

        [JsonProperty("tasks")]
        public Tasks Tasks { get; set; }

        public AnalyzeCustomEntitiesRequestBody(string documentText, string modelId)
        {
            DisplayName = RandomGenerator.GenerateRandomString();
            AnalysisInput = new AnalysisInput()
            {
                Documents = new Document[] {
                    new Document() {
                        Id = RandomGenerator.GenerateRandomId(),
                        Text = documentText
                    }
                }
            };
            Tasks = new Tasks()
            {
                CustomEntityRecognitionTasks = new CustomEntityRecognitionTask[] {
                    new CustomEntityRecognitionTask(){
                        Parameters = new Parameters(){
                            ModelId = modelId,
                            StringIndexType = "TextElements_v8"
                        }
                    }
                }
            };
        }
    }

    public class AnalysisInput
    {
        [JsonProperty("documents")]
        public Document[] Documents { get; set; }
    }

    public class Document
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Tasks
    {
        [JsonProperty("customEntityRecognitionTasks")]
        public CustomEntityRecognitionTask[] CustomEntityRecognitionTasks { get; set; }
    }

    public class CustomEntityRecognitionTask
    {
        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }
    }

    public class Parameters
    {
        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("stringIndexType")]
        public string StringIndexType { get; set; }
    }
}
