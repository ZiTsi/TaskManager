using TaskManager.Common;
using TaskManager.Common.TypeMapping;
using TaskManager.Data.Exceptions;
using TaskManager.Data.QueryProcessors;
using TaskManager.Web.Api.Models;

namespace TaskManager.Web.Api.MaintenanceProcessing
{
    public class ReactivateTaskWorkflowProcessor : IReactivateTaskWorkflowProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ITaskByIdQueryProcessor _taskByIdQueryProcessor;
        private readonly IUpdateTaskStatusQueryProcessor _updateTaskStatusQueryProcessor;
        public ReactivateTaskWorkflowProcessor(ITaskByIdQueryProcessor taskByIdQueryProcessor,
                                               IUpdateTaskStatusQueryProcessor updateTaskStatusQueryProcessor,
                                               IAutoMapper autoMapper)
        {
            _taskByIdQueryProcessor = taskByIdQueryProcessor;
            _updateTaskStatusQueryProcessor = updateTaskStatusQueryProcessor;
            _autoMapper = autoMapper;
        }

        public Task ReactivateTask(long taskId)
        {
            var taskEntity = _taskByIdQueryProcessor.GetTask(taskId);
            if (taskEntity == null)
            {
                throw new RootObjectNotFoundException("Task not found");
            }
            // Simulate some workflow logic...
            if (taskEntity.Status.Name != "Completed")
            {
                throw new BusinessRuleViolationException(
                "Incorrect task status. Expected status of 'Completed'.");
            }

            taskEntity.CompletedDate = null;

            _updateTaskStatusQueryProcessor.UpdateTaskStatus(taskEntity, "In Progress");

            var task = _autoMapper.Map<Task>(taskEntity);
            return task;
        }
    }
}