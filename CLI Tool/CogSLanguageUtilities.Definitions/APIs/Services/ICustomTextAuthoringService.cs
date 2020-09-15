using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ICustomTextAuthoringService
    {
        Task<CustomTextGetLabeledExamplesResponse> GetLabeledExamples();
        Task<Dictionary<string, string>> GetModelsDictionary();
    }
}