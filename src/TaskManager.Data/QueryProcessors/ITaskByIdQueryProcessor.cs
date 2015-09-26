using TaskManager.Data.Entities;

namespace TaskManager.Data.QueryProcessors
{
    public interface ITaskByIdQueryProcessor
    {
        Task GetTask(long taskId);
    }
}