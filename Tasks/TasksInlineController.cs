using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Web.Http.Cors;

namespace webapi
{
    [ApiController]
    [Route("")]
    //[EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    public class TasksInlineController : ControllerBase
    {
        private TasksListService tasksListService;

        public TasksInlineController( TasksListService tasksListService)
        {
           this.tasksListService = tasksListService;
        }

        [HttpGet("/lists")]
        public IEnumerable<TasksList> GetAllLists()
        {
            return tasksListService.GetLists();
        }
        
        [HttpPost("/lists")]
        public ActionResult<TasksList> PostNewList(string name)
        {
            TasksList createdList = tasksListService.CreateList(name);
            return Created($"/lists/{createdList.tasksListId}", createdList);
        }

        [HttpGet("/lists/{listId}/tasks")]
        public IEnumerable<Task> GetTaskList(int listId)
        {
            bool all = bool.Parse(Request.Query["all"]);
            return tasksListService.GetTaskList(listId, all); 
        }

        [HttpPost("/lists/{listId}/tasks")]
        public ActionResult<Task> AddTask(int listId, Task task)
        {   
            Task createdTask = tasksListService.CreateTask(listId, task);
            return Created($"/lists/{listId}/tasks/{createdTask.tasksListId}", createdTask);
        }
        
        [HttpPut("/lists/{listId}")]
        public ActionResult<TasksList> PatchList(int listId, string name)
        {
            TasksList updatedList = tasksListService.PatchList(listId, name);
            return Created($"/lists/{updatedList.tasksListId}", updatedList);
        }

        [HttpPut("/lists/{listId}/tasks/{taskId}")]
        public ActionResult<Task> PatchTask(int listId, int taskId, Task task)
        {
            Task updatedTask = tasksListService.PatchTask(listId, taskId, task);
            return Created($"/lists/{updatedTask.tasksListId}/tasks/{updatedTask.taskId}", updatedTask);
        }

        [HttpDelete("/lists/{listId}")]
        public ActionResult DeleteList(int listId)
        {
            tasksListService.DeleteList(listId);
            return Ok($"Deleted: /lists/{listId}");
        }

        [HttpDelete("/lists/{listId}/tasks/{taskId}")]
        public ActionResult DeleteTask(int listId, int taskId)
        {
            tasksListService.DeleteTask(listId, taskId);
            return Ok($"/lists/{listId}/tasks/{taskId}");
        }

        [HttpGet("/dashboard")]
        public DashboardDTO GetDashboard()
        {
            return tasksListService.GetDashboard();
        }

        [HttpGet("/collection/today")]
        public IEnumerable<TodayTaskDTO> GetTodayTasks()
        {
            return tasksListService.GetTodayTasks();
        }
    }
}
