using Microsoft.CogSLanguageUtilities.Definitions.Models.Luis;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ILuisPredictionService
    {
        public Task<CustomLuisResponse> Predict(string query);
    }
}