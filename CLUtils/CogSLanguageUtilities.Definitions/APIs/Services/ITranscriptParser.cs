using Microsoft.CogSLanguageUtilities.Definitions.Models.IAP;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ITranscriptParser
    {
        Task<IAPTranscript> ParseTranscriptAsync(Stream file);
    }
}