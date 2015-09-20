using System.Net.Http;
using System.Web.Http;
using TaskManager.Web.Api.Models;
using TaskManager.Web.Common;
using TaskManager.Web.Common.Routing;
using TaskManager.Web.Api.MaintenanceProcessing;

namespace TaskManager.Web.Api.Controllers.V1
{
    [ApiVersion1RoutePrefix("tasks")]
    [UnitOfWorkActionFilter]
        public class TasksController : ApiController
    {
        private readonly IAddTaskMaintenanceProcessor _addTaskMaintenanceProcessor;

        public TasksController(IAddTaskMaintenanceProcessor addTaskMaintenanceProcessor)
        {
            _addTaskMaintenanceProcessor = addTaskMaintenanceProcessor;
        }

        [Route("", Name = "AddTaskRoute")]
        [HttpPost]
        public IHttpActionResult AddTask(HttpRequestMessage requestMessage, NewTask newTask)
        {
            var task = _addTaskMaintenanceProcessor.AddTask(newTask);
            var result = new TaskCreatedActionResult(requestMessage, task);
            return result;
        }
    }
}

pg 113 : Though links won’t be fully developed