using Newtonsoft.Json;
using System;

namespace CustomTextUtilities.MiniSDK.Models.GetJobResult
{
    public partial class GetJobResultResponse
    {
        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }

        [JsonProperty("jobId")]
        public Guid JobId { get; set; }

        [JsonProperty("lastUpdateDateTime")]
        public DateTimeOffset LastUpdateDateTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("tasks")]
        public WelcomeTasks Tasks { get; set; }
    }

    public partial class WelcomeTasks
    {
        [JsonProperty("details")]
        public Details Details { get; set; }

        [JsonProperty("completed")]
        public long Completed { get; set; }

        [JsonProperty("failed")]
        public long Failed { get; set; }

        [JsonProperty("inProgress")]
        public long InProgress { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("tasks")]
        public TasksTasks Tasks { get; set; }
    }

    public partial class Details
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastUpdateDateTime")]
        public DateTimeOffset LastUpdateDateTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public Results Results { get; set; }
    }

    public partial class Results
    {
        [JsonProperty("documents")]
        public Document[] Documents { get; set; }

        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }
    }

    public partial class Document
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("entities")]
        public Entity[] Entities { get; set; }

        [JsonProperty("warnings")]
        public object[] Warnings { get; set; }
    }

    public partial class Entity
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("confidenceScore")]
        public double ConfidenceScore { get; set; }
    }

    public partial class Statistics
    {
        [JsonProperty("documentsCount")]
        public long DocumentsCount { get; set; }

        [JsonProperty("validDocumentsCount")]
        public long ValidDocumentsCount { get; set; }

        [JsonProperty("erroneousDocumentsCount")]
        public long ErroneousDocumentsCount { get; set; }

        [JsonProperty("transactionsCount")]
        public long TransactionsCount { get; set; }
    }

    public partial class TasksTasks
    {
        [JsonProperty("customEntityRecognitionTasks")]
        public Details[] CustomEntityRecognitionTasks { get; set; }
    }
}
