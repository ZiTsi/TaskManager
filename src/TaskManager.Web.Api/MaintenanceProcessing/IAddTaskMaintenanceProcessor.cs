using TaskManager.Web.Api.Models;

namespace TaskManager.Web.Api.MaintenanceProcessing
{
    public interface IAddTaskMaintenanceProcessor
    {
        Task AddTask(NewTask newTask);
    }
}