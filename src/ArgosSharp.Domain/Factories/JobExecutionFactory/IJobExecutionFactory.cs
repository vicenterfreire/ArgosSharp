using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.ValueObjects;


namespace ArgosSharp.Domain.Factories.JobExecutionFactory
{
    public interface IJobExecutionFactory
    {
        JobExecution Create(int jobId, JobParameters parameters);
    }
}
