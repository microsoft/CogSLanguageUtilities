using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace computer_vision_quickstart
{
    class Program
    {
        static readonly String endpoint = "https://eastus.api.cognitive.microsoft.com/";
        static readonly String azureResourceKey = "2e10bb66ed3e46bebd3ca1b3522b8f6b";
        static ComputerVisionClient client;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(azureResourceKey))
            { Endpoint = endpoint };

            FileStream fs = File.OpenRead("C:\\Users\\a-noyass\\Documents\\CrackingDocs\\loremipsum\\loremipsum-2.pdf");
            String text = await ConvertAsync(fs, "https://eastus.api.cognitive.microsoft.com/", "2e10bb66ed3e46bebd3ca1b3522b8f6b");
            Console.WriteLine(text);
        }

        public static async Task<String> ConvertAsync(FileStream fs, string endpoint, string azureResourceKey)
        {
            ReadInStreamHeaders response = await client.ReadInStreamAsync(fs);
            const int numberOfCharsInOperationId = 36;
            string operationId = response.OperationLocation.Substring(response.OperationLocation.Length - numberOfCharsInOperationId);

            ReadOperationResult result;
            do
            {
                result = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((result.Status == OperationStatusCodes.Running ||
                result.Status == OperationStatusCodes.NotStarted));
            StringBuilder finalText = new StringBuilder();
            foreach (ReadResult rr in result.AnalyzeResult.ReadResults)
            {
                foreach (Line l in rr.Lines)
                {
                    finalText.AppendFormat($"{l.Text} ");
                }
            }
            return finalText.ToString();
        }
    }
}
