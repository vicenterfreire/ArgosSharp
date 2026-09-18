using ArgosSharp.Application.Contracts.JobExecutions;
using ArgosSharp.Application.Interfaces.Repositories;
using ArgosSharp.Application.Services.JobExecutionQueue;
using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Factories.JobExecutionFactory;

namespace ArgosSharp.Application.UseCase.JobsExecution.CreateJobExecution
{
    internal class CreateJobExecutionUseCase(
        IJobExecutionFactory jobExecutionFactory,
        IJobExecutionRepository jobExecutionRepository,
        IJobExecutionQueue jobQueue
    ) : ICreateJobExecutionUseCase
    {
        public async Task<JobExecution> CreateJobExecution(CreateJobExecutionRequest createJobExecutionRequest)
        {
            var execution = jobExecutionFactory.Create(createJobExecutionRequest.JobId, createJobExecutionRequest.Parameters);
            await jobExecutionRepository.AddAsync(execution);
            await jobQueue.EnqueueAsync(execution);
            
            execution.Enqueued();
            await jobExecutionRepository.UpdateAsync(execution);
            
            return execution;
        }
    }
}
