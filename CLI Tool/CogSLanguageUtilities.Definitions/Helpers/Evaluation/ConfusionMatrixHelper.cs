namespace Microsoft.CogSLanguageUtilities.Definitions.Helpers.Evaluation
{
    class ConfusionMatrixHelper
    {
        public double CalculatePrecision(int TruePositives, int FalsePositives)
        {
            return (double)TruePositives / (TruePositives + FalsePositives);
        }

        public double CalculateRecall(int TruePositives, int FalseNegatives)
        {
            return (double)TruePositives / (TruePositives + FalseNegatives);
        }

        public double CalculateFScore(double precision, double recall)
        {
            return (2 * precision * recall) / (precision + recall);
        }
    }
}
