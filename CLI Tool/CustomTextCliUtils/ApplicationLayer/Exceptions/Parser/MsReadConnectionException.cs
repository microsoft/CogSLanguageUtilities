﻿using Microsoft.CustomTextCliUtils.Configs.Consts;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Parser
{
    public class MsReadConnectionException : CliException
    {
        public MsReadConnectionException(string message, string cognitiveServicesKey, string cognitiveServicesEndpoint)
            : base(ConstructMessage(message, cognitiveServicesKey, cognitiveServicesEndpoint))
        { }

        private static string ConstructMessage(string message, string cognitiveServicesKey, string cognitiveServicesEndpoint)
        {
            return $"Connection to Azure Cognitive Services failed with message \"{message}\" \nCheck {ConfigKeys.MSReadCognitiveServicesKey}: "
                + $"{cognitiveServicesKey} or {ConfigKeys.MSReadCognitiveServicesEndpoint}: {cognitiveServicesEndpoint}";
        }
    }
}
