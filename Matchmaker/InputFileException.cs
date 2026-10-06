public sealed class InputFileException : ApplicationException
{
    public string InvalidKey { get; }  // "inovarBOM" | "CBOM" | "asBuilt" | "FAReport"

    public InputFileException(string message, string invalidKey, Exception? inner = null)
        : base(message, inner)
    {
        InvalidKey = invalidKey;
    }
}
