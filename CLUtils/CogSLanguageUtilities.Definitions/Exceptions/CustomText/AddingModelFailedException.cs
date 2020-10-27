namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.CustomText
{
    public class AddingModelFailedException : CliException
    {
        public AddingModelFailedException(string statusCode)
            : base(ConstructMessage(statusCode))
        { }

        public static string ConstructMessage(string statusCode)
        {
            return $"Failed to add model to custom text application with status code: {statusCode}";
        }
    }
}
