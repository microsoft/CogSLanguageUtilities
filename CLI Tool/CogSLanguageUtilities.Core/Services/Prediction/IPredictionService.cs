using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Response.Result;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction
{
    public interface IPredictionService
    {
        public Task<CustomTextPredictionResponse> GetPredictionAsync(string query);
    }
}
