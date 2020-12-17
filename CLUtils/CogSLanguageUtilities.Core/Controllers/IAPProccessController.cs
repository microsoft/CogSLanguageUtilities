// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Luis;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class IAPProccessController : IIAPProccessController
    {
        IStorageService _storageService;
        ITranscriptParser _transcriptParser;
        ILuisPredictionService _luisPredictionService;
        IIAPResultGenerator _transcriptGenerator;

        public IAPProccessController(
            IStorageService storageService,
            ITranscriptParser transcriptParser,
            ILuisPredictionService luisPredictionService,
            IIAPResultGenerator transcriptGenerator)
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
            var fileTasks = files.Select(async file =>
            {
                //  parse file (extract utterances)
                var fileStream = await _storageService.ReadFileAsync(file);
                var transcript = await _transcriptParser.ParseTranscriptAsync(fileStream);

                var predictionsDictionary = new ConcurrentDictionary<long, CustomLuisResponse>();
                var tasks = transcript.Utterances.Select(async utterance =>
                {
                    // run luis prediction endpoint
                    predictionsDictionary[utterance.Timestamp] = await _luisPredictionService.Predict(utterance.Text);
                    // TODO: run TA prediction endpoint
                });
                await Task.WhenAll(tasks);

                // concatenate result
                var processedTranscript = _transcriptGenerator.GenerateResult(predictionsDictionary, transcript.Channel, transcript.Id);

                // write result file
                var outString = JsonConvert.SerializeObject(processedTranscript, Formatting.Indented);
                await _storageService.StoreDataAsync(outString, "test.json");
            });
            await Task.WhenAll(fileTasks);
        }
    }
}
