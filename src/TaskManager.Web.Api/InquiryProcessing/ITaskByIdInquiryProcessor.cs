using TaskManager.Web.Api.Models;

namespace TaskManager.Web.Api.InquiryProcessing
{
    public interface ITaskByIdInquiryProcessor
    {
        Task GetTask(long taskId);
    }
}
