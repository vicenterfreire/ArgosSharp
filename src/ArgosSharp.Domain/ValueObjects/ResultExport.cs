namespace ArgosSharp.Domain.ValueObjects
{
    public sealed record ResultExport(Stream Content, string FileName, string ContentType, string Extension);
}
