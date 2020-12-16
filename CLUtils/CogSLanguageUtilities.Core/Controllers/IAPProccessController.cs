using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Luis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class IAPProccessController
    {
        IStorageService _storageService;
        ITranscriptParser _transcriptParser;
        ILuisPredictionService _luisPredictionService;
        IIAPTranscriptGenerator _transcriptGenerator;

        public IAPProccessController(
            IStorageService storageService,
            ITranscriptParser transcriptParser,
            ILuisPredictionService luisPredictionService,
            IIAPTranscriptGenerator transcriptGenerator)
        {
            _storageService = storageService;
            _transcriptParser = transcriptParser;
            _luisPredictionService = luisPredictionService;
            _transcriptGenerator = transcriptGenerator;
        }

        public async Task Run()
        {
            // read files from dir
            var files = await _storageService.ListFilesAsync();

            // loop on files
            foreach (var file in files)
            {
                //  1- parse file (extract utterances)
                var fileStream = await _storageService.ReadFileAsync(file);
                var transcript = await _transcriptParser.ParseTranscriptAsync(fileStream);

                var predictionsDictionary = new Dictionary<long, CustomLuisResponse>();
                foreach (var utterance in transcript.Utterances)
                {
                    //  2- run luis prediction endpoint
                    predictionsDictionary[utterance.Timestamp] = await _luisPredictionService.Predict(utterance.Text);
                    //  3- run TA prediction endpoint
                }
                //  4- concatenate result
                var processedTranscript = _transcriptGenerator.GenerateTranscript(predictionsDictionary, transcript.Channel, transcript.Id);

                //  5- write result file
                var outString = JsonConvert.SerializeObject(processedTranscript, Formatting.Indented);
                await _storageService.StoreDataAsync(outString, "test.json");
            }
        }
    }
}
