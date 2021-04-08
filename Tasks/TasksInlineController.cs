using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace webapi
{
    [ApiController]
    [Route("/inline")]
    public class TasksInlineController : ControllerBase
    {
        private TasksListService tasksListService;

        public TasksInlineController( TasksListService tasksListService)
        {
           this.tasksListService = tasksListService;
        }

        [HttpGet("/inline/lists")]
        public IEnumerable<TasksList> GetAllLists()
        {
            return tasksListService.GetLists();
        }
        
        [HttpPost("/inline/lists")]
        public ActionResult<TasksList> PostNewList(string name)
        {
            TasksList createdList = tasksListService.CreateList(name);
            return Created($"/lists/{createdList.tasksListId}", createdList);
        }

        [HttpGet("/inline/lists/{listId}/tasks")]
        public IEnumerable<Task> GetTaskList(int listId)
        {
            bool all = bool.Parse(Request.Query["all"]);
            return tasksListService.GetTaskList(listId, all); 
        }

        [HttpPost("/inline/lists/{listId}/tasks")]
        public ActionResult<Task> AddTask(int listId, Task task)
        {   
            // if (ModelState.IsValid)
            // {
                Console.WriteLine("test");
                Task createdTask = tasksListService.CreateTask(listId, task);
                return Created($"/lists/{listId}/tasks/{createdTask.tasksListId}", createdTask);
            
            // return StatusCodes.
        }
        
        [HttpPatch("/inline/lists/{listId}")]
        public ActionResult<TasksList> PatchList(int listId, string name)
        {
            TasksList updatedList = tasksListService.PatchList(listId, name);
            return Created($"/lists/{updatedList.tasksListId}", updatedList);
        }

        [HttpPatch("/inline/lists/{listId}/tasks/{taskId}")]
        public ActionResult<Task> PatchTask(int listId, int taskId, Task task)
        {
            Task updatedTask = tasksListService.PatchTask(listId, taskId, task);
            return Created($"/lists/{updatedTask.tasksListId}/tasks/{updatedTask.taskId}", updatedTask);
        }

        [HttpDelete("/inline/lists/{listId}")]
        public ActionResult DeleteList(int listId)
        {
            tasksListService.DeleteList(listId);
            return Ok($"Deleted: /inline/lists/{listId}");
        }

        [HttpDelete("/inline/lists/{listId}/tasks/{taskId}")]
        public ActionResult DeleteTask(int listId, int taskId)
        {
            tasksListService.DeleteTask(listId, taskId);
            return Ok($"/inline/lists/{listId}/tasks/{taskId}");
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
