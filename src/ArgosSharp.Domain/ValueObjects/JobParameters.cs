namespace ArgosSharp.Domain.ValueObjects
{
    public sealed class JobParameters
    {
        public string Query { get; init; }
        public IReadOnlyCollection<string> Sources { get; init; }
        public int Depth { get; init; }

        private JobParameters() { }

        public JobParameters(string query, IReadOnlyCollection<string> sources, int depth)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            ArgumentNullException.ThrowIfNull(sources);
            ArgumentOutOfRangeException.ThrowIfZero(sources.Count, nameof(sources));

            ArgumentNullException.ThrowIfNull(depth, nameof(depth));
            ArgumentOutOfRangeException.ThrowIfLessThan(depth, 1, nameof(depth));

            Query = query;
            Sources = sources;
            Depth = depth;
        }
    }
}
