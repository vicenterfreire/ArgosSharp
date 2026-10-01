using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;

namespace ArgosSharp.Domain.Entity
{
    public sealed class Job
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public JobType JobType { get; init; }
        public JobParameters Parameters { get; init; }
        public JobPriority Priority { get; init; }
        public bool Enabled { get; init; }

        private Job() { }

        public Job(string name, JobType jobType, JobParameters parameters, JobPriority priority, bool enabled)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(name);

            if (!Enum.IsDefined(jobType))
                throw new ArgumentOutOfRangeException(nameof(jobType));

            ArgumentNullException.ThrowIfNull(parameters);

            if (!Enum.IsDefined(priority))
                throw new ArgumentOutOfRangeException(nameof(priority));

            Name = name;
            JobType = jobType;
            Parameters = parameters;
            Priority = priority;
            Enabled = enabled;
        }
    }
}
