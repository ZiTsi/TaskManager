using System.Net.Http;
using System.Web.Http;
using TaskManager.Web.Api.Models;
using TaskManager.Web.Common.Routing;

namespace TaskManager.Web.Api.Controllers.V1
{
    [ApiVersion1RoutePrefix("tasks")]
    public class TasksController : ApiController
    {
        [Route("", Name = "AddTaskRoute")]
        [HttpPost]
        public Task AddTask(HttpRequestMessage requestMessage, Task newTask)
        {            
            if(!(newTask == null))          
                return new Task { Subject = "In v1, newTask.Subject = " + newTask.Subject };
            else            
                return new Task { Subject = "In v1, not provided newTask.Subject"};           
        }
    }
}