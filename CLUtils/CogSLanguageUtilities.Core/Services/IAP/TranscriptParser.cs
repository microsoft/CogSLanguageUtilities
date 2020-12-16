using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.IAP;
using Microsoft.CogSLanguageUtilities.Definitions.Models.IAP;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.IAP
{
    public class TranscriptParser : ITranscriptParser
    {
        public async Task<IAPTranscript> ParseTranscriptAsync(Stream file)
        {
            string line;
            var sr = new StreamReader(file);
            List<ConversationUtterance> utterances = new List<ConversationUtterance>();
            var firstLine = await sr.ReadLineAsync();
            ExtractMetaData(firstLine, out string id, out string channel);
            while ((line = await sr.ReadLineAsync()) != null)
            {
                utterances.Add(ParseLine(line));
            }

            return new IAPTranscript
            {
                Channel = (ChannelType)int.Parse(channel),
                Id = id,
                Utterances = utterances
            };
        }

        private static void ExtractMetaData(string firstLine, out string id, out string channel)
        {
            Regex pattern = new Regex(@"Id:(?<Id>\d+) Channel:(?<Channel>\d+)", RegexOptions.IgnoreCase);
            var match = pattern.Match(firstLine);
            if (match.Success)
            {
                id = match.Groups["Id"].Value;
                channel = match.Groups["Channel"].Value;
            }
            else
            {
                throw new System.Exception("Invalid Id or Channel");
            }
        }

        private ConversationUtterance ParseLine(string line)
        {
            var timestampIndex = line.IndexOf(':');
            var timestamp = long.Parse(line.Substring(0, timestampIndex));
            return new ConversationUtterance
            {
                Timestamp = timestamp,
                Text = line.Substring(timestampIndex + 1)
            };
        }
    }
}
