using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArgosSharp.Domain.UnitTests")]

namespace ArgosSharp.Domain.Factories.JobExecutionFactory
{
    internal class JobExecutionFactory : IJobExecutionFactory
    {
        public JobExecution Create(int jobId, JobParameters parameters) =>
            new(jobId, JobStatusEnum.Created, parameters);
    }
}
