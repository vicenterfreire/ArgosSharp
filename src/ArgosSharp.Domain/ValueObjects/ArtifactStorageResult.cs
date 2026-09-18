namespace ArgosSharp.Domain.ValueObjects
{
    public sealed record ArtifactStorageResult(string StorageKey, long Size, string ContentType);
}
