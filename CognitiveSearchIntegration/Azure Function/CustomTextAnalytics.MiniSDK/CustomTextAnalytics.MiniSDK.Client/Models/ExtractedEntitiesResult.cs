
using CustomTextUtilities.MiniSDK.Models.GetJobResult;
using System.Linq;

namespace CustomTextAnalytics.MiniSDK.Client.Models
{
    public partial class ExtractedEntitiesResult
    {
        public Entity[] Entities { get; set; }

        public static ExtractedEntitiesResult FromResponse(GetJobResultResponse getJobResultResponse)
        {
            var extractedEntities = getJobResultResponse.Tasks.Tasks.CustomEntityRecognitionTasks[0].Results.Documents[0].Entities;
            var result = extractedEntities.ToList().Select(entity =>
            {
                return new Entity()
                {
                    Text = entity.Text,
                    Category = entity.Category,
                    Offset = entity.Offset,
                    Length = entity.Length,
                    ConfidenceScore = entity.ConfidenceScore
                };
            }).ToArray();
            return new ExtractedEntitiesResult()
            {
                Entities = result
            };
        }
    }

    public partial class Entity
    {
        public string Text { get; set; }

        public string Category { get; set; }

        public long Offset { get; set; }

        public long Length { get; set; }

        public double ConfidenceScore { get; set; }
    }
}
