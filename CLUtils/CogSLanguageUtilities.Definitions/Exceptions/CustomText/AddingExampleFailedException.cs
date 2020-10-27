namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.CustomText
{
    public class AddingExampleFailedException : CliException
    {
        public AddingExampleFailedException(string statusCode)
            : base(ConstructMessage(statusCode))
        { }

        public static string ConstructMessage(string statusCode)
        {
            return $"Failed to add labeled example to custom text application with status code: {statusCode}";
        }
    }
}
