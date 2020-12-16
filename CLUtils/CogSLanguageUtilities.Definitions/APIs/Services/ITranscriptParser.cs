using Microsoft.CogSLanguageUtilities.Definitions.Models.IAP;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ITranscriptParser
    {
        Task<IAPTranscript> ParseTranscriptAsync(string filePath);
    }
}