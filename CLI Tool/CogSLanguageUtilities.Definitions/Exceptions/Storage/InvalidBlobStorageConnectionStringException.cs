﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Storage
{
    public class InvalidBlobStorageConnectionStringException : CliException
    {
        public InvalidBlobStorageConnectionStringException(string connectionString)
            : base(ConstructMessage(connectionString))
        { }

        public static string ConstructMessage(string connectionString)
        {
            return "Invalid Blob Storage Connection String: " + connectionString;
        }
    }
}
