using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Prediction;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers
{
    public interface IBatchTestingController
    {
        public Task EvaluateModelAsync(StorageType sourceStorageType, StorageType destinationStorageType, CognitiveServiceType service);
    }
}
