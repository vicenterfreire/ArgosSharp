using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;

namespace ArgosSharp.Domain.Entity
{
    public sealed class JobExecution
    {
        /// <summary>
        /// Represents a uniq ID for the entity
        /// </summary>
        public int Id { get; init; }
        
        /// <summary>
        /// Represents the ID of a <see cref="Job"/> 
        /// </summary>
        public int JobId { get; init; }
        
        /// <summary>
        /// Represents the Job Status
        /// </summary>
        public JobStatusEnum JobStatus { get; private set; }
        
        /// <summary>
        /// Represents the Parameters of an execution (inherit from <see cref="Job"/>
        /// </summary>
        public JobParameters Parameters { get; init; }
        
        /// <summary>
        /// Represents the date/time that the execution was queued
        /// </summary>
        public DateTime? QueuedAt { get; private set; }
        
        /// <summary>
        /// Represents the date/time that the execution started processing
        /// </summary>
        public DateTime? StartedAt { get; private set; }
        
        /// <summary>
        /// Represents the date/time that the execution ended processing
        /// </summary>
        public DateTime? FinishedAt { get; private set; }
        
        /// <summary>
        /// Represents the error message given during the execution
        /// </summary>
        public string? Error { get; private set; }

        private JobExecution() { }

        public JobExecution(int jobId, JobStatusEnum jobStatus, JobParameters parameters)
        {
            // How to validate the <c>jobId</c>
            
            if (!Enum.IsDefined(jobStatus))
                throw new ArgumentOutOfRangeException(nameof(jobStatus));
            
            ArgumentNullException.ThrowIfNull(parameters);
            
            JobId = jobId;
            JobStatus = jobStatus;
            Parameters = parameters;
        }

        public void Enqueued()
        {
            QueuedAt = DateTime.UtcNow;
            JobStatus = JobStatusEnum.Enqueued;
        }

        public void StartProcessing()
        {
            StartedAt = DateTime.UtcNow;
            JobStatus = JobStatusEnum.Processing;
        }

        public void CompleteProcessing()
        {
            FinishedAt = DateTime.UtcNow;
            JobStatus = JobStatusEnum.Completed;
        }
        
        public void FailProcessing(string error)
        {
            FinishedAt = DateTime.UtcNow;
            JobStatus = JobStatusEnum.Failed;
            Error = error;
        }
    }
}
