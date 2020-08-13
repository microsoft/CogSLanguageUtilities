// 'use strict';

const async = require('async');
const fs = require('fs');
const path = require("path");
const { Console } = require('console');
const createReadStream = require('fs').createReadStream
const sleep = require('util').promisify(setTimeout);
const ComputerVisionClient = require('@azure/cognitiveservices-computervision').ComputerVisionClient;
const ApiKeyCredentials = require('@azure/ms-rest-js').ApiKeyCredentials;

let key = "2e10bb66ed3e46bebd3ca1b3522b8f6b";//process.env['COMPUTER_VISION_SUBSCRIPTION_KEY'];
let endpoint = "https://eastus.api.cognitive.microsoft.com/";//process.env['COMPUTER_VISION_ENDPOINT']
if (!key) { throw new Error('Set your environment variables for your subscription key and endpoint.'); }

let computerVisionClient = new ComputerVisionClient(
  new ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': key } }), endpoint);

function computerVision() {
  async.series([
    async function () {
      // URL images containing printed and handwritten text
      const printedText = 'https://moderatorsampleimages.blob.core.windows.net/samples/sample2.jpg';
      const handwrittenText = 'https://raw.githubusercontent.com/MicrosoftDocs/azure-docs/master/articles/cognitive-services/Computer-vision/Images/readsample.jpg';

      console.log('Recognizing pdf text...');
      var pdf = await readText(computerVisionClient);
      printRecText(pdf);

      // Perform text recognition and await the result
      async function readText(client) {
        // To recognize text in a local image, replace client.recognizeText() with recognizeTextInStream() as shown:
        // result = await client.recognizeTextInStream(mode, () => createReadStream(localImagePath));
        let result = await client.readInStream(() => createReadStream("C:\\Users\\a-noyass\\Documents\\CrackingDocs\\loremipsum\\loremipsum-2.pdf"));
        // Operation ID is last path segment of operationLocation (a URL)
        let operation = result.operationLocation.split('/').slice(-1)[0];

        // Wait for text recognition to complete
        // result.status is initially undefined, since it's the result of recognizeText
        while (result.status !== 'succeeded') { await sleep(1000); result = await client.getReadResult(operation); }
        // console.log(result.analyzeResult.readResults);
        return result.analyzeResult.readResults;
      }


      // Prints all text from OCR result
      function printRecText(readResults) {
        if (readResults.length) {
          readResults.forEach(readResult => {
            readResult.lines.forEach(line => {
              console.log(line.text);
            });
          });
        }
        else { console.log('No recognized text.'); }
      }
    },
    function () {
      return new Promise((resolve) => {
        resolve();
      })
    }
  ], (err) => {
    throw (err);
  });
}

computerVision();