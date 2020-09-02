namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Storage
{
    class FileNotFoundException : CliException
    {
        public FileNotFoundException(string filePath)
            : base(ConstructMessage(filePath))
        { }

        public static string ConstructMessage(string filePath)
        {
            return $"File Not Found: {filePath}";
        }
    }
}
