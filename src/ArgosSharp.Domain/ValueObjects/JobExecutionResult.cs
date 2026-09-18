namespace ArgosSharp.Domain.ValueObjects
{
    public sealed record JobExecutionResult(Type JobType, IEnumerable<object> Data);
}
