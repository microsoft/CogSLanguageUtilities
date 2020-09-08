using Microsoft.CogSLanguageUtilities.Definitions.Models.Models.Prediction.CustomTextResponse;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction
{
    public interface IPredictionService
    {
        public Task<CustomTextPredictionResponse> GetPredictionAsync(string query);
    }
}
