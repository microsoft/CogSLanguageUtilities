namespace Microsoft.LUIS.Endpoint.Operations.Models
{
    public class TestingExample
    {
        public string Text { get; set; }
        public PredictionData LabeledData { get; set; }
        public PredictionData ActualData { get; set; }
    }
}
